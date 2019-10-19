import os

os.sys.path
import sys
import random
import math
import re
import time
import numpy as np
import cv2
import matplotlib
import matplotlib.pyplot as plt

# Root directory of the project
ROOT_DIR = os.path.abspath("../../../")

# Import Mask RCNN
sys.path.append(ROOT_DIR)  # To find local version of the library
from mrcnn.config import Config
from mrcnn import utils
import mrcnn.model as modellib
from mrcnn import visualize
from mrcnn.model import log
import yaml
from PIL import Image
from PIL import ImageFile
ImageFile.LOAD_TRUNCATED_IMAGES=True
import tensorflow as tf
from skimage import morphology
from datetime import datetime
import skimage.io

# Directory to save logs and trained model
MODEL_DIR = os.path.join(ROOT_DIR, "logs")

iter_num = 0

# Local path to trained weights file
COCO_MODEL_PATH = os.path.join(ROOT_DIR, "mask_rcnn_coco.h5")
# Download COCO trained weights from Releases if needed
if not os.path.exists(COCO_MODEL_PATH):
    utils.download_trained_weights(COCO_MODEL_PATH)


class ShapesConfig(Config):
    """Configuration for training on the toy shapes dataset.
    Derives from the base Config class and overrides values specific
    to the toy shapes dataset.
    """
    # Give the configuration a recognizable name
    NAME = "shapes"

    # Train on 1 GPU and 8 images per GPU. We can put multiple images on each
    # GPU because the images are small. Batch size is 8 (GPUs * images/GPU).
    GPU_COUNT = 1
    IMAGES_PER_GPU = 2

    # Number of classes (including background)
    NUM_CLASSES = 1 + 1  # background + 1 shapes

    # Use small images for faster training. Set the limits of the small side
    # the large side, and that determines the image shape.
    IMAGE_MIN_DIM = 320
    IMAGE_MAX_DIM = 384

    # Use smaller anchors because our image and objects are small
    RPN_ANCHOR_SCALES = (20, 40, 80, 160, 320)  # anchor side in pixels

    # Reduce training ROIs per image because the images are small and have
    # few objects. Aim to allow ROI sampling to pick 33% positive ROIs.
    TRAIN_ROIS_PER_IMAGE = 20

    # Use a small epoch since the data is simple
    STEPS_PER_EPOCH = 83

    # use small validation steps since the epoch is small
    VALIDATION_STEPS = 5


config = ShapesConfig()


def get_ax(rows=1, cols=1, size=8):
    """Return a Matplotlib Axes array to be used in
    all visualizations in the notebook. Provide a
    central point to control graph sizes.

    Change the default size attribute to control the size
    of rendered images
    """
    _, ax = plt.subplots(rows, cols, figsize=(size * cols, size * rows))
    return ax

class InferenceConfig(ShapesConfig):
    # Set batch size to 1 since we'll be running inference on
    # one image at a time. Batch size = GPU_COUNT * IMAGES_PER_GPU
    GPU_COUNT = 1
    IMAGES_PER_GPU = 1


def area(image):
    area = 0
    height, width = image.shape
    for i in range(height):
        for j in range(width):
            if image[i, j] == 1:
                area += 1
    return area


def intersection(line1, line2):
    """Finds the intersection of two lines given in Hesse normal form.
    Returns closest integer pixel locations.
    """
    rho1, theta1 = line1[0]
    rho2, theta2 = line2[0]
    A = np.array([
        [np.cos(theta1), np.sin(theta1)],
        [np.cos(theta2), np.sin(theta2)]
    ])
    b = np.array([[rho1], [rho2]])
    x0, y0 = np.linalg.solve(A, b)
    x0, y0 = int(np.round(x0)), int(np.round(y0))
    return [(x0, y0)]

def midle_vline(line1, line2):
    rho1, theta1 = line1[0]
    rho2, theta2 = line2[0]
    rho = (rho1+rho2)/2
#     top1 = [int(rho1/np.cos(theta1)),0] #该直线与第一行的交点
#     top2 = [int(rho2/np.cos(theta2)),0] #该直线与第一行的交点
#     top = [(top1[0]+top2[0])/2, 0]
#     bt1 = (int((rho1-y_max*np.sin(theta1))/np.cos(theta1)),y_max)
#     bt2 = (int((rho2-y_max*np.sin(theta2))/np.cos(theta2)),y_max)
#     bt = [(bt1[0]+bt2[0])/2, y_max]
    theta = (theta1+theta2)/2
    line = [[rho, theta]]
    return line
    
def segmented_intersections(lines):
    """Finds the intersections between groups of lines."""

    intersections = []
    group = lines[0]
    next_group = lines[1]
    for line1 in group:
        for line2 in next_group:
            intersections.append(intersection(line1, line2))
    return intersections

from samples.coco import coco


def facade():
    points_group = []
    # Import COCO config
    sys.path.append(os.path.join(ROOT_DIR, "samples/coco/"))  # To find local version

    # Local path to trained weights file
    COCO_MODEL_PATH = os.path.join(MODEL_DIR, "mask_rcnn_shapes_0030.h5")
    # Download COCO trained weights from Releases if needed
    if not os.path.exists(COCO_MODEL_PATH):
        utils.download_trained_weights(COCO_MODEL_PATH)
        print("Downloading***********************")

    config = InferenceConfig()
    model = modellib.MaskRCNN(mode="inference", model_dir=MODEL_DIR, config=config)

    # Create model object in inference mode.
    model = modellib.MaskRCNN(mode="inference", model_dir=MODEL_DIR, config=config)
    # Load weights trained on MS-COCO
    model.load_weights(COCO_MODEL_PATH, by_name=True)
    # COCO Class names
    # Index of the class in the list is its ID. For example, to get ID of
    # the teddy bear class, use: class_names.index('teddy bear')
    class_names = ['BG', 'facade']

    image = skimage.io.imread(os.path.join("../test/image.png"))
    image = cv2.cvtColor(image, cv2.COLOR_RGBA2RGB)

    img_gray = cv2.cvtColor(image, cv2.COLOR_RGB2GRAY)
    img_gray = cv2.GaussianBlur(img_gray, (3, 3), 0)
    a = datetime.now()
    # Run detection
    results = model.detect([image], verbose=1)
    b = datetime.now()
    # Visualize results
    print("shijian", (b - a).seconds)
    r = results[0]

    facades_mask = []
    facades_w = []
    facades_h = []
    
    for i in range(len(r['masks'][0,0,:])):
        mask = r['masks'][:,:,i] 
        
        w = np.sum(np.max(mask, axis=0))
        facades_w.append(w)
        h = np.sum(np.max(mask, axis=1))
        facades_h.append(h)
        print("w=",w, " h=",h)
        
        kernel = np.ones((int(h/10),int(w/10)),np.uint8)
        mask = mask.astype('uint8')
        dilation = cv2.dilate(mask,kernel,iterations = 1)
        mask_work = dilation
        
        # building edges
        edgs_detail = cv2.Canny(img_gray, 50, 150, apertureSize = 3)

        # building mask
        building_mask = np.bitwise_and(edgs_detail, dilation)
        
        # density of building
        mask_area = area(mask)
        builde_area = area(np.bitwise_and(edgs_detail, mask))
        density = builde_area/mask_area
        print(density)

        if w > len(image[0])/4 and density > 0.20:
            kernel2 = np.ones((int(h/10),int(w/3)),np.uint8)
            mask_erode = cv2.erode(mask,kernel2,iterations = 1)
            mask_work= np.bitwise_xor(mask_erode,dilation)

        # building facade part
        building_details = np.bitwise_and(edgs_detail,mask_work)
        
        facades_mask.append(building_details)
    
    result = image.copy()
    for i in range(len(facades_mask)):   
    
        building_details = facades_mask[i]
        w = facades_w[i]
        h = facades_h[i]
        h_min = int(h/8)
        vertical_lines = cv2.HoughLines(building_details,1,np.pi/180,h_min)
        max_vertical = -1
        min_vertical = 30000
        max_vindex = -1
        min_vindex = 30000
        # print(vertical_lines)
        # print("###########################")
        num_max = math.sqrt((w/building_details.shape[1]))*30
        #num_max = (w/building_details.shape[1])*20
        if vertical_lines is not None:
            num = 0
            for k, line in enumerate(vertical_lines):
                if num>num_max:
                    break
                rho = line[0][0] #第一个元素是距离rho 
                theta= line[0][1] #第二个元素是角度theta 
                if (theta < (1.5*np.pi/18. )) or (theta > (16.*np.pi/18.0)): #垂直直线 
                    num += 1
                    pt1 = (int(rho/np.cos(theta)),0) #该直线与第一行的交点
                    #该直线与最后一行的焦点 
                    pt2 = (int((rho-result.shape[0]*np.sin(theta))/np.cos(theta)),result.shape[0])
                    #cv2.line(byproduction, pt1, pt2, (255)) # 绘制一条白线
                    if (int(rho/np.cos(theta)) > max_vertical):
                        max_vertical = rho/np.cos(theta)
                        max_vindex = k
                    elif (int(rho/np.cos(theta))< min_vertical):
                        min_vertical = rho/np.cos(theta)
                        min_vindex = k
        if min_vindex != 30000 and max_vindex != -1:
            medle_line = midle_vline(vertical_lines[max_vindex], vertical_lines[min_vindex])               
            if h/w>=2:
                horizontal_lines = cv2.HoughLines(building_details,1,np.pi/180,int(w/8))
                #horizontal_lines = horizontal_lines_order(horizontal_lines,building_details)
            else:
                horizontal_lines = cv2.HoughLines(building_details,1,np.pi/180,int(w/4))
                #horizontal_lines = horizontal_lines_order(horizontal_lines,building_details)
            max_horizontal = -1
            min_horizontal = 30000
            max_hindex = -1
            min_hindex = 30000
            num_max = math.sqrt(h/building_details.shape[0])*20
            #num_max = (w/building_details.shape[1])*20
            if horizontal_lines is not None:
                num = 0
                for j, line in enumerate(horizontal_lines):
                    if num>num_max:
                        break
                    rho = line[0][0] #第一个元素是距离rho 
                    theta= line[0][1] #第二个元素是角度theta 
                    if (theta > (5.*np.pi/18.0) and theta < (13.*np.pi/18.0)): #水平直线 
                        num +=1
                        pt1 = (0,int(rho/np.sin(theta))) # 该直线与第一列的交点
                        y = intersection(medle_line, line)[0][1]
                        #该直线与最后一列的交点 
                        pt2 = (result.shape[1], int((rho-result.shape[1]*np.cos(theta))/np.sin(theta))) 
                        #cv2.line(byproduction, pt1, pt2, (255), 1)
    #                     if (int(rho/np.sin(theta))> max_horizontal):
    #                         max_horizontal = int(rho/np.sin(theta))
    #                         max_hindex = j
    #                     if (int(rho/np.sin(theta))< min_horizontal):
    #                         min_horizontal = int(rho/np.sin(theta))
    #                         min_hindex = j
                        if y>max_horizontal:
                            max_horizontal = y
                            max_hindex = j
                        elif y< min_horizontal:
                            min_horizontal = y
                            min_hindex = j                   
        print(min_hindex,max_hindex,min_vindex, max_vindex)
        if(min_hindex != 30000 and min_vindex != 30000 and max_hindex != -1 and max_vindex != -1):
            segmented = [[vertical_lines[max_vindex], vertical_lines[min_vindex]], [horizontal_lines[max_hindex], horizontal_lines[min_hindex]] ]
            intersections = segmented_intersections(segmented)
        
            if len(intersections) == 4:
                intersection0 = tuple(intersections[0][0])
                intersection1 = tuple(intersections[1][0])
                intersection2 = tuple(intersections[3][0])
                intersection3 = tuple(intersections[2][0])
                intersection_group = [intersection0,intersection1,intersection2,intersection3]
                points_group.append(intersection_group)
    print(points_group)
    return points_group


if __name__ == "__main__":
    v = facade()
    


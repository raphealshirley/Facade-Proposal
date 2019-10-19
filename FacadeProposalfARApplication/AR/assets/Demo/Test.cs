using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    float z;
    bool b;
    public void DrawOverlay()
    {
        //Helper.CreatePlane((float)1, (float)1);

        /*GameObject canvasGO = new GameObject();
        canvasGO.name = "Canvas";
        canvasGO.AddComponent<Canvas>();
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        // Get canvas from the GameObject.
        Canvas canvas;
        canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        // Create the Text GameObject.
        GameObject textGO = new GameObject();
        textGO.transform.parent = canvasGO.transform;
        textGO.AddComponent<SimpleImage>();
        SimpleImage simpleImage = textGO.GetComponent<SimpleImage>();
        simpleImage.CustomShapeSetup(new VertexHelper());*/

        //GameObject.Find("myui").GetComponent<SimpleImage>().CustomShapeSetup();


       //DecodePoints.SetZ(z);
        // QuadCreate.CreateQuad(114, 385, 909, 406, 139, 1851, 929, 2112, 3);
       // DecodePoints.DecodeAndCreate("");


    }

    public void OnValueChanged(float newValue)
    {
        z = newValue;
        Text myText;
        myText = GameObject.Find("distance").GetComponent<Text>();
        myText.text = "Distance: " + z;
        DecodePoints.SetZ(z);
    }

    public void ToggleChanged(bool newValue)
    {
        QuadCreate.realWorld = newValue;
    }

}

# Facade Proposal For Urban Area Version 1.0.0
A modified facade recognization system including AR application to adding advertisement overlay.

## Getting Started
### Pre-requirements
1. Facade Recognition: Python 3.5, Tensorflow, Opencv, MaskRCNN, matplotlib, yaml, PIL, skimage, datetime.
2. Tool: jupyter.
3. AR: Unity.

### Installing
1. Clone Repository
```console
git clone https://github.com/raphealshirley/Facade-Proposal.git
```
2. Run server: <br> 
Under FacadeProposalfARApplication/server. <br>
Noted that different platforms should be under same loacal area network and do not forget to change IP address in the file.<br>
```console
python3 ./TcpServer.py
```
3. Run ML model under Facade Proposal folder.
```console
python3 ./TcpServer.py
```

## Result
Modified MaskRCNN improved facade recognition recall by 11.8% compared with SegNet using Tensorflow.

## Built With
<li> Python 3.5 
<li> Tensorflow
<li> Opencv
<li> MaskRCNN
<li> matplotlib
<li> yaml
<li> PIL
<li> skimage
<li> Unity

## Versioning
### Version 1.0.0
Model training and testing on SageMaker; AR application; 
### Version 2.0.0
Design Web UI; Deployed on AWS.

## License
This system is licensed under MIT license.
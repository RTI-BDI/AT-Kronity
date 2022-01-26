# Kronity
Andrea Traldi / RTI-BDI Project 

## Setup guide

1. [Download & Install UnityHub for Linux](https://docs.unity3d.com/hub/manual/InstallHub.html#install-hub-linux), then perform Sign-in (SSO supported), get free personal activation license 

2. Go to the [UnityDownload Archive](https://unity3d.com/get-unity/download/archive) and find the "Unity Hub" green button corresponding to the version you're interested in. In our case it should be Unity 2021.1.4, hence the link attached to the corresponding button would be something like "unityhub://2021.1.4f1/4cd64a618c1b"; Retrieve it (use *Inspect element* in the browser and look for the `href` property) and copy it. 

3. Go to the location of your UnityHub.AppImage and type the following command in a terminal replacing it with the string version you copied from the previous point (in case it differs): `./UnityHub.AppImage unityhub://2021.1.4f1/4cd64a618c1b`. 
In some debian based distribution (e.g. Mint) you could look for the unityhub executable which you should find under `/opt/unityhub/unityhub`). 
Check in the pop-up the correct version of the installation you required, if it's another, press "Skip installation" and another pop-up dialog should appear with the correct version you ask for. Basic Editor App should suffice, for a more complete installation, check WebGL Build, Windows, Linux build support.  

4. Install Optic, which can be found [here](https://github.com/roveri-marco/optic). Put the generated `release` folder under `Tesi/Assets/optic-master/optic/` replacing the one already there. Requirements for the installation of optic: 
  ```
  sudo apt-get install cmake coinor-libcbc-dev coinor-libclp-dev coinor-libcoinutils-dev libbz2-dev bison flex
  sudo apt-get install libfl-dev gsl-bin libgsl-dev doxygen # could be needed... just run it to make sure
  ``` 

5. Install OMNET & Kronosim. Follow instructions in the respective [repository](https://github.com/RTI-BDI/Kronosim-clone)
 
6. Start OMNET, import and compile Kronosim: the folder you're interested in would be `backend/simulator/kronosim`. If build successful, from the same folder, open a terminal and you should be able to start kronosim with the following command:
 ```
./kronosim -u Cmdenv -c General omnetpp.ini
  ```     
7. Open the Kronity project in Unity 2021.1.4, select the main scene and press play. 

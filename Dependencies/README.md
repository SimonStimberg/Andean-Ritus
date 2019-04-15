# Dependencies
To run the project certain packages, externals and configurations are needed to be downloaded,  installed and set.  


MAX/MSP:
1. install the CNMAT Externals (most convenient through the max package manager)
2. from the Dependencies folder copy the java-class *DirectionAndDistanceHandler.class* into the java-classes/classes folder and the *vecmath-1.5.1.jar* into the java-classes/lib folder  
    (on Mac you find the java-classes folder in the Package Contents of your Max application: /Applications/Max.app/Contents/Resources/C74/packages/max-mxj)
3. to enable Max to find all the subpatchers and assets files you have to add the project folcer to the Max search path (options -> file preferences. let it search the subfoldes as well)
4. to work well with the binaural spatializer you have to adjust the sample rate to 44.1k and the Signal Vector Size to 1024 (under Audio settings)
5. to ensure that Max runs smoothly and without audio interuptions you will want also to turn on the Schedular Overdrive in the Max Preferences and set the Refresh Rate to 10ms  


UNITY  
**these files are not included in the Git repository because they are to big - they have to be copied manually**
1. copy the Rocks Pack Asset to your Asset folder inside the Unity project folder
2. copy the 360°-Videofile PebblyBeach360_processed.mp4 to the Video360 folder inside the Unity project folder

# LogFile Orientationproject: Andean Ritus (AT) #
<br>


### 13. December 2018 ###  
*Breakthrough: Audio Bridge working*    
Using **Jack Audio for Unity**
* reason not clear - maybe application start order crucial:
1. Start Jack Server
2. load Unity and start Game
3. Start Max -> Set Output Device to JackRouter
4. in JackRouter: disconnect Max from SystemPlayback and connect to Unity

<hr>
<br>

### 5. December 2018 ###   
*Trying to implement Audio Bridge between Max and Unity*  
Several attempts failed:  


<br>  

**Unity Native SDK**  
https://docs.unity3d.com/Manual/AudioMixerNativeAudioPlugin.html  
https://bitbucket.org/Unity-Technologies/nativeaudioplugins  
https://japan.unity3d.com/unite/unite2015/files/DAY1_1900_room1_Jan.pdf  
* has a "Teleport" script which should receive External Sounds
* Demo with command line script works (written in C++)
* has a AU-Plugin for DAWs (-> works only on MAC / even on Mac didnt showed up under AU-Plugins)

<br>  
<br>

**Jack Audio Connection Kit**  
http://jackaudio.org/downloads/  
http://jackaudio.org/faq/jack_on_windows.html  
* enables to route freely different Applications Audio Inns + Outs  
* requires Admin Rights for installation (under Windows DLLs must be registered manually)  

<br>


to connect JACK with Unity  :  
**Jack4U**  
https://assetstore.unity.com/packages/tools/audio/jack4u-19291
* route Audio from DAW (via ASIO) to Unity
* costs 27 Euro  

**Jack Audio for Unity**  
https://github.com/rodrigodzf/Jack-Audio-For-Unity
* Open Source / frree  
* worked on Mac partly  
* Connection kind of possible but only Error glitchy/distorted Audio recieved  
-> TO BE REVISED  

  
<br>  
<br>  
   

*Miscellaneous*  
The Conductor  
OCS - Communication (mainly to have a vitual interface in Unity to control parameters in Max/Live  
https://github.com/cgfarmer4/TheConductor  

Another OSC-Bridge  
http://www.monoflow.org/unity3d-assets/uniosc/
<hr>
<br>


### 23. November 2018 ###  
*Setting up OSC-communication between Max/MSP and Unity:*    
* MaxMSP: installed the 'CNMAT Externals' package via the package manager. (no admin rights required)  
Communication runs via the Max-Object 'OpenSoundControl'
* Unity: worked with PerilNoise sample project immediately    
  
Resources:  
https://www.uni-weimar.de/kunst-und-gestaltung/wiki/GMU:Tutorials/Visual_Interaction/How_to_Control_Unity_with_MaxMsp
<hr>

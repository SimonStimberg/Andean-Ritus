# LogFile Orientationproject: Andean Ritus (AT) #
<br>


### 27. February 2019 ###  

brainstormed about musical concept  
* algorithmic/serial composition 
* no classic electronic music elements (such as bass/pad/lead..)
  * rather multiple sound events, that are synced to a pulse (timegrid) which form through theri interplay, musical patterns
    * -> twelve tone music?
* but still ritual/hypnotic/trance inducing feeling
  * techno-loops, sequencing / westafrican ritual music  

**Interesting Inspirations**   

Step Sequencer in Max/MSP  
https://audiocookbook.org/step-sequencer-built-in-maxmsp/  
  
Celular Automaton (e.g. Conways Game of Life)  
https://en.wikipedia.org/wiki/Conway's_Game_of_Life  
  
Asked Mischa to maybe participate in "composing" the music.


<hr>
<br>


### 19. February 2019 ###  
* played around with max
  * there is a "analogue drum computer" entirely synthesized in max - sounds nice - every aspect is possible to manipulate
  * tryed to setup a pulse/metronome - to generate some kind of sequencer - in the end a prefab would work better
  
* implemented VomHauptplatz Mouseview script - works better!

* one thought for visual effects:
  * more/bigger mirrors in the scene then rotate the world -> psychedelic mirroring effect

* another thought - if there were a video buffer to write in max and display in unity, would be fancy
  * but if its possibly at all would be to complicated

* a possible solution to download equirectangular 360°-Videos from YouTube instead of their cubemap format:
https://github.com/rg3/youtube-dl/issues/15267
  * needs youtube-dl - best to install maybe homebrew first..
  
* concluded to just start doing something - rather than thinking how it could look or sound and beeing afraid from starting the wrong way


<hr>
<br>


### 29. January 2019 ###  
* found a free 3 month licence for MaxMSP yay!

* played around with the seaboard - understood conventions and constraints of the midi protocol and the signal flow in Max/MSP  (one has to prepend a 144 before the midi notes - dont know why yet--)
https://cycling74.com/forums/sending-midi-notes-to-vst-synth

* found out how to handle the multidimensional polyphonic expression (MPE), transmitted by the seaboard:
https://rolisupport.freshdesk.com/support/solutions/articles/36000024621-max-using-blocks-with-max
https://rolisupport.freshdesk.com/support/solutions/articles/36000027933-what-is-mpe-

* tried to access the post processing behaviour in unity via a script, but realized that its far more complicated

<hr>
<br>


### 22. January 2019 ###  
* added a better mirror material through a script and a shader after this tutorial:  
  https://www.youtube.com/watch?v=jD9LfpRH5Z4  
  script resource:  
  http://wiki.unity3d.com/index.php/MirrorReflection4  
  
  Mirror Material and script have to be put both on an object to be reflective  
  seems that it has to be attached to a plane to work the best way. (rather then volumetric shapes)  
  
* added a simple and working FreeFly Camera Script  
  possible now to look around with the mouse and move via the WASD keys  
  resource:  
  https://gist.github.com/gunderson/d7f096bd07874f31671306318019d996  
  (resp. used a modified version by Ryan Breaker found in the comments below)  
  
* played around with post-processing profile > trippy results  
  (e.g. motion blur: increase frame blending / chrom aberration: up to max / color grading: hue shifting around)  
  
  found some resources in order to automate the behaviour.  
  unity-manual:  
  https://github.com/Unity-Technologies/PostProcessing/wiki/Manipulating-the-Stack  
  a promising forum post:  
  https://answers.unity.com/questions/1355103/modifying-the-new-post-processing-stack-through-co.html  
  
  got to be revised next time!  
  
  


<hr>
<br>

### 13. January 2019 ###  
installed Unity at Workstation at home.  
(Version 2017.4.18f because 2018 requires at least OSX 10.11) 

* Added Video-Test-World project.  
* Wrote a script that slightly transforms the World-Sphere (with 360°-Video)
  and moves the ground subtle -> psychedelic effect!
  
Also installed Max 7.3.6  
(Max 8 also doesn't run on OSX 10.10)  


<hr>
<br>

### 13. December 2018 ###  
*Breakthrough: Audio Bridge WORKING*    
Using **Jack Audio for Unity**:
* reason not clear - maybe application start order crucial:
  1. Start Jack Server
  2. load Unity and start Game
  3. Start Max -> Set Output Device to JackRouter
  4. in JackRouter: disconnect Max from SystemPlayback and connect to Unity
* BufferSize and SampleRate must be identical in all three parties  
<br>
  
Setup in Unity:
* one Instance of the *Jack Multiplexer* must be set on the Camera/Audio-Listener Object  
  there define the number of channels
* create Object:  
  add Audio Source component, tick Loop on  
  add Jack Source Receive script, connect Multiplexer to the Camera-object where the multiplexer resides, choose input channel (0, 1, 2, 3, ...)  
  <br>
  
![alt text](JackAudio-Prefs.png "Preferences")

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

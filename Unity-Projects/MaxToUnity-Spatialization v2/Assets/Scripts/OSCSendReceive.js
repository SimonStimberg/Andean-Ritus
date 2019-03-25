private var oscHandler: Osc = null;
public var remoteIp : String = "127.0.0.1";
public var sendToPort : int = 9000;
public var listenerPort : int = 8000;


private var valX : float;
private var valY : float;
private var valZ : float;
private var val2X : float;
private var val2Y : float;
private var val2Z : float;

public var gameReceiver = "Ball02"; //the tag of the object on stage that you want to manipulate
public var gameReceiver2 = "Box";

private var obj01 : GameObject;
private var obj02 : GameObject;
private var m_MainCamera : Camera;
private var cameraPos : String;


// Start is called just before any of the Update methods is called the first time.
public function Start()
{

    var udp:UDPPacketIO  = GetComponent("UDPPacketIO");
    udp.init(remoteIp, sendToPort, listenerPort);

    oscHandler = GetComponent("Osc");
    oscHandler.init(udp);

    oscHandler.SetAllMessageHandler(AllMessageHandler);

    obj01 = GameObject.Find("Ball02");
    obj01.GetComponent(MeshRenderer).enabled = false;

    obj02 = GameObject.Find("Box");
    obj02.GetComponent(MeshRenderer).enabled = false;
    m_MainCamera = Camera.main;
}



// Update is called every frame, if the MonoBehaviour is enabled.
function Update()
{
    //Debug.LogWarning("time = " + Time.time);

    var oscM : OscMessage = null;
    if(valX != 0) 
    {
        obj01.GetComponent(MeshRenderer).enabled = true;
        obj01.transform.localPosition = new Vector3(valX,valY,valZ);
    }
    else
    {
        obj01.GetComponent(MeshRenderer).enabled = false;
    }

    if(val2X != 0) 
    {
        obj02.GetComponent(MeshRenderer).enabled = true;
        obj02.transform.localPosition = new Vector3(val2X,val2Y,val2Z);
    }
    else
    {
        obj02.GetComponent(MeshRenderer).enabled = false;
    }
    

    // if (Input.GetKey(KeyCode.Q))
    // {
        cameraPos = "/mePos " + m_MainCamera.transform.position.x + " " + m_MainCamera.transform.position.z + " " + m_MainCamera.transform.position.y + " " + m_MainCamera.transform.rotation.w + " " + m_MainCamera.transform.rotation.x + " " + m_MainCamera.transform.rotation.y + " " + m_MainCamera.transform.rotation.y;;
        cameraRot = m_MainCamera.transform.rotation.w + " " + m_MainCamera.transform.rotation.x + " " + m_MainCamera.transform.rotation.y + " " + m_MainCamera.transform.rotation.y;

        oscM = Osc.StringToOscMessage(cameraPos);
        oscHandler.Send(oscM);
               
        // Debug.Log(cameraPos);
        // Debug.Log(cameraRot);
    // } 


}



public function AllMessageHandler(msg: OscMessage){

    // log the OSC message
    // Debug.Log(osc.OscMessageToString(msg));

    // message parameters
    var address = msg.Address;
    var values = msg.Values;

    
    // different actions, based on the address pattern
    switch (address){

        // FORMAT:  /cursor id group_id x y z x_world y_world z_world 
        case "/obj1Pos":

            // extract the data
            
            valX = values[0];
            // valX -= 5.0f;

            valZ = values[1];
            // valZ -= 5.0f;

            valY = values[2];

            // log the data

            Debug.Log(
                  "Object1: \nX = " + valX + "  Y = " + valY + "  Z = " + valZ
            );

            break;
        
        
        case "/obj2Pos":

            // extract the data
            
            val2X = values[0];
            // valX -= 5.0f;

            val2Z = values[1];
            // valZ -= 5.0f;

            val2Y = values[2];

            // log the data

            Debug.Log(
                  "Object2: \nX = " + val2X + "  Y = " + val2Y + "  Z = " + val2Z
            );

            break;

    }
}









function OnDisable()
{
    // close OSC UDP socket
    Debug.Log("closing OSC UDP socket in OnDisable");
    oscHandler.Cancel();
    oscHandler = null;
}



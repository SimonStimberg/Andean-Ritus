using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class OSCManager : MonoBehaviour {


	// OSC related stuff
	private Osc oscHandler;

	private UDPPacketIO udp;
	public string remoteIp = "127.0.0.1";
	public int sendToPort = 9000;
	public int listenerPort = 8000;


	//// variables to store values that are received from MAX
	// these global variables have to be used because 
	// the OSC plugin and its related classes are not running in the main thread
	// and therefore can't communicate with others scripts directly
	public int step = 0;
	public int drumTrigger = 0;

	// an array of four is used because of the four synths (can be played at the same time)
	public int[] target = new int[] {0, 0, 0, 0};
	public int[] pitch = new int[] {0, 0, 0, 0};
	public int[] velocity = new int[] {0, 0, 0, 0};

	private Camera m_MainCamera;




	void Start () {
		udp = GetComponent<UDPPacketIO>();
		udp.init(remoteIp, sendToPort, listenerPort);

		oscHandler = GetComponent<Osc>();
		oscHandler.init(udp);
		oscHandler.SetAllMessageHandler(AllMessageHandler);

		m_MainCamera = Camera.main;
	}
	

	void Update () {

		// if there is new data from MAX (tone is played) 
		// -> hand these values over to TriggerMaster script to stimulate the corresponding objects
		for(int i = 0; i < 4; i++)
		{
			if(target[i] != 0)
			{
				GameObject.Find("Trigger Master").GetComponent<TriggerMaster>().stimulate(target[i], pitch[i], velocity[i]);
				target[i] = 0;
			}
		}

		// do the same with percussion events
		// (distort the enviroment)
		if(drumTrigger == 36)
		{
			m_MainCamera.GetComponent<RipplePostProcessor>().shakeScreen();
			drumTrigger = 0;
		}
		else if (drumTrigger == 38 || drumTrigger == 42)
		{
			GameObject.Find("HalluCylinder").GetComponent<ShaderManipulator>().Distort();
			drumTrigger = 0;
		}

	}



	// send messages via OSC
	public void SendNewMessage(string adress, string newMsg)
	{
		newMsg = adress + " " + newMsg;

		OscMessage oscM;
		oscM = Osc.StringToOscMessage(newMsg);
        oscHandler.Send(oscM);
	}


	// receive messages via OSC
	public void AllMessageHandler(OscMessage msg)
	{

		string received = Osc.OscMessageToString(msg);
		string[] msgArray = received.Split(' ');


		// unpack the received data and write the values into the global variables so they can be processed further
		if(msgArray[0] == "/stimObj")
		{
			int synthNo = Convert.ToInt32(msgArray[4]) - 1;

			target[synthNo] = Convert.ToInt32(msgArray[1]);
			pitch[synthNo] = Convert.ToInt32(msgArray[2]);
			velocity[synthNo] = Convert.ToInt32(msgArray[3]);
		}

		if(msgArray[0] == "/drums")
		{
			drumTrigger = Convert.ToInt32(msgArray[1]);
			step = Convert.ToInt32(msgArray[2]);

		}
		
	}


	void OnDisable()
	{
		// close OSC UDP socket
		Debug.Log("closing OSC UDP socket in OnDisable");
		oscHandler.Cancel();
		oscHandler = null;
	}
}

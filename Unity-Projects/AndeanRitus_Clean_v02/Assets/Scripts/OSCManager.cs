using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
// using System.IO;
// using System.Collections;
// using System.Net;
// using System.Net.Sockets;
// using System.Threading;
// using System.Text;

// using Osc;
// using UDPPacketIO;


public class OSCManager : MonoBehaviour {


	private Osc oscHandler;

	private UDPPacketIO udp;
	public string remoteIp = "127.0.0.1";
	public int sendToPort = 9000;
	public int listenerPort = 8000;


	public int step = 0;
	public int drumTrigger = 0;

	public int[] target = new int[] {0, 0, 0, 0};
	public int[] pitch = new int[] {0, 0, 0, 0};
	public int[] velocity = new int[] {0, 0, 0, 0};

	private Camera m_MainCamera;
	// public int synthNo = 0;








	// Use this for initialization
	void Start () {
		udp = GetComponent<UDPPacketIO>();
		
		udp.init(remoteIp, sendToPort, listenerPort);



		oscHandler = GetComponent<Osc>();
		oscHandler.init(udp);

		oscHandler.SetAllMessageHandler(AllMessageHandler);

		m_MainCamera = Camera.main;
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.Q))
    	{
			stimulate(2, 50, 50);
			Debug.Log("stimulated");
		}

		for(int i = 0; i < 4; i++)
		{

			if(target[i] != 0)
			{
				GameObject.Find("Trigger Master").GetComponent<TriggerMaster>().stimulate(target[i], pitch[i], velocity[i]);
				target[i] = 0;

			}
		}

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

	// if (Input.GetKey(KeyCode.Q))
    // {
    //     string letter = "/newtest hallo";
	// 	OscMessage oscM;
    //     oscM = Osc.StringToOscMessage(letter);
    //     oscHandler.Send(oscM);
               
    //     // Debug.Log(cameraPos);        
    //     Debug.Log("testKey");
    // } 
		
	}

	public void SendNewMessage(string adress, string newMsg)
	{
		newMsg = adress + " " + newMsg;

		OscMessage oscM;
		oscM = Osc.StringToOscMessage(newMsg);
        oscHandler.Send(oscM);

		// Debug.Log(newMsg);

	}


	public void AllMessageHandler(OscMessage msg)
	{
		// log the OSC message
    	// Debug.Log(oscHandler.OscMessageToString(msg));
		// string address = msg.Address;
		// ArrayList values = msg.Values;
		// float[] valueArray =  values.ToArray();
		// string valueNew = values[0].ToString();
		// System.Convert.ToInt32(values[0]);
		string received = Osc.OscMessageToString(msg);
		string[] msgArray = received.Split(' ');

		if(msgArray[0] == "/stimObj")
		{
			int synthNo = Convert.ToInt32(msgArray[4]) - 1;

			target[synthNo] = Convert.ToInt32(msgArray[1]);
			pitch[synthNo] = Convert.ToInt32(msgArray[2]);
			velocity[synthNo] = Convert.ToInt32(msgArray[3]);

			

			// GameObject.Find("Trigger Master").GetComponent<TriggerMaster>().stimulate(target, pitch, velocity);

			// stimulate(target, pitch, velocity);

			// float noise = 7.0f;
			// GameObject.Find("MysticalSphere"+target).GetComponent<NoiseDeformer>().stimIntensity = noise;
		

			// Debug.Log("Stimulate No. " + target + " with " + pitch + " " + velocity);


		}

		if(msgArray[0] == "/drums")
		{
			drumTrigger = Convert.ToInt32(msgArray[1]);
			step = Convert.ToInt32(msgArray[2]);

		}
		
		
		// step = evenNewer;

		// Debug.Log(step);


	}

	public void stimulate(int target, int pitch, int vel)
	{
		float noise = 7.0f;
		// GameObject.Find("MysticalSphere"+target).GetComponent<NoiseDeformer>().stimIntensity = noise;
		GameObject.Find("Trigger Master").GetComponent<TriggerMaster>().stimulate(2, 50, 50);
		Debug.Log("Stimulate No. " + target + " with " + pitch + " " + vel);
	}


	void OnDisable()
	{
		// close OSC UDP socket
		Debug.Log("closing OSC UDP socket in OnDisable");
		oscHandler.Cancel();
		oscHandler = null;
	}
}

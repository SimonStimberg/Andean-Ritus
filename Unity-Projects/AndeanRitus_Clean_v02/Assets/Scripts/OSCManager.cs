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


	public int step = 5;
	public int target = 0;
	public int pitch = 0;
	public int velocity = 0;






	// Use this for initialization
	void Start () {
		udp = GetComponent<UDPPacketIO>();
		
		udp.init(remoteIp, sendToPort, listenerPort);



		oscHandler = GetComponent<Osc>();
		oscHandler.init(udp);

		oscHandler.SetAllMessageHandler(AllMessageHandler);
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.Q))
    	{
			stimulate(2, 50, 50);
			Debug.Log("stimulated");
		}

		if(target != 0)
		{
			GameObject.Find("Trigger Master").GetComponent<TriggerMaster>().stimulate(target, pitch, velocity);
			target = 0;

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
			target = Convert.ToInt32(msgArray[1]);
			pitch = Convert.ToInt32(msgArray[2]);
			velocity = Convert.ToInt32(msgArray[3]);

			// GameObject.Find("Trigger Master").GetComponent<TriggerMaster>().stimulate(target, pitch, velocity);

			// stimulate(target, pitch, velocity);

			// float noise = 7.0f;
			// GameObject.Find("MysticalSphere"+target).GetComponent<NoiseDeformer>().stimIntensity = noise;
		

			// Debug.Log("Stimulate No. " + target + " with " + pitch + " " + velocity);


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

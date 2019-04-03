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
        string letter = "/newtest hallo";
		OscMessage oscM;
        oscM = Osc.StringToOscMessage(letter);
        oscHandler.Send(oscM);
               
        // Debug.Log(cameraPos);        
        Debug.Log("testKey");
    } 
		
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
		string newNew = Osc.OscMessageToString(msg);
		string[] newSplitted = newNew.Split(' ');
		int evenNewer = Convert.ToInt32(newSplitted[1]);

		
		step = evenNewer;

		Debug.Log(step);


	}


	void OnDisable()
{
    // close OSC UDP socket
    Debug.Log("closing OSC UDP socket in OnDisable");
    oscHandler.Cancel();
    oscHandler = null;
}
}

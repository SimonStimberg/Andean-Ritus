using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMaster : MonoBehaviour {


	public float triggerTime = 15.0f;
	private float timeCount;
	private bool triggerReady = false;
	private bool manualTrigger = false;

	private Camera m_MainCamera;

	public float spawnDistance = 2.0f;
	private int objCounter = 1;



	//Holds the previous frames rotation
    Quaternion lastRotation;
 
    //References to the relevent axis angle variables
    float magnitude;
    Vector3 axis;
 
    public Vector3 angularVelocity { 
        get {
            //DIVDED by Time.deltaTime to give you the degrees of rotation per axis per second
            return (axis * magnitude) / Time.deltaTime;
        }
    }

	float[] magArray = new float[] {1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f};
	int arrayCounter = 0;

	// float test;

	// Use this for initialization
	void Start () 
	{
		timeCount = triggerTime;
		m_MainCamera = Camera.main;

		lastRotation = m_MainCamera.transform.rotation;

		
	}
	
	// Update is called once per frame
	void Update () 
	{
		sendCameraPos();

		timeCount -= Time.deltaTime;
 
		if (timeCount <= 0.0f)
		{
			
			triggerReady = true;
			
		}


		//The fancy, relevent math
		//code found at https://forum.unity.com/threads/manually-calculate-angular-velocity-of-gameobject.289462/
        Quaternion deltaRotation = m_MainCamera.transform.rotation * Quaternion.Inverse (lastRotation);
        deltaRotation.ToAngleAxis(out magnitude, out axis);
        lastRotation = m_MainCamera.transform.rotation;

		// Debug.Log(magnitude);


		magArray[arrayCounter] = magnitude;
		arrayCounter++;
		if(arrayCounter == magArray.Length)
		{
			arrayCounter = 0;
		}
		
		float test = 0.0f;
		for (int i = 0; i < magArray.Length; i++)
		{
			test += magArray[i];
		}
		test = test / magArray.Length;
		
		// Debug.Log(test);

		
		if(triggerReady && test <= 0.5)
		{
			triggerNew();
			triggerReady = false;
			timeCount = triggerTime;
			// manualTrigger = false;

		}


		// Debug.Log(m_MainCamera.transform.eulerAngles.y);

		// if (Input.GetKey(KeyCode.Q))
    	// {
		// 	stimulate(2, 50, 50);
		// 	Debug.Log("stimulated");
		// }


		
	}

	void sendCameraPos()
	{
		string cameraPos = m_MainCamera.transform.position.x + " " + m_MainCamera.transform.position.z + " " + m_MainCamera.transform.position.y + " " + m_MainCamera.transform.rotation.w + " " + m_MainCamera.transform.rotation.x + " " + m_MainCamera.transform.rotation.y + " " + m_MainCamera.transform.rotation.y;
		GameObject.Find("OSC Receiver").GetComponent<OSCManager>().SendNewMessage("/mePos", cameraPos);


	}




	void triggerNew()
	{
		// m_MainCamera.transform.eulerAngles.y

		// GenerateNewPlanet myScript = (GenerateNewPlanet)target;
		float newAngle = m_MainCamera.transform.eulerAngles.y + 90.0f + Random.Range(0.0f, 180.0f);
		if(newAngle > 360)
		{
			newAngle -= 360;
		}
		// Debug.Log(m_MainCamera.transform.eulerAngles.y);
		
		float varyDistance = spawnDistance + Random.Range(0.0f, 5.0f);

		float newX = m_MainCamera.transform.position.x + varyDistance * Mathf.Sin(newAngle * Mathf.Deg2Rad);
		float newZ = m_MainCamera.transform.position.z + varyDistance * Mathf.Cos(newAngle * Mathf.Deg2Rad);
		float newY = Random.Range(2.5f, 8.0f);

		// Debug.Log(newX + " / " + newZ);

		this.GetComponent<GenerateNewPlanet>().CreatePlanet(objCounter, newX, newY, newZ);

		string newMsg = objCounter + " " + newX + " " + newZ + " " + newY;
		GameObject.Find("OSC Receiver").GetComponent<OSCManager>().SendNewMessage("/createObj", newMsg);

		objCounter++;

		Debug.Log("/createObj " + newMsg);



		// Debug.Log("You staring weirdo!");

	}

	public void stimulate(int target, int pitch, int vel)
	{
		float noise = 7.0f;
		GameObject.Find("MysticalSphere"+target).GetComponent<NoiseDeformer>().stimIntensity = noise;
		Debug.Log("Stimulate No. " + target + " with " + pitch + " " + vel);
	}


}

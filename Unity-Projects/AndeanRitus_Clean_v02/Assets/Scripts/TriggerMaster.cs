using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMaster : MonoBehaviour {


	public float triggerTime = 15.0f;
	private float timeCount;
	private bool triggerReady = false;
	private bool manualTrigger = false;

	public float spawnDistance = 2.0f;
	private int objCounter = 0;

	private float globalIntensity = 0f;
	private float distortion = 0f;
	private float colorIntensity = 0f;
	private float shrutiVel = 0f;

	[Range(0,100)]
	public float testIn = 0f;
	public float testPower = 2f;
	public float testOut = 0f;


	private Camera m_MainCamera;

	//Holds the previous frames rotation
    Quaternion lastRotation;
 
    //References to the relevent axis angle variables
	Vector3 axis;
    float magnitude;
    
	// intermediate Movement value
	float interMov;
 
    public Vector3 angularVelocity { 
        get {
            //DIVDED by Time.deltaTime to give you the degrees of rotation per axis per second
            return (axis * magnitude) / Time.deltaTime;
        }
    }

	// the bigger the array the bigger the movement buffer
	float[] magArray = new float[128];
	int arrayCounter = 0;


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
		calcAngularVel();
		increaseIntensity();

		
		timeCount -= Time.deltaTime;
 
		if (timeCount <= 0.0f)
		{	
			triggerReady = true;			
		}
		
		if (triggerReady && interMov <= 0.5 && objCounter < 64)
		{
			triggerNew();
			
			triggerReady = false;
			timeCount = triggerTime;
			// manualTrigger = false;
		}

		testOut = scaleMeExp(testIn, 0f, 100f, 0f, 100f, testPower);

		


		// Debug.Log(m_MainCamera.transform.rotation.x);
		
	}

	void sendCameraPos()
	{
		string cameraPos = m_MainCamera.transform.position.x + " " + m_MainCamera.transform.position.z + " " + m_MainCamera.transform.position.y + " " + m_MainCamera.transform.rotation.w + " " + m_MainCamera.transform.rotation.x + " " + m_MainCamera.transform.rotation.z + " " + m_MainCamera.transform.rotation.y + " " + magnitude;
		GameObject.Find("OSC Receiver").GetComponent<OSCManager>().SendNewMessage("/mePos", cameraPos);


	}

	void calcAngularVel()
	{

		//The fancy, relevent math
		//code found at https://forum.unity.com/threads/manually-calculate-angular-velocity-of-gameobject.289462/
        Quaternion deltaRotation = m_MainCamera.transform.rotation * Quaternion.Inverse (lastRotation);
        deltaRotation.ToAngleAxis(out magnitude, out axis);
        lastRotation = m_MainCamera.transform.rotation;

		// Debug.Log(magnitude);

		// calculate the average magnitude (amount of rotation movement) over time
		magArray[arrayCounter] = magnitude;
		arrayCounter++;
		if(arrayCounter == magArray.Length)
		{
			arrayCounter = 0;
		}
		
		interMov = 0.0f;
		for (int i = 0; i < magArray.Length; i++)
		{
			interMov += magArray[i];
		}
		interMov = interMov / magArray.Length;
		
		// Debug.Log(interMov);

	}



	void increaseIntensity()
	{
		float newIntenstiy = scaleMe((float)objCounter, 0f, 64f, 0f, 100f);
		float newIntenstiyExp = scaleMeExp((float)objCounter, 0f, 64f, 0f, 100f, 2f);
		float newColorInt = scaleMeExp((float)objCounter, 0f, 64f, 0f, 1f, 3f);
		

		globalIntensity = Mathf.Lerp(globalIntensity, newIntenstiy, 0.1f);
		distortion = Mathf.Lerp(distortion, newIntenstiyExp, 0.01f);
		colorIntensity = Mathf.Lerp(colorIntensity, newColorInt, 0.01f);



		if (globalIntensity != newIntenstiy)
		{
			string newMsg = globalIntensity + "";
			GameObject.Find("OSC Receiver").GetComponent<OSCManager>().SendNewMessage("/increase", newMsg);
			// Debug.Log(globalIntensity);
		}

		GameObject.Find("HalluCylinder").GetComponent<ShaderManipulator>().Strength = distortion * 0.005f;
		m_MainCamera.GetComponent<RipplePostProcessor>().MaxAmount = distortion * 0.15f;
		GameObject.Find("PP-Volume").GetComponent<PostProDynamic>().weight = colorIntensity;

		
		// Debug.Log(newIntenstiy);

			


	}




	void triggerNew()
	{

		objCounter++;

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



		Debug.Log("/createObj " + newMsg);

		
	}

	public void stimulate(int target, int pit, int vel)
	{
		float noise = 7.0f;
		float pitch = (float)pit;
		float velocity = (float)vel;

		float speed = scaleMe(pitch, 26.0f, 74.0f, 0.5f, 1.5f);
		float viscosity = scaleMe(pitch, 26.0f, 74.0f, 0.02f, 0.07f);
		float size = scaleMe(pitch, 26.0f, 74.0f, 2.0f, 0.5f);
		float intensity = scaleMe(velocity, 0.0f, 127.0f, 0.3f, 5.0f);


		GameObject.Find("MysticalSphere"+target).GetComponent<NoiseDeformer>().stimIntensity = intensity;
		GameObject.Find("MysticalSphere"+target).GetComponent<NoiseDeformer>().speed = speed;
		GameObject.Find("MysticalSphere"+target).GetComponent<NoiseDeformer>().viscosity = viscosity;
		GameObject.Find("MysticalSphere"+target).GetComponent<NoiseDeformer>().size = size;
		Debug.Log("Stimulate No. " + target + " with speed " + speed + " / viscosity " + viscosity);
	}



	private float scaleMe(float OldValue, float OldMin, float OldMax, float NewMin, float NewMax){
     
        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;
     
        return(NewValue);
    }

	private float scaleMeExp(float value, float start1, float stop1, float start2, float stop2, float power) {
		float inT = scaleMe(value, start1, stop1, 0, 1);
		float outT = Mathf.Pow(inT, power);
		return scaleMe(outT, 0, 1, start2, stop2);
	}





}

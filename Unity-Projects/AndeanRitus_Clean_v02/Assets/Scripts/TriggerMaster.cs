using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerMaster : MonoBehaviour {


	public float triggerTimeMin = 4.0f;
	public float triggerTimeMax = 8.0f;
	private float initTimeMin, initTimeMax;
	public float timeCount;
	private bool triggerReady = false;
	private bool start = false;
	private bool triggerCollapse = false;
	private bool peak = false;

	public float spawnDistance = 2.0f;
	private int objCounter = 0;

	private float globalIntensity = 0f;
	private float distortion = 0f;
	private float colorIntensity = 0f;
	

	// [Range(0,100)]
	// public float testIn = 0f;
	// public float testPower = 2f;
	// public float testOut = 0f;


	private Camera m_MainCamera;
	private GameObject m_PPVolume;
	private GameObject m_OSCManager;
	private GameObject m_HalluCylinder;

	//Holds the previous frames rotation
    Quaternion lastRotation;
 
    //References to the relevent axis angle variables
	Vector3 axis;
    float magnitude;
    
	// intermediate Movement value
	float interMov;

	float medianMov;

	float scapeVolume = 0f;
	float targetVolume = 0f;
	bool fader = false;






    public Vector3 angularVelocity { 
        get {
            //DIVDED by Time.deltaTime to give you the degrees of rotation per axis per second
            return (axis * magnitude) / Time.deltaTime;
        }
    }

	// the bigger the array the bigger the movement buffer
	float[] magArray = new float[64];
	int arrayCounter = 0;

	public bool debug = false;
	public Text debugUI;
	string debugTxt;


	// Use this for initialization
	void Start () 
	{
		timeCount = triggerTimeMax;
		m_MainCamera = Camera.main;
		m_PPVolume = GameObject.Find("PP-Volume");
		m_OSCManager = GameObject.Find("OSC Receiver");
		m_HalluCylinder = GameObject.Find("HalluCylinder");

		lastRotation = m_MainCamera.transform.rotation;

		// debugUI.text = "null";

		initTimeMin = triggerTimeMin;
		initTimeMax = triggerTimeMax;

		
	}
	


	// Update is called once per frame
	void Update () 
	{
		sendCameraPos();
		calcAngularVel();

		if(triggerCollapse)
		{
			collapse();
		}
		else if(!peak)
		{
			increaseIntensity();
		}
		


		if (fader) 
		{
			fadeSoundscape();
		}
		

		if (start)
		{
			timeCount -= Time.deltaTime;
		}
 
		if (timeCount <= triggerTimeMax-triggerTimeMin)
		{	
			triggerReady = true;			
		}
		

		if ((triggerReady && medianMov < 10f) || timeCount <= 0f)
		{
			if (peak)
			{
				Debug.Log("Apocalypse Now!");
				triggerCollapse = true;
				m_OSCManager.GetComponent<OSCManager>().SendNewMessage("/control", "0 0");
				scapeVolume = 1.2f;
				fader = true;
				peak = false;
			}
			if (objCounter < 64)
			{
				triggerNew();
				triggerReady = false;
				timeCount = triggerTimeMax;
			}
			if (objCounter == 64 && !triggerCollapse)
			{
				triggerTimeMax = 7f;
				triggerTimeMin = 7f;
				timeCount = triggerTimeMax;
				triggerReady = false;
				peak = true;
				Debug.Log(triggerTimeMax + " secs to blow!");
			}

		}


		// testOut = scaleMeExp(testIn, 0f, 100f, 0f, 100f, testPower);

		if (Input.GetKeyDown(KeyCode.Return))
        {
			start = true;
			m_OSCManager.GetComponent<OSCManager>().SendNewMessage("/control", "1 1");
		}
		if (Input.GetKeyDown(KeyCode.Escape))
        {
			start = false;
			m_OSCManager.GetComponent<OSCManager>().SendNewMessage("/control", "0 0");
		}
		if (Input.GetKeyDown(KeyCode.Space))
        {
			if (targetVolume != 1f)
			{
				targetVolume = 1f;
			}
			else
			{
				targetVolume = 0f;
			}
			fader = true;			
		}

		


		// Debug.Log(m_MainCamera.transform.rotation.x);
		if(debug)
		{
			debugUI.enabled = true;
			debugUI.text = "Mmov: " + medianMov + "\nObj: " + debugTxt;
		}
		else
		{
			debugUI.enabled = false;
			// debugUI.SetActive(false);
		}
		
		// "AVel: " + angularVelocity.magnitude + 
		
	}

	void sendCameraPos()
	{
		string cameraPos = m_MainCamera.transform.position.x + " " + m_MainCamera.transform.position.z + " " + m_MainCamera.transform.position.y + " " + m_MainCamera.transform.rotation.w + " " + m_MainCamera.transform.rotation.x + " " + m_MainCamera.transform.rotation.z + " " + m_MainCamera.transform.rotation.y + " " + angularVelocity.magnitude;
		m_OSCManager.GetComponent<OSCManager>().SendNewMessage("/mePos", cameraPos);


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
		magArray[arrayCounter] = angularVelocity.magnitude;
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


		float[] sortedPNumbers = (float[])magArray.Clone();
        System.Array.Sort(sortedPNumbers);
		if(sortedPNumbers.Length % 2 != 0)
		{
			int i = (sortedPNumbers.Length + 1 ) /2;
			medianMov = sortedPNumbers[i];
		}
		else
		{
			int i = sortedPNumbers.Length / 2;
			medianMov = (sortedPNumbers[i] + sortedPNumbers[i+1]) / 2;
		}
		
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
			m_OSCManager.GetComponent<OSCManager>().SendNewMessage("/increase", newMsg);
			// Debug.Log(globalIntensity);
		}

		m_HalluCylinder.GetComponent<ShaderManipulator>().Strength = distortion * 0.007f;
		m_MainCamera.GetComponent<RipplePostProcessor>().MaxAmount = distortion * 0.14f;
		m_MainCamera.GetComponent<RipplePostProcessor>().Friction = 0.9f - distortion * 0.0005f;
		m_PPVolume.GetComponent<PostProDynamic>().weight = colorIntensity;
		this.GetComponent<worldmorph>().factor = 1f + distortion * 0.06f;
		this.GetComponent<worldmorph>().shift = distortion * 0.25f;

		// shorten spawn time according to intensity
		// full intensity = half the time
		float targetTmax = initTimeMax - (initTimeMax * 0.5f) * (newIntenstiy * 0.01f);
		triggerTimeMax = Mathf.Lerp(triggerTimeMax, targetTmax, 0.1f);

		float targetTmin = initTimeMin - (initTimeMin * 0.5f) * (newIntenstiy * 0.01f);
		triggerTimeMin = Mathf.Lerp(triggerTimeMin, targetTmin, 0.1f);


		 

		
		// Debug.Log(newIntenstiy);

	}

	void collapse()
	{
		this.GetComponent<worldmorph>().factor = Mathf.Lerp(this.GetComponent<worldmorph>().factor, 1f, 0.1f);
		this.GetComponent<worldmorph>().shift = Mathf.Lerp(this.GetComponent<worldmorph>().shift, 0f, 0.1f);
		m_PPVolume.GetComponent<PostProDynamic>().weight = Mathf.Lerp(m_PPVolume.GetComponent<PostProDynamic>().weight, 0f, 0.1f);

		m_HalluCylinder.GetComponent<ShaderManipulator>().Strength = 0f;
		m_MainCamera.GetComponent<RipplePostProcessor>().MaxAmount = 0f;
		m_MainCamera.GetComponent<RipplePostProcessor>().Friction = 0.9f;

		float checkSum = this.GetComponent<worldmorph>().factor + this.GetComponent<worldmorph>().shift + m_PPVolume.GetComponent<PostProDynamic>().weight;
		for(int i = 1; i <= 64; i++)
		{
			if(GameObject.Find("MysticalSphere"+i) != null)
			{
				Vector3 current = GameObject.Find("MysticalSphere"+i).transform.localScale;
				current.x = Mathf.Lerp(current.x, 0f, 0.1f);
				GameObject.Find("MysticalSphere"+i).transform.localScale = new Vector3(current.x, current.x, current.x);

				checkSum += current.x;
				// if(current.x == 0f)
				// {
				// 	Destroy(GameObject.Find("MysticalSphere"+i));
				// }
			}
		}
		
		// Debug.Log("check sum: " + checkSum);
		if (checkSum <= 1.1f)
		{
			totalReset();
		}

	}

	void totalReset()
	{
		for(int i = 1; i <= objCounter; i++)
		{
			Destroy(GameObject.Find("MysticalSphere"+i));
		}
		
		objCounter = 0;
		triggerCollapse = false;
		start = false;
		
		triggerTimeMax = initTimeMax;
		triggerTimeMin = initTimeMin;
		timeCount = triggerTimeMax;
		triggerReady = false;

		globalIntensity = 0f;
		distortion = 0f;
		colorIntensity = 0f;

		m_OSCManager.GetComponent<OSCManager>().SendNewMessage("/control", "1 x 1.");

		Debug.Log("RESET COMPLETE");


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
		float newY = Random.Range(2.5f, 12.0f);

		// Debug.Log(newX + " / " + newZ);

		this.GetComponent<GenerateNewPlanet>().CreatePlanet(objCounter, newX, newY, newZ);

		string newMsg = objCounter + " " + newX + " " + newZ + " " + newY;
		m_OSCManager.GetComponent<OSCManager>().SendNewMessage("/createObj", newMsg);



		Debug.Log("/createObj " + newMsg);
		debugTxt = newMsg;

		
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
		// Debug.Log("Stimulate No. " + target + " with speed " + speed + " / viscosity " + viscosity);
	}



	void fadeSoundscape()
	{
		scapeVolume = Mathf.Lerp(scapeVolume, targetVolume, 0.02f);
		m_OSCManager.GetComponent<OSCManager>().SendNewMessage("/control", "0 x " + scapeVolume);

		if (scapeVolume > 0.99f && scapeVolume < 1.01f || targetVolume == 0f && scapeVolume < 0.01f) 
		{
			fader = false;
		}
		// Debug.Log(scapeVolume);
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

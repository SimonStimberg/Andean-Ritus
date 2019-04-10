using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerMaster : MonoBehaviour {


	public float triggerTimeMin = 4.0f;
	public float triggerTimeMax = 8.0f;
	private float timeCount;
	private bool triggerReady = false;
	private bool start = false;
	private bool triggerCollapse = false;

	public float spawnDistance = 2.0f;
	private int objCounter = 0;

	private float globalIntensity = 0f;
	private float distortion = 0f;
	private float colorIntensity = 0f;
	private float shrutiVel = 0f;

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






    public Vector3 angularVelocity { 
        get {
            //DIVDED by Time.deltaTime to give you the degrees of rotation per axis per second
            return (axis * magnitude) / Time.deltaTime;
        }
    }

	// the bigger the array the bigger the movement buffer
	float[] magArray = new float[64];
	int arrayCounter = 0;


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

		
	}
	
	// Update is called once per frame
	void Update () 
	{
		sendCameraPos();
		calcAngularVel();
		if(!triggerCollapse)
		{
			increaseIntensity();
		}
		else
		{
			collapse();
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
			if (objCounter < 64)
			{
				triggerNew();
				triggerReady = false;
				timeCount = triggerTimeMax;
			}
			if (objCounter == 64)
			{
				triggerCollapse = true;
				m_OSCManager.GetComponent<OSCManager>().SendNewMessage("/control", "1 1");
			}
			
			

			// manualTrigger = false;
		}

		// testOut = scaleMeExp(testIn, 0f, 100f, 0f, 100f, testPower);

		if (Input.GetKeyDown(KeyCode.Return))
        {
			start = true;
			m_OSCManager.GetComponent<OSCManager>().SendNewMessage("/control", "1 1");

		}

		


		// Debug.Log(m_MainCamera.transform.rotation.x);

		debugUI.text = "Mmov: " + medianMov + "\nObj: " + debugTxt;
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


		for(int i = 1; i <= 64; i++)
		{
			if(GameObject.Find("MysticalSphere"+i) != null)
			{
				Vector3 current = GameObject.Find("MysticalSphere"+i).transform.localScale;
				current.x = Mathf.Lerp(current.x, 0f, 0.1f);
				GameObject.Find("MysticalSphere"+i).transform.localScale = new Vector3(current.x, current.x, current.x);
				if(current.x == 0f)
				{
					Destroy(GameObject.Find("MysticalSphere"+i));
				}
			}
		}
		






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

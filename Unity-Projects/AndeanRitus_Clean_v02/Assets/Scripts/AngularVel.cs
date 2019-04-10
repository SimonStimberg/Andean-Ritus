using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AngularVel : MonoBehaviour {



	private Camera m_MainCamera;

	//Holds the previous frames rotation
    Quaternion lastRotation;
 
    //References to the relevent axis angle variables
	float magnitude;
    Vector3 axis;


	Vector3 angularVelo2;
	Vector3 angularVelo3;

	Quaternion previousRotation;
 
    public Vector3 angularVelocity { 
        get {
            //DIVDED by Time.deltaTime to give you the degrees of rotation per axis per second
            return (axis * magnitude) / Time.deltaTime;
        }
    }


		public Text debugUI;
	string debugTxt;

	// Use this for initialization
	void Start () {
		m_MainCamera = Camera.main;

		lastRotation = m_MainCamera.transform.rotation;
		previousRotation = m_MainCamera.transform.rotation;
		
	}
	
	// Update is called once per frame
	void Update () {
		// debugUI.text = "S1: " + angularVelocity.magnitude + "\nS1m: " + magnitude + "\nS3: " + angularVelo3.magnitude;

		
	}

	void FixedUpdate()
	{
		angFreq1();
		angFreq2();
		angFreq3();
		



	}

	void angFreq1()
	{
		//The fancy, relevent math
		//code found at https://forum.unity.com/threads/manually-calculate-angular-velocity-of-gameobject.289462/
        Quaternion deltaRotation = m_MainCamera.transform.rotation * Quaternion.Inverse (lastRotation);
        deltaRotation.ToAngleAxis(out magnitude, out axis);
        lastRotation = m_MainCamera.transform.rotation;


		float mag1 = angularVelocity.magnitude;




		Debug.Log("Script 1: " + magnitude);
		// Debug.Log("Script 1: " + angularVelocity);
		// Debug.Log("Script myMag: " + mag1);

	}

	void angFreq2()
	{
		Quaternion deltaRot = m_MainCamera.transform.rotation * Quaternion.Inverse( lastRotation );
		Vector3 eulerRot = new Vector3( Mathf.DeltaAngle( 0, deltaRot.eulerAngles.x ), Mathf.DeltaAngle( 0, deltaRot.eulerAngles.y ),Mathf.DeltaAngle( 0, deltaRot.eulerAngles.z ) );
		
		angularVelo2 = eulerRot / Time.fixedDeltaTime;



		// Debug.Log("Script 2: " + angularVelo2);

	}

	void angFreq3()
	{
		Quaternion deltaRotation = m_MainCamera.transform.rotation * Quaternion.Inverse(previousRotation);
		
		previousRotation = m_MainCamera.transform.rotation;
		
		float angle  = 0.0f;
		Vector3 axis = Vector3.zero;
		
		deltaRotation.ToAngleAxis(out angle, out axis);
		
		angle *= Mathf.Deg2Rad;
		
		angularVelo3 = axis * angle * (1.0f / Time.deltaTime);


		float mag3 = angularVelo3.magnitude;
		
		// Debug.Log("Script 3: " + angularVelo3);
		Debug.Log("Script 3: " + mag3);

	}
}

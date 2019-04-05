using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class worldmorph : MonoBehaviour {


	private float factor;
	private GameObject soil;
	private Vector3 myPostion;



	// Use this for initialization
	void Start () {
		factor = 0.005f;

		soil = GameObject.Find("Earth");
		
	}
	
	// Update is called once per frame
	void Update () {
		if (factor > 3.0) 
		{
			factor = -factor;			
		}
		if (factor < -3.0) 
		{
			factor = -factor;			
		}

		float timeFactor = (float)Time.realtimeSinceStartup * (0.1f/600);


		this.transform.localScale = new Vector3(this.transform.localScale.x + factor, this.transform.localScale.y - factor, this.transform.localScale.z);
		soil.transform.localPosition = new Vector3(soil.transform.localPosition.x + factor*0.1f,soil.transform.localPosition.y + factor*timeFactor, -100);

		// Debug.Log(Time.realtimeSinceStartup);

	}
}

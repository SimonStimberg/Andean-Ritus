using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class worldmorph1 : MonoBehaviour {


	public float MorphRatio;
	private float scalar;
	private float factor;
	private GameObject soil;
	private Vector3 myPostion;


	// Use this for initialization
	void Start () {
		factor = 0.005f;
		scalar = 0.0f;
		soil = GameObject.Find("Earth");
		myPostion = this.transform.localScale;
		Debug.Log(myPostion);
		
	}
	
	// Update is called once per frame
	void Update () {
		if (scalar > 3.0) 
		{
			factor = -factor;			
		}
		if (scalar < -3.0) 
		{
			factor = -factor;			
		}



		// this.transform.localScale = new Vector3(100 + scalar, 100 - scalar, 100);

		this.transform.localScale = new Vector3(100 + scalar, 100 - scalar, 100);
		soil.transform.localPosition = new Vector3(-100 + scalar*0.1f, 0.84f, -100);
		scalar += factor;
	}
}

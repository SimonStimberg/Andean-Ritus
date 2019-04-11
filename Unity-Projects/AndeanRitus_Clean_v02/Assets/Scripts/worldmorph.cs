using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class worldmorph : MonoBehaviour {


	public float factor;
	public float shift;

	private GameObject soil;
	private GameObject world;

	private Vector3 initPostion;
	private Vector3 initScale;

	



	// Use this for initialization
	void Start () {
		factor = 1f;
		shift = 0f;

		soil = GameObject.Find("Earth");
		world = GameObject.Find("World-Sphere");
		initScale = world.transform.localScale;
		initPostion = soil.transform.localPosition;
		
	}
	
	// Update is called once per frame
	void Update () {
		// if (factor > 3.0) 
		// {
		// 	factor = -factor;			
		// }
		// if (factor < -3.0) 
		// {
		// 	factor = -factor;			
		// }

		// float timeFactor = (float)Time.realtimeSinceStartup * (0.1f/600);


		world.transform.localScale = new Vector3(initScale.x * factor, initScale.y * (3/(2+factor)), initScale.z);
		soil.transform.localPosition = new Vector3(initPostion.x + shift*0.2f, initPostion.y + shift*0.03f, initPostion.z);

		// Debug.Log(Time.realtimeSinceStartup);

	}
}

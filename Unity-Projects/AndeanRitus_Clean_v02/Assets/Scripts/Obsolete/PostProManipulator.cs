using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProManipulator : MonoBehaviour {

	// properties of class
    public float bloom = 10f;
    public float saturation = 5f;
	Bloom            bloomLayer            = null;
    ColorGrading     colorGradingLayer     = null;

	// Use this for initialization
	void Start () {

		// somewhere during initializing
     	PostProcessVolume volume = gameObject.GetComponent<PostProcessVolume>();
		volume.profile.TryGetSettings(out bloomLayer);
		volume.profile.TryGetSettings(out colorGradingLayer);
		
	}
	
	// Update is called once per frame
	void Update () {

		bloomLayer.enabled.value = true;
		bloomLayer.intensity.value = bloom;
		
		colorGradingLayer.enabled.value = true;
		colorGradingLayer.saturation.value = saturation;
		
	}
}
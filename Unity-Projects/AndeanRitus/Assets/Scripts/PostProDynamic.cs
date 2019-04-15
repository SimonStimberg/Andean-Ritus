using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProDynamic : MonoBehaviour {


	PostProcessVolume volume = null;
	ColorGrading hueShifter = null;

	public float weight = 0.0f;	
	private float shifter = 0f;
	private float increment = 1f;


	void Start () 
	{
        hueShifter = ScriptableObject.CreateInstance<ColorGrading>();
        hueShifter.enabled.Override(true);
		hueShifter.saturation.Override(10f);
	
		volume = PostProcessManager.instance.QuickVolume(gameObject.layer, 100f, hueShifter);			
	}
	
	
	void Update () 
	{
		hueShifter.hueShift.Override(shifter);
				
		shifter += increment;		
		if (shifter >= 180f)
		{
			increment = -increment;
		}
		if (shifter <= -180f)
		{
			increment = -increment;
		}

		volume.weight = weight;		
	}
}
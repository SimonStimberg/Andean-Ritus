using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
// using DG.Tweening;

public class PostProDynamic : MonoBehaviour {


	public float weight = 0.0f;

	[Range(0, 1)]
	public float weightTest = 1f;
	
	PostProcessVolume volume = null;
	ColorGrading hueShifter = null;

		

	
	private float shifter = 0f;
	private float increment = 1f;
	// private GameObject oscr;
	// private OSCReceiver scripty;

	// Use this for initialization
	void Start () {
        hueShifter = ScriptableObject.CreateInstance<ColorGrading>();
        hueShifter.enabled.Override(true);
		hueShifter.saturation.Override(10f);

		// vignette = ScriptableObject.CreateInstance<Vignette>();
		// vignette.enabled.Override(true);
        // vignette.intensity.Override(1f);
        // hueShifter.intensity.Override(1f);

        // volume = PostProcessManager.instance.QuickVolume(gameObject.layer, 100f, vignette);
		volume = PostProcessManager.instance.QuickVolume(gameObject.layer, 100f, hueShifter);
		// oscr = GameObject.Find("OSC Receiver");
		// scripty = oscr.GetComponent(OSCReceiver);


        // DOTween.Sequence()
        //     .Append(DOTween.To(() => volume.weight, x => volume.weight = x, 1f, 1f))
        //     .AppendInterval(1f)
        //     .Append(DOTween.To(() => volume.weight, x => volume.weight = x, 0f, 1f))
        //     .OnComplete(() =>
        //     {
        //         // RuntimeUtilities.DestroyVolume(volume, true, true);
        //         // Destroy(this);
        //     });
		
	}
	
	// Update is called once per frame
	void Update () {

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

		// stepNow = GameObject.Find("OSC Receiver").GetComponent("OSCReceiver");
		// stepper = stepNow.step;

		// if(getStep == 1)
		// {
		// 	volume.weight = 1f;
		// }
		// else
		// {
		// 	volume.weight = 0f;
		// }
		
		
	}
}




using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using DG.Tweening;

public class PostProDynamic_BackUp : MonoBehaviour {


	public float useMe = 0.0f;
	public int getStep = 0;
	private PostProcessVolume volume;

	private Component stepNow;
	private int stepper = 0;
	// private GameObject oscr;
	// private OSCReceiver scripty;

	// Use this for initialization
	void Start () {
        var hueShifter = ScriptableObject.CreateInstance<ColorGrading>();
        hueShifter.enabled.Override(true);
        // hueShifter.intensity.Override(1f);

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

		// volume.ColorGrading.HueShift = -53f;

		// volume.weight = useMe;

		// stepNow = GameObject.Find("OSC Receiver").GetComponent("OSCReceiver");
		// stepper = stepNow.step;

		if(getStep == 1)
		{
			volume.weight = 1f;
		}
		else
		{
			volume.weight = 0f;
		}
		
		
	}
}




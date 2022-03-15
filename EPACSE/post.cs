using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
public class post : MonoBehaviour {
    public PostProcessingBehaviour beHavior;
    private VignetteModel.Settings VigModel;
    public static post Intance = null;
    public bool start = false;
    public bool canmove;
    float number = 0f;
	// Use this for initialization
	void Start () {
        Intance = this;
        VigModel.intensity = 3f;
        VigModel.center.x = 0.5f;
        VigModel.center.y = 0.5f;
        VigModel.roundness = 1f;
        VigModel.rounded = true;
        VigModel.smoothness = 1f;
        beHavior.profile.vignette.enabled = true;
        beHavior.profile.vignette.settings = VigModel;
	}
	
	// Update is called once per frame
	void Update () {
        number += 0.03f;
        if (beHavior.profile.vignette.enabled == true)
        {
            canmove = true;
            VigModel.intensity -= (0.1f + number) * Time.deltaTime;
            beHavior.profile.vignette.settings = VigModel;
        }
        if(VigModel.intensity <=0f)
        {
            VigModel.intensity = 0f;
            start = true;
        }
        //if(end1.Intance.start == true)
        //{
        //    beHavior.profile.vignette.enabled = false;
        //}

	}
}

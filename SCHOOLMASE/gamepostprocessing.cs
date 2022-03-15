using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
public class gamepostprocessing : MonoBehaviour {
    private PostProcessingBehaviour beHaviour;
    private VignetteModel.Settings VigModel;
    float number = 0;
    public GameObject player, ani,maze;
    // Use this for initialization
    void Start () {
        beHaviour = GetComponent<PostProcessingBehaviour>();
        beHaviour.profile.vignette.settings = VigModel;
        VigModel.intensity = 0f;
        VigModel.center.x = 0.5f;
        VigModel.center.y = 0.5f;
        VigModel.roundness = 1f;
        VigModel.rounded = true;
        VigModel.smoothness = 1f;
        beHaviour.profile.vignette.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (GameManager.instance.gamestage>=6)
        {
            timer.instance.start = false;
            maze.SetActive(false);
             number += 0.001f;
            beHaviour.profile.vignette.enabled = true;
			GameManager.instance.gamestart = false;
			timer.instance.start = false;
        }
        if (beHaviour.profile.vignette.enabled == true)
        {
            VigModel.intensity += (0.1f + number) * Time.deltaTime;
            beHaviour.profile.vignette.settings = VigModel;
            if (VigModel.intensity >= 0.87f)
            {
                
                GameManager.instance.gamestage = 0;
                ani.SetActive(true);
                player.SetActive(false);
                VigModel.intensity = 0;
                beHaviour.profile.vignette.enabled = false;
            }
        }
    }
}

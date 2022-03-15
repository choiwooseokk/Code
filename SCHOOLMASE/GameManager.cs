using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{

    public GameObject[] stage;
    public static GameManager instance;
    public int gamestage = 0;
    public GameObject camera, flash;
    public bool eye,start,gamestart;
    public int life = 3;
    public GameObject animation, player,ui,ending,time,eyeright,handright;
    // Use this for initialization
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
		if (gamestart) {
			timer.instance.start = true;
			time.SetActive(true);
			ui.SetActive(false);
			stage[gamestage].SetActive(true);
			laser.instance.start = false;

		}
        if (Input.GetKey(KeyCode.Alpha1))
            gamestage++;
            if (Input.GetKey(KeyCode.E))
        {
            animation.SetActive(false);
            player.SetActive(true);
        }
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene("test");
        }
        if (Input.GetKey(KeyCode.Q)) eye = true;
        if (Input.GetKey(KeyCode.W))
            eye = false;
        if (eye)
        {
			eyeright.SetActive (true);
            camera.GetComponent<laser>().enabled = true;
            flash.GetComponent<laser>().enabled = false;
			handright.SetActive (false);
        }
        else
        {
			eyeright.SetActive (false);
            camera.GetComponent<laser>().enabled = false;
            flash.GetComponent<laser>().enabled = true;
			handright.SetActive (true);
        }
    }
}

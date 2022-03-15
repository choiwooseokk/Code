using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pacman : MonoBehaviour
{
    private AudioSource audioplay;
    public AudioClip run,walk;
	public GameObject clock,st;
	private Animator ani;
    private Sound script;
    public static pacman instance;
    public float timer = 0f;
    public int point = 0;
    int Currentpoint = 0;
    public GameObject[] coin;
	private GameObject enemy;
    // Use this for initialization
    void Start()
    {
        audioplay = GetComponent<AudioSource>();
        script=GameObject.Find("SoundManage").GetComponent<Sound>();
        instance = this;
		ani = GetComponent<Animator> ();
    }

    // Update is called once per frame
    void Update()
    {
		enemy = GameObject.FindGameObjectWithTag("enemy");
		if (enemy != null) {
			Debug.Log (Vector3.Distance (this.gameObject.transform.position, enemy.transform.position));
			if (grademanager.instance.gamestart) {
                if (Vector3.Distance(this.gameObject.transform.position, enemy.transform.position) < 0.3f)
                {
                    ani.SetBool("running", true);
                    audioplay.clip = run;
                    Debug.Log(Vector3.Distance(this.gameObject.transform.position, enemy.transform.position));
                }
                else
                {
                    ani.SetBool("running", false);
                    audioplay.clip = walk;
                }
			}
            if (!grademanager.instance.gamestart)
            {
                ani.SetBool("running", false);
                audioplay.clip = walk;
            }
		}
    }

    void OnTriggerEnter(Collider col)
    {
		if (col.tag == "clock") {
			if (grademanager.instance.gamestart) {
                Destroy(st.gameObject);
				clock.GetComponent<clock> ().enabled = true;
				clock.GetComponent<Rigidbody> ().useGravity = true;
                script.soundm(2);
			}
		}
        if(col.tag == "enemystart")
        {
			if (grademanager.instance.gamestart) 
				grademanager.instance.enemystart = true;
        }
        if (col.tag == "enemy")
        {
            if (grademanager.instance.gamestart)
            {
                haptic.instance.start = true;
                script.soundm(1);
            }
            grademanager.instance.enemystart = false;
            grademanager.instance.gameover = true;
            grademanager.instance.gamestart = false;

            //Destroy(gameObject);
        }
        if (col.tag == "Cubes")
        {

            if (grademanager.instance.gamestart)
            {
                haptic.instance.start = true;
                script.soundm(0);
            }
                grademanager.instance.enemystart = false;
            grademanager.instance.gameover = true;
            grademanager.instance.gamestart = false;

            //Destroy(gameObject);
        }
        if (col.tag == "Start")
        {
            grademanager.instance.gamestart = true;
            grademanager.instance.gameover = false;
        }
        if (col.tag == "goal")
        {
			grademanager.instance.gamestart = false;
            grademanager.instance.gameclear = true;
        }
    }
}

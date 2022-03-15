using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laser : MonoBehaviour {
	public static laser instance;
	RaycastHit hit;
	float maxdis = 10;
	public GameObject player;
	public LayerMask lay,eye,hand;
	Animator ani;
	public bool start;
	// Use this for initialization
	void Start()
	{
		instance = this;
	}

	// Update is called once per frame
	void Update()
	{
		Debug.DrawRay(transform.position, transform.forward, Color.blue, 0.3f);
		if (Physics.Raycast(transform.position, transform.forward, out hit, maxdis, lay))
		{
			ani = player.GetComponent<Animator>();
			Vector3 pos = hit.point;
			player.transform.position = new Vector3(pos.x,pos.y,pos.z);
			ani.SetBool("walk", true);
		}
		if(Physics.Raycast(transform.position, transform.forward, out hit, maxdis, eye))
		{
			Debug.Log("눈");
			start = true;
			GameManager.instance.eye = true;
			ani.SetBool("walk", true);
		}
		if (Physics.Raycast(transform.position, transform.forward, out hit, maxdis, hand))
		{
			Debug.Log("손");
			start = true;
			GameManager.instance.eye = false;
			ani.SetBool("walk", true);
		}
	}
}

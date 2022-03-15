using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class raycast : MonoBehaviour {
    RaycastHit hit;
    public float MaxDistance = 10f;
    public LayerMask lay;
    public GameObject dool;

    Animator ani;
    bool co = true;
	// Use this for initialization
    void Awake()
    {

        
        //ani = GetComponent<Animator>();
        
    }
	
	// Update is called once per frame
    void Update()
    {

        Debug.DrawRay(transform.position, transform.forward * MaxDistance, Color.blue, 0.3f);
        if (Physics.Raycast(transform.position, transform.forward, out hit, MaxDistance, lay))
        {

            sound.instance.doolsound = false;

            co = false;
        }
        else if(co)
        {
            if (ani == null)
                return;
            Vector3 dir = transform.position - dool.transform.position;
            dool.transform.rotation = Quaternion.Lerp(dool.transform.rotation, Quaternion.LookRotation(dir), 3f * Time.deltaTime);
            sound.instance.doolsound = true;
        }
        
        
	}
}

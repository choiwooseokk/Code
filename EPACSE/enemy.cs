using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class enemy : MonoBehaviour
{
   // public static enemy Intence = null;
    public Transform[] point = new Transform[3];
    public Transform target;
    private bool see;
    [HideInInspector]
    public int number;

    void Update()
    {
        FindVisibleTargets();

    }
    public void FindVisibleTargets()
    {

        RaycastHit RayHit;
        if (Physics.Raycast(this.transform.position + new Vector3(0, 0.5f, 0), (this.transform.forward * 5f) * -1f, out RayHit))
        {
           
            if (RayHit.collider.GetComponent<enemy>())
            {
                int r = Random.Range(0, point.Length);
                RayHit.collider.transform.position = point[r].transform.position;
                //if (Vector3.Distance(RayHit.collider.transform.position, target.transform.position) < 3f)
                //{
                int e = Random.Range(0, point.Length);
                RayHit.collider.transform.position = point[e].transform.position;
                //}
            }
        }


    }
    void OnTriggerEnter(Collider col)
    {
        if (col.name == "Stopenemy")
        {
            GetComponent<Animator>().SetBool("MoveStop", true);
            GetComponent<Animator>().SetBool("Stop", true);
            LayerMask mask = LayerMask.NameToLayer("Default");
            this.gameObject.layer = mask;
            GetComponent<enemy>().enabled = false;

            GetComponent<AudioSource>().enabled = false;
            GetComponent<NavMeshAgent>().enabled = false;

            EnemySpwan.Intance.number++;
        }
        if (col.name == "StopenemyB")
        {
            GetComponent<Animator>().SetBool("MoveStop", true);
            GetComponent<Animator>().SetBool("Stop", true);
            LayerMask mask = LayerMask.NameToLayer("Default");
            this.gameObject.layer = mask;
            GetComponent<enemy>().enabled = false;

            GetComponent<AudioSource>().enabled = false;
            GetComponent<NavMeshAgent>().enabled = false;
            
            EnemySpwan.Intance.numberB++;
        }
    }
}
//}

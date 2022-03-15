using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Player : MonoBehaviour
{
    [SerializeField]
    public float ViewDistance;
    public GameObject EnemyHand;
    public GameObject EnemyHandright;
    private float DeadTime;
    private Transform enemy;
    public Camera cam;
    public bool DeadCheck;
    private bool Dead;
    private bool test;

    void Start()
    {
        SoundManager.instance.BgmSoundPlay(0);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha0)){
            test = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            test = false;
        }

        if (!test) FindTarget();
        
        if (Dead == true)
        {

            SoundManager.instance.EffectSoundPlay(8);
            
            DeadTime += Time.deltaTime;

            EnemyHand.SetActive(true);
            EnemyHandright.SetActive(true);
            EnemyHandright.transform.localRotation *= Quaternion.Euler(0, 10f, 0);
            EnemyHand.transform.localRotation *= Quaternion.Euler(0, 0, -10f);

            if (DeadTime > 0.5f)
                Application.LoadLevel("EndScene");
        }



        Tutorial();
        
    }

    public void FindTarget()
    {

        Collider[] targets = Physics.OverlapSphere(transform.position, ViewDistance, 1 << 9);
        for (int i = 0; i < targets.Length; i++)
        {

            enemy = targets[i].transform;

            Vector3 left = cam.WorldToViewportPoint(enemy.position + enemy.right * 2f);
            Vector3 right = cam.WorldToViewportPoint(enemy.position + enemy.right * -2f);
            Vector3 up = cam.WorldToViewportPoint(enemy.position + enemy.up * 2f);
            Vector3 down = cam.WorldToViewportPoint(enemy.position + enemy.up * -2f);

            UnityEngine.AI.NavMeshAgent e = enemy.GetComponent<UnityEngine.AI.NavMeshAgent>();

            // not see
            if ((CanMove(left) && CanMove(right) && CanMove(up)) && CanMove(down))
            {

                // enemy speed
                if (Vector3.Distance(enemy.position, transform.position) < 3f)
                {
                    e.speed = 0f;
                    DeadCheck = true;
                    Dead = true;
                }
                else e.speed = 1.5f;
                // enemy sound
                if (SoundPlay(enemy.position))
                {
                    SoundManager.instance.EffectSoundPlay(1);
                }
            }
            // see
            else
            {
                DeadCheck = false;
                e.speed = 0f;
                e.velocity = new Vector3(0, 0, 0);

                if (SoundPlay(enemy.position))
                    SoundManager.instance.EffectSoundStop(1);

            }
            // enemy move navmesh
            if (enemy.GetComponent<UnityEngine.AI.NavMeshAgent>())
            {
                enemy.GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(transform.position);
            }

        }

    }
   
    private bool SoundPlay(Vector3 target)
    {
        if (Vector3.Distance(target, transform.position) < 12f)
        {
            return true;
        }
        else
            return false;
    }

    private bool CanMove(Vector3 pos)
    {
        if (pos.z <= 0f)
            return true;

        if (pos.x < 0f || pos.x > 1f)
            return true;

        if (pos.y < 0f || pos.y > 1f)
            return true;

        if (pos.x < 0f && pos.x > 1f && pos.y < 0f && pos.y > 1f)
            return true;

        return false;
    }

    public void Tutorial()
    {
         Collider[] targets = Physics.OverlapSphere(transform.position, ViewDistance, 1 << 8);
            

            for (int i = 0; i < targets.Length; i++)
            {
                enemy = targets[i].transform;

                Vector3 left = cam.WorldToViewportPoint(enemy.position + enemy.right * -1f);
                Vector3 right = cam.WorldToViewportPoint(enemy.position + enemy.right * 1f);
                Vector3 up = cam.WorldToViewportPoint(enemy.position + enemy.up * 2f);
                Vector3 down = cam.WorldToViewportPoint(enemy.position + enemy.up * -2f);

            UnityEngine.AI.NavMeshAgent e = enemy.GetComponent<UnityEngine.AI.NavMeshAgent>();

                if (CanMove(left) && CanMove(right)&&CanMove(up)&&CanMove(down))
                {
                    e.speed = 1.5f;
                    SoundManager.instance.EffectSoundPlay(1);
            }
                else{
                    e.speed = 0f;
                    SoundManager.instance.EffectSoundStop(1);
                }

                if (enemy.GetComponent<UnityEngine.AI.NavMeshAgent>())
                {
                    enemy.GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(transform.position);
                }

            }

        }

    //IEnumerator FindEnemy()
    //{
    //    while (true)
    //    {
    //        FindTarget();
    //        yield return new WaitForSeconds(0.1f);
    //    }
    //}


}

    

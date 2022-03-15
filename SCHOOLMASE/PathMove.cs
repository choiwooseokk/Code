using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathMove : MonoBehaviour {
    public Transform start;
    public EditorPath pathToFollow;
    public static PathMove instance = null;
    public int CurrentWayPointID = 0;
    public float speed;
    private float reachDistance = 0.0f;
    public float rotationSpeed = 5.0f;
    public string pathName;
    public Color colorStart = Color.black;
    public Color colorEnd = Color.white;
    public float duration = 1.0F;
    Vector3 last_position;
    Vector3 current_position;
    bool IsMaxRotate = false;
    public bool end = false;
	// Use this for initialization
	void Start () {
        instance = this;
        last_position = transform.position;
        //RenderSettings.skybox.SetColor("_Tint", colorStart);
	}
	
	// Update is called once per frame
	void Update () {
        if(end)
        {
            CurrentWayPointID = 0;
            this.transform.position = pathToFollow.path_objs[CurrentWayPointID].transform.position;
            end = false;
        }
       

            float distance = Vector3.Distance(pathToFollow.path_objs[CurrentWayPointID].position, transform.position);
            transform.position = Vector3.MoveTowards(transform.position, pathToFollow.path_objs[CurrentWayPointID].position, Time.deltaTime * speed);

        var rotation = Quaternion.LookRotation(pathToFollow.path_objs[CurrentWayPointID].position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        //Vector2 dirToTarget = pathToFollow.path_objs[CurrentWayPointID].position - this.transform.position;
        //this.transform.forward = dirToTarget.normalized*-1;

        if (distance <= reachDistance)
            {
                CurrentWayPointID++;
            }
            if (CurrentWayPointID >= pathToFollow.path_objs.Count)
            {
                CurrentWayPointID = pathToFollow.path_objs.Count - 1;
            }
        }
}

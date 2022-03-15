using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CameraBlocker : MonoBehaviour 
{
    Camera CameraComponent;

    public float CameraRange;

    //public GameObject T;

    void Awake()
    {
        CameraComponent = GetComponent<Camera>();
    }

	void Update () 
    {
        CameraComponent.farClipPlane = Vector3.Distance(new Vector3(transform.localPosition.x, 0, transform.localPosition.z), Vector3.zero) < CameraRange ? 300 : CameraComponent.nearClipPlane + 0.01f;
        //if(CameraComponent.farClipPlane != 300){
        //    T.gameObject.SetActive(true);
        //}
        //else
        //{
        //    T.gameObject.SetActive(false);
        //}
	}
}

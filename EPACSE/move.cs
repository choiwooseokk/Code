using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{

    [SerializeField]
    public Transform rig;
    private float time = 0f;
    public Valve.VR.InteractionSystem.Hand hand;
    public Camera cam;

    private Vector2 ControllerAxis;

    void Start()
    {
        hand = GetComponent<Valve.VR.InteractionSystem.Hand>();
    }

    //Update is called once per frame
    void FixedUpdate()
    {

        ControllerAxis = hand.controller.GetAxis();

        Vector3 outlineAxis = cam.transform.rotation * new Vector3(ControllerAxis.x, 0, ControllerAxis.y) * 3f * Time.deltaTime;

        rig.Translate(new Vector3(outlineAxis.x,0,outlineAxis.z));

        if (ControllerAxis != new Vector2(0, 0))
            SoundManager.instance.EffectSoundPlay(0);
        else
            SoundManager.instance.EffectSoundStop(0);



    }
}

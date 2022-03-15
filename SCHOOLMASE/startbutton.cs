using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;
namespace Valve.VR.InteractionSystem.Sample
{
    public class startbutton : MonoBehaviour
    {
        private SteamVR_Input_Sources handType;
        public SteamVR_Action_Boolean plantAction;
        public Hand hand;
        public GameObject time;
        // Use this for initialization
        private void OnEnable()
        {
            if (hand == null)
                hand = this.GetComponent<Hand>();

            if (plantAction == null)
            {
                Debug.LogError("<b>[SteamVR Interaction]</b> No plant action assigned");
                return;
            }

            //plantAction.AddOnChangeListener(OnPlantActionChange, hand.handType);
        }

        void Update()
        {
            //if (plantAction.GetStateDown(rightHand))
            //{
            //    GameManager.instance.start = true;
            //    ui.SetActive(false);
            //    laser.instance.start = false;
            //}
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameManager.instance.gamestart = true;
            }
			if (plantAction.GetLastStateDown (handType)) {
				if (laser.instance.start) {
					GameManager.instance.gamestart = true;
				}
			}
        }



        private void OnPlantActionChange(SteamVR_Action_Boolean actionIn, SteamVR_Input_Sources inputSource, bool newValue)
        {
            if (newValue)
            {
                if (laser.instance.start)
                {
					GameManager.instance.gamestart = true;
                }
            }
        }
    }
}
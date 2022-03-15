using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
namespace Valve.VR.InteractionSystem.Sample
{
    public class keyboard : MonoBehaviour
    {
		public GameObject GameOver,board;
		private SteamVR_Input_Sources handType;
        public SteamVR_Action_Boolean inputkeyboard;
        public Hand hand;
        public Text name;
        public float maxdis = 10f;
        RaycastHit hit;
        public LayerMask key,enter;
        public InputField ip;
        public List<string> iptext = new List<string>();
        string data = "";
        Transform changeobj;
		int numberindex=0;
        // Use this for initialization

        // Update is called once per frame
        void Start()
        {
            
			if (!PlayerPrefs.HasKey("N"))
			{
				Debug.Log("1");
				data = "심수민바보";
				PlayerPrefs.SetString("N", data);
			}
			name.text = PlayerPrefs.GetString("N");

		}
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                PlayerPrefs.DeleteAll();
            }
			if (inputkeyboard.GetLastStateUp (handType)) {
				inputkey ();
			}
            Debug.DrawRay(transform.position, transform.forward, Color.red, 0.3f);
            if (Physics.Raycast(transform.position, transform.forward, out hit, maxdis, key))
            {
                if (changeobj != null)
                {
                    if (changeobj != hit.transform)
                    {
                        changeobj.GetComponent<Image>().color = new Color(255, 255, 255, 0.3f);
                        Debug.Log(changeobj.GetComponent<Image>().color);
                    }
                }
                hit.transform.gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 0);
                changeobj = hit.transform;
                if (Input.GetKeyDown(KeyCode.A))
                {
                    data = data + (hit.transform.GetChild(0).GetComponent<Text>().text);
                    ip.text = data;
                }
            }
            if (Physics.Raycast(transform.position, transform.forward, out hit, maxdis, enter))
            {
				if (changeobj != null)
				{
					if (changeobj != hit.transform)
					{
						changeobj.GetComponent<Image>().color = new Color(255, 255, 255, 0.3f);
						Debug.Log(changeobj.GetComponent<Image>().color);
					}
				}
				hit.transform.gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 0);
				changeobj = hit.transform;
                if (Input.GetKeyDown(KeyCode.A))
                {
					PlayerPrefs.SetString("N", data);
					GameOver.SetActive (true);
					board.SetActive (false);
                }
                
            }
        }
        private void inputkey()
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, maxdis, key))
            {
				if (numberindex == 0)
					data = "";
				if (numberindex <= 5) {
					numberindex++;
					data = data + (hit.transform.GetChild (0).GetComponent<Text> ().text);
					ip.text = data;
				}
            }
            if(Physics.Raycast(transform.position, transform.forward, out hit, maxdis, enter))
            {
				name.text = data;
				PlayerPrefs.SetString("N", data);
				GameOver.SetActive (true);
				board.SetActive (false);
            }
        }
    }
}
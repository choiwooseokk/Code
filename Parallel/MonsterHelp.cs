using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHelp : MonoBehaviour
{
    public GameObject player;
    public GameObject krasue;
    public GameObject woman;
    public GameObject woman2;
    public GameObject spider;
    public GameObject mannequin;
    public GameObject demonic;

    public GameObject krasueHelp;
    public GameObject womanHelp;
    public GameObject spiderHelp;
    public GameObject mannequinHelp;
    public GameObject demonicHelp;

    public GameObject QuitBtn;

    public Camera cam;
    Vector3 originPos;

    RaycastHit hit;
    public float viewDistance;
    public float viewAngle;
    public LayerMask targetMask;
    public LayerMask elseMask;

    void Start()
    {
        originPos = cam.transform.localPosition;
        if (!PlayerPrefs.HasKey("KrasueHelp"))
            PlayerPrefs.SetInt("KrasueHelp", 0);

        if (!PlayerPrefs.HasKey("WomanHelp"))
            PlayerPrefs.SetInt("WomanHelp", 1);

        if (!PlayerPrefs.HasKey("SpiderHelp"))
            PlayerPrefs.SetInt("SpiderHelp", 2);

        if (!PlayerPrefs.HasKey("MannequinHelp"))
            PlayerPrefs.SetInt("MannequinHelp", 3);

        if (!PlayerPrefs.HasKey("DemonicHelp"))
            PlayerPrefs.SetInt("DemonicHelp", 4);

    }
    public void FindMonster()
    {
        Collider[] targets = Physics.OverlapSphere(player.transform.position, viewDistance, targetMask);
        

        for (int i = 0; i < targets.Length; i++)
        {
            Transform ttarget = targets[i].transform;
            Vector3 dirToTarget = (ttarget.position - player.transform.position).normalized;
            if (Vector3.Dot(player.transform.forward, dirToTarget) > Mathf.Cos((viewAngle / 2) * Mathf.Deg2Rad))
            {
                float distToTarget = Vector3.Distance(player.transform.position, ttarget.position);

                if (!Physics.Raycast(player.transform.position, dirToTarget, distToTarget, elseMask))
                {
                    Debug.DrawLine(player.transform.position, ttarget.position,Color.red);
                    if (ManagerCS.MInstance.CheckObjectIsInCamera(krasue, cam) && EnemyDistance(player, krasue) < 7f && ManagerCS.MInstance.HorrorMap.activeSelf == true)
                    {
                        ShowMonsterHelp("KrasueHelp");
                    }

                    if (ManagerCS.MInstance.CheckObjectIsInCamera(woman, cam) && EnemyDistance(player, woman) < 14f ||
                        (ManagerCS.MInstance.CheckObjectIsInCamera(woman2, cam) && EnemyDistance(player, woman2) < 14f))
                    {
                        ShowMonsterHelp("WomanHelp");
                        StartCoroutine(Shake(0.005f, 1));
                    }

                    if (ManagerCS.MInstance.CheckObjectIsInCamera(spider, cam) && EnemyDistance(player, spider) < 5f && ManagerCS.MInstance.HorrorMap.activeSelf == true)
                    {
                        ShowMonsterHelp("SpiderHelp");
                        StartCoroutine(Shake(0.005f, 1));
                    }

                    if (ManagerCS.MInstance.CheckObjectIsInCamera(mannequin, cam) && EnemyDistance(player, mannequin) < 7f)
                    {
                        ShowMonsterHelp("MannequinHelp");
                        StartCoroutine(Shake(0.005f, 1));
                    }

                    if (ManagerCS.MInstance.CheckObjectIsInCamera(demonic, cam) && EnemyDistance(player, demonic) < 5f)
                    {
                        ShowMonsterHelp("DemonicHelp");
                        StartCoroutine(Shake(0.005f, 1));
                    }
                }
            }
        }
    }
    public float EnemyDistance(GameObject player, GameObject enemy)
    {
        float distance = Vector3.Distance(player.transform.position, enemy.transform.position);
        return distance;
    }

    public void Update()
    {
        FindMonster();

        if (Player.playerHP == 0)
        {
            EndClickQuitBtn();
        }
    }
    public IEnumerator Shake(float _amount, float _duration)
    {
        float timer = 0;
        while (timer <= _duration)
        {
            cam.transform.localPosition = (Vector3)Random.insideUnitCircle * _amount + originPos;

            timer += Time.deltaTime;
            yield return null;
        }
        cam.transform.localPosition = originPos;

    }
    public void ClickQuitBtn()
    {
        krasueHelp.SetActive(false);
        womanHelp.SetActive(false);
        spiderHelp.SetActive(false);
        mannequinHelp.SetActive(false);
        demonicHelp.SetActive(false);
        QuitBtn.SetActive(false);
        ManagerCS.MInstance.PlayingCheck = true;
        TouchManager.Instance.joystick.isTouch = true;
    }

    public void EndClickQuitBtn()
    {
        krasueHelp.SetActive(false);
        womanHelp.SetActive(false);
        spiderHelp.SetActive(false);
        mannequinHelp.SetActive(false);
        demonicHelp.SetActive(false);
        QuitBtn.SetActive(false);
    }
    public void ShowMonsterHelp(string s)
    {
        switch (PlayerPrefs.GetInt(s))
        {
            case 0:
                krasueHelp.SetActive(true);
                QuitBtn.SetActive(true);
                TouchManager.Instance.joystick.isTouch = false;
                ManagerCS.MInstance.PlayingCheck = false;
                PlayerPrefs.SetInt("KrasueHelp", 5);
                break;
            case 1:
                womanHelp.SetActive(true);
                QuitBtn.SetActive(true);
                TouchManager.Instance.joystick.isTouch = false;
                ManagerCS.MInstance.PlayingCheck = false;
                PlayerPrefs.SetInt("WomanHelp", 6);
                break;
            case 2:
                spiderHelp.SetActive(true);
                QuitBtn.SetActive(true);
                TouchManager.Instance.joystick.isTouch = false;
                ManagerCS.MInstance.PlayingCheck = false;
                PlayerPrefs.SetInt("SpiderHelp", 7);
                break;
            case 3:
                mannequinHelp.SetActive(true);
                QuitBtn.SetActive(true);
                TouchManager.Instance.joystick.isTouch = false;
                ManagerCS.MInstance.PlayingCheck = false;
                PlayerPrefs.SetInt("MannequinHelp", 8);
                break;
            case 4:
                demonicHelp.SetActive(true);
                QuitBtn.SetActive(true);
                TouchManager.Instance.joystick.isTouch = false;
                ManagerCS.MInstance.PlayingCheck = false;
                PlayerPrefs.SetInt("DemonicHelp", 9);
                break;
        }
    }
}
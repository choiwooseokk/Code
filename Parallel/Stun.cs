using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Stun : MonoBehaviour
{
    public GameObject krasue;
    public GameObject spider;
    public GameObject monseterWoman;
    public Camera cam;
    public static bool isStop = false;

    public Image flashImage;
    public float flashSpeed = 5f;
    public Color flashColor = new Color(1f, 0f, 0f, 0.1f);
    private bool flashCtrl = false;

    public GameObject cameraBtn;
    public static int stunCount = 3;

    // Update is called once per frame
    void Update()
    {
        if (flashCtrl == true)
        {
            flashImage.color = Color.Lerp(flashImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
    }
    public void CameraFlash() // 스크린샷 기능 넣기
    {
        flashImage.color = flashColor;
        flashCtrl = true;
        if (stunCount > 0)
        {
            stunCount--;
            ImageFill.instance.Fill(stunCount);
        }
    }

    public void BtnDown()
    {
        cameraBtn.SetActive(true);
    }
    public void BtnUp()
    {
        ManagerCS.MInstance.effectSource.clip = ManagerCS.MInstance.effecSound[3];
        ManagerCS.MInstance.effectSource.Play();
        cameraBtn.SetActive(false);
        StartCoroutine(CaptureIt());
        if (stunCount > 0)
        {
            if (ManagerCS.MInstance.CheckObjectIsInCamera(krasue, cam) ||
                ManagerCS.MInstance.CheckObjectIsInCamera(spider, cam) ||
                ManagerCS.MInstance.CheckObjectIsInCamera(monseterWoman, cam))
            {
                isStop = true;
            }
            else
            {
                isStop = false;
            }
        }
    }

    public void PhoneBtnInit()
    {
        cameraBtn.SetActive(false); // 카메라버튼 꺼주고
        // 어차피 문 데이터는 자동으로 초기화 될거고
    }

    IEnumerator CaptureIt() // 파일 저장명
    {
        string timeStamp = System.DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
        string fileName = "ScreenShot_" + timeStamp + ".png";
        string pathToSave = fileName;
        // if(Application.platform == RuntimePlatform.Android)
        //     ScreenCapture.CaptureScreenshot("/storage/emulated/0/Android/data" + "/" + pathToSave);
        // else if(Application.isEditor == true)
            ScreenCapture.CaptureScreenshot(Application.persistentDataPath + "/" + pathToSave);
        yield return new WaitForSeconds(0.1f);
        CameraFlash();
        yield return new WaitForEndOfFrame();
    }
}

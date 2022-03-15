using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using UnityEngine.EventSystems;
using TMPro;

public class ManagerCS : MonoBehaviour
{
    public static ManagerCS MInstance = null;

    #region public
    public GameObject NightMap, HorrorMap;
    public GameObject PhoneUI;
    public bool PlayingCheck = true;
    public Image bloodScreen;
    public GameObject CenterImage;

    public TextMeshProUGUI BgmText, EffectText;

    public AudioMixerGroup EffectMixer;
    public AudioMixerGroup BGMMixer;
    public AudioSource BGMSource;
    public AudioClip[] BgmClip;

    public AudioSource effectSource;
    public AudioClip[] effecSound;

    public Image BGM_Image;
    public Image Effect_Image;

    public Sprite[] ONOFF_Image;

    public Sprite tempImg;
    public GameObject[] ItemUI;

    public float[] TestFloat;

    public GameObject KrasueMesh, patrolGhost1F, patrolGhost2F, HorrorSpider, player;
    public bool gymGameOver = false;
    public Image GymGameOver_Image, GameOver_Image, Clear_Image;
    public GameObject Btn_Retry, Btn_Main, Btn_LookScreenShot, Btn_Interact;

    public Text CaptionText, PlayTime_text;

    public GameObject[] Items;
    public GameObject[] Doors;
    public GameObject gymEventCol;
    public GameObject Canvas_ScreenShot, Canvas_Play, Canvas_Hint, Canvas_Title;

    public GameObject SpiderObj;

    public int itemCount = 0;
    public PlayableDirector playableDirector;
    public GameObject EndingCam;
    public GameObject endingMonster;
    public GameObject numLock;
    public bool lastCheck = false;
    public GameObject CameraBtn;
    public GameObject ItemUIList;
    public GameObject LastDoor;
    public Image BtyFill;

    public int FloorNum = 2;
    public GameObject IntroKeyObj;
    public Toggle cameraEffectCheck;

    public GameObject OptionPanel;
    #endregion

    #region private
    private float GameTimer = 0f;
    private float FadeTime = 0f;
    private string sPlaytime;
    ScreenShot shot;
    #endregion

    #region Function
    private void Awake()
    {
        if (MInstance == null)
        {
            MInstance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Screen.SetResolution(1920, 1080, true);
        effectSource = this.GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayingCheck = false;
        if (!PlayerPrefs.HasKey("Effect"))
            PlayerPrefs.SetInt("Effect", 1);
        else
            OnOffPlayer("Effect", EffectMixer, Effect_Image);

        if (!PlayerPrefs.HasKey("BGM"))
            PlayerPrefs.SetInt("BGM", 1);
        else
            OnOffPlayer("BGM", BGMMixer, BGM_Image);

        RandomItemInit();
        StartCoroutine(ShowBloodScreen());
        shot = Canvas_ScreenShot.GetComponent<ScreenShot>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayingCheck == true)
        {
            GameTimer += Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.M))
                MapTurnOnOff();

            if (Input.GetKeyDown(KeyCode.H))
            {
                PhoneUIOnOff();
            }
        }
        else
        {
            InteractableControl(false);
        }

        if(Application.platform == RuntimePlatform.Android)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                if(OptionPanel.activeSelf)
                    OptionPanel.SetActive(false);
                else
                    OptionPanel.SetActive(true);

                if(Canvas_Title.activeSelf == true && Canvas_Title.GetComponent<MainBtnManager>().HowToPanel.activeSelf == true)
                    Canvas_Title.GetComponent<MainBtnManager>().HowToPanel.SetActive(false);
            }
        }

        if (Player.playerHP == 0)
        {
            PlayingCheck = false;
            GameOver();
        }
        else if(Player.playerHP >= 2)
        {
            GameClear();
            PlayingCheck = false;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            Player.playerHP -= 1;
            PlayingCheck = false;
        }

        if(playableDirector.time >= 33f)
        {
            Player.playerHP = 2;
        }

        if (ItemUI[ScrollSnap.minButtonNum].GetComponent<ItemCS>().item == Item.Key)
        {
            player.GetComponent<Player>().dData = ItemUI[ScrollSnap.minButtonNum].GetComponent<ItemCS>().iteminfo;
        }

        // 자물쇠 맵에따라 보이거나 안보이거나
        if (ManagerCS.MInstance.NightMap.activeSelf == true && lastCheck == false)
        {
            ManagerCS.MInstance.numLock.SetActive(true);
        }
        else
        {
            ManagerCS.MInstance.numLock.SetActive(false);
        }
    }

    public void MapTurnOnOff()
    {
        if (NightMap.active && !HorrorMap.active)
        {
            NightMap.SetActive(false);
            HorrorMap.SetActive(true);
            Items[14].SetActive(false);
            KrasueMesh.GetComponent<SkinnedMeshRenderer>().enabled = true;
        }
        else if (!NightMap.active && HorrorMap.active)
        {
            ImageFill.instance.FillPerfect(Stun.stunCount);
            NightMap.SetActive(true);
            HorrorMap.SetActive(false);
            Items[14].SetActive(true);
            KrasueMesh.GetComponent<SkinnedMeshRenderer>().enabled = false;
        }
    }

    public void PhoneUIOnOff()
    {
        PhoneUI.SetActive(!PhoneUI.active);
    }

    public bool CheckObjectIsInCamera(GameObject _target, Camera Cam)
    {
        Transform enemy = _target.transform;

        Camera selectedCamera = Cam.GetComponent<Camera>();
        Vector3 left = selectedCamera.WorldToViewportPoint(enemy.position + enemy.right * TestFloat[0]);
        Vector3 right = selectedCamera.WorldToViewportPoint(enemy.position + enemy.right * -TestFloat[1]);
        Vector3 up = selectedCamera.WorldToViewportPoint(enemy.position + enemy.up * TestFloat[2]);
        Vector3 down = selectedCamera.WorldToViewportPoint(enemy.position + enemy.up * -TestFloat[3]);

        // not see
        if ((CanMove(left) && CanMove(right) && CanMove(up)) && CanMove(down))
        {
            return false;
        }
        else
            return true;
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

    //0 : off, 1 : on
    public void OnOffPlayer(string s, AudioMixerGroup ag, Image t)
    {
        switch (PlayerPrefs.GetInt(s))
        {
            case 0:
                ag.audioMixer.SetFloat(s, -80f);
                break;
            case 1:
                ag.audioMixer.SetFloat(s, 20f);
                break;
        }
    }
    /// <summary>
    /// 0���� �⺻ BGM
    /// </summary>
    /// <param name="i"></param>
    public void AudioChanger(int i)
    {
        if (!BGMSource.clip.Equals(BgmClip[i]))
        {
            BGMSource.Stop();
            BGMSource.clip = BgmClip[i];
            BGMSource.Play();
        }
    }

    public void ChangeFirstFloor()
    {
        SpiderObj.transform.position -= new Vector3(0, 3.5f, 0);
    }
    public void ChangeSecondFloor()
    {
        SpiderObj.transform.position += new Vector3(0, 3.5f, 0);
    }

    public bool GetItem(Sprite img, int info, Item i)
    {
        //가운데가 비어있다면 가운데에 넣기
        if (ItemUI[ScrollSnap.minButtonNum].GetComponent<Image>().sprite == tempImg)
        {

            ItemUI[ScrollSnap.minButtonNum].GetComponent<Image>().sprite = img;
            ItemUI[ScrollSnap.minButtonNum].GetComponent<ItemCS>().ItemImage = img;
            ItemUI[ScrollSnap.minButtonNum].GetComponent<ItemCS>().iteminfo = info;
            ItemUI[ScrollSnap.minButtonNum].GetComponent<ItemCS>().item = i;
            itemCount++;
            return true;
        }

        if (ItemUI[ScrollSnap.minButtonNum].GetComponent<Image>().sprite != tempImg
           && ItemUI[ScrollSnap.minButtonNum] == ItemUI[1]
          && ItemUI[ScrollSnap.minButtonNum - 1].GetComponent<Image>().sprite != tempImg
          && ItemUI[ScrollSnap.minButtonNum + 1].GetComponent<Image>().sprite != tempImg
          && ItemUI[4].GetComponent<Image>().sprite != tempImg)
        {
            ItemUI[ScrollSnap.minButtonNum + 2].GetComponent<Image>().sprite = img;
            ItemUI[ScrollSnap.minButtonNum + 2].GetComponent<ItemCS>().ItemImage = img;
            ItemUI[ScrollSnap.minButtonNum + 2].GetComponent<ItemCS>().iteminfo = info;
            ItemUI[ScrollSnap.minButtonNum + 2].GetComponent<ItemCS>().item = i;
            itemCount++;
            return true;
        }

        if (ItemUI[ScrollSnap.minButtonNum].GetComponent<Image>().sprite != tempImg
             && ItemUI[ScrollSnap.minButtonNum] == ItemUI[1]
            && ItemUI[ScrollSnap.minButtonNum - 1].GetComponent<Image>().sprite != tempImg
            && ItemUI[ScrollSnap.minButtonNum + 1].GetComponent<Image>().sprite != tempImg)
        {
            ItemUI[4].GetComponent<Image>().sprite = img;
            ItemUI[4].GetComponent<ItemCS>().ItemImage = img;
            ItemUI[4].GetComponent<ItemCS>().iteminfo = info;
            ItemUI[4].GetComponent<ItemCS>().item = i;
            itemCount++;
            return true;
        }

        //------------------------------------------------------------------------------------------
        if (ItemUI[ScrollSnap.minButtonNum].GetComponent<Image>().sprite != tempImg
           && ItemUI[ScrollSnap.minButtonNum] == ItemUI[4]
           && ItemUI[ScrollSnap.minButtonNum - 1].GetComponent<Image>().sprite != tempImg
           && ItemUI[0].GetComponent<Image>().sprite != tempImg
           && ItemUI[ScrollSnap.minButtonNum - 2].GetComponent<Image>().sprite != tempImg)
        {
            ItemUI[1].GetComponent<Image>().sprite = img;
            ItemUI[1].GetComponent<ItemCS>().ItemImage = img;
            ItemUI[1].GetComponent<ItemCS>().iteminfo = info;
            ItemUI[1].GetComponent<ItemCS>().item = i;
            itemCount++;
            return true;
        }

        if (ItemUI[ScrollSnap.minButtonNum].GetComponent<Image>().sprite != tempImg
            && ItemUI[ScrollSnap.minButtonNum] == ItemUI[4]
            && ItemUI[ScrollSnap.minButtonNum - 1].GetComponent<Image>().sprite != tempImg
            && ItemUI[0].GetComponent<Image>().sprite != tempImg)
        {
            ItemUI[ScrollSnap.minButtonNum - 2].GetComponent<Image>().sprite = img;
            ItemUI[ScrollSnap.minButtonNum - 2].GetComponent<ItemCS>().ItemImage = img;
            ItemUI[ScrollSnap.minButtonNum - 2].GetComponent<ItemCS>().iteminfo = info;
            ItemUI[ScrollSnap.minButtonNum - 2].GetComponent<ItemCS>().item = i;
            itemCount++;
            return true;
        }

        //가운데에 아이템이있고 배열이[4]번째이고 왼쪽에 아이템이있으면 배열[0]번째에 아이템 넣기
        if (ItemUI[ScrollSnap.minButtonNum].GetComponent<Image>().sprite != tempImg
           && ItemUI[ScrollSnap.minButtonNum] == ItemUI[4]
           && ItemUI[ScrollSnap.minButtonNum - 1].GetComponent<Image>().sprite != tempImg)
        {
            ItemUI[0].GetComponent<Image>().sprite = img;
            ItemUI[0].GetComponent<ItemCS>().ItemImage = img;
            ItemUI[0].GetComponent<ItemCS>().iteminfo = info;
            ItemUI[0].GetComponent<ItemCS>().item = i;
            itemCount++;
            return true;
        }

        //가운데에 아이템이있고 가운데배열이[4]번째이면 옆에 아이템 넣기
        if (ItemUI[ScrollSnap.minButtonNum].GetComponent<Image>().sprite != tempImg &&
          ItemUI[ScrollSnap.minButtonNum] == ItemUI[4])
        {
            ItemUI[ScrollSnap.minButtonNum - 1].GetComponent<Image>().sprite = img;
            ItemUI[ScrollSnap.minButtonNum - 1].GetComponent<ItemCS>().ItemImage = img;
            ItemUI[ScrollSnap.minButtonNum - 1].GetComponent<ItemCS>().iteminfo = info;
            ItemUI[ScrollSnap.minButtonNum - 1].GetComponent<ItemCS>().item = i;
            itemCount++;
            return true;
        }

        //가운데에 아이템이있고 배열리[0]번째고 [4]번째 있고 오른쪽이 있고 [3]번째에 있으면 +2에 아이템 넣기
        if (ItemUI[ScrollSnap.minButtonNum].GetComponent<Image>().sprite != tempImg
          && ItemUI[ScrollSnap.minButtonNum] == ItemUI[0]
          && ItemUI[4].GetComponent<Image>().sprite != tempImg
          && ItemUI[ScrollSnap.minButtonNum + 1].GetComponent<Image>().sprite != tempImg
          && ItemUI[3].GetComponent<Image>().sprite != tempImg)
        {
            ItemUI[ScrollSnap.minButtonNum + 2].GetComponent<Image>().sprite = img;
            ItemUI[ScrollSnap.minButtonNum + 2].GetComponent<ItemCS>().ItemImage = img;
            ItemUI[ScrollSnap.minButtonNum + 2].GetComponent<ItemCS>().iteminfo = info;
            ItemUI[ScrollSnap.minButtonNum + 2].GetComponent<ItemCS>().item = i;
            itemCount++;
            return true;
        }
        //가운데에 아이템이있고 배열이[0]번째고 [4]번째 있고 오른쪽이 있으면 [3]번째에 넣기
        if (ItemUI[ScrollSnap.minButtonNum].GetComponent<Image>().sprite != tempImg
          && ItemUI[ScrollSnap.minButtonNum] == ItemUI[0]
          && ItemUI[4].GetComponent<Image>().sprite != tempImg
          && ItemUI[ScrollSnap.minButtonNum + 1].GetComponent<Image>().sprite != tempImg)
        {
            ItemUI[3].GetComponent<Image>().sprite = img;
            ItemUI[3].GetComponent<ItemCS>().ItemImage = img;
            ItemUI[3].GetComponent<ItemCS>().iteminfo = info;
            ItemUI[3].GetComponent<ItemCS>().item = i;
            itemCount++;
            return true;
        }

        //가운데에 아이템이있고 가운데가 배열[0]번째이고 배열[4]번째에도 아이템이있으면 오른쪽에 아이템 넣기
        if (ItemUI[ScrollSnap.minButtonNum].GetComponent<Image>().sprite != tempImg
           && ItemUI[ScrollSnap.minButtonNum] == ItemUI[0]
           && ItemUI[4].GetComponent<Image>().sprite != tempImg)
        {
            ItemUI[ScrollSnap.minButtonNum + 1].GetComponent<Image>().sprite = img;
            ItemUI[ScrollSnap.minButtonNum + 1].GetComponent<ItemCS>().ItemImage = img;
            ItemUI[ScrollSnap.minButtonNum + 1].GetComponent<ItemCS>().iteminfo = info;
            ItemUI[ScrollSnap.minButtonNum + 1].GetComponent<ItemCS>().item = i;
            itemCount++;
            return true;
        }

        //가운데에 아이템이있는데 가운데가 배열[0]번째 이면 배열[4]번째에 아이템 넣기
        if (ItemUI[ScrollSnap.minButtonNum].GetComponent<Image>().sprite != tempImg &&
          ItemUI[ScrollSnap.minButtonNum] == ItemUI[0])
        {
            ItemUI[4].GetComponent<Image>().sprite = img;
            ItemUI[4].GetComponent<ItemCS>().ItemImage = img;
            ItemUI[4].GetComponent<ItemCS>().iteminfo = info;
            ItemUI[4].GetComponent<ItemCS>().item = i;
            itemCount++;
            return true;
        }

        //가운데에 아이템이있고 배열이[3]번째이고 왼쪽 오른쪽 -2번째에 아이템이 있다면 [0]번째에 아이템 넣기
        if (ItemUI[ScrollSnap.minButtonNum].GetComponent<Image>().sprite != tempImg &&
            ItemUI[ScrollSnap.minButtonNum] == ItemUI[3] &&
         ItemUI[ScrollSnap.minButtonNum - 1].GetComponent<Image>().sprite != tempImg &&
         ItemUI[ScrollSnap.minButtonNum + 1].GetComponent<Image>().sprite != tempImg &&
         ItemUI[ScrollSnap.minButtonNum - 2].GetComponent<Image>().sprite != tempImg)
        {
            ItemUI[0].GetComponent<Image>().sprite = img;
            ItemUI[0].GetComponent<ItemCS>().ItemImage = img;
            ItemUI[0].GetComponent<ItemCS>().iteminfo = info;
            ItemUI[0].GetComponent<ItemCS>().item = i;
            itemCount++;
            return true;
        }

        //-----------------------------------------------------------------------------------
        //가운데 왼쪽 오른쪽에 -2번째에 아이템이있으면 가운데기준[배열 +2]번째에 아이템 넣기
        if (ItemUI[ScrollSnap.minButtonNum].GetComponent<Image>().sprite != tempImg &&
          ItemUI[ScrollSnap.minButtonNum - 1].GetComponent<Image>().sprite != tempImg &&
          ItemUI[ScrollSnap.minButtonNum + 1].GetComponent<Image>().sprite != tempImg &&
          ItemUI[ScrollSnap.minButtonNum - 2].GetComponent<Image>().sprite != tempImg)
        {
            ItemUI[ScrollSnap.minButtonNum + 2].GetComponent<Image>().sprite = img;
            ItemUI[ScrollSnap.minButtonNum + 2].GetComponent<ItemCS>().ItemImage = img;
            ItemUI[ScrollSnap.minButtonNum + 2].GetComponent<ItemCS>().iteminfo = info;
            ItemUI[ScrollSnap.minButtonNum + 2].GetComponent<ItemCS>().item = i;
            itemCount++;
            return true;
        }

        //가운데 왼쪽 오른쪽에 아이템이있으면 가운데기준[배열 -2]번째에 아이템 넣기
        if (ItemUI[ScrollSnap.minButtonNum].GetComponent<Image>().sprite != tempImg &&
            ItemUI[ScrollSnap.minButtonNum - 1].GetComponent<Image>().sprite != tempImg &&
            ItemUI[ScrollSnap.minButtonNum + 1].GetComponent<Image>().sprite != tempImg)
        {
            ItemUI[ScrollSnap.minButtonNum - 2].GetComponent<Image>().sprite = img;
            ItemUI[ScrollSnap.minButtonNum - 2].GetComponent<ItemCS>().ItemImage = img;
            ItemUI[ScrollSnap.minButtonNum - 2].GetComponent<ItemCS>().iteminfo = info;
            ItemUI[ScrollSnap.minButtonNum - 2].GetComponent<ItemCS>().item = i;
            itemCount++;
            return true;
        }

        //가운데에 아이템이 있고 옆에 아이템이있가면 오른쪽에 아이템 넣기
        if (ItemUI[ScrollSnap.minButtonNum - 1].GetComponent<Image>().sprite != tempImg)
        {
            ItemUI[ScrollSnap.minButtonNum + 1].GetComponent<Image>().sprite = img;
            ItemUI[ScrollSnap.minButtonNum + 1].GetComponent<ItemCS>().ItemImage = img;
            ItemUI[ScrollSnap.minButtonNum + 1].GetComponent<ItemCS>().iteminfo = info;
            ItemUI[ScrollSnap.minButtonNum + 1].GetComponent<ItemCS>().item = i;
            itemCount++;
            return true;
        }

        //가운데에 아이템이 있다면 옆에 아이템 넣기
        if (ItemUI[ScrollSnap.minButtonNum].GetComponent<Image>().sprite != tempImg)
        {
            ItemUI[ScrollSnap.minButtonNum - 1].GetComponent<Image>().sprite = img;
            ItemUI[ScrollSnap.minButtonNum - 1].GetComponent<ItemCS>().ItemImage = img;
            ItemUI[ScrollSnap.minButtonNum - 1].GetComponent<ItemCS>().iteminfo = info;
            ItemUI[ScrollSnap.minButtonNum - 1].GetComponent<ItemCS>().item = i;
            itemCount++;
            return true;
        }
        return false;
    }

    public void UseItem()
    {
        if (itemCount > 0)
        {
            ItemUI[ScrollSnap.minButtonNum].GetComponent<Image>().sprite = tempImg;
            ItemUI[ScrollSnap.minButtonNum].GetComponent<ItemCS>().ItemImage = null;
            ItemUI[ScrollSnap.minButtonNum].GetComponent<ItemCS>().iteminfo = 0;
            ItemUI[ScrollSnap.minButtonNum].GetComponent<ItemCS>().item = Item.Empty;
            itemCount -= 1;
        }
    }
    public void BatteryUse() //배터리 증가
    {
        if (ItemUI[ScrollSnap.minButtonNum].GetComponent<ItemCS>().item == Item.Battery && EventSystem.current.currentSelectedGameObject ==ItemUI[ScrollSnap.minButtonNum])
        {
            if (Stun.stunCount == 3)
            {
                CaptionList(3);
                Stun.stunCount = 3;
            }
            else
            {
                UseItem();
                Stun.stunCount = 3;
                ImageFill.instance.Fill(Stun.stunCount);
            }
        }
    }
    public void GameOver()
    {
        PlayingCheck = false;
        CheckPlayTime();
        for (int i = 15; i < 21; i++)
        {
            if (Items[i].activeSelf == true)
            {
                Items[i].SetActive(false);
            }
        }
        GameOver_Image.gameObject.SetActive(true);
        GameOver_Image.color = new Color(0, 0, 0, FadeTime);
        FadeTime += Time.deltaTime;
        if (FadeTime >= 1)
        {
            FadeTime = 1;
        }

        if (GameOver_Image.color.a == 1f)
        {
            GameOver_Image.transform.GetChild(0).gameObject.SetActive(true);
            Btn_Main.SetActive(true);
            Btn_Retry.SetActive(true);
            PlayTime_text.gameObject.SetActive(true);
        }
        else
        {
            GameOver_Image.transform.GetChild(0).gameObject.SetActive(false);
            Btn_Main.SetActive(false);
            Btn_Retry.SetActive(false);
            PlayTime_text.gameObject.SetActive(false);
        }
    }
    public void GameClear()
    {
        PlayingCheck = false;
        CheckPlayTime();
        Clear_Image.gameObject.SetActive(true);
        Clear_Image.color = new Color(0, 0, 0, FadeTime);
        FadeTime += Time.deltaTime;
        if (FadeTime >= 1)
        {
            FadeTime = 1;
        }
        if (Clear_Image.color.a == 1f)
        {
            Clear_Image.transform.GetChild(0).gameObject.SetActive(true);
            Btn_Main.SetActive(true);
            Btn_Retry.SetActive(true);
            PlayTime_text.gameObject.SetActive(true);
            Btn_LookScreenShot.SetActive(true);
        }
        else
        {
            Clear_Image.transform.GetChild(0).gameObject.SetActive(false);
            Btn_Main.SetActive(false);
            Btn_Retry.SetActive(false);
            PlayTime_text.gameObject.SetActive(false);
            Btn_LookScreenShot.SetActive(false);
        }
    }

    public void GameInit()
    {
        FloorNum = 2;
        player.GetComponent<Player>().PlayerInit();
        SpiderObj.GetComponent<Mover>().SpiderSetting();
        prowlGhost.instance.patrolGhostInit();
        TestSc.instance.KrasueInit();
        HorrorEvent.getInstance().HorrorEventInit();
        Canvas_Hint.GetComponent<HintObject>().HintInit();
        RandomItemInit();
        GetLastKey.getInstance().LastKeyInit();
        KrasueMesh.GetComponent<SkinnedMeshRenderer>().enabled = false;
        for (int i = 0; i < Doors.Length; i++)
        {
            Doors[i].GetComponent<DoorData>().DoorDataInit();
        }
        // 아이템창 초기화
        for (int i = 0; i < ItemUI.Length; i++)
        {
            ItemUI[i].GetComponent<Image>().sprite = tempImg;
            ItemUI[i].GetComponent<ItemCS>().ItemImage = null;
            ItemUI[i].GetComponent<ItemCS>().iteminfo = 0;
            ItemUI[i].GetComponent<ItemCS>().item = Item.Empty;
        }
        // 없어진 아이템 생성
        for (int i = 0; i < Items.Length - 6; i++)
        {
            if (Items[i].activeSelf == false)
            {
                Items[i].gameObject.SetActive(true);
            }
        }
        if (Stun.stunCount < 3)
        {
            Stun.stunCount = 3;
        }
        if(BtyFill!=null)
        BtyFill.fillAmount = 1;
        // 게임오버 초기화
        GameTimer = 0;
        PlayTime_text.gameObject.SetActive(false);
        Btn_LookScreenShot.SetActive(false);
        Btn_Main.SetActive(false);
        Btn_Retry.SetActive(false);
        PhoneUI.SetActive(false);
        FadeTime = 0;
        Player.playerHP = 1;
        EndingCam.SetActive(false);
        endingMonster.SetActive(false);
        GameOver_Image.gameObject.SetActive(false);
        Clear_Image.transform.GetChild(0).gameObject.SetActive(false);
        Clear_Image.gameObject.SetActive(false);
        GymGameOver_Image.transform.GetChild(0).gameObject.SetActive(false);
        GymGameOver_Image.gameObject.SetActive(false);
        CameraBtn.SetActive(true);
        ItemUIList.SetActive(true);
        if (!NightMap.active && HorrorMap.active)
        {
            NightMap.SetActive(true);
            HorrorMap.SetActive(false);
        }
    }

    public void CaptionList(int i)
    {
        StartCoroutine(CaptionCoroutine());
        switch (i)
        {
            case 0:
                CaptionText.text = "열쇠가 맞지 않습니다.";
                break;
            case 1:
                CaptionText.text = "비밀번호가 틀립니다.";
                break;
            case 2:
                CaptionText.text = "문이 잠겨있습니다.";
                break;
            case 3:
                CaptionText.text = "배터리가 풀입니다";
                break;
            case 4:
                CaptionText.text = "아직 열 수 없습니다.";
                break;
        }
    }

    public void RandomItemInit()
    {
        for (int i = 0; i < 3;)
        {
            int rand = Random.Range(15, 21);

            if (Items[rand].activeSelf == false)
            {
                Items[rand].SetActive(true);
                i++;
            }
            else
            {
                while (Items[rand].activeSelf == true)
                {
                    rand = Random.Range(15, 21);

                    if (Items[rand].activeSelf == false)
                        break;
                    else
                        continue;
                }
                Items[rand].SetActive(true);
                i++;
            }
        }
    }
    void CheckPlayTime()
    {
        sPlaytime = "playtime : " + (int)GameTimer / 60 + "M " + (int)GameTimer % 60 + "S";
        PlayTime_text.text = sPlaytime;
    }

    #endregion

    #region Button Event

    public void BGM_ONOFF_Click()
    {
        switch (PlayerPrefs.GetInt("BGM"))
        {
            case 0:
                PlayerPrefs.SetInt("BGM", 1);
                BgmText.color = Color.white;
                break;
            case 1:
                PlayerPrefs.SetInt("BGM", 0);
                BgmText.color = Color.red;
                break;
        }

        OnOffPlayer("BGM", BGMMixer, BGM_Image);
    }

    public void Effect_ONOFF_Click()
    {
        switch (PlayerPrefs.GetInt("Effect"))
        {
            case 0:
                PlayerPrefs.SetInt("Effect", 1);
                EffectText.color = Color.white;
                break;
            case 1:
                PlayerPrefs.SetInt("Effect", 0);
                EffectText.color = Color.red;
                break;
        }

        OnOffPlayer("Effect", EffectMixer, Effect_Image);
    }

    public void Retry()
    {
        GameInit();
        PlayingCheck = true;
    }

    public void GotoMain()
    {
        GameInit();
        SceneManager.LoadScene(0);
        // Canvas_Title.SetActive(true);
        // Canvas_Title.transform.GetChild(2).gameObject.SetActive(true);
        PlayingCheck = false;
    }

    public void InteractableControl(bool _show = true)
    {
        Btn_Interact.SetActive(_show);
        if (_show)
        {
            CenterImage.GetComponent<RectTransform>().localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }
        else
        {
            CenterImage.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
        }
    }

    public void Btn_Interactable()
    {
        GameObject g = player.GetComponent<Player>().dObj;
        if (g.GetComponent<DoorData>())
        {
            if (player.GetComponent<Player>().dData == g.GetComponent<DoorData>().dNum
            && ItemUI[ScrollSnap.minButtonNum].GetComponent<ItemCS>().item == Item.Key)
            {
                UseItem();
                effectSource.clip = effecSound[0];
                effectSource.Play();
                if (g.GetComponent<DoorData>().dNum == 11)
                {
                    playableDirector.Play();
                    EndingCam.SetActive(true);
                    endingMonster.SetActive(true);
                    CameraBtn.SetActive(false);
                    ItemUIList.SetActive(false);
                    if (PhoneUI.activeSelf == true)
                    {
                        PhoneUI.SetActive(false);
                    }
                    if(NightMap.activeSelf == true)
                        MapTurnOnOff();
                    PlayingCheck = false;
                }
                if(g == Doors[6] || g == Doors[7])
                {
                    Doors[6].GetComponent<DoorData>().dNum = 0;
                    Doors[6].GetComponent<DoorData>()._openCheck = !Doors[6].GetComponent<DoorData>()._openCheck;
                    Doors[7].GetComponent<DoorData>().dNum = 0;
                    Doors[7].GetComponent<DoorData>()._openCheck = !Doors[7].GetComponent<DoorData>()._openCheck;
                    return;
                }
                // g.GetComponent<Animator>().SetBool("openCheck", true);
                g.GetComponent<DoorData>().dNum = 0;
                g.GetComponent<DoorData>()._openCheck = !g.GetComponent<DoorData>()._openCheck;
            }
            else if (g.GetComponent<DoorData>().dNum == 0)
            {
                effectSource.clip = effecSound[0];
                effectSource.Play();
                // g.GetComponent<Animator>().SetBool("openCheck", true);
                if(g == Doors[6] || g == Doors[7])
                {
                    Doors[6].GetComponent<DoorData>()._openCheck = !Doors[6].GetComponent<DoorData>()._openCheck;
                    Doors[7].GetComponent<DoorData>()._openCheck = !Doors[7].GetComponent<DoorData>()._openCheck;
                    return;
                }
                g.GetComponent<DoorData>()._openCheck = !g.GetComponent<DoorData>()._openCheck;
            }
            else if (ItemUI[ScrollSnap.minButtonNum].GetComponent<ItemCS>().item == Item.Key
                    && g.GetComponent<DoorData>().dNum != 0
                    && player.GetComponent<Player>().dData != g.GetComponent<DoorData>().dNum)
            {
                CaptionList(0);
            }
            else
            {
                effectSource.clip = effecSound[1];
                effectSource.Play();
                CaptionList(2);
            }
        }

        if (g.GetComponent<GetLastKey>() && GetLastKey.isGetLastKey == true)
        {
            g.GetComponent<GetLastKey>().LockerMask.SetActive(true);
        }

        if (g.tag == "Item")
        {
            if(g.GetComponent<ItemCS>().item == Item.Key)
            {
                effectSource.clip = effecSound[2];
                effectSource.Play();
            }
            if (g.GetComponent<ItemCS>().item == Item.Key && g.GetComponent<ItemCS>().iteminfo == 10)
            {
                GetLastKey.isGetLastKey = true;
            }
            if(g.GetComponent<ItemCS>().item == Item.Key && g.GetComponent<ItemCS>().iteminfo == 2)
            {
                SpiderObj.SetActive(true);
            }
            GetItem(g.GetComponent<ItemCS>().ItemImage, g.GetComponent<ItemCS>().iteminfo, g.GetComponent<ItemCS>().item);
            g.gameObject.SetActive(false);
        }
    }

    public void LookScreenShot()
    {
        Canvas_ScreenShot.SetActive(true);
    }

    public void OptionOnOff()
    {
        if(OptionPanel.activeSelf)
        {
            OptionPanel.SetActive(false);
            PlayingCheck = true;
        }
        else
        {
            OptionPanel.SetActive(true);
            PlayingCheck = false;
        }
    }
    #endregion

    #region Coroutine

    public IEnumerator CaptionCoroutine()
    {
        if (CaptionText.gameObject.activeSelf == false)
        {
            CaptionText.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            CaptionText.gameObject.SetActive(false);
            CaptionText.text = null;
        }
    }

    public IEnumerator ShowBloodScreen()
    {
        bloodScreen.color = new Color(1, 0, 0, UnityEngine.Random.Range(0.2f, 0.3f));
        yield return new WaitForSeconds(0.1f);
        bloodScreen.color = Color.clear;
    }

    public IEnumerator GymGameOver()
    {
        GameOver_Image.gameObject.SetActive(false);

        GymGameOver_Image.color = new Color(0.3f, 0.3f, 0.3f, 0.2f);
        yield return new WaitForSeconds(0.5f);
        GymGameOver_Image.color = new Color(0.3f, 0.3f, 0.3f, 0.4f);
        yield return new WaitForSeconds(0.5f);
        GymGameOver_Image.color = new Color(0.3f, 0.3f, 0.3f, 0.6f);
        yield return new WaitForSeconds(0.5f);
        GymGameOver_Image.color = new Color(0.3f, 0.3f, 0.3f, 0.8f);
        yield return new WaitForSeconds(0.5f);
        GymGameOver_Image.color = new Color(0.3f, 0.3f, 0.3f, 1f);

        if (GymGameOver_Image.color.a == 1f)
        {
            yield return new WaitForSeconds(0.5f);
            GymGameOver_Image.transform.GetChild(0).gameObject.SetActive(true);
            Btn_Main.SetActive(true);
            Btn_Retry.SetActive(true);
        }

        yield break;
    }
    #endregion
}
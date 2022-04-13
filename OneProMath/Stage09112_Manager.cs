using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ExpandFuncs_Common;
using Funtion_CWS;
using UnityEngine.EventSystems;
public class Stage09112_Manager : MonoBehaviour, IStage
{
    private static Stage09112_Manager instance;
    public static Stage09112_Manager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<Stage09112_Manager>();
            return instance;
        }
    }
    Transform shadow;
    Transform main;
    Transform panel;
    public struct QuestionInfo
    {
        public int stage;
        public List<int> question;
        public List<string> a;
        public List<Text> q;
        public int answer;
    }
    public QuestionInfo[] questions;
    /// <summary>
    /// 문제전환 애니메이션 예시를 위한 캔버스그룹
    /// </summary>
    GameObject outro_anim;
    /// <summary>
    /// 최대 문제 개수
    /// </summary>w
    int maxQno;
    /// <summary>
    /// 현재문제번호
    /// </summary>
    int curQno = 0;
    /// <summary>
    /// 인트로 애니메이션 유무
    /// </summary>
    bool isIntroAnim = false;
    Animator fix;
    public bool isSelectEnable = false;
    public DragObject curDragObject = null;
    Animation hint,roy;
    CanvasGroup mask,bg,bg1,wall,wall1;
    DragObject[] dragObjects;
    SlotObject[] slotObjects;
    public RopeObject[] ropeObjects;
    Transform endingmask;
    Image fixhead;
    public Transform curslot;
    [Header("Inspector Setting")]
    public Color[] slotcolor;
    public Sprite[] curslotimage;
    public Sprite[] normalimage;
    public Sprite slotimage;
    public Sprite bgimgae;
    public Color c;
    public Sprite[] yellowstar;
    public Sprite[] redstar;
    public Sprite[] fixheadsprite;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        maxQno = Prefab_DB.Instance.prefab_db.max_q;
        questions = new QuestionInfo[maxQno];
        MakeQuestion();
        Animator anim = transform.FindGameObjectByName<Animator>("Intro");
        outro_anim = transform.FindGameObjectByName("Outro");
        fix = transform.FindGameObjectByName<Animator>("fix");
        fixhead = fix.transform.Find("head").GetComponent<Image>();
        isIntroAnim = anim != null;
        main = transform.FindGameObjectByName<Transform>("main");
        mask = transform.FindGameObjectByName<CanvasGroup>("mask");
        bg = transform.FindGameObjectByName<CanvasGroup>("BG");
        endingmask = transform.FindGameObjectByName<Transform>("endingmask");
        wall = transform.FindGameObjectByName<CanvasGroup>("wall");
        bg1 = transform.FindGameObjectByName<CanvasGroup>("BG1");
        wall1 = transform.FindGameObjectByName<CanvasGroup>("wall1");
        hint = transform.FindGameObjectByName<Animation>("Hint");
        roy = transform.FindGameObjectByName<Animation>("Char");
        //드래그 오브젝트 컴포넌트 셋팅
        dragObjects = new DragObject[4];
        panel = transform.FindGameObjectByName("panel0").transform;
        Transform group = transform.FindGameObjectByName("Group_dragObject").transform;
        for (int i = 0; i < dragObjects.Length; i++)
        {
            dragObjects[i] = group.GetChild(i).gameObject.AddComponent<DragObject>();
        }
        fix.enabled = false;
        //슬롯 오브젝트 컴포넌트 셋팅
        slotObjects = new SlotObject[3];
        group = transform.FindGameObjectByName("Group_slotObject").transform;
        for (int i = 0; i < slotObjects.Length; i++)
        {
            slotObjects[i] = group.GetChild(i).gameObject.AddComponent<SlotObject>();
        }

        ropeObjects = new RopeObject[7];
        group = transform.FindGameObjectByName("RopeObject").transform;
        for (int i = 0; i < ropeObjects.Length; i++)
        {
            ropeObjects[i] = group.GetChild(i).gameObject.AddComponent<RopeObject>();
        }
        if (anim != null)
            anim.gameObject.SetActive(true);
    }
    private void Start()
    {
        for (int i = 0; i < ropeObjects.Length; i++)
        {
            ropeObjects[i].GetComponent<RopeObject>().PosSave();
            ropeObjects[i].transform.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(ropeObjects[i].originpos.x, ropeObjects[i].originpos.y + 150, 0);
        }
        Study_1_GameManager.Instance.PauseTimer();
        if (isIntroAnim != true)
        {
            Intro();
        }
        //for (int i = 0; i < ropeObjects.Length; i++)
        //{
        //    float a = ropeObjects[0].Pos.y - ropeObjects[i].Pos.y;
        //    StartCoroutine(MoveToposition(ropeObjects[i].transform, new Vector3(ropeObjects[i].originpos.x, (ropeObjects[i].originpos.y - a), ropeObjects[i].originpos.z), 0.5f));
        //}
    }
    public IEnumerator MoveToposition(Transform transform, Vector3 p, float timeToMove = 1)
    {
        var currentPos = transform.GetComponent<RectTransform>().anchoredPosition3D;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            transform.GetComponent<RectTransform>().anchoredPosition3D = Vector3.Lerp(currentPos, p, t);
            yield return null;
        }
    }
    public void Intro()
    {
        StartCoroutine(IEIntro());
    }
    public IEnumerator IEIntro()
    {
        //ropeObjects[0].transform.parent.gameObject.SetActive(true);
        //만일 시작 연출이 따로 있다면 이곳에 작성.
        SFX_Prefab_Script.Instance.PlayOneShot(11);
        for (int i = 0; i < ropeObjects.Length; i++)
        {
            StartCoroutine(MoveToposition(ropeObjects[i].transform, new Vector3(ropeObjects[i].originpos.x, (ropeObjects[i].originpos.y), ropeObjects[i].originpos.z), 0.4f));
            StartCoroutine(StarShake(ropeObjects[i].transform.GetComponent<RectTransform>(), 0.35f, 30f));
        }
        fix.enabled = true;
        yield return new WaitUntil(() => fix.GetCurrentAnimatorStateInfo(0).IsName("fix") && fix.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        fix.SetTrigger("idle");
        
        StartCoroutine(NextQuestion());
        yield break;
    }
    public void SlotImage()
    {
        if (curslot != null)
            curslot.GetComponent<SlotObject>().Origin();
        curslot = null;
    }
    Queue<cal_ws> makeQ = new Queue<cal_ws>();
    cal_ws a = new cal_ws();
    bool returnbool = false;
    int returnnum = 0;
    public int night = 0;
    bool imagechange = false;
    public void ImageChange(int _n)
    {
        fixhead.sprite = fixheadsprite[_n];
    }
    /// <summary>
    /// 문제 로직을 짜는 부분.
    /// </summary>
    public void MakeQuestion()
    {

        for (int i = 0; i < maxQno; i++)
        {
            if (returnbool)
            {
                i = returnnum;
                returnbool = false;
            }
            questions[i].a = new List<string>();
            questions[i].a.Clear();
            questions[i].question = new List<int>();
            questions[i].question.Clear();
            if (i < maxQno *0.25f)
            {
                questions[i].stage = 0;
                a.MakeQuestion(0, true);
            }
            else if(i>=maxQno*0.25f&&i<maxQno*0.5f)
            {
                questions[i].stage = 1;
                a.MakeQuestion(Random.Range(1,3), true);
            }
            else if (i >= maxQno * 0.5f && i < maxQno * 0.75f)
            {
                questions[i].stage = 0;
                a.MakeQuestion(0, true);
            }
            else
            {
                questions[i].stage = 1;
                a.MakeQuestion(Random.Range(1, 3), true);
            }
            for (int y = 0; y < a.questioninfo.Count; y++)
            {
                questions[i].a.Add(a.questioninfo[y]);
            }
            if (i < maxQno / 2)
            {
                int rand = Random.Range(0, 2);
                int rand1 = Random.Range(0, 2);
                string[] easy = { "+", "-" };
                string[] hard = { "×", "÷" };
                int num = 0;
                string s = "";
                for (int y = 0; y < questions[i].a.Count; y++)
                {
                    if (!int.TryParse(questions[i].a[y], out returnnum))
                    {
                        if (questions[i].a[y] == easy[rand])
                            questions[i].question.Add(y);
                        if (questions[i].a[y] == hard[rand1])
                            questions[i].question.Add(y);
                    }
                    s += questions[i].a[y];
                }
                questions[i].a.Add("=");
                s += "= ";
                s.Tostring1(a.answer.ToString());

                questions[i].a.Add(a.answer.ToString());
                questions[i].answer = a.answer;
            }
            else
            {
                int num = 0;
                string s = "";
                for (int y = 0; y < questions[i].a.Count; y++)
                {
                    if (!int.TryParse(questions[i].a[y], out returnnum)&&questions[i].a[y]!="("&&questions[i].a[y]!=")")
                    {
                        questions[i].question.Add(y);
                    }
                    s += questions[i].a[y];
                }
                questions[i].a.Add("=");
                s += "= ";
                s.Tostring1(a.answer.ToString());

                questions[i].a.Add(a.answer.ToString());
                questions[i].answer = a.answer;
            }
            
        }
    }
    bool wrong = false;
    public void SettQuestion()
    {
        Transform group = transform.FindGameObjectByName("Group_slotObject").transform;
        panel = transform.FindGameObjectByName("panel0").transform;
        group = transform.FindGameObjectByName("panel1").transform;
        panel.GetComponent<CanvasGroup>().alpha = 0;
        group.GetComponent<CanvasGroup>().alpha = 0;
        group = transform.FindGameObjectByName("Group_slotObject").transform;
        List<int> a = new List<int>();
        a.Clear();

        int rand = Random.Range(0, questions[curQno].question.Count);
        for (int i = 0; i < questions[curQno].question.Count;)
        {
            if (a.Contains(rand))
            {
                rand = Random.Range(0, questions[curQno].question.Count);
                continue;
            }
            else
            {
                a.Add(rand);
                i++;
            }
        }

        questions[curQno].q = new List<Text>();
        questions[curQno].q.Clear();
        for (int i = 0; i < slotObjects.Length; i++)
        {
            slotObjects[i].transform.SetParent(group);
            slotObjects[i].gameObject.SetActive(false);
        }
        if (questions[curQno].stage == 0)
        {
            panel = transform.FindGameObjectByName("panel0").transform;
            group = transform.FindGameObjectByName("panel0").transform;
            Funtion_CWS.Funtion_CWS.FindGameObjectByNameend(group, "num", "answer", "abcd", 20, questions[curQno].q);
            for (int i = 0; i < questions[curQno].a.Count; i++)
            {
                questions[curQno].q[i].text = questions[curQno].a[i];
                questions[curQno].q[i].gameObject.SetActive(true);
            }
        }
        else
        {
            panel = transform.FindGameObjectByName("panel1").transform;
            group = transform.FindGameObjectByName("panel1").transform;
            Funtion_CWS.Funtion_CWS.FindGameObjectByNameend(group, "num", "answer", "abcd", 20, questions[curQno].q);
            for (int i = 0; i < questions[curQno].a.Count; i++)
            {
                questions[curQno].q[i].text = questions[curQno].a[i];
                questions[curQno].q[i].gameObject.SetActive(true);
            }
        }

        if (curQno < maxQno/2)
        {
            
            for (int i = 0; i < 2; i++)
            {
                slotObjects[i].gameObject.SetActive(true);
            }

            string t = group.GetChild(questions[curQno].question[a[0]]).GetComponent<Text>().text;
            string t1 = group.GetChild(questions[curQno].question[a[1]]).GetComponent<Text>().text;
            group.GetChild(questions[curQno].question[a[0]]).gameObject.SetActive(false);
            group.GetChild(questions[curQno].question[a[1]]).gameObject.SetActive(false);
            if (a[0] > a[1])
            {
                slotObjects[0].name = t;
                slotObjects[0].transform.SetParent(group);
                slotObjects[0].transform.SetSiblingIndex(questions[curQno].question[a[0]]);
                slotObjects[1].name = t1;
                slotObjects[1].transform.SetParent(group);
                slotObjects[1].transform.SetSiblingIndex(questions[curQno].question[a[1]]);
            }
            else
            {
                slotObjects[0].name = t1;
                slotObjects[0].transform.SetParent(group);
                slotObjects[0].transform.SetSiblingIndex(questions[curQno].question[a[1]]);
                slotObjects[1].name = t;
                slotObjects[1].transform.SetParent(group);
                slotObjects[1].transform.SetSiblingIndex(questions[curQno].question[a[0]]);
            }
            slotObjects[0].text.text = slotObjects[0].name;
            slotObjects[1].text.text = slotObjects[1].name;
            slotObjects[1].text.GetComponent<CanvasGroup>().alpha = 0;
            slotObjects[0].text.GetComponent<CanvasGroup>().alpha = 0;
        }
        else
        {
            for (int i=0;i<3;i++)
            {
                slotObjects[i].gameObject.SetActive(true);
            }
            night = 1;
            string t = group.GetChild(questions[curQno].question[a[0]]).GetComponent<Text>().text;
            string t1 = group.GetChild(questions[curQno].question[a[1]]).GetComponent<Text>().text;
            string t2 = group.GetChild(questions[curQno].question[a[2]]).GetComponent<Text>().text;
            group.GetChild(questions[curQno].question[a[0]]).gameObject.SetActive(false);
            group.GetChild(questions[curQno].question[a[1]]).gameObject.SetActive(false);
            group.GetChild(questions[curQno].question[a[2]]).gameObject.SetActive(false);
            if (a[2] > a[1] && a[1] > a[0])
            {
                slotObjects[0].name = t2;
                slotObjects[0].transform.SetParent(group);
                slotObjects[0].transform.SetSiblingIndex(questions[curQno].question[a[2]]);
                slotObjects[1].name = t1;
                slotObjects[1].transform.SetParent(group);
                slotObjects[1].transform.SetSiblingIndex(questions[curQno].question[a[1]]);
                slotObjects[2].name = t;
                slotObjects[2].transform.SetParent(group);
                slotObjects[2].transform.SetSiblingIndex(questions[curQno].question[a[0]]);
            }
            else if (a[2] > a[1] && a[1] < a[0] && a[2] > a[0])
            {
                slotObjects[0].name = t2;
                slotObjects[0].transform.SetParent(group);
                slotObjects[0].transform.SetSiblingIndex(questions[curQno].question[a[2]]);
                slotObjects[1].name = t;
                slotObjects[1].transform.SetParent(group);
                slotObjects[1].transform.SetSiblingIndex(questions[curQno].question[a[0]]);
                slotObjects[2].name = t1;
                slotObjects[2].transform.SetParent(group);
                slotObjects[2].transform.SetSiblingIndex(questions[curQno].question[a[1]]);
            }
            else if (a[1] > a[2] && a[2] > a[0])
            {
                slotObjects[0].name = t1;
                slotObjects[0].transform.SetParent(group);
                slotObjects[0].transform.SetSiblingIndex(questions[curQno].question[a[1]]);
                slotObjects[1].name = t2;
                slotObjects[1].transform.SetParent(group);
                slotObjects[1].transform.SetSiblingIndex(questions[curQno].question[a[2]]);
                slotObjects[2].name = t;
                slotObjects[2].transform.SetParent(group);
                slotObjects[2].transform.SetSiblingIndex(questions[curQno].question[a[0]]);
            }
            else if (a[1] > a[2] && a[1] > a[0] && a[2] < a[0])
            {
                slotObjects[0].name = t1;
                slotObjects[0].transform.SetParent(group);
                slotObjects[0].transform.SetSiblingIndex(questions[curQno].question[a[1]]);
                slotObjects[1].name = t;
                slotObjects[1].transform.SetParent(group);
                slotObjects[1].transform.SetSiblingIndex(questions[curQno].question[a[0]]);
                slotObjects[2].name = t2;
                slotObjects[2].transform.SetParent(group);
                slotObjects[2].transform.SetSiblingIndex(questions[curQno].question[a[2]]);
            }
            else if (a[0] > a[2] && a[2] > a[1])
            {
                slotObjects[0].name = t;
                slotObjects[0].transform.SetParent(group);
                slotObjects[0].transform.SetSiblingIndex(questions[curQno].question[a[0]]);
                slotObjects[1].name = t2;
                slotObjects[1].transform.SetParent(group);
                slotObjects[1].transform.SetSiblingIndex(questions[curQno].question[a[2]]);
                slotObjects[2].name = t1;
                slotObjects[2].transform.SetParent(group);
                slotObjects[2].transform.SetSiblingIndex(questions[curQno].question[a[1]]);
            }
            else
            {
                slotObjects[0].name = t;
                slotObjects[0].transform.SetParent(group);
                slotObjects[0].transform.SetSiblingIndex(questions[curQno].question[a[0]]);
                slotObjects[1].name = t1;
                slotObjects[1].transform.SetParent(group);
                slotObjects[1].transform.SetSiblingIndex(questions[curQno].question[a[1]]);
                slotObjects[2].name = t2;
                slotObjects[2].transform.SetParent(group);
                slotObjects[2].transform.SetSiblingIndex(questions[curQno].question[a[2]]);
            }
            slotObjects[0].text.text = slotObjects[0].name;
            slotObjects[1].text.text = slotObjects[1].name;
            slotObjects[2].text.text = slotObjects[2].name;
            slotObjects[2].text.GetComponent<CanvasGroup>().alpha = 0;
            slotObjects[1].text.GetComponent<CanvasGroup>().alpha = 0;
            slotObjects[0].text.GetComponent<CanvasGroup>().alpha = 0;
        }

        group.GetComponent<HorizontalLayoutGroup>().enabled = true;
        LayoutRebuilder.ForceRebuildLayoutImmediate(group.GetComponent<RectTransform>());
    }
    int wrongnum = 0;
    public IEnumerator NextQuestion()
    {
        fiximge = false;
        for (int i = 0; i < ropeObjects.Length; i++)
        {
            ropeObjects[i].ImageChange1(false);
            //ropeObjects[i].ImageChange(0);
        }
        SettQuestion();

        wrongnum = 0;
        for (int i = 0; i < dragObjects.Length; i++)
        {
            dragObjects[i].image.sprite = normalimage[night];
        }
        if (curQno>=maxQno/2)
        {
            if (!imagechange)
            {
                for (int i = 0; i < slotObjects.Length; i++)
                {
                    slotObjects[i].GetComponent<Image>().sprite = slotimage;
                    slotObjects[i].border.color = slotcolor[night];
                }
                mask.GetComponent<Image>().sprite = bgimgae;
                StartCoroutine(bg1.SetAlphaCanvasGroup(true, 1));
                StartCoroutine(wall1.SetAlphaCanvasGroup(true, 1));
                yield return new WaitForSeconds(1);
                imagechange = true;
            }
        }
        SFX_Prefab_Script.Instance.PlayOneShot(14);
        mask.alpha = 1;
        panel.GetComponent<CanvasGroup>().alpha = 1;
        StartCoroutine(mask.SetAlphaCanvasGroup(false));
        StartCoroutine(BoxPop(dragObjects[0].transform));
        StartCoroutine(BoxPop(dragObjects[1].transform));
        StartCoroutine(BoxPop(dragObjects[2].transform));
        StartCoroutine(BoxPop(dragObjects[3].transform));
        yield return new WaitForSeconds(0.5f);
        if (curQno == 0)
        {
            Study_1_GameManager.Instance.SetPenColor(c);
            Study_1_GameManager.Instance.PauseEndTimer();
        }
        Study_1_GameManager.Instance.Draw_Clear();
        dragObjects[0].GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        dragObjects[1].GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        dragObjects[2].GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        dragObjects[3].GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        curDragObject = null;
        isSelectEnable = true;
        yield break;
    }
    public IEnumerator BoxPop(Transform _T, bool _B = true)
    {
        if (_B)
        {
            yield return StartCoroutine(_T.SmoothExpandScale(Vector3.one * 1.1f, 0.2f));
            PlaySoundManager.Instance.winstar_sound_play();
            yield return StartCoroutine(_T.SmoothExpandScale(Vector3.one, 0.2f));
        }
        else
        {
            yield return StartCoroutine(_T.SmoothExpandScale(Vector3.one * 1.1f, 0.2f));
            PlaySoundManager.Instance.winstar_sound_play();
            yield return StartCoroutine(_T.SmoothExpandScale(Vector3.zero, 0.2f));
        }
    }
    public IEnumerator TrueAnswer()
    {
        SFX_Prefab_Script.Instance.PlayOneShot(17);
        hint.transform.GetComponent<Button>().enabled = false;
        fiximge = true;
        ImageChange(1);
        if(hint.GetComponent<RectTransform>().localScale.x>0.3f)
        hint.Play("hintend");
        SFX_Prefab_Script.Instance.PlayOneShot(18);
        roy.Play();
        SFX_Prefab_Script.Instance.PlayOneShot(13);
        for (int i = 0; i < ropeObjects.Length; i++)
        {
            ropeObjects[i].ImageChange1(true);
            //ropeObjects[i].ImageChange(1);
        }
        wrongnum = 0;
        wrong = false;
        Study_1_GameManager.Instance.answer_true(2);
        Study_1_GameManager.Instance.next_question_bar();
        if (curQno == maxQno - 1)
            Study_1_GameManager.Instance.StopTimer();
        yield return new WaitForSeconds(1);

        if (curQno < maxQno - 1)
        {
            StartCoroutine(mask.GetComponent<CanvasGroup>().SetAlphaCanvasGroup(true));
            Transform group = transform.FindGameObjectByName("Group_dragObject").transform;
            for (int i = 0; i < 4; i++)
            {
                if (group == dragObjects[i].transform.parent)
                    StartCoroutine(BoxPop(dragObjects[i].transform, false));
            }
            yield return new WaitForSeconds(0.5f);
            panel.GetComponent<CanvasGroup>().alpha = 0;
            mask.alpha = 0;
            dragObjects[0].GetComponent<RectTransform>().localScale = new Vector2(0, 0);
            dragObjects[1].GetComponent<RectTransform>().localScale = new Vector2(0, 0);
            dragObjects[2].GetComponent<RectTransform>().localScale = new Vector2(0, 0);
            dragObjects[3].GetComponent<RectTransform>().localScale = new Vector2(0, 0);
            List<Coroutine> coList = new List<Coroutine>();
            if (night==0)
            {
                coList.Add(slotObjects[0].StartCoroutine(slotObjects[0].DetatchDragObject()));
                coList.Add(slotObjects[1].StartCoroutine(slotObjects[1].DetatchDragObject()));
                foreach (Coroutine co in coList)
                {
                    yield return co;
                }
            }
            else
            {
                for (int i = 0; i < slotObjects.Length; i++)
                {
                    coList.Add(slotObjects[i].StartCoroutine(slotObjects[i].DetatchDragObject()));
                }
                foreach (Coroutine co in coList)
                {
                    yield return co;
                }
            }
            yield return new WaitForSeconds(0.2f);
            Study_1_GameManager.Instance.next_question_number();
            curQno++;
            ImageChange(0);
            StartCoroutine(NextQuestion());

        }
        else
        {

            yield return new WaitForSeconds(0.4f);
            endingmask.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            ropeObjects[1].transform.parent.gameObject.SetActive(false);
            //outro_anim.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            Outro();
        }
        yield break;
    }
    bool fiximge = false;
    public IEnumerator Fixhead()
    {
        float f = Random.Range(0.1f, 1.9f);
        if(!fiximge)
        {
            yield return new WaitForSeconds(f);
            ImageChange(2);
            yield return new WaitForSeconds(0.1f);
            ImageChange(0);
        }
    }
    public IEnumerator StarShake(RectTransform target, float time = 1f, float power = 20f)
    {
        if (Random.Range(0, 2) == 0)
        {
            yield return StartCoroutine(target.MoveToVector(new Vector3(target.anchoredPosition3D.x + 10, target.anchoredPosition3D.y, 0), 0.05f));
            yield return StartCoroutine(target.MoveToVector(new Vector3(target.anchoredPosition3D.x - 20, target.anchoredPosition3D.y, 0), 0.1f));
            yield return StartCoroutine(target.MoveToVector(new Vector3(target.anchoredPosition3D.x + 10, target.anchoredPosition3D.y, 0), 0.05f));
        }
        else
        {
            yield return StartCoroutine(target.MoveToVector(new Vector3(target.anchoredPosition3D.x - 10, target.anchoredPosition3D.y, 0), 0.05f));
            yield return StartCoroutine(target.MoveToVector(new Vector3(target.anchoredPosition3D.x + 20, target.anchoredPosition3D.y, 0), 0.1f));
            yield return StartCoroutine(target.MoveToVector(new Vector3(target.anchoredPosition3D.x - 10, target.anchoredPosition3D.y, 0), 0.05f));
        }

    }
    public IEnumerator WrongAnswer()
    {
        wrongnum++;
        Study_1_GameManager.Instance.wrong_answer(2);
        SFX_Prefab_Script.Instance.PlayOneShot(12);
        SFX_Prefab_Script.Instance.PlayOneShot(15);
        for (int i=0;i<ropeObjects.Length;i++)
        {
            StartCoroutine(StarShake(ropeObjects[i].transform.GetComponent<RectTransform>(),0.35f, 30f));
        }
        if (curQno < maxQno/2)
        {
            yield return slotObjects[0].transform.parent.GetComponent<RectTransform>().ShakeObjectByAnchored3D(0.35f, 15f);
            yield return new WaitForSeconds(0.5f);
            SFX_Prefab_Script.Instance.PlayOneShot(0);
            List<Coroutine> coList = new List<Coroutine>();
            coList.Add(slotObjects[0].StartCoroutine(slotObjects[0].DetatchDragObject()));
            coList.Add(slotObjects[1].StartCoroutine(slotObjects[1].DetatchDragObject()));
            foreach (Coroutine co in coList)
            {
                yield return co;
            }
            if (wrongnum >= 3)
            {
                if (!wrong)
                {
                    if (slotObjects[1].text.GetComponent<CanvasGroup>().alpha < 1)
                    {
                       if (hint.transform.GetComponent<RectTransform>().localScale.x <0.2f)
                       {
                            SFX_Prefab_Script.Instance.PlayOneShot(16);
                            hint.Play("hint");
                            yield return new WaitForSeconds(1);
                            hint.Play("hintidle");
                            hint.transform.GetComponent<Button>().enabled = true;
                       }
                    }
                }
            }
        }
        else
        {
            yield return slotObjects[0].transform.parent.GetComponent<RectTransform>().ShakeObjectByAnchored3D(0.35f, 15f);
            yield return new WaitForSeconds(0.5f);
            SFX_Prefab_Script.Instance.PlayOneShot(0);
            List<Coroutine> coList = new List<Coroutine>();
            for (int i = 0; i < slotObjects.Length; i++)
            {
                coList.Add(slotObjects[i].StartCoroutine(slotObjects[i].DetatchDragObject()));
            }
            foreach (Coroutine co in coList)
            {
                yield return co;
            }
            if (wrongnum >= 3 && wrongnum < 4)
            {
                if (!wrong)
                {
                    if (slotObjects[2].text.GetComponent<CanvasGroup>().alpha < 1)
                    {
                        if (hint.transform.GetComponent<RectTransform>().localScale.x < 0.2f)
                        {
                            SFX_Prefab_Script.Instance.PlayOneShot(16);
                            hint.Play("hint");
                            yield return new WaitForSeconds(1);
                            hint.Play("hintidle");
                            hint.transform.GetComponent<Button>().enabled = true;
                        }
                    }
                }
            }
            else if (wrongnum >= 4)
            {
                if (!wrong)
                {
                  if (hint.transform.GetComponent<RectTransform>().localScale.x < 0.2f)
                   {
                        if (slotObjects[2].text.GetComponent<CanvasGroup>().alpha < 1)
                        {
                            SFX_Prefab_Script.Instance.PlayOneShot(16);
                            hint.Play("hint");
                            yield return new WaitForSeconds(1);
                            hint.Play("hintidle");
                            hint.transform.GetComponent<Button>().enabled = true;
                        }
                        else if (slotObjects[2].text.GetComponent<CanvasGroup>().alpha > 0 && slotObjects[1].text.GetComponent<CanvasGroup>().alpha < 1)
                        {
                            hint.Play("hint");
                            yield return new WaitForSeconds(1);
                            hint.Play("hintidle");
                            hint.transform.GetComponent<Button>().enabled = true;
                        }
                   }
                }
            }
        }

        curDragObject = null;
        isSelectEnable = true;
        yield break;
    }
    public void ClickBtn()
    {
        hint.transform.GetComponent<Button>().enabled = false;
        if (!wrong)
        {
            SFX_Prefab_Script.Instance.PlayOneShot(10);
            hint.Play("hintend");
            StartCoroutine(Hint());
        }
        wrong = true;
    }
    public IEnumerator Hint()
    {
        if (night == 0)
        {
                yield return StartCoroutine(slotObjects[1].text.GetComponent<CanvasGroup>().SetAlphaCanvasGroup(true, 1));
        }
        else
        {
            if (wrongnum > 2 && wrongnum < 4)
            {
                yield return StartCoroutine(slotObjects[2].text.GetComponent<CanvasGroup>().SetAlphaCanvasGroup(true, 1));
            }
            else
            {
                if (wrongnum >= 4)
                {
                    if (slotObjects[2].text.GetComponent<CanvasGroup>().alpha == 0)
                        yield return StartCoroutine(slotObjects[2].text.GetComponent<CanvasGroup>().SetAlphaCanvasGroup(true, 1));
                    else
                        yield return StartCoroutine(slotObjects[1].text.GetComponent<CanvasGroup>().SetAlphaCanvasGroup(true, 1));
                }
            }
        }
        wrong = false;
        yield break;
    }
    public void Outro()
    {
        if (outro_anim != null)
            outro_anim.GetComponent<Animator>().enabled=true;
        else
            Study_1_GameManager.Instance.next_question_number();
    }
    public bool IsAllSlotFull()
    {
        if (curQno < maxQno/2)
        {
            if (slotObjects[0].dragObject == null||slotObjects[1].dragObject == null)
            {
                return false;
            }
            return true;
        }
        else
        {
            for (int i = 0; i < slotObjects.Length; i++)
            {
                if (slotObjects[i].dragObject == null)
                {
                    return false;
                }
            }
            return true;
        }
    }
    public void CheckAnswer()
    {

        if (IsAllSlotFull() == true)
        {
            Text[] a = panel.GetComponentsInChildren<Text>();
            List<string> ans = new List<string>();
            string s = "";
            foreach (Text child in a)
            {
                if (child.name != "end" && child.name != "answer" && child.name != "hint")
                    ans.Add(child.text);
            }
            for (int i = 0; i < ans.Count; i++)
            {
                s += ans[i];
            }
            if (Funtion_CWS.Funtion_CWS.Equation(s) == questions[curQno].answer)
            {
                isSelectEnable = false;
                StartCoroutine(TrueAnswer());
            }
            else
            {
                isSelectEnable = false;
                StartCoroutine(WrongAnswer());
            }
        }
    }
    public class SlotObject : MonoBehaviour
    {
        public Sprite originsprite;
        public Image image;
        public Image border;
        public Text text;
        public RectTransform rectTr;
        public DragObject dragObject;
        public void Awake()
        {
            text = GetComponentInChildren<Text>();
            image = GetComponentInChildren<Image>();
            rectTr = GetComponent<RectTransform>();
            originsprite = image.sprite;

            border = transform.FindGameObjectByName<Image>("border");
        }
        public void Origin()
        {
            border.enabled = false;
        }
        /// <summary>
        /// DragObject가 SlotObject에 놓일때 호출이 되는 함수
        /// </summary>
        /// <param name="_dragObject"></param>
        public void OnDrop(DragObject _dragObject)
        {
            if (dragObject != null)//이미 드래그 오브젝트가 들어 있는 경우
            {
                dragObject.StopAllCoroutines();
                dragObject.StartCoroutine(dragObject.BackToOrigin());
            }
            dragObject = _dragObject;
        }
        /// <summary>
        /// slotObject에 있는 DragObject를 드래그할때 호출이 되는 함수
        /// </summary>
        public void OnLeave()
        {
            dragObject = null;
        }
        /// <summary>
        /// slotObject에 있는 dragObject를 비우는 함수
        /// </summary>
        /// <returns></returns>
        public IEnumerator DetatchDragObject()
        {
            dragObject.StartCoroutine(dragObject.BackToOrigin());
            dragObject = null;
            yield break;
        }
    }
    [RequireComponent(typeof(EventTrigger))]
    public class DragObject : MonoBehaviour
    {
        public RectTransform rectTr;
        public Image image;
        public Text text;
        public Vector3 originPos;
        public EventTrigger trigger;
        public Transform originParent;
        bool isDragging;
        bool isEnableTouch;
        SlotObject slot = null;
        int? nowTouch = null;
        bool moused = false;
        private void Awake()
        {
            rectTr = GetComponent<RectTransform>();
            image = transform.FindGameObjectByName<Image>("Image");
            text = GetComponentInChildren<Text>();
            trigger = GetComponent<EventTrigger>();
            trigger.AddTrigger(EventTriggerType.PointerDown, OnPointerDown);
            trigger.AddTrigger(EventTriggerType.PointerUp, OnPointerUp);
            originPos = rectTr.anchoredPosition3D;
            originParent = transform.parent;
            isEnableTouch = true;
        }
        /// <summary>
        /// 셋업 함수
        /// </summary>
        /// <param name="_problem"></param>
        public void SetUp()
        {
            //text.text = problem.q0;
        }
        /// <summary>
        /// 클릭 시 호출이 되는 함수
        /// </summary>
        /// <param name="data"></param>
        public void OnPointerDown(PointerEventData data)
        {

            if (isEnableTouch == true && Instance.isSelectEnable == true && Instance.curDragObject == null)
            {
                if (Input.touchCount > 0 && nowTouch == null)
                {

                    for (int i = 0; i < Input.touchCount; i++)
                    {
                        if (TouchPhase.Began == Input.GetTouch(i).phase && RectTransformUtility.RectangleContainsScreenPoint(transform.GetComponent<RectTransform>(), (Vector2)Input.GetTouch(i).position, Camera.main))
                        {
                            SFX_Prefab_Script.Instance.PlayOneShot(2);
                            //터치로 드래그 시작되는 부분
                            
                            if (slot != null)
                            {
                                slot.OnLeave();
                                slot = null;

                                transform.SetParent(originParent);
                            }
                            moused = false;
                            nowTouch = Input.GetTouch(i).fingerId;
                            isDragging = true;
                            Instance.curDragObject = this;
                            isEnableTouch = false;
                            StartCoroutine(Dragging());
                        }
                    }
                }
                else if (Input.GetMouseButtonDown(0) && nowTouch == null)
                {
                    if (RectTransformUtility.RectangleContainsScreenPoint(transform.GetComponent<RectTransform>(), (Vector2)Input.mousePosition, Camera.main))
                    {
                        SFX_Prefab_Script.Instance.PlayOneShot(2);
                        //마우스로 드래그 시작되는 부분
                        
                        if (slot != null)
                        {
                            slot.OnLeave();
                            slot = null;

                            transform.SetParent(originParent);
                        }
                        moused = true;
                        nowTouch = -111;
                        isDragging = true;
                        Instance.curDragObject = this;
                        isEnableTouch = false;
                        StartCoroutine(Dragging());
                    }

                }

            }
        }
        /// <summary>
        /// OnPointerDown 함수에서 호출하는 함수로, 드래그 중일때 마우스포인터의 위치에 놓이게 하는 함수
        /// </summary>
        /// <returns></returns>
        public IEnumerator Dragging()
        {
            transform.SetAsLastSibling();
            Camera camera = Camera.main;
            Vector3 curPos = transform.position, mouseToWorldPos = transform.position, lerpPos = transform.position;

            bool touching = false;
            do
            {
                touching = false;

                if (moused == true)
                {
                    if (Input.GetMouseButton(0) && !(Input.GetMouseButtonUp(0)))
                    {
                        RaycastHit2D hit = Physics2D.Raycast(mouseToWorldPos, Vector3.forward);
                        if (hit == true)
                        {
                            slot = hit.transform.GetComponent<SlotObject>();

                            Instance.curslot = slot.transform;
                            slot.border.enabled = true;
                        }
                        else
                        {
                            Instance.SlotImage();
                        }
                        image.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
                        text.fontSize = 80;
                        touching = true;

                    }
                    mouseToWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mouseToWorldPos.z = 0;
                }
                else
                {
                    for (int i = 0; i < Input.touchCount; i++)
                    {
                        if (Input.GetTouch(i).fingerId == nowTouch)
                        {
                            if (Input.GetTouch(i).phase != TouchPhase.Ended)
                            {
                                RaycastHit2D hit = Physics2D.Raycast(mouseToWorldPos, Vector3.forward);
                                if (hit == true)
                                {
                                    slot = hit.transform.GetComponent<SlotObject>();

                                    Instance.curslot = slot.transform;
                                    slot.border.enabled = true;
                                }
                                else
                                {
                                    Instance.SlotImage();
                                }
                                image.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
                                text.fontSize = 80;
                                touching = true;

                            }
                            mouseToWorldPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
                            mouseToWorldPos.z = 0;
                        }

                    }

                }


                if (!touching)
                {
                    Instance.SlotImage();
                    nowTouch = null;
                    RaycastHit2D hit = Physics2D.Raycast(mouseToWorldPos, Vector3.forward);
                    if (hit == true)
                    {
                        slot = hit.transform.GetComponent<SlotObject>();
                        if (slot != null)//Drop
                        {
                            SFX_Prefab_Script.Instance.PlayOneShot(9);
                            //drop으로 서서히 이동
                            //OnDrop 호출
                            slot.OnDrop(this);
                            //slot으로 이동
                            transform.SetParent(slot.transform);
                            StartCoroutine(SetToSlot());
                        }
                    }
                    else //복귀
                    {
                        image.GetComponent<RectTransform>().sizeDelta = new Vector2(132, 140);
                        text.fontSize = 110;
                        PlaySoundManager.Instance.NonDropPlay();
                        StartCoroutine(BackToOrigin());
                    }
                    Instance.curDragObject = null;
                    break;
                }
                curPos = transform.position;

                if (Vector2.Distance(curPos, mouseToWorldPos) > 0.01f)
                    lerpPos = Vector2.Lerp(curPos, mouseToWorldPos, 15 * Time.deltaTime);


                transform.position = lerpPos;

                yield return null;
            } while (touching);
            yield break;
        }
        /// <summary>
        /// 마우스를 뗐을때 호출이 되는 함수
        /// </summary>
        /// <param name="data"></param>
        public void OnPointerUp(PointerEventData data)
        {
            //if (isDragging && Instance.curDragObject == this)
            //{
            //    isDragging = false;
            //    //EventSystem.current.IsPointerOverGameObject()
            //    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward);
            //    if (hit == true)
            //    {
            //        instance.isSelectEnable = false;
            //        slot = hit.transform.GetComponent<SlotObject>();
            //        if (slot != null)//Drop
            //        {
            //            //drop으로 서서히 이동
            //            //OnDrop 호출
            //            slot.OnDrop(this);
            //            //slot으로 이동
            //            transform.SetParent(slot.transform);
            //            StartCoroutine(SetToSlot());
            //        }
            //    }
            //    else //복귀
            //    {
            //        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(142, 142);
            //        text.fontSize = 100;
            //        PlaySoundManager.Instance.NonDropPlay();
            //        StartCoroutine(BackToOrigin());
            //    }
            //    Instance.curDragObject = null;
            //}
        }
        /// <summary>
        /// 원래의 자리로 돌아가는 함수
        /// </summary>
        /// <returns></returns>
        public IEnumerator BackToOrigin()
        {
            isEnableTouch = false;
            //초기화 start
            transform.SetParent(originParent);
            slot = null;
            isDragging = false;
            //초기화 end
            float curTime = 0;
            Vector3 deltaPos = rectTr.anchoredPosition3D - originPos;
            while (curTime < 1)
            {
                rectTr.anchoredPosition3D = originPos + deltaPos * (1 - curTime);
                yield return null;
                curTime += 5 * Time.deltaTime;
            }
            image.GetComponent<RectTransform>().sizeDelta = new Vector2(132, 140);
            rectTr.GetChild(1).GetComponent<Text>().fontSize = 110;
            rectTr.anchoredPosition3D = originPos;
            isEnableTouch = true;
            yield break;
        }
        /// <summary>
        /// SlotObject에 놓일때 호출이 되는 함수
        /// </summary>
        /// <returns></returns>
        public IEnumerator SetToSlot()
        {
            float curTime = 0;
            Vector3 deltaPos = rectTr.anchoredPosition3D;
            //PlaySoundManager.Instance.DropPlay();
            SFX_Prefab_Script.Instance.PlayOneShot(9);
            while (curTime < 1)
            {
                rectTr.anchoredPosition3D = deltaPos * (1 - curTime);
                yield return null;
                curTime += 5 * Time.deltaTime;
            }

            rectTr.anchoredPosition3D = Vector3.zero;
            isEnableTouch = true;
            instance.isSelectEnable = true;
            //정답 체크
            Instance.CheckAnswer();
        }
    }
}
public class RopeObject : MonoBehaviour
{
    public Vector3 Pos;
    public Vector3 originpos;
    public Image image;
    public void Awake()
    {
        Pos = transform.position;
        //Pos = GetComponent<RectTransform>().anchoredPosition3D;
        image = GetComponentInChildren<Image>();
        image.transform.GetChild(0).gameObject.AddComponent<CanvasGroup>().alpha = 0;
    }
    public void PosSave()
    {
        originpos = GetComponent<RectTransform>().anchoredPosition3D;
        //originpos = transform.position;
    }
    public void ImageChange(int num)
    {
        if(image.name=="red")
        {
            image.sprite = Stage09112_Manager.Instance.redstar[num];
        }
        else
            image.sprite = Stage09112_Manager.Instance.yellowstar[num];
    }
    public void ImageChange1(bool b)
    {
        StartCoroutine(image.transform.GetChild(0).GetComponent<CanvasGroup>().SetAlphaCanvasGroup(b));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ExpandFuncs_Common;
using Funtion_CWS;
using UnityEngine.EventSystems;
public class Stage09116_Manager : GetInstance<Stage09116_Manager>
{
    Transform main;
    Transform panel,panel1, gwahopanel;
    public struct QuestionInfo
    {
        public List<int> question;
        public List<string> a;
        public List<string> a1;
        public List<Text> q;
        public List<Text> q1;
        public List<Text> q2;
        public int answer,gwaho,bra;
    }
    float screenx;//실제 world position 가로길이
    float screeny;//실제 world position 세로길이
    public Transform angle;
    public QuestionInfo[] questions;
    /// <summary>
    /// 문제전환 애니메이션 예시를 위한 캔버스그룹
    /// </summary>
    GameObject outro_anim;
    /// <summary>
    /// 인트로 애니메이션 유무
    /// </summary>
    bool isIntroAnim = false;
    public bool isSelectEnable = false;
    public DragObject curDragObject = null;
    public Transform ball;
    Text gwaho;
    Animator game;
    CanvasGroup mask;
    public Transform currslot;
    public DragObject[] dragObjects;
    public SlotObject[] slotObjects;
    Transform tree;
    public Animation box,hand;
    RectTransform boxs, larm, rarm;
    bool Boxscale = true;
    [Header("Inspector Setting")]
    public AnimationCurve cur;
    public Material m;
    public Color c;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        //m = transform.FindGameObjectByName<Material>("tree");
        //m.SetFloat("_SmokeTime", 1.5f);
        tree = transform.FindGameObjectByName<Transform>("tree");
        tree.GetComponent<Image>().material.SetFloat("_SmokeTime", 2);
        ball = transform.FindGameObjectByName<Transform>("ball");
        angle = transform.FindGameObjectByName<Transform>("angle");
        maxQno = Prefab_DB.Instance.prefab_db.max_q;
        game = transform.FindGameObjectByName<Animator>("main");
        questions = new QuestionInfo[maxQno];
        gwaho = transform.FindGameObjectByName<Text>("gwaho");
        MakeQuestion();
        box = transform.FindGameObjectByName<Animation>("Box");
        hand = transform.FindGameObjectByName<Animation>("hand");
        gwahopanel = transform.FindGameObjectByName<Transform>("gwahopanel");
        Animator anim = transform.FindGameObjectByName<Animator>("Intro");
        outro_anim = transform.FindGameObjectByName("Outro");
        isIntroAnim = anim != null;
        main = transform.FindGameObjectByName<Transform>("main");
        mask = transform.FindGameObjectByName<CanvasGroup>("mask");
        //드래그 오브젝트 컴포넌트 셋팅
        dragObjects = new DragObject[1];
        panel = transform.FindGameObjectByName("panel0").transform;
        boxs = box.transform.Find("box").GetComponent<RectTransform>();
        larm = box.transform.Find("Larm").GetComponent<RectTransform>();
        rarm = box.transform.Find("Rarm").GetComponent<RectTransform>();
        panel1 = transform.FindGameObjectByName("panel1").transform;
        Transform group = transform.FindGameObjectByName("Group_dragObject").transform;
        for (int i = 0; i < dragObjects.Length; i++)
        {
            dragObjects[i] = group.GetChild(i).gameObject.AddComponent<DragObject>();
        }
        //tree.GetComponent<Image>().material.SetFloat("_SmokeTime", 1.2f);
        //슬롯 오브젝트 컴포넌트 셋팅
        slotObjects = new SlotObject[4];
        group = transform.FindGameObjectByName("Group_slotObject").transform;
        for (int i = 0; i < slotObjects.Length; i++)
        {
            slotObjects[i] = group.GetChild(i).gameObject.AddComponent<SlotObject>();
        }
        if (anim != null)
            anim.gameObject.SetActive(true);
    }

    private void Start()
    {
        EP_DrawPadRT.Instance.ChangePen2Color(6);

        Study_1_GameManager.Instance.PauseTimer();
        if (isIntroAnim != true)
        {
            Intro();
        }
    }

    public IEnumerator BoxAni(bool B)
    {


        if (B)
        {
            if (Boxscale)
            {
                StopCoroutine("TimeScale1");
                StopCoroutine("MoveToVector1");
                StartCoroutine("TimeScale");
                yield return StartCoroutine("MoveToVector");
            }
        }
        else
        {
            StopCoroutine("TimeScale");
            StopCoroutine("MoveToVector");
            StartCoroutine("TimeScale1");
            yield return StartCoroutine("MoveToVector1");
        }
    }
    public IEnumerator TimeScale()
    {
        Vector3 p = new Vector3(0.9f, 1f, 1);
        var currentPos = boxs.localScale;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / 0.5f;
            boxs.localScale = Vector3.Lerp(currentPos, p, t);
            yield return null;
        }
    }
    public IEnumerator TimeScale1()
    {
        Vector3 p = new Vector3(1, 1, 1);
        var currentPos = boxs.localScale;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / 0.5f;
            boxs.localScale = Vector3.Lerp(currentPos, p, t);
            yield return null;
        }
    }
    public IEnumerator MoveToVector()
    {
        var currentPos = larm.anchoredPosition3D;
        var currentPos1 = rarm.anchoredPosition3D;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / 0.5f;
            larm.anchoredPosition3D = Vector3.Lerp(currentPos, new Vector3(-254,-326,0), t);
            rarm.anchoredPosition3D = Vector3.Lerp(currentPos1, new Vector3(261,-326,0), t);
            yield return null;
        }
    }
    public IEnumerator MoveToVector1()
    {
        var currentPos = larm.anchoredPosition3D;
        var currentPos1 = rarm.anchoredPosition3D;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / 0.5f;
            larm.anchoredPosition3D = Vector3.Lerp(currentPos, new Vector3(-270, -326, 0), t);
            rarm.anchoredPosition3D = Vector3.Lerp(currentPos1, new Vector3(279, -326, 0), t);
            yield return null;
        }
    }

    public IEnumerator treealpha()
    {

        //tree.GetComponent<Image>().material = m;
        tree.GetComponent<Image>().material.SetFloat("_SmokeTime", 1.5f);
        float a = tree.GetComponent<Image>().material.GetFloat("_SmokeTime");
        float currentfloat = a;
        float t = 0;
        while (t<1)
        {
            t += Time.deltaTime/1;
            a = currentfloat - (currentfloat * t);
            tree.GetComponent<Image>().material.SetFloat("_SmokeTime", a);
            yield return null;
        }
    }
    public override void Intro()
    {
        StartCoroutine(IEIntro());
    }
    List<Transform> textpappin = new List<Transform>();
    public IEnumerator TextPos()
    {
        textpappin.Clear();
        Transform group = transform.FindGameObjectByName("Group_slotObject").transform;
        gwaho.transform.SetParent(main);
        gwaho.gameObject.SetActive(false);
        for (int i = 0; i < slotObjects.Length; i++)
        {
            slotObjects[i].transform.SetParent(group);
            if(slotObjects[i].transform==currslot)
            {
                StartCoroutine(currslot.GetComponent<CanvasGroup>().SetAlphaCanvasGroup(false, 3));
            }
            else
            slotObjects[i].gameObject.SetActive(false);
        }
        //for (int i = 0; i < questions[curQno].q.Count; i++)
        //{
        //    questions[curQno].q[i].gameObject.SetActive(true);
        //}
        for (int i = 0; i < questions[curQno].q.Count; i++)
        {
            questions[curQno].q[i].transform.GetComponent<RectTransform>().sizeDelta = questions[curQno].q1[i].transform.GetComponent<RectTransform>().sizeDelta;
            StartCoroutine(questions[curQno].q[i].transform.MoveToPosition(questions[curQno].q1[i].transform, 0.5f));
        }
        yield return new WaitForSeconds(0.5f);
        SFX_Prefab_Script.Instance.PlayOneShot(10);
        
        for (int i = 0; i < questions[curQno].q.Count; i++)
        {
            if(questions[curQno].q[i].gameObject.active==false)
            {
                questions[curQno].q[i].gameObject.SetActive(true);
                StartCoroutine(questions[curQno].q[i].GetComponent<CanvasGroup>().SetAlphaCanvasGroup(true, 1));
                textpappin.Add(questions[curQno].q[i].transform);
            }
        }
    }
    public IEnumerator IEIntro()
    {
        yield return new WaitUntil(() => game.GetCurrentAnimatorStateInfo(0).IsName("intro") && game.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        SFX_Prefab_Script.Instance.PlayOneShot(1);
        yield return StartCoroutine(box.transform.MoveToVector(new Vector3(0, 0, 0), 1));
        SFX_Prefab_Script.Instance.PlayOneShot(3);
        yield return StartCoroutine(box.transform.Find("angle").MoveToVector(new Vector3(0, -144, 0), 1));
        Study_1_GameManager.Instance.PauseEndTimer();
        StartCoroutine(NextQuestion());
        yield break;
    }
    public IEnumerator BoxMove()
    {
        yield return StartCoroutine(box.transform.MoveToVector(new Vector3(0, -219, 0), 1));
    }
    Queue<cal_ws> makeQ = new Queue<cal_ws>();
    cal_ws a = new cal_ws();
    bool returnbool = false;
    int returnnum = 0;
    public int night = 0;
    bool imagechange = false;
    /// <summary>
    /// 문제 로직을 짜는 부분.
    /// </summary>
    public override void MakeQuestion()
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
            questions[i].a1 = new List<string>();
            questions[i].a1.Clear();
            questions[i].question = new List<int>();
            questions[i].question.Clear();
            a.MakeQuestion1(1, true);
            if(a.question_real.Length==5)
            {
                returnnum = i;
                returnbool = true;
                MakeQuestion();
                return;
            }
            int bra = 0;
            string s = ""; //s에는 식이들어감
            string s1 = ""; //괄호식이들어감
            for (int y = 0; y < a.questioninfo.Count; y++)
            {
                if (a.questioninfo[y] == "(")
                    bra = y;

                questions[i].a.Add(a.questioninfo[y]);
                s += a.questioninfo[y];
            }
            questions[i].bra = bra;
            for(int y=bra+1;y<bra+4;y++)
            {
                questions[i].a1.Add(a.questioninfo[y]);
                s1 += a.questioninfo[y];
            }
            questions[i].gwaho = Funtion_CWS.Funtion_CWS.Equation(s1);
            for (int y = 0; y < a.question_real.Length; y++)
            {
                if (a.question_real[y] == questions[i].gwaho || a.question_real[y] == a.answer||questions[i].gwaho==a.answer||a.answer>99||questions[i].gwaho==1)
                {
                    returnnum = i;
                    returnbool = true;
                    MakeQuestion();
                    return;
                }
                for (int z = 0; z < a.question_real.Length; z++)
                {
                    if (a.question_real[y] == a.question_real[z] && y != z)
                    {
                        returnnum = i;
                        returnbool = true;
                        MakeQuestion();
                        return;
                    }
                }
            }
            int num = 0;
            questions[i].a.Add("=");
            s += "= ";
            s.Tostring1(a.answer.ToString());

            questions[i].answer = a.answer;
            questions[i].a.Add(a.answer.ToString());
            for(int y=0;y<questions[i].a.Count;y++)
            {
                if(int.TryParse(questions[i].a[y],out num))
                {
                    if(y<bra||y>bra+4)
                    {
                        questions[i].question.Add(y);
                    }
                }
            }
            questions[i].a1.Add("=");
            questions[i].a1.Add(questions[i].gwaho.ToString());

        }
    }
    bool wrong = false;
    public override void SettingQuestion()
    {
        Transform group = transform.FindGameObjectByName("Group_slotObject").transform;
        panel.GetComponent<CanvasGroup>().alpha = 0;

        questions[curQno].q = new List<Text>();
        questions[curQno].q.Clear();
        questions[curQno].q1 = new List<Text>();
        questions[curQno].q1.Clear();
        questions[curQno].q2 = new List<Text>();
        questions[curQno].q2.Clear();
        gwaho.transform.SetParent(main);
        gwaho.transform.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-41, -224, 0);
        for (int i = 0; i < slotObjects.Length; i++)
        {
            slotObjects[i].GetComponent<CanvasGroup>().alpha = 1;
            slotObjects[i].transform.SetParent(group);
            slotObjects[i].gameObject.SetActive(false);
        }
        int z = 0;
        Funtion_CWS.Funtion_CWS.FindGameObjectByNameend(panel, "num", "answer", "abcd", 20, questions[curQno].q);
        Funtion_CWS.Funtion_CWS.FindGameObjectByNameend(panel1, "num", "answer", "abcd", 20, questions[curQno].q1);
        Funtion_CWS.Funtion_CWS.FindGameObjectByNameend(gwahopanel, "num", "answer", "abcd", 20, questions[curQno].q2);
        for (int i = 0; i < questions[curQno].a.Count; i++)
        {
            questions[curQno].q[i].text = questions[curQno].a[i];
            questions[curQno].q1[i].text = questions[curQno].a[i];
            questions[curQno].q[i].GetComponent<CanvasGroup>().alpha = 1;
            questions[curQno].q[i].gameObject.SetActive(true);
        }
        for(int i = 0; i < questions[curQno].a.Count; i++)
        {
            if (int.TryParse(questions[curQno].q[i].text, out z))
            {
                if (i < questions[curQno].bra || i > questions[curQno].bra + 4)
                {
                    questions[curQno].q[i].GetComponent<RectTransform>().sizeDelta = new Vector2(120, 102);
                }
                else
                {
                    if(questions[curQno].q[i].text.Length==1)
                    questions[curQno].q[i].GetComponent<RectTransform>().sizeDelta = new Vector2(45, 102);
                    else
                        questions[curQno].q[i].GetComponent<RectTransform>().sizeDelta = new Vector2(90, 102);
                }
            }
            else
            {
                questions[curQno].q[i].GetComponent<RectTransform>().sizeDelta = new Vector2(46, 102);
            }
        }
        for(int i=0;i<questions[curQno].a1.Count;i++)
        {
            questions[curQno].q2[i].text = questions[curQno].a1[i];
        }
        for (int i = 0; i < slotObjects.Length; i++)
        {
            slotObjects[i].gameObject.SetActive(true);
        }

        string t = panel.GetChild(questions[curQno].question[0]).GetComponent<Text>().text;
        string t1 = panel.GetChild(questions[curQno].question[1]).GetComponent<Text>().text;
        string t2 = panel.GetChild(questions[curQno].question[2]).GetComponent<Text>().text;
        //panel.GetChild(questions[curQno].question[0]).gameObject.SetActive(false);
        //panel.GetChild(questions[curQno].question[1]).gameObject.SetActive(false);
        //panel.GetChild(questions[curQno].question[2]).gameObject.SetActive(false);
        for(int i=questions[curQno].bra;i<=questions[curQno].bra+4;i++)
        {
            panel.GetChild(i).GetComponent<CanvasGroup>().alpha = 0;
            panel.GetChild(i).gameObject.SetActive(false);
        }
        gwaho.gameObject.SetActive(true);
        gwaho.text = questions[curQno].gwaho.ToString();
        gwaho.transform.SetParent(panel);
        gwaho.transform.SetSiblingIndex(questions[curQno].bra);



        panel.GetComponent<HorizontalLayoutGroup>().enabled = true;
        LayoutRebuilder.ForceRebuildLayoutImmediate(panel.GetComponent<RectTransform>());
        panel.GetComponent<HorizontalLayoutGroup>().enabled = false;

        slotObjects[0].text.text = t2;
        slotObjects[1].text.text = t1;
        slotObjects[2].text.text = t;
        slotObjects[3].text.text = questions[curQno].gwaho.ToString();

        slotObjects[0].transform.SetParent(panel);
        slotObjects[1].transform.SetParent(panel);
        slotObjects[2].transform.SetParent(panel);
        slotObjects[3].transform.SetParent(panel);

        for (int i = 0; i < slotObjects.Length; i++)
        {
            for (int y = 0; y < 12; y++)
            {
                if(slotObjects[i].text.text==panel.GetChild(y).GetComponent<Text>().text)
                {
                    slotObjects[i].GetComponent<RectTransform>().anchoredPosition3D = panel.GetChild(y).GetComponent<RectTransform>().anchoredPosition3D;
                }
            }
        }


    }
    int wrongnum = 0;
    public override IEnumerator NextQuestion()
    {
        Boxscale = true;
        ball.gameObject.SetActive(true);
        isSelectEnable = false;
        dragObjects[0].GetComponent<CanvasGroup>().alpha = 1;
        dragObjects[0].pointline.GetComponent<CanvasGroup>().alpha = 1;
        ball.SetParent(box.transform);
        ball.SetAsFirstSibling();
        ball.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, -255, 0);
        ball.GetComponent<RectTransform>().localScale = Vector3.one;
        ball.GetComponent<CanvasGroup>().alpha = 1;
        dragObjects[0].gameObject.SetActive(true);
        dragObjects[0].image.enabled = false;
        panel.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-1.8f, 38, 0);
        gwahopanel.GetComponent<CanvasGroup>().alpha = 0;
        SettingQuestion();
        SFX_Prefab_Script.Instance.PlayOneShot(4);
        yield return StartCoroutine(Stage09116_point.Instance().PointCreate(0.1f));
        
        for (int i=0;i<slotObjects.Length;i++)
        {
            slotObjects[i].GetComponent<RectTransform>().localScale = Vector3.zero;
        }
        dragObjects[0].image.enabled = true;
        yield return StartCoroutine(BoxPop1(dragObjects[0].transform));

        while (panel.GetComponent<CanvasGroup>().alpha<0.3f)
        {
            panel.GetComponent<CanvasGroup>().alpha += Time.deltaTime * 0.5f;
            yield return null;
        }

        SFX_Prefab_Script.Instance.PlayOneShot(18);
        yield return StartCoroutine(panel.MoveToPosition(panel1, 1));
        StartCoroutine(gwahopanel.GetComponent<CanvasGroup>().SetAlphaCanvasGroup(true, 1));
        panel.GetComponent<CanvasGroup>().alpha = 1;

        for (int i = 0; i < slotObjects.Length; i++)
        {
            StartCoroutine(BoxPop(slotObjects[i].transform));
        }
        yield return new WaitForSeconds(0.5f);
        if (curQno == 0)
        {
            SFX_Prefab_Script.Instance.PlayOneShot(2);
            hand.Play();
        }
        curDragObject = null;
        isSelectEnable = true;
        yield break;
    }
    public IEnumerator BoxPop(Transform _T, bool _B = true)
    {
        if (_B)
        {
            yield return StartCoroutine(_T.SmoothExpandScale(Vector3.one * 1.1f, 0.2f));
            
            yield return StartCoroutine(_T.SmoothExpandScale(Vector3.one, 0.2f));
        }
        else
        {
            yield return StartCoroutine(_T.SmoothExpandScale(Vector3.one * 1.1f, 0.2f));
            yield return StartCoroutine(_T.SmoothExpandScale(Vector3.zero, 0.2f));
        }
    }
    public IEnumerator BoxPop1(Transform _T, bool _B = true)
    {
        if (_B)
        {
            yield return StartCoroutine(_T.SmoothExpandScale(Vector3.one * 1.2f, 0.2f));
            yield return StartCoroutine(_T.SmoothExpandScale(Vector3.one, 0.2f));
        }
        else
        {
            yield return StartCoroutine(_T.SmoothExpandScale(Vector3.one * 1.1f, 0.2f));
            yield return StartCoroutine(_T.SmoothExpandScale(Vector3.zero, 0.2f));
        }
    }
    public IEnumerator Ball(float t)
    {
        SFX_Prefab_Script.Instance.PlayOneShot(7);
        StartCoroutine(ball.TimeScale(0.6f, 0.6f, t));
        yield return StartCoroutine(ball.MoveToVector(Vector3.one, t));
        currslot.GetComponent<Animator>().enabled = true;
        SFX_Prefab_Script.Instance.PlayOneShot(9);
        yield return new WaitUntil(() => currslot.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("boxpung") && currslot.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        //yield return StartCoroutine(ball.GetComponent<CanvasGroup>().SetAlphaCanvasGroup(false,1));
    }
    public void SlotImage()
    {
        currslot = null;
        for (int i = 0; i < 4; i++)
            slotObjects[i].taduri.enabled = false;
    }
    public IEnumerator TextPappin()
    {
        for(int i=0;i<textpappin.Count;i++)
        {
            if(i==textpappin.Count-1)
                yield return StartCoroutine(textpappin[i].SetScaleByAnimationCurve(cur,4));
            else
            StartCoroutine(textpappin[i].SetScaleByAnimationCurve(cur,4));
        }
    }
     bool imagechange1 = false;
     bool imagechange2 = false;
    public override IEnumerator TrueAnswer()
    {
        dragObjects[0].pointline.GetComponent<CanvasGroup>().alpha = 0;
        dragObjects[0].GetComponent<CanvasGroup>().alpha = 0;
        StartCoroutine(dragObjects[0].BackToOrigin());
        Study_1_GameManager.Instance.answer_true(2);
        Study_1_GameManager.Instance.next_question_bar();
        if (curQno == maxQno - 1)
            Study_1_GameManager.Instance.StopTimer();
        yield return new WaitUntil(() => boxs.localScale.x < 0.91f);
        dragObjects[0].gameObject.SetActive(false);
        //yield return StartCoroutine(ball.MoveToVector(new Vector3(0,-161,0), 0.7f));
        ball.SetParent(currslot);
        ball.SetAsFirstSibling();
        box.Play();
        yield return new WaitForSeconds(0.25f);
        yield return StartCoroutine(Ball(0.5f));
        yield return StartCoroutine(TextPos());
        yield return new WaitForSeconds(1);
        SFX_Prefab_Script.Instance.PlayOneShot(11);
        yield return StartCoroutine(TextPappin());
        currslot.GetComponent<Animator>().enabled = false;
        SlotImage();
        if (curQno < maxQno - 1)
        {
            if(curQno>=maxQno*0.25f&&curQno<maxQno*0.5f)
            {
                if(imagechange==false)
                {
                    imagechange = true;
                    game.SetTrigger("next");
                    yield return new WaitUntil(() => game.GetCurrentAnimatorStateInfo(0).IsName("next") && game.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
                }
                else
                    yield return new WaitForSeconds(1);
            }
            else if(curQno>=maxQno*0.5f&&curQno<maxQno*0.75f)
            {
                if (imagechange1 == false)
                {
                    imagechange1 = true;
                    game.SetTrigger("next1");
                    yield return new WaitUntil(() => game.GetCurrentAnimatorStateInfo(0).IsName("next1") && game.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
                }
                else
                    yield return new WaitForSeconds(1);
            }
            else if(curQno >= maxQno * 0.75f && curQno < maxQno)
            {
                if (imagechange2 == false)
                {
                    imagechange2 = true;
                    game.SetTrigger("next2");
                    yield return new WaitUntil(() => game.GetCurrentAnimatorStateInfo(0).IsName("next2") && game.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
                }
                else
                    yield return new WaitForSeconds(1);
            }
            else
                yield return new WaitForSeconds(1);

            StartCoroutine(panel.GetComponent<CanvasGroup>().SetAlphaCanvasGroup(false));
            StartCoroutine(gwahopanel.GetComponent<CanvasGroup>().SetAlphaCanvasGroup(false));
            yield return new WaitForSeconds(0.5f);
            Study_1_GameManager.Instance.next_question_number();
            curQno++;

            StartCoroutine(NextQuestion());

        }
        else
        {
            yield return new WaitForSeconds(1);
            StartCoroutine(panel.GetComponent<CanvasGroup>().SetAlphaCanvasGroup(false));
            StartCoroutine(gwahopanel.GetComponent<CanvasGroup>().SetAlphaCanvasGroup(false));
            SFX_Prefab_Script.Instance.PlayOneShot(1);
            yield return StartCoroutine(box.transform.MoveToVector(new Vector3(0, -445, 0), 1));
            box.transform.gameObject.SetActive(false);
            //outro_anim.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            SFX_Prefab_Script.Instance.PlayOneShot(22);
            game.SetTrigger("ending");
            //Outro();
        }
        yield break;
    }

    public override IEnumerator WrongAnswer()
    {
        StartCoroutine(Stage09116_Manager.Instance().BoxAni(false));
        Stage09116_Manager.Instance().Boxscale = true;
        Study_1_GameManager.Instance.wrong_answer(2);
        yield return box.transform.GetComponent<RectTransform>().ShakeObjectByAnchored3D(0.35f, 15f);
        yield return new WaitForSeconds(0.5f);

        curDragObject = null;
        isSelectEnable = true;
        yield break;
    }

    public override void Outro()
    {
        if (outro_anim != null)
            outro_anim.GetComponent<Animator>().enabled = true;
        else
            Study_1_GameManager.Instance.next_question_number();
    }
    //public bool IsAllSlotFull()
    //{
    //   for (int i = 0; i < slotObjects.Length; i++)
    //   {
    //       if (slotObjects[i].dragObject == null)
    //       {
    //           return false;
    //       }
    //   }
    //}
    public void CheckAnswer()
    {

        //if (IsAllSlotFull() == true)
        //{
        if (currslot != null)
        {
            if (currslot.GetComponent<SlotObject>().text.text == questions[curQno].gwaho.ToString())
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
        else
        {
            isSelectEnable = false;
            StartCoroutine(WrongAnswer());
        }
        //}
    }
    public class SlotObject : MonoBehaviour
    {
        public Text text;
        public Image image;
        public Image taduri;
        public RectTransform rectTr;
        public DragObject dragObject;
        public void Awake()
        {
            text = GetComponentInChildren<Text>();
            image = GetComponentInChildren<Image>();
            taduri = transform.FindGameObjectByName<Image>("taduri");
            rectTr = GetComponent<RectTransform>();
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
        public Transform t, t1, t2, t3;
        public RectTransform rectTr;
        public Image image;
        public Text text;
        public Vector3 originPos;
        public EventTrigger trigger;
        public Transform originParent;
        public Transform point;
        public Transform collider;
        public RectTransform pointline;
        bool isDragging;
        bool isEnableTouch;
        SlotObject slot = null;
        int? nowTouch = null;
        bool moused = false;
        Vector3 stopPos;
        float distance;
        private void Awake()
        {
            pointline = transform.parent.Find("points").GetComponent<RectTransform>();
            t = transform.parent.Find("nopo").GetComponent<RectTransform>();
            t1 = transform.parent.Find("nopo1").GetComponent<RectTransform>();
            t2 = transform.parent.Find("nopo2").GetComponent<RectTransform>();
            t3 = transform.parent.Find("nopo3").GetComponent<RectTransform>();
            distance = Vector3.Distance(pointline.transform.position, transform.position);
            point = transform.parent.Find("point").transform;
            rectTr = GetComponent<RectTransform>();
            image = transform.GetChild(0).GetComponent<Image>();
            text = GetComponentInChildren<Text>();
            trigger = GetComponent<EventTrigger>();
            trigger.AddTrigger(EventTriggerType.PointerDown, OnPointerDown);
            trigger.AddTrigger(EventTriggerType.PointerUp, OnPointerUp);
            originPos = rectTr.anchoredPosition3D;
            originParent = transform.parent;
            isEnableTouch = true;
        }
        private void Update()
        {
            Vector3 point0 = pointline.transform.position;
            Vector3 point1 = transform.position;
            float curdistance = Vector2.Distance(point0, point1);
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
            if (isEnableTouch == true && Instance().isSelectEnable == true && Instance().curDragObject == null)
            {
                Instance().hand.gameObject.SetActive(false);
                if (Input.touchCount > 0 && nowTouch == null)
                {

                    for (int i = 0; i < Input.touchCount; i++)
                    {
                        
                        if (TouchPhase.Began == Input.GetTouch(i).phase && RectTransformUtility.RectangleContainsScreenPoint(transform.GetComponent<RectTransform>(), (Vector2)Input.GetTouch(i).position, Camera.main))
                        {
                            //터치로 드래그 시작되는 부분
                            SFX_Prefab_Script.Instance.PlayOneShot(6);
                            SFX_Prefab_Script.Instance.PlayOneShot(5);
                            StartCoroutine(Stage09116_Manager.Instance().BoxAni(true));
                            Stage09116_Manager.Instance().Boxscale = false;

                            if (slot != null)
                            {
                                slot.OnLeave();
                                slot = null;

                                transform.SetParent(originParent);
                            }
                            PlaySoundManager.Instance.Spin_3_Play();
                            moused = false;
                            nowTouch = Input.GetTouch(i).fingerId;
                            isDragging = true;
                            Instance().curDragObject = this;
                            isEnableTouch = false;
                            StartCoroutine("Dragging");
                        }
                    }
                }
                else if (Input.GetMouseButtonDown(0) && nowTouch == null)
                {
                    if (RectTransformUtility.RectangleContainsScreenPoint(transform.GetComponent<RectTransform>(), (Vector2)Input.mousePosition, Camera.main))
                    {
                        SFX_Prefab_Script.Instance.PlayOneShot(6);
                        SFX_Prefab_Script.Instance.PlayOneShot(5);
                        StartCoroutine(Stage09116_Manager.Instance().BoxAni(true));
                        Stage09116_Manager.Instance().Boxscale = false;
                        //마우스로 드래그 시작되는 부분
                        
                        if (slot != null)
                        {
                            slot.OnLeave();
                            slot = null;

                            transform.SetParent(originParent);
                        }
                        PlaySoundManager.Instance.Spin_3_Play();
                        moused = true;
                        nowTouch = -111;
                        isDragging = true;
                        Instance().curDragObject = this;
                        isEnableTouch = false;
                        StartCoroutine("Dragging");
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
            bool Stop = false;
            Vector3 startPos = point.position;
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

                            Instance().currslot = slot.transform;
                            slot.taduri.enabled = true;
                        }
                        else
                        {
                            Instance().SlotImage();
                        }
                        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x < t.position.x)
                        {
                            Stop = true;
                            stopPos = mouseToWorldPos;
                        }
                        else if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x > t2.position.x)
                        {
                            Stop = true;
                            stopPos = mouseToWorldPos;
                        }
                        else if (Camera.main.ScreenToWorldPoint(Input.mousePosition).y < t1.position.y)
                        {
                            Stop = true;
                            stopPos = mouseToWorldPos;
                        }
                        else if (Camera.main.ScreenToWorldPoint(Input.mousePosition).y > t3.position.y)
                        {
                            Stop = true;
                            stopPos = mouseToWorldPos;
                        }
                        else
                        {
                            Stop = false;
                        }
                        //if (!Stop)
                        //{
                        //Vector3 point0 = pointline.transform.position;
                        //Vector3 point1 = transform.position;
                        //pointline.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(point1.y - point0.y, point1.x - point0.x) * Mathf.Rad2Deg);
                        //float curdistance = Vector2.Distance(pointline.transform.GetComponent<RectTransform>().anchoredPosition3D, rectTr.anchoredPosition3D);
                        //if(curdistance<200)
                        //pointline.sizeDelta = new Vector2(((curdistance/30))*25, 13);
                        //else
                        //    pointline.sizeDelta = new Vector2(((curdistance / 27)) * 25, 13);
                        //}
                        touching = true;
                    }
                    if (!Stop)
                    {
                        mouseToWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        mouseToWorldPos.z = 0;
                    }
                    else
                    {
                        mouseToWorldPos = stopPos;
                        mouseToWorldPos.z = 0;
                    }
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

                                    Instance().currslot = slot.transform;
                                    slot.taduri.enabled = true;
                                }
                                else
                                {
                                    Instance().SlotImage();
                                }
                                if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x < t.position.x)
                                {
                                    Stop = true;
                                    stopPos = mouseToWorldPos;
                                }
                                else if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x > t2.position.x)
                                {
                                    Stop = true;
                                    stopPos = mouseToWorldPos;
                                }
                                else if (Camera.main.ScreenToWorldPoint(Input.mousePosition).y < t1.position.y)
                                {
                                    Stop = true;
                                    stopPos = mouseToWorldPos;
                                }
                                else if (Camera.main.ScreenToWorldPoint(Input.mousePosition).y > t3.position.y)
                                {
                                    Stop = true;
                                    stopPos = mouseToWorldPos;
                                }
                                else
                                {
                                    Stop = false;
                                }
                                touching = true;
                                //if (!Stop)
                                //{
                                //Vector3 point0 = pointline.transform.position;
                                //Vector3 point1 = transform.position;
                                //pointline.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(point1.y - point0.y, point1.x - point0.x) * Mathf.Rad2Deg);

                                //}
                            }
                            if (!Stop)
                            {
                                mouseToWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                                mouseToWorldPos.z = 0;
                            }
                            else
                            {
                                mouseToWorldPos = stopPos;
                                mouseToWorldPos.z = 0;
                            }
                        }

                    }

                }


                if (!touching)
                {
                    nowTouch = null;
                    RaycastHit2D hit = Physics2D.Raycast(mouseToWorldPos, Vector3.forward);
                    if (hit == true)
                    {
                        slot = hit.transform.GetComponent<SlotObject>();
                        if (slot != null)//Drop
                        {
                            //drop으로 서서히 이동
                            //OnDrop 호출
                            //slot.OnDrop(this);
                            //slot으로 이동
                            //transform.SetParent(slot.transform);
                            StartCoroutine(SetToSlot());
                        }
                    }
                    else //복귀
                    {
                        //transform.GetComponent<RectTransform>().sizeDelta = new Vector2(132, 140);
                        //text.fontSize = 110;
                        Instance().SlotImage();
                        StartCoroutine(Stage09116_Manager.Instance().BoxAni(false));
                        Stage09116_Manager.Instance().Boxscale = true;
                        //PlaySoundManager.Instance.NonDropPlay();
                        StartCoroutine(BackToOrigin1());
                    }
                    Instance().curDragObject = null;
                    break;
                }
                curPos = transform.position;

                if (Vector2.Distance(curPos, mouseToWorldPos) > 0.01f)
                    lerpPos = Vector2.Lerp(curPos, mouseToWorldPos, 15 * Time.deltaTime);


                transform.position = lerpPos;

                Vector3 point0 = pointline.transform.position;
                Vector3 point1 = transform.position;
                pointline.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(point1.y - point0.y, point1.x - point0.x) * Mathf.Rad2Deg);

                float curdistance = Vector2.Distance(pointline.transform.GetComponent<RectTransform>().anchoredPosition3D, rectTr.anchoredPosition3D);
                if (curdistance < 200)
                    pointline.sizeDelta = new Vector2(((curdistance / 30)) * 25, 13);
                else
                    pointline.sizeDelta = new Vector2(((curdistance / 27)) * 25, 13);

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
            //rectTr.sizeDelta = new Vector2(132, 140);
            //rectTr.GetChild(0).GetComponent<Text>().fontSize = 110;
            rectTr.anchoredPosition3D = originPos;
            rectTr.eulerAngles = new Vector3(0, 0, 0);
            pointline.eulerAngles = new Vector3(0, 0, 90);
            isEnableTouch = true;
            Instance().curDragObject = null;
            Instance().isSelectEnable = true;
            nowTouch = null;
            yield break;
        }
        public IEnumerator BackToOrigin1()
        {
            isEnableTouch = false;
            //초기화 start
            transform.SetParent(originParent);
            slot = null;
            isDragging = false;
            //초기화 end
            isEnableTouch = true;
            Instance().curDragObject = null;
            Instance().isSelectEnable = true;
            nowTouch = null;
            yield break;
        }
        /// <summary>
        /// SlotObject에 놓일때 호출이 되는 함수
        /// </summary>
        /// <returns></returns>
        public IEnumerator SetToSlot()
        {
            //float curTime = 0;
            //Vector3 deltaPos = rectTr.anchoredPosition3D;
            //while (curTime < 1)
            //{
            //    rectTr.anchoredPosition3D = deltaPos * (1 - curTime);
            //    yield return null;
            //    curTime += 5 * Time.deltaTime;
            //}

            //rectTr.anchoredPosition3D = Vector3.zero;
            isEnableTouch = true;
            instance.isSelectEnable = true;
            //정답 체크
            Instance().CheckAnswer();
            yield break;
        }
        public void Stop()
        {
            StopCoroutine("Dragging");
            StartCoroutine(BackToOrigin());
            //rectTr.anchoredPosition3D = originPos;
            //rectTr.eulerAngles = new Vector3(0, 0, 0);
        }
    }
}
public class DragCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "nopo")
        {
            Stage09116_Manager.Instance().dragObjects[0].Stop();
        }
    }
}

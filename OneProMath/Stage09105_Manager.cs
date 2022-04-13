using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ExpandFuncs_Common;
using Funtion_CWS;
using System.Threading.Tasks;
public class Stage09105_Manager : MonoBehaviour, IStage, KeyPadUser
{
    private static Stage09105_Manager instance;
    public static Stage09105_Manager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<Stage09105_Manager>();
            return instance;
        }
    }
    public int curquestion = 0;
    Transform panelimage, panelimage1, panelimage2;
    int truenum = 0;
    Animator main;
    public Transform panel;
    int fornum = 0;
    string[] sign = { "+", "-", "×", "=" };
    Vector2[] panelpos;
    Text text_answer;
    Text text_question0, text_question1;
    Image image_answer;
    /// <summary>
    /// 문제전환 애니메이션 예시를 위한 캔버스그룹
    /// </summary>
    CanvasGroup cg_group, cg_group1;
    GameObject outro_anim;
    [Header("Inspector Setting")]
    public AnimationCurve poping_curve;
    public AnimationCurve poping_curve1;
    public Color c;
    public Color[] color;
    public struct QuestionInfo
    {
        public List<string> a;
        public List<string> a1;
        public List<string> a2;
        public List<Text> q;
        public List<Text> q1;
        public List<Text> q2;
        public string answer, answer1, answer2;
    }
    public QuestionInfo[] questions;
    Text answertext;
    bool returnbool = false;
    /// <summary>
    /// 최대 문제 개수
    /// </summary>
    int maxQno;
    /// <summary>
    /// 현재문제번호
    /// </summary>
    int curQno = 0;
    /// <summary>
    /// 현재 정답을 입력방지 여부 true:입력불가, false:입력가능
    /// </summary>
    bool answerCheck = true;
    /// <summary>
    /// 인트로 애니메이션 유무
    /// </summary>
    bool isIntroAnim = false;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        maxQno = Prefab_DB.Instance.prefab_db.max_q;
        main = transform.FindGameObjectByName<Animator>("main");
        cg_group = transform.FindGameObjectByName<CanvasGroup>("panel");
        cg_group1 = transform.FindGameObjectByName<CanvasGroup>("panel1");
        questions = new QuestionInfo[maxQno];
        //cg_group.alpha = 0;
        //text_answer.text = "";
        MakeQuestion();

        //
        //


        Animator anim = transform.FindGameObjectByName<Animator>("Intro");
        isIntroAnim = anim != null;

        if (anim != null)
            anim.gameObject.SetActive(true);
    }
    private void Start()
    {
        Study_1_GameManager.Instance.PauseTimer();
        Delegate_Keypad_SH.Instance.SetDelegate(this, true, 5);
        Study_1_GameManager.Instance.SetPenColor(c);
        if (isIntroAnim != true)
        {
            Intro();
        }
    }
    public void Intro()
    {
        //SettingQuestion();
        StartCoroutine(IEIntro());
    }
    public IEnumerator IEIntro()
    {
        //만일 시작 연출이 따로 있다면 이곳에 작성.
        main.GetComponent<Animator>().enabled = true;
        yield return new WaitUntil(() => main.GetCurrentAnimatorStateInfo(0).IsName("start") && main.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        Study_1_GameManager.Instance.PauseEndTimer();
        StartCoroutine(NextQuestion());
        yield break;
    }
    Queue<cal_ws> makeQ = new Queue<cal_ws>();
    cal_ws a = new cal_ws();
    object locker = new object();
    int size = 0;
    /// <summary>
    /// 문제 로직을 짜는 부분.
    /// </summary>
    public void MakeQuestion()
    {
        
        for (int i = 0; i < 2; i++)
        {
            int length=0;
            int numlenth = 0;
            if (returnbool)
            {
                i = returnnum;
                returnbool = false;
            }
            questions[i].a = new List<string>();
            questions[i].a1 = new List<string>();
            questions[i].a2 = new List<string>();
            questions[i].a.Clear();
            questions[i].a1.Clear();
            questions[i].a2.Clear();


            a.GihoRandmaker(true);
            for (int y = 0; y < 5; y++)
            {
                questions[i].a.Add(a.question_real[y].ToString());
                if (a.question_real[y].ToString().Length == 2)
                    length++;
                if (a.question_real[y] > 20)
                    numlenth++;
                questions[i].a1.Add(a.question_real[y].ToString());
                questions[i].a2.Add(a.question_real[y].ToString());
                if (y != 4)
                {
                    questions[i].a.Add(a.threegihos[0, y]);
                    questions[i].a1.Add(a.threegihos[1, y]);
                    questions[i].a2.Add(a.threegihos[2, y]);
                }
            }
            string s = "";
            string s1 = "";
            string s2 = "";
            for (int y = 0; y < 9; y++)
            {
                s += questions[i].a[y];
                s1 += questions[i].a1[y];
                s2 += questions[i].a2[y];
            }
            s += "=";
            s1 += "=";
            s2 += "=";
            questions[i].answer = Funtion_CWS.Funtion_CWS.Equation(s).ToString();
            questions[i].answer1 = Funtion_CWS.Funtion_CWS.Equation(s1).ToString();
            questions[i].answer2 = Funtion_CWS.Funtion_CWS.Equation(s2).ToString();
            for (int y = 0; y < i; y++)
            {
                if (questions[i].answer == questions[y].answer || questions[i].answer1 == questions[y].answer1 || questions[i].answer2 == questions[y].answer2)
                {
                    returnnum = i;
                    returnbool = true;
                    MakeQuestion();
                    return;
                }
                if (length>3||numlenth>2)
                {
                    returnnum = i;
                    returnbool = true;
                    MakeQuestion();
                    return;
                }
            }
#if UNITY_EDITOR
            s.Tostring1(questions[i].answer);
            s1.Tostring1(questions[i].answer1);
            s2.Tostring1(questions[i].answer2);
#endif
            makeQ.Enqueue(a);
            size++;





            //questions[i].a = new List<string>();
            //questions[i].a1 = new List<string>();
            //questions[i].a2 = new List<string>();

            //questions[i].a.Add(36.ToString());
            //questions[i].a.Add("+");
            //questions[i].a.Add(12.ToString());
            //questions[i].a.Add("÷");
            //questions[i].a.Add(6.ToString());
            //questions[i].a.Add("×");
            //questions[i].a.Add(2.ToString());
            //questions[i].a.Add("-");
            //questions[i].a.Add(2.ToString());


            //string s = "";
            //for (int y = 0; y < questions[i].a.Count; y++)
            //{
            //    questions[i].a1.Add("a");
            //    questions[i].a2.Add("a");
            //    s += questions[i].a[y];
            //}
            //string temp = questions[i].a[1];
            //string temp1 = questions[i].a[3];
            //string temp2 = questions[i].a[5];
            //string temp3 = questions[i].a[7];
            //questions[i].a1[1] = temp3;
            //questions[i].a1[3] = temp;
            //questions[i].a1[5] = temp1;
            //questions[i].a1[7] = temp2;
            //questions[i].a2[1] = temp2;
            //questions[i].a2[3] = temp3;
            //questions[i].a2[5] = temp;
            //questions[i].a2[7] = temp1;
            //string ss = "";
            //string sss = "";
            //for (int y = 0; y < questions[i].a.Count; y++)
            //{
            //    if (y % 2 == 0)
            //    {
            //        questions[i].a1[y] = questions[i].a[y];
            //        questions[i].a2[y] = questions[i].a[y];
            //    }
            //    ss += questions[i].a1[y];
            //    sss += questions[i].a2[y];
            //}
            //questions[i].answer = (Funtion_CWS.Funtion_CWS.Equation(s));
            //questions[i].answer1 = (Funtion_CWS.Funtion_CWS.Equation(ss));
            //questions[i].answer2 = (Funtion_CWS.Funtion_CWS.Equation(sss));
            //s.Tostring1(Funtion_CWS.Funtion_CWS.Equation(s).ToString());
            //ss.Tostring1(Funtion_CWS.Funtion_CWS.Equation(ss).ToString());
            //sss.Tostring1(Funtion_CWS.Funtion_CWS.Equation(sss).ToString());
        }
        Task task = new Task(Tasking);
        task.Start();
    }
    int returnnum = 0;
    public void Tasking()
    {
        
        for (int i = 2; i < (maxQno)/3; i++)
        {
            int length = 0;
            int numlength = 0;
            if (returnbool)
            {
                i = returnnum;
                returnbool = false;
            }
            questions[i].a = new List<string>();
            questions[i].a1 = new List<string>();
            questions[i].a2 = new List<string>();
            questions[i].a.Clear();
            questions[i].a1.Clear();
            questions[i].a2.Clear();


            a.GihoRandmaker(false);
            for (int y = 0; y < 5; y++)
            {
                questions[i].a.Add(a.question_real[y].ToString());
                if (a.question_real[y].ToString().Length == 2)
                    length++;
                if (a.question_real[y] > 20)
                    numlength++;
                questions[i].a1.Add(a.question_real[y].ToString());
                questions[i].a2.Add(a.question_real[y].ToString());
                if (y != 4)
                {
                    questions[i].a.Add(a.threegihos[0, y]);
                    questions[i].a1.Add(a.threegihos[1, y]);
                    questions[i].a2.Add(a.threegihos[2, y]);
                }
            }
            if (length > 3)
            {
                returnnum = i;
                returnbool = true;
                MakeQuestion();
                return;
            }
            string s = "";
            string s1 = "";
            string s2 = "";
            for (int y = 0; y < 9; y++)
            {
                s += questions[i].a[y];
                s1 += questions[i].a1[y];
                s2 += questions[i].a2[y];
            }
            s += "=";
            s1 += "=";
            s2 += "=";
            questions[i].answer = Funtion_CWS.Funtion_CWS.Equation(s).ToString();
            questions[i].answer1 = Funtion_CWS.Funtion_CWS.Equation(s1).ToString();
            questions[i].answer2 = Funtion_CWS.Funtion_CWS.Equation(s2).ToString();
            for(int y=0;y<i;y++)
            {
                if(questions[i].answer==questions[y].answer|| questions[i].answer1 == questions[y].answer1 || questions[i].answer2 == questions[y].answer2)
                {
                    returnnum = i;
                    returnbool = true;
                    Task task = new Task(Tasking);
                    task.Start();
                    return;
                }
                if (length > 3 || numlength > 2)
                {
                    returnnum = i;
                    returnbool = true;
                    Task task = new Task(Tasking);
                    task.Start();
                    return;
                }
            }
#if UNITY_EDITOR
            s.Tostring1(questions[i].answer);
            s1.Tostring1(questions[i].answer1);
            s2.Tostring1(questions[i].answer2);
#endif
            lock (locker)
            {
                makeQ.Enqueue(a);
                size++;
            }

        }
    }
    public Image line;
    public IEnumerator NextQuestion()
    {
        truenum = 0;
        answerCheck = true;
        if (curQno < 2)
        {
            panel = transform.FindGameObjectByName<Transform>("gameboard");
        }
        else
        {
            panel = transform.FindGameObjectByName<Transform>("gameboard1");
        }
        if(curQno<2)
           group = panel.FindGameObjectByName<Transform>("panel0");
        else
           group = panel.FindGameObjectByName<Transform>("panel3");
        line = panel.transform.FindGameObjectByName<Image>("line");
        line.color = color[0];
        answertext = group.Find("Image").Find("answer").GetComponent<Text>();
        panelimage = panel.GetChild(1).Find("Image");
        panelimage1 = panel.GetChild(2).Find("Image");
        panelimage2 = panel.GetChild(3).Find("Image");
        panelimage1.GetComponent<Image>().fillAmount = 0;
        panelimage2.GetComponent<Image>().fillAmount = 0;
        panelimage.Find("Image1").gameObject.SetActive(true);
        panelimage1.Find("Image1").gameObject.SetActive(true);
        panelimage2.Find("Image1").gameObject.SetActive(true);
        panelimage.Find("answer").GetComponent<Text>().text = "";
        panelimage.Find("answer").GetComponent<RectTransform>().anchoredPosition3D = new Vector3(99, -38.2f, 0);
        panelimage1.Find("answer").GetComponent<Text>().text = "";
        panelimage1.Find("answer").GetComponent<RectTransform>().anchoredPosition3D = new Vector3(99, -38.2f, 0);
        panelimage2.Find("answer").GetComponent<Text>().text = "";
        panelimage2.Find("answer").GetComponent<RectTransform>().anchoredPosition3D = new Vector3(99, -38.2f, 0);
        //panel.GetChild(1).GetComponent<CanvasGroup>().alpha = 1;
        panel.GetChild(2).GetComponent<CanvasGroup>().alpha = 0;
        panel.GetChild(3).GetComponent<CanvasGroup>().alpha = 0;

        SettingQuestion();
        yield return StartCoroutine(panel.GetChild(1).GetComponent<CanvasGroup>().SetAlphaCanvasGroup(true,1.5f));
        line.color = color[1];
        StartCoroutine(BoxSet());
        answerCheck = false;
        yield break;
    }
    public IEnumerator TrueAnswer()
    {
        answerCheck = true;
        Study_1_GameManager.Instance.answer_true(2);
        Study_1_GameManager.Instance.next_question_bar();
        if (curquestion == maxQno - 1)
            Study_1_GameManager.Instance.StopTimer();
        truenum++;
        if (truenum < 3)
        {
            if (truenum == 1)
                {
                if(curQno<2)
                    group = panel.FindGameObjectByName<Transform>("panel0");
                else
                    group = panel.FindGameObjectByName<Transform>("panel3");
                group.Find("Image").Find("Image1").gameObject.SetActive(false);
                    float x = group.GetChild(0).GetComponent<RectTransform>().anchoredPosition3D.x;
                    float x1 = group.GetChild(2).GetComponent<RectTransform>().anchoredPosition3D.x;
                    float x2 = group.GetChild(4).GetComponent<RectTransform>().anchoredPosition3D.x;
                    float x3 = group.GetChild(6).GetComponent<RectTransform>().anchoredPosition3D.x;
                    float x4 = group.GetChild(8).GetComponent<RectTransform>().anchoredPosition3D.x;
                    float s = group.GetChild(1).GetComponent<RectTransform>().anchoredPosition3D.x;
                    float s1 = group.GetChild(3).GetComponent<RectTransform>().anchoredPosition3D.x;
                    float s2 = group.GetChild(5).GetComponent<RectTransform>().anchoredPosition3D.x;
                    float s3 = group.GetChild(7).GetComponent<RectTransform>().anchoredPosition3D.x;
                    float y = group.GetChild(0).GetComponent<RectTransform>().anchoredPosition3D.y;
                if (questions[curQno].answer.Length == 1)
                {
                    yield return StartCoroutine(group.Find("Image").Find("answer").MoveToVector(new Vector3(71, -38.2f, 0),0.5f));
                }
                else if(questions[curQno].answer.Length == 2)
                {
                    yield return StartCoroutine(group.Find("Image").Find("answer").MoveToVector(new Vector3(80, -38.2f, 0), 0.5f));
                }
                else
                {
                    yield return StartCoroutine(group.Find("Image").Find("answer").MoveToVector(new Vector3(85, -38.2f, 0), 0.5f));
                }
                if (curQno < 2)
                    group = panel.FindGameObjectByName<Transform>("panel1");
                else
                    group = panel.FindGameObjectByName<Transform>("panel4");
                //int origin = 1;
                //for (int i = 0; i < questions[curQno].a.Count;)
                //{
                //if (i % 2 != 0)
                //{
                //    if (group.GetChild(i).GetComponent<Text>().text == questions[curQno].q[origin].text)
                //    {
                //        group.GetChild(i).GetComponent<RectTransform>().anchoredPosition3D = new Vector3(questions[curQno].q[origin].transform.GetComponent<RectTransform>().anchoredPosition3D.x, group.GetChild(i).GetComponent<RectTransform>().anchoredPosition3D.y, 0);
                //        origin = 1;
                //        i++;
                //    }
                //    else
                //        origin+=2;
                //}
                //else
                //    i++;
                //}
                //panel.GetComponent<HorizontalLayoutGroup>().enabled = true;
                //LayoutRebuilder.ForceRebuildLayoutImmediate(panel.GetComponent<RectTransform>());
                //panel.GetComponent<HorizontalLayoutGroup>().enabled = false;
                group.GetComponent<CanvasGroup>().alpha = 1;
                    group.Find("Image").GetComponent<Image>().fillAmount = 0;
                    group.Find("Image").GetComponent<RectTransform>().anchoredPosition3D = new Vector3(group.Find("Image").GetComponent<RectTransform>().anchoredPosition3D.x, group.Find("Image").GetComponent<RectTransform>().anchoredPosition3D.y - 154, 0);
                answertext = group.Find("Image").Find("answer").GetComponent<Text>();
                //PlaySoundManager.Instance.Spin_4_Play();
                    StartCoroutine(group.GetChild(1).MoveToVector(new Vector3(s, y - 154, 0), 0.7f));
                    StartCoroutine(group.GetChild(3).MoveToVector(new Vector3(s1, y - 154, 0), 0.7f));
                    StartCoroutine(group.GetChild(5).MoveToVector(new Vector3(s2, y - 154, 0), 0.7f));
                    StartCoroutine(group.GetChild(7).MoveToVector(new Vector3(s3, y - 154, 0), 0.7f));
                    StartCoroutine(group.GetChild(0).MoveToVector(new Vector3(x, y - 154, 0), 0.7f));
                    StartCoroutine(group.GetChild(2).MoveToVector(new Vector3(x1, y - 154, 0), 0.7f));
                    StartCoroutine(group.GetChild(4).MoveToVector(new Vector3(x2, y - 154, 0), 0.7f));
                    StartCoroutine(group.GetChild(6).MoveToVector(new Vector3(x3, y - 154, 0), 0.7f));
                    yield return StartCoroutine(group.GetChild(8).MoveToVector(new Vector3(x4, y - 154, 0), 0.7f));
                    StartCoroutine(group.GetChild(1).GetComponent<CanvasGroup>().SetAlphaCanvasGroup(true,1));
                    StartCoroutine(group.GetChild(3).GetComponent<CanvasGroup>().SetAlphaCanvasGroup(true,1));
                    StartCoroutine(group.GetChild(5).GetComponent<CanvasGroup>().SetAlphaCanvasGroup(true,1));
                    yield return StartCoroutine(group.GetChild(7).GetComponent<CanvasGroup>().SetAlphaCanvasGroup(true,1));
                PlaySoundManager.Instance.Button_Play();
                StartCoroutine(group.GetChild(1).transform.SetScaleByAnimationCurve1(poping_curve1, 4));
                StartCoroutine(group.GetChild(3).transform.SetScaleByAnimationCurve1(poping_curve1, 4));
                StartCoroutine(group.GetChild(5).transform.SetScaleByAnimationCurve1(poping_curve1, 4));
                yield return StartCoroutine(group.GetChild(7).transform.SetScaleByAnimationCurve1(poping_curve1, 4));
                yield return StartCoroutine(group.Find("Image").Barfill(true, 1.5f));
                    Study_1_GameManager.Instance.next_question_number();
                    curquestion++;
                    StartCoroutine(BoxSet());
                    answerCheck = false;
            }
            else if (truenum == 2)
                {
                if (curQno < 2)
                    group = panel.FindGameObjectByName<Transform>("panel1");
                else
                    group = panel.FindGameObjectByName<Transform>("panel4");
                group.Find("Image").Find("Image1").gameObject.SetActive(false);
                if (questions[curQno].answer1.Length == 1)
                {
                    yield return StartCoroutine(group.Find("Image").Find("answer").MoveToVector(new Vector3(71, -38.2f, 0), 0.5f));
                }
                else if (questions[curQno].answer1.Length == 2)
                {
                    yield return StartCoroutine(group.Find("Image").Find("answer").MoveToVector(new Vector3(80, -38.2f, 0), 0.5f));
                }
                else
                {
                    yield return StartCoroutine(group.Find("Image").Find("answer").MoveToVector(new Vector3(85, -38.2f, 0), 0.5f));
                }
                float x = group.GetChild(0).GetComponent<RectTransform>().anchoredPosition3D.x;
                float x1 = group.GetChild(2).GetComponent<RectTransform>().anchoredPosition3D.x;
                float x2 = group.GetChild(4).GetComponent<RectTransform>().anchoredPosition3D.x;
                float x3 = group.GetChild(6).GetComponent<RectTransform>().anchoredPosition3D.x;
                float x4 = group.GetChild(8).GetComponent<RectTransform>().anchoredPosition3D.x;
                float s = panel.GetChild(1).GetChild(1).GetComponent<RectTransform>().anchoredPosition3D.x;
                float s1 = panel.GetChild(1).GetChild(3).GetComponent<RectTransform>().anchoredPosition3D.x;
                float s2 = panel.GetChild(1).GetChild(5).GetComponent<RectTransform>().anchoredPosition3D.x;
                float s3 = panel.GetChild(1).GetChild(7).GetComponent<RectTransform>().anchoredPosition3D.x;
                float y = group.GetChild(0).GetComponent<RectTransform>().anchoredPosition3D.y;
                if (curQno < 2)
                    group = panel.FindGameObjectByName<Transform>("panel2");
                else
                    group = panel.FindGameObjectByName<Transform>("panel5");
                //int origin = 1;
                //    for (int i = 0; i < questions[curQno].a.Count;)
                //    {
                //    if (i % 2 != 0)
                //    {
                //        if (group.GetChild(i).GetComponent<Text>().text == questions[curQno].q1[origin].text)
                //        {
                //            group.GetChild(i).GetComponent<RectTransform>().anchoredPosition3D = new Vector3(questions[curQno].q1[origin].transform.GetComponent<RectTransform>().anchoredPosition3D.x, group.GetChild(i).GetComponent<RectTransform>().anchoredPosition3D.y, 0);
                //            origin = 1;
                //            i++;
                //        }
                //        else
                //            origin+=2;
                //    }
                //    else
                //        i++;
                //    }
                //questions[curQno].q.Clear();
                //Funtion_CWS.Funtion_CWS.FindGameObjectByNameend<Text>(panel, "num", "answer", "abcd", 15, questions[curQno].q);
                //for (int i = 0; i < questions[curQno].a.Count; i++)
                //{
                //    questions[curQno].q[i].text = questions[curQno].a[i];
                //}
                //panel.GetComponent<HorizontalLayoutGroup>().enabled = true;
                //LayoutRebuilder.ForceRebuildLayoutImmediate(panel.GetComponent<RectTransform>());
                //panel.GetComponent<HorizontalLayoutGroup>().enabled = false;
                answertext = group.Find("Image").Find("answer").GetComponent<Text>();
                group.GetComponent<CanvasGroup>().alpha = 1;
                group.Find("Image").GetComponent<Image>().fillAmount = 0;
                group.Find("Image").GetComponent<RectTransform>().anchoredPosition3D = new Vector3(group.Find("Image").GetComponent<RectTransform>().anchoredPosition3D.x, group.Find("Image").GetComponent<RectTransform>().anchoredPosition3D.y - 154, 0);
                
                
                //PlaySoundManager.Instance.Spin_4_Play();
                StartCoroutine(group.GetChild(1).MoveToVector(new Vector3(s, -204, 0), 0.7f));
                StartCoroutine(group.GetChild(3).MoveToVector(new Vector3(s1, -204, 0), 0.7f));
                StartCoroutine(group.GetChild(5).MoveToVector(new Vector3(s2, -204, 0), 0.7f));
                StartCoroutine(group.GetChild(7).MoveToVector(new Vector3(s3, -204, 0), 0.7f));
                StartCoroutine(group.GetChild(0).MoveToVector(new Vector3(x, -204, 0), 0.7f));
                StartCoroutine(group.GetChild(2).MoveToVector(new Vector3(x1, -204, 0), 0.7f));
                StartCoroutine(group.GetChild(4).MoveToVector(new Vector3(x2, -204, 0), 0.7f));
                StartCoroutine(group.GetChild(6).MoveToVector(new Vector3(x3, -204, 0), 0.7f));
                yield return StartCoroutine(group.GetChild(8).MoveToVector(new Vector3(x4, -204, 0), 0.7f));
                StartCoroutine(group.GetChild(1).GetComponent<CanvasGroup>().SetAlphaCanvasGroup(true, 1));
                StartCoroutine(group.GetChild(3).GetComponent<CanvasGroup>().SetAlphaCanvasGroup(true, 1));
                StartCoroutine(group.GetChild(5).GetComponent<CanvasGroup>().SetAlphaCanvasGroup(true, 1));
                yield return StartCoroutine(group.GetChild(7).GetComponent<CanvasGroup>().SetAlphaCanvasGroup(true, 1));
                PlaySoundManager.Instance.Button_Play();
                StartCoroutine(group.GetChild(1).transform.SetScaleByAnimationCurve1(poping_curve1, 4));
                StartCoroutine(group.GetChild(3).transform.SetScaleByAnimationCurve1(poping_curve1, 4));
                StartCoroutine(group.GetChild(5).transform.SetScaleByAnimationCurve1(poping_curve1, 4));
                yield return StartCoroutine(group.GetChild(7).transform.SetScaleByAnimationCurve1(poping_curve1, 4));
                

                //PlaySoundManager.Instance.Spin_4_Play();
                //StartCoroutine(group.GetChild(0).MoveToVector(new Vector3(x, y, 0), 0.7f));
                //StartCoroutine(group.GetChild(2).MoveToVector(new Vector3(x1, y, 0), 0.7f));
                //StartCoroutine(group.GetChild(4).MoveToVector(new Vector3(x2, y, 0), 0.7f));
                //StartCoroutine(group.GetChild(6).MoveToVector(new Vector3(x3, y, 0), 0.7f));
                //yield return StartCoroutine(group.GetChild(8).MoveToVector(new Vector3(x4, y, 0), 0.7f));

                //PlaySoundManager.Instance.Spin_4_Play();
                //StartCoroutine(group.GetChild(1).MoveToVector(new Vector3(s1, y, 0), 0.7f));
                //StartCoroutine(group.GetChild(3).MoveToVector(new Vector3(s2, y, 0), 0.7f));
                //StartCoroutine(group.GetChild(5).MoveToVector(new Vector3(s3, y, 0), 0.7f));
                //yield return StartCoroutine(group.GetChild(7).MoveToVector(new Vector3(s, y, 0), 0.7f));
                yield return StartCoroutine(group.Find("Image").Barfill(true, 1.5f));
                Study_1_GameManager.Instance.next_question_number();
                curquestion++;
                StartCoroutine(BoxSet());
                answerCheck = false;
            }
        }
        else
        {
            group.Find("Image").Find("Image1").gameObject.SetActive(false);
            if (questions[curQno].answer2.Length == 1)
            {
                yield return StartCoroutine(group.Find("Image").Find("answer").MoveToVector(new Vector3(71, -38.2f, 0), 0.5f));
            }
            else if (questions[curQno].answer2.Length == 2)
            {
                yield return StartCoroutine(group.Find("Image").Find("answer").MoveToVector(new Vector3(80, -38.2f, 0), 0.5f));
            }
            else
            {
                yield return StartCoroutine(group.Find("Image").Find("answer").MoveToVector(new Vector3(85, -38.2f, 0), 0.5f));
            }
            yield return new WaitForSeconds(1);
            curQno++;
            curquestion++;
            if (curquestion < maxQno)
            {
                StartCoroutine(panel.GetChild(1).GetComponent<CanvasGroup>().SetAlphaCanvasGroup(false));
                StartCoroutine(panel.GetChild(2).GetComponent<CanvasGroup>().SetAlphaCanvasGroup(false));
                yield return StartCoroutine(panel.GetChild(3).GetComponent<CanvasGroup>().SetAlphaCanvasGroup(false));
                if(curQno==2)
                {
                    main.SetTrigger("next");
                    yield return new WaitUntil(() => main.GetCurrentAnimatorStateInfo(0).IsName("next") && main.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
                }
                Study_1_GameManager.Instance.next_question_number();
                StartCoroutine(NextQuestion());
            }
            else
            {
                main.SetTrigger("last");
                yield return new WaitUntil(() => main.GetCurrentAnimatorStateInfo(0).IsName("last") && main.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
                Outro();
            }
        }
        yield break;
    }
    public IEnumerator WrongAnswer()
    {
        group.GetComponent<HorizontalLayoutGroup>().enabled = false;
        answerCheck = true;
        Study_1_GameManager.Instance.keypad_block(1);
        Study_1_GameManager.Instance.wrong_answer(2);
        group.Find("Image").Find("answer").GetComponent<Text>().color = Color.red;
        group.Find("Image").Find("end").SetParent(group);
        yield return StartCoroutine(group.Find("Image").GetComponent<RectTransform>().ShakeObjectByAnchored3D(0.35f, 15f));
        yield return new WaitForSeconds(1f);
        group.Find("end").SetParent(group.Find("Image"));
        group.Find("Image").Find("end").SetAsFirstSibling();
        answerCheck = false;
        yield break;
    }

    public void Outro()
    {
        if (outro_anim != null)
            outro_anim.SetActive(true);
        else
        {
            StartCoroutine(IEOutro());
        }

    }
    public IEnumerator IEOutro()
    {
        Study_1_GameManager.Instance.next_question_number();
        yield break;
    }
    //Keypad 함수들
    public void InputKeyEvent(string _input)
    {

        if (string.IsNullOrEmpty(_input) == false)
        {
            answertext.color = Color.black;
            answertext.text = _input;
        }
        else
        {
            answertext.text = "";
        }
    }
    public void EnterKeyEvent(string _input)
    {
        if (!string.IsNullOrEmpty(_input))
        {
            if (truenum == 0)
            {
                if (_input == questions[curQno].answer)
                {
                    StartCoroutine(TrueAnswer());
                }
                else
                {
                    StartCoroutine(WrongAnswer());
                }
            }
            else if (truenum == 1)
            {
                if (_input == questions[curQno].answer1)
                {
                    StartCoroutine(TrueAnswer());
                }
                else
                {
                    StartCoroutine(WrongAnswer());
                }
            }
            else if (truenum == 2)
            {
                if (_input == questions[curQno].answer2)
                {
                    StartCoroutine(TrueAnswer());
                }
                else
                {
                    StartCoroutine(WrongAnswer());
                }
            }
        }
    }
    public Transform group;
    public bool GetAnswerCheck()
    {
        return answerCheck;
    }
    public IEnumerator BoxSet()
    {   
        SFX_Prefab_Script.Instance.BoxPoping_Play();        // 20210710 정명수정
        StartCoroutine(group.Find("Image").Find("Image1").transform.SetScaleByAnimationCurve1(poping_curve, 4));
        yield return StartCoroutine(group.Find("Image").Find("answer").transform.SetScaleByAnimationCurve1(poping_curve, 4));
        
    }
    public void SettingQuestion()
    {
        questions[curQno].q = new List<Text>();
        questions[curQno].q1 = new List<Text>();
        questions[curQno].q2 = new List<Text>();
        panel.GetChild(1).GetComponent<HorizontalLayoutGroup>().enabled = true;
        panel.GetChild(2).GetComponent<HorizontalLayoutGroup>().enabled = true;
        panel.GetChild(3).GetComponent<HorizontalLayoutGroup>().enabled = true;
        Funtion_CWS.Funtion_CWS.FindGameObjectByNameend<Text>(panel.GetChild(1), "num", "answer", "abcd", 20, questions[curQno].q);
        Funtion_CWS.Funtion_CWS.FindGameObjectByNameend<Text>(panel.GetChild(2), "num", "answer", "abcd", 20, questions[curQno].q1);
        Funtion_CWS.Funtion_CWS.FindGameObjectByNameend<Text>(panel.GetChild(3), "num", "answer", "abcd", 20, questions[curQno].q2);
        Delegate_Keypad_SH.Instance.SetMultyNum(3);
        for (int i = 0; i < questions[curQno].a.Count; i++)
        {
            questions[curQno].q[i].text = questions[curQno].a[i];
            questions[curQno].q1[i].text = questions[curQno].a1[i];
            questions[curQno].q2[i].text = questions[curQno].a2[i];
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(panel.GetChild(1).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(panel.GetChild(2).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(panel.GetChild(3).GetComponent<RectTransform>());
        //panel.GetChild(2).GetComponent<HorizontalLayoutGroup>().enabled = false;
        //panel.GetChild(3).GetComponent<HorizontalLayoutGroup>().enabled = false;
        for (int i = 0; i < questions[curQno].a.Count; i++)
        {
            if (i % 2 == 0)
            {
                //questions[curQno].q1[i].transform.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(questions[curQno].q[i].transform.GetComponent<RectTransform>().anchoredPosition3D.x, questions[curQno].q1[i].transform.GetComponent<RectTransform>().anchoredPosition3D.y, 0);
                //questions[curQno].q2[i].transform.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(questions[curQno].q[i].transform.GetComponent<RectTransform>().anchoredPosition3D.x, questions[curQno].q2[i].transform.GetComponent<RectTransform>().anchoredPosition3D.y, 0);
            }
            else
            {
                questions[curQno].q1[i].transform.GetComponent<CanvasGroup>().alpha = 0;
                questions[curQno].q2[i].transform.GetComponent<CanvasGroup>().alpha = 0;
            }
        }
#if UNITY_EDITOR
        Debug.Log(questions[curQno].answer);
        Debug.Log(questions[curQno].answer1);
        Debug.Log(questions[curQno].answer2);
#endif
    }
}
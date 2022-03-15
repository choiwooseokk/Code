using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using ExpandFuncs_SH;

public class Keypad_CWS : MonoBehaviour
{
    private static Keypad_CWS instance;
    public static Keypad_CWS Instance


    {
        get { return instance; }
    }
    public Text inputTxt;
    protected int checkNum = 0;
    public bool answercheck;
    public string user_answer = "";
    public bool multi_check;


    public int num_N_line = 2;
    public int originnum_N_line = 2;
    public int num_Point_line = 3;


    public bool num_Limit = false;
    public int value_withoutpoint = 0;
    public int pointpos = 0;
    bool point_fix = false;
    bool sound_check = false;
    [Header("숫자버튼 없을경우 기본값")]
    public AudioClip numclip;
    [Header("입력버튼 없을경우 기본값")]
    public AudioClip enterclip;
    [Header("지우기버튼 없을경우 기본값")]
    public AudioClip deleteclip;

    public Image[] images;
    private void Awake()
    {
        //    Debug.Log("aa");
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        multi_check = false;
        PlaySoundManager.Instance.KeyPadSoundChange(numclip,enterclip,deleteclip);

        Button[] _buttons = GetComponentsInChildren<Button>();
        images = new Image[_buttons.Length];
        for (int i = 0; i < _buttons.Length; i++)
            images[i] = _buttons[i].GetComponent<Image>();
    }

    void SetEnableButtons()
    {
        getAnswerCheck();
        foreach (var _i in images)
        {
            _i.raycastTarget = !answercheck;
        }
    }
    void Update()
    {
        if (Study_1_GameManager.Instance.finStage)
        {
            return;
        }
        SetEnableButtons();

        var inputString = Input.inputString;
        //if (inputString == ".")
        //{
        //    pointcheck = true;
        //}
        //else
        //    pointcheck = false;

        if (inputString.Length >= 2)
            return;

        if (Input.anyKeyDown)
        {
            if (int.TryParse(inputString, out checkNum) || inputString == ".")
            {

                inputNumSingle(Input.inputString);

            }
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            inputDel();
        }
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            inputDel();

        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            inputEnter();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            inputEnter();
        }
    }

    public void getAnswerCheck()
    {
        //  Debug.Log(Delegate_Keypad_JM.Instance.AnswerCheck());
        answercheck = Delegate_Keypad_WS.Instance.AnswerCheck();
    }


    public void inputNumSingle(string _num)
    {
        if (_num == ".")
        {
            pointcheck = true;
            Delegate_Keypad_WS.Instance.Pointcheck();
        }
        else
        {
            pointcheck = false;
            Delegate_Keypad_WS.Instance.Pointcheck();
        }
        if (multi_check == false)
        {
            getAnswerCheck();
            if (!answercheck && _num != ".")
            {
                if (!sound_check)
                    sound_check = true;
                PlaySoundManager.Instance.NumberPlay();
                pointpos = 0;
                point_fix = false;
                inputTxt.text = _num;
                Delegate_Keypad_WS.Instance.InputKeyEvent(inputTxt.text);
            }
        }
        else
        {
            //if (!answercheck)
            //{
            //    if (sound_check)
            //        PlaySoundManager.Instance.NumberPlay();
            //}
            if (!answercheck)
            {
                PlaySoundManager.Instance.NumberPlay();
            }
            inputNumMulti(_num);
        }
    }




    public bool pointcheck = false;


    public string old_input = "";
    public void inputNumMulti(string _num)
    {

        getAnswerCheck();
        if (!answercheck)
        {
            //if (!sound_check)
            //{
            //    sound_check = true;
            //    PlaySoundManager.Instance.NumberPlay();
            //}
            old_input = inputTxt.text;

            if (!num_Limit)
            {

                if (_num == ".")
                {
                    if (old_input != "" && !point_fix && num_Point_line > 0)
                    {
                        point_fix = true;
                        inputTxt.text = value_withoutpoint.ToString() + ".";

                    }

                }
                else if (old_input == "")
                {
                    inputTxt.text = _num;
                    point_fix = false;
                    value_withoutpoint = int.Parse(_num);
                    pointpos = 0;


                }
                else
                {
                    int int_old_input = value_withoutpoint;
                    string value = "";
                    if (point_fix)
                    {

                        if (pointpos < num_Point_line)
                        {
                            pointpos++;
                            value_withoutpoint = int_old_input * 10 + int.Parse(_num);
                            value = value_withoutpoint.ToString();

                            int valuelength = value.Length;
                            for (int i = 0; i <= pointpos - valuelength; i++)
                            {
                                value = "0" + value;

                            }
                            value = value.Insert(value.Length - pointpos, ".");
                        }
                        else
                            value = old_input;

                    }
                    else
                    {
                        value_withoutpoint = int_old_input * 10 + int.Parse(_num);
                        if (value_withoutpoint.ToString().Length <= num_N_line)
                        {
                            value = value_withoutpoint.ToString();
                        }
                        else
                        {
                            value = old_input;
                            value_withoutpoint = int_old_input;
                        }
                    }

                    inputTxt.text = value;


                }
            }
            else
            {
                Delegate_Keypad_WS.Instance.SetMultyPointNum(originnum_N_line, originnum_N_line-1);
                if(originnum_N_line == inputTxt.text.Length)
                    Delegate_Keypad_WS.Instance.SetMultyPointNum(originnum_N_line, 0);
                if (_num == ".")
                {
                    if (old_input != "" && !point_fix && num_Point_line > 0)
                    {
                        point_fix = true;
                        inputTxt.text = value_withoutpoint.ToString() + ".";

                    }

                }
                else if (old_input == "")
                {
                    inputTxt.text = _num;
                    point_fix = false;
                    value_withoutpoint = int.Parse(_num);
                    pointpos = 0;


                }
                else
                {
                    int int_old_input = value_withoutpoint;
                    string value = "";
                    if (point_fix)
                    {
                        int dot = 0;
                        for (int i = 0; i < inputTxt.text.Length; i++)
                        {
                            if (inputTxt.text.Substring(i, 1) == ".")
                            {
                                dot = i;
                                Delegate_Keypad_WS.Instance.SetMultyPointNum(dot, num_N_line - (dot));
                            }
                        }
                        if (pointpos < num_Point_line)
                        {
                            
                            pointpos++;
                            value_withoutpoint = int_old_input * 10 + int.Parse(_num);
                            value = value_withoutpoint.ToString();

                            int valuelength = value.Length;
                            for (int i = 0; i <= pointpos - valuelength; i++)
                            {
                                value = "0" + value;

                            }
                            value = value.Insert(value.Length - pointpos, ".");
                        }
                        else
                            value = old_input;

                    }
                    else
                    {
                        value_withoutpoint = int_old_input * 10 + int.Parse(_num);
                        if (value_withoutpoint.ToString().Length <= num_N_line)
                        {
                            value = value_withoutpoint.ToString();
                        }
                        else
                        {
                            value = old_input;
                            value_withoutpoint = int_old_input;
                        }
                    }

                    inputTxt.text = value;


                }
            }

            Delegate_Keypad_WS.Instance.InputKeyEvent(inputTxt.text);
        }
    }
    public void Num_Limit(int N)
    {
        num_Limit = true;
        num_N_line = N;
        originnum_N_line = num_N_line;
    }
    public void inputDel()
    {

        getAnswerCheck();
        if (!answercheck)
        {
            PlaySoundManager.Instance.DeletePlay();
            user_answer = inputTxt.text;
            inputTxt.text = "";
            point_fix = false;
            value_withoutpoint = 0;
            pointpos = 0;
            Delegate_Keypad_WS.Instance.InputKeyEvent(inputTxt.text);
        }
    }
    public void resetanswer()
    {
        var inputString = Input.inputString;
        if (inputString == ".")
            pointcheck = true;
        else
            pointcheck = false;
    }
    public void inputEnter()
    {
        //실사용 코드

        //if (Study_1_GameManager.Instance.keypad_block_check() == 0)
        //{
        //    return;
        //}

        getAnswerCheck();
        if ((!answercheck) && (!(point_fix && pointpos == 0)))
        {
            sound_check = false;
            PlaySoundManager.Instance.EnterPlay();
            user_answer = inputTxt.text;

            Delegate_Keypad_WS.Instance.EnterKeyEvent(user_answer);

            inputTxt.text = "";
            point_fix = false;
            value_withoutpoint = 0;
            pointpos = 0;




        }
    }
    public void resetInput()
    {
        inputTxt.text = "";
        point_fix = false;
        value_withoutpoint = 0;
        pointpos = 0;

    }





    public void OnMouseEnter(GameObject _numpadObj)
    {
        //Debug.Log("mouse_on");
        _numpadObj.transform.GetChild(1).gameObject.SetActive(true);

    }


    public void OnMouseExit(GameObject _numpadObj)
    {
        //Debug.Log("mouse_out");
        _numpadObj.transform.GetChild(1).gameObject.SetActive(false);

    }


}

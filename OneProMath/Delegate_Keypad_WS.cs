using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ExpandFuncs_SH;
public class Delegate_Keypad_WS : MonoBehaviour
{
    private static Delegate_Keypad_WS instance;
    public static Delegate_Keypad_WS Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public Keypad_CWS keypad_ws;
    public bool pointcheck;

    public delegate void Delegate_void();
    public delegate void Delegate_void_input(string a);
    public delegate bool Delegate_bool();

    Delegate_void_input D_EnterKeyEvent, D_InputKeyEvent;
    Delegate_bool D_AnswerCheck;

    public void Dele_test()
    {
        Debug.Log("ok delegate");
    }

    public void SetDelegate(KeyPadUser padUser, bool check_multi = true, int num_Natrueline = 2, int num_Underpoint = 3)
    {
        D_EnterKeyEvent = padUser.EnterKeyEvent;
        D_AnswerCheck = padUser.GetAnswerCheck;
        D_InputKeyEvent = padUser.InputKeyEvent;

        keypad_ws.multi_check = check_multi;
        keypad_ws.num_N_line = num_Natrueline;
        keypad_ws.num_Point_line = num_Underpoint;

    }


    public int int_num_without_point()
    {

        return keypad_ws.value_withoutpoint;

    }

    public int point_position()
    {

        return keypad_ws.pointpos;

    }

    public void SetMultyPointNum(int _numNature, int _numUnderPoint)
    {

        keypad_ws.multi_check = true;
        keypad_ws.num_N_line = _numNature;
        keypad_ws.num_Point_line = _numUnderPoint;

    }
    public void Pointcheck()
    {
        pointcheck= keypad_ws.pointcheck;
    }
    public bool Pointcheck1()
    {
        return keypad_ws.pointcheck;
    }
    // 엔터키
    public void EnterKeyEvent(string _input)
    {
        D_EnterKeyEvent(_input);
    }

    // 정답 맞춘후 이벤트 진행중인지 체크 true면 키패트 동작x
    public bool AnswerCheck()
    {
        return D_AnswerCheck();
    }

    // 숫자키, del키
    public void InputKeyEvent(string _input)
    {
        D_InputKeyEvent(_input);
    }
}

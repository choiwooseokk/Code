using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading;

public class cal_ws
{
    public int bunmonum;
    public int bunjanum;
    public int num;
    public int bunNum;
    public int AnswerMo;
    public int AnswerJa;
    public int AnswerNum;
    public bool easymode;
    public string[] three;
    public List<int>[] threeanswer;
    public int[,] threeGwal;
    public string[,] threegihos;
    public List<string> questioninfo = new List<string>();
    public int answer;
    public void GihoRandmaker(bool b)
    {
        easymode = b;
        int returunnum = 0;
        List<int> rand = new List<int>();
        int[] arr = new int[24];
        for (int i = 0; i < 24; i++)
        {
            arr[i] = i;
        }

        bool fail = false;
        int county = 0;
        do
        {
            county = 1;
            threeanswer = new List<int>[3];
            question_real = new int[5];
            threegihos = new string[3, 4];
            three = new string[3];
            for (int i = 0; i < 40; i++)
            {
                int rand0 = random.Next(0, 24);
                int rand1 = random.Next(0, 24);
                int temp = arr[rand0];
                arr[rand0] = arr[rand1];
                arr[rand1] = temp;
            }
            bool isitfail = false;
            do
            {
                returunnum++;
                if(returunnum>100)
                {
                    GihoRandmaker(easymode);
                    return;
                }
                InfixMaker(arr[0], 0);
                MakeNumber(InfixToPostfix());
                ReplaceRealnum();
                (isitfail, threeanswer[0]) = Calculation();
            } while (isitfail);
            three[0] = Getstring();
            fail = false;
            for (int i = 1; i < 24; i++)
            {
                InfixMaker(arr[i], county);
                ReplaceRealnum();
                isitfail = false;
                (isitfail, threeanswer[county]) = Calculation();
                if (isitfail == true)
                {
                }
                else
                {
                    three[county] = Getstring();
                    county++;
                    if (county > 2)
                        break;
                }
            }
        } while (county < 3);
    }
    public void QuestionString()
    {
        string St = three[0];
        char[] phraseAsChars = St.ToCharArray();
        int a = 0;
        string s = "";
        for (int i = 0; i < phraseAsChars.Length; i++)
        {
            if (i != 0)
            {
                if (int.TryParse(phraseAsChars[i].ToString(), out a))
                {
                    s += phraseAsChars[i].ToString();
                }
                else
                {
                    if (s != "")
                    {
                        questioninfo.Add(s);
                        s = "";
                    }
                    questioninfo.Add(phraseAsChars[i].ToString());
                }
            }
            else if (i == 0)
            {
                if (!int.TryParse(phraseAsChars[i].ToString(), out a))
                {
                    questioninfo.Add(phraseAsChars[i].ToString());
                }
                else
                {
                    s += phraseAsChars[i].ToString();
                }
            }
        }
        if (s != "")
            questioninfo.Add(s);
        s = "";
        for (int i = 0; i < questioninfo.Count; i++)
            s += questioninfo[i];
    }
    public void MakeQuestion(int gwalho, bool b,int num = 30)
    {
        int returnnum = 0;
        questioninfo.Clear();
        three = new string[1];
        easymode = b;
        List<int> rand = new List<int>();
        int[] arr = new int[24];
        for (int i = 0; i < 24; i++)
        {
            arr[i] = i;
        }

        int j = random.Next(0, 24);
        question_real = new int[5];
        

        int gwalholenght = gwalho;
        if (gwalho != 0)
        {
            gwalhoStart = new int[1];
            gwalhoEnd = new int[1];
        }
        else
        {
            gwalhoStart = new int[gwalholenght];
            gwalhoEnd = new int[gwalholenght];
        }

        if (gwalholenght == 1)
        {
            gwalhoStart[0] = random.Next(0, 4);
            gwalhoEnd[0] = gwalhoStart[0] + 1;
            //gwalhoEnd[0] = random.Next(gwalhoStart[0] + 1, 5);

        }
        if (gwalholenght == 2)
        {
            gwalhoStart[0] = random.Next(0, 2);
            gwalhoEnd[0] = gwalhoStart[0] + 2;
            //gwalhoStart[0] = random.Next(0, 2);
            //gwalhoEnd[0] = random.Next(gwalhoStart[0] + 1, 3);

            //gwalhoStart[1] = random.Next(gwalhoEnd[0] + 1, 4);
            //gwalhoEnd[1] = random.Next(gwalhoStart[1] + 1, 5);
        }
        InfixMaker(j);
        Getindex();
        do
        {
            returnnum++;
            if(returnnum>100)
            {
                MakeQuestion(gwalholenght, easymode, num);
                return;
            }
            MakeNumber1(InfixToPostfix(),num);
            ReplaceRealnum();
        }
        while (Calculation().fail);
        three[0] = Getstring();
        QuestionString();
        answer = answers[answers.Count - 1];
    }
    public void MakeQuestion1(int gwalho, bool b, int num = 30)
    {
        int returnnum = 0;
        questioninfo.Clear();
        three = new string[1];
        easymode = b;
        List<int> rand = new List<int>();
        int[] arr = new int[24];
        for (int i = 0; i < 24; i++)
        {
            arr[i] = i;
        }

        int j = random.Next(0, 24);
        question_real = new int[4];


        int gwalholenght = gwalho;
        if (gwalho != 0)
        {
            gwalhoStart = new int[1];
            gwalhoEnd = new int[1];
        }
        else
        {
            gwalhoStart = new int[gwalholenght];
            gwalhoEnd = new int[gwalholenght];
        }

        if (gwalholenght == 1)
        {
            gwalhoStart[0] = random.Next(0, 3);
            gwalhoEnd[0] = gwalhoStart[0] + 1;
            //gwalhoEnd[0] = random.Next(gwalhoStart[0] + 1, 5);

        }
        if (gwalholenght == 2)
        {
            gwalhoStart[0] = random.Next(0, 2);
            gwalhoEnd[0] = gwalhoStart[0] + 2;
            //gwalhoStart[0] = random.Next(0, 2);
            //gwalhoEnd[0] = random.Next(gwalhoStart[0] + 1, 3);

            //gwalhoStart[1] = random.Next(gwalhoEnd[0] + 1, 4);
            //gwalhoEnd[1] = random.Next(gwalhoStart[1] + 1, 5);
        }
        InfixMaker(j);
        Getindex();
        do
        {
            returnnum++;
            if (returnnum > 100)
            {
                MakeQuestion(gwalholenght, easymode, num);
                return;
            }
            MakeNumber1(InfixToPostfix(), num);
            ReplaceRealnum();
        }
        while (Calculation().fail);
        three[0] = Getstring();
        QuestionString();
        answer = answers[answers.Count - 1];
    }
    public void MakeQuestionnum(bool B = true)
    {
        int returnnum = 0;
        questioninfo.Clear();
        three = new string[1];
        List<int> rand = new List<int>();
        int[] arr = new int[24];
        for (int i = 0; i < 24; i++)
        {
            arr[i] = i;
        }

        bool fail = false;
        int county = 0;
        int j = random.Next(0, 24);
        question_real = new int[4];

        InfixMaker(j);
        Getindex();
        do
        {
            returnnum++;
            if (returnnum > 100)
            {
                MakeQuestionnum(B);
                return;
            }
            SameMakeNumber(B);
            ReplaceRealnum();


        }
        while (Calculation().fail);
        three[0] = Getstring();
        QuestionString();
        answer = answers[answers.Count - 1];
    }
    List<int>[] ListofOrderList = new List<int>[3];


    System.Random random = new System.Random();

    public void GwalhoRandmaker(int _index = 0)
    {
        question_real = new int[5];


        question_real[0] = 0;
        question_real[1] = 0;
        question_real[2] = 0;
        question_real[3] = 0;
        question_real[4] = 0;


        calculationer.Clear();
        calculationer_origin.Clear();


        bool fail;
        List<string> three2 = new List<string>();
        List<int> whatisthat = new List<int>();
        threeanswer = new List<int>[3];
        three = new string[3];
        threeGwal = new int[3, 2];

        do
        {
            whatisthat.Clear();
            three2.Clear();
            gwalhoStart = new int[0];
            gwalhoEnd = new int[0];
            int j;
            do
            {
                j = random.Next(0, 24);
                InfixMaker(j);
                MakeNumber(InfixToPostfix());
                ListofOrderList[0] = Getindex();
                ReplaceRealnum();
                (fail, threeanswer[0]) = Calculation();
            } while (fail);
            three[0] = Getstring();
            threeGwal[0, 0] = 0;
            threeGwal[0, 1] = 0;
            gwalhoStart = new int[1];
            gwalhoEnd = new int[1];
            do
            {
                gwalhoStart[0] = random.Next(0, 2);
                if (_index % 2 != 0)
                {
                    gwalhoStart[0] = random.Next(0, 4);
                }
                gwalhoEnd[0] = gwalhoStart[0] + 1;
            } while (gwalhoStart[0] == (ListofOrderList[0][0] - 1) / 2);
            InfixMaker(j);
            ListofOrderList[1] = Getindex();
            ReplaceRealnum();
            (fail, threeanswer[1]) = Calculation();
            if (fail)
            {
                continue;
            }
            three[1] = Getstring();
            threeGwal[1, 0] = gwalhoStart[0];
            threeGwal[1, 1] = gwalhoEnd[0];
            if (gwalhoStart[0] > 0 && gwalhoEnd[0] < 4)
            {
                if (random.Next(0, 2) == 0)
                {
                    gwalhoStart[0]--;
                }
                else
                {
                    gwalhoEnd[0]++;
                }
            }
            else if (gwalhoStart[0] > 0)
                gwalhoStart[0]--;
            else
                gwalhoEnd[0]++;
            if (gwalhoStart[0] == (ListofOrderList[0][0] - 1) / 2)
            {
                fail = true;
                continue;
            }
            InfixMaker(j);
            ListofOrderList[2] = Getindex();
            ReplaceRealnum();
            (fail, threeanswer[2]) = Calculation();
        }
        while (fail);
        three[2] = Getstring();
        threeGwal[2, 0] = gwalhoStart[0];
        threeGwal[2, 1] = gwalhoEnd[0];
    }
    public void Start()
    {
        //for (int i = 0; i < 1; i++)
        //{
        //    GwahoRandomaker();
        //}
    }
    List<string> calculationer_origin = new List<string>();
    List<string> calculationer = new List<string>();
    List<int> orderList = new List<int>();
    List<int> answers = new List<int>();
    //건드리지말것!
    string[] giho = { "÷", "+", "-", "×" };
    public int[] question_real = new int[4];
    public int[] gwalhoStart = new int[0];
    public int[] gwalhoEnd = new int[0];
    public void SameMakeNumber(bool b)
    {
        if (b)
        {
            int rand = random.Next(2, 10);
            for (int i = 0; i < question_real.Length; i++)
                question_real[i] = rand;
        }
        else
        {
            //List<int> a = new List<int>();
            //int ran = random.Next(0, 4);
            //for (int i = 0; i < 4;)
            //{
            //    if (a.Contains(ran))
            //    {
            //        ran = random.Next(0, 4);
            //    }
            //    else
            //    {
            //        a.Add(ran);
            //        i++;
            //    }
            //}
            int rand = random.Next(2, 10);
            int rand1 = random.Next(2, 10);
            while (rand == rand1)
                rand1 = random.Next(2, 10);
            for (int i = 0; i < 4; i++)
            {
                if (i < 2)
                {
                    question_real[i] = rand;
                    //question_real[a[i]] = rand;
                }
                else
                {
                    question_real[i] = rand1;
                    //question_real[a[i]] = rand1;
                }
            }
        }
    }
    public void MakeNumber()
    {

        for (int i = 0; i < question_real.Length; i++)
            question_real[i] = random.Next(2, 100);
    }
    public void MakeNumber1(string _str,int Num = 30)
    {

        for (int i = 0; i < question_real.Length; i++)
            question_real[i] = -1;
        if (!easymode)
        {
            if (_str.Contains("-"))
            {
                int i = _str.IndexOf('-');
                if (_str[i - 1] <= 'z' && _str[i - 1] >= 'a' && _str[i - 2] >= 'a' && _str[i - 2] <= 'z')
                {
                    question_real[_str[i - 2] - 'a'] = random.Next(4, 100);
                    question_real[_str[i - 1] - 'a'] = random.Next(2, question_real[_str[i - 2] - 'a']);
                }
            }
            if (_str.Contains("÷"))
            {
                int i = _str.IndexOf('÷');
                if (_str[i - 1] <= 'z' && _str[i - 1] >= 'a' && _str[i - 2] >= 'a' && _str[i - 2] <= 'z')
                {
                    question_real[_str[i - 1] - 'a'] = random.Next(2, 10);
                    question_real[_str[i - 2] - 'a'] = random.Next(2, 10) * question_real[_str[i - 1] - 'a'];
                }
            }
            if (_str.Contains("×"))
            {
                int i = _str.IndexOf('×');
                if (_str[i - 1] <= 'z' && _str[i - 1] >= 'a' && _str[i - 2] >= 'a' && _str[i - 2] <= 'z')
                {
                    question_real[_str[i - 1] - 'a'] = random.Next(2, 12);
                    question_real[_str[i - 2] - 'a'] = random.Next(2, 12);
                }
            }
            if (_str.Contains("+"))
            {
                int i = _str.IndexOf('+');
                if (_str[i - 1] <= 'z' && _str[i - 1] >= 'a' && _str[i - 2] >= 'a' && _str[i - 2] <= 'z')
                {
                    question_real[_str[i - 1] - 'a'] = random.Next(2, 100);
                    question_real[_str[i - 2] - 'a'] = random.Next(2, 100);
                }
            }
            for (int i = 0; i < question_real.Length; i++)
            {
                if (question_real[i] == -1)
                    question_real[i] = random.Next(2, 100);
            }
        }
        else if (easymode)
        {
            if (_str.Contains("-"))
            {
                int i = _str.IndexOf('-');
                if (_str[i - 1] <= 'z' && _str[i - 1] >= 'a' && _str[i - 2] >= 'a' && _str[i - 2] <= 'z')
                {
                    question_real[_str[i - 2] - 'a'] = random.Next(4, Num);
                    question_real[_str[i - 1] - 'a'] = random.Next(2, question_real[_str[i - 2] - 'a']);
                }
            }
            if (_str.Contains("÷"))
            {
                int i = _str.IndexOf('÷');
                if (_str[i - 1] <= 'z' && _str[i - 1] >= 'a' && _str[i - 2] >= 'a' && _str[i - 2] <= 'z')
                {
                    question_real[_str[i - 1] - 'a'] = random.Next(2, 10);
                    question_real[_str[i - 2] - 'a'] = random.Next(2, 4) * question_real[_str[i - 1] - 'a'];
                    while(question_real[_str[i - 2] - 'a']>=Num)
                    {
                        question_real[_str[i - 1] - 'a'] = random.Next(2, 10);
                        question_real[_str[i - 2] - 'a'] = random.Next(2, 4) * question_real[_str[i - 1] - 'a'];
                    }
                }
            }
            if (_str.Contains("×"))
            {
                int i = _str.IndexOf('×');
                if (_str[i - 1] <= 'z' && _str[i - 1] >= 'a' && _str[i - 2] >= 'a' && _str[i - 2] <= 'z')
                {
                    question_real[_str[i - 1] - 'a'] = random.Next(2, 9);
                    question_real[_str[i - 2] - 'a'] = random.Next(2, 9);
                }
            }
            if (_str.Contains("+"))
            {
                int i = _str.IndexOf('+');
                if (_str[i - 1] <= 'z' && _str[i - 1] >= 'a' && _str[i - 2] >= 'a' && _str[i - 2] <= 'z')
                {
                    question_real[_str[i - 1] - 'a'] = random.Next(2, Num);
                    question_real[_str[i - 2] - 'a'] = random.Next(2, Num);
                }
            }
            for (int i = 0; i < question_real.Length; i++)
            {
                if (question_real[i] == -1)
                    question_real[i] = random.Next(2, Num);
            }
        }
    }
    public void MakeNumber(string _str)
    {

        for (int i = 0; i < question_real.Length; i++)
            question_real[i] = -1;
        if (!easymode)
        {
            if (_str.Contains("-"))
            {
                int i = _str.IndexOf('-');
                if (_str[i - 1] <= 'z' && _str[i - 1] >= 'a' && _str[i - 2] >= 'a' && _str[i - 2] <= 'z')
                {
                    question_real[_str[i - 2] - 'a'] = random.Next(4, 100);
                    question_real[_str[i - 1] - 'a'] = random.Next(2, question_real[_str[i - 2] - 'a']);
                }
            }
            if (_str.Contains("÷"))
            {
                int i = _str.IndexOf('÷');
                if (_str[i - 1] <= 'z' && _str[i - 1] >= 'a' && _str[i - 2] >= 'a' && _str[i - 2] <= 'z')
                {
                    question_real[_str[i - 1] - 'a'] = random.Next(2, 10);
                    question_real[_str[i - 2] - 'a'] = random.Next(2, 10) * question_real[_str[i - 1] - 'a'];
                }
            }
            if (_str.Contains("×"))
            {
                int i = _str.IndexOf('×');
                if (_str[i - 1] <= 'z' && _str[i - 1] >= 'a' && _str[i - 2] >= 'a' && _str[i - 2] <= 'z')
                {
                    question_real[_str[i - 1] - 'a'] = random.Next(2, 12);
                    question_real[_str[i - 2] - 'a'] = random.Next(2, 12);
                }
            }
            if (_str.Contains("+"))
            {
                int i = _str.IndexOf('+');
                if (_str[i - 1] <= 'z' && _str[i - 1] >= 'a' && _str[i - 2] >= 'a' && _str[i - 2] <= 'z')
                {
                    question_real[_str[i - 1] - 'a'] = random.Next(2, 100);
                    question_real[_str[i - 2] - 'a'] = random.Next(2, 100);
                }
            }
            for (int i = 0; i < question_real.Length; i++)
            {
                if (question_real[i] == -1)
                    question_real[i] = random.Next(2, 100);
            }
        }
        else if (easymode)
        {
            if (_str.Contains("-"))
            {
                int i = _str.IndexOf('-');
                if (_str[i - 1] <= 'z' && _str[i - 1] >= 'a' && _str[i - 2] >= 'a' && _str[i - 2] <= 'z')
                {
                    question_real[_str[i - 2] - 'a'] = random.Next(4, 30);
                    question_real[_str[i - 1] - 'a'] = random.Next(2, question_real[_str[i - 2] - 'a']);
                }
            }
            if (_str.Contains("÷"))
            {
                int i = _str.IndexOf('÷');
                if (_str[i - 1] <= 'z' && _str[i - 1] >= 'a' && _str[i - 2] >= 'a' && _str[i - 2] <= 'z')
                {
                    question_real[_str[i - 1] - 'a'] = random.Next(2, 10);
                    question_real[_str[i - 2] - 'a'] = random.Next(2, 5) * question_real[_str[i - 1] - 'a'];
                }
            }
            if (_str.Contains("×"))
            {
                int i = _str.IndexOf('×');
                if (_str[i - 1] <= 'z' && _str[i - 1] >= 'a' && _str[i - 2] >= 'a' && _str[i - 2] <= 'z')
                {
                    question_real[_str[i - 1] - 'a'] = random.Next(2, 6);
                    question_real[_str[i - 2] - 'a'] = random.Next(2, 6);
                }
            }
            if (_str.Contains("+"))
            {
                int i = _str.IndexOf('+');
                if (_str[i - 1] <= 'z' && _str[i - 1] >= 'a' && _str[i - 2] >= 'a' && _str[i - 2] <= 'z')
                {
                    question_real[_str[i - 1] - 'a'] = random.Next(2, 30);
                    question_real[_str[i - 2] - 'a'] = random.Next(2, 30);
                }
            }
            for (int i = 0; i < question_real.Length; i++)
            {
                if (question_real[i] == -1)
                    question_real[i] = random.Next(2, 30);
            }
        }
    }
    public int cal_ex(string _str)
    {

        int a = question_real[_str[0] - 'a'];
        int b = 0;
        if (_str.Length > 2)
            b = question_real[_str[2] - 'a'];
        for (int i = 1; i < _str.Length;)
        {
            if (i == 1)
            {
                switch (_str[i])
                {
                    case '+':
                        question_real[_str[i - 1] - 'a'] = random.Next(2, 100);
                        question_real[_str[i + 1] - 'a'] = random.Next(2, 100);
                        break;
                    case '×':
                        question_real[_str[i - 1] - 'a'] = random.Next(3, 10);
                        question_real[_str[i + 1] - 'a'] = random.Next(3, 15);
                        break;
                    case '÷':
                        question_real[_str[i + 1] - 'a'] = random.Next(3, 10);
                        question_real[_str[i - 1] - 'a'] = random.Next(3, 15) * question_real[_str[i + 1] - 'a'];
                        break;
                }
            }
            if (i == 3)
            {
                switch (_str[i])
                {
                    case '+':
                        switch (_str[i - 2])
                        {
                            case '÷':
                                question_real[_str[i - 1] - 'a'] = random.Next(3, 10);
                                question_real[_str[i - 3] - 'a'] = random.Next(3, 15) * question_real[_str[i - 1] - 'a'];
                                question_real[_str[i + 1] - 'a'] = random.Next(3, 100);
                                break;
                            case '×':
                                question_real[_str[i - 1] - 'a'] = random.Next(3, 15);
                                question_real[_str[i - 3] - 'a'] = random.Next(3, 10);
                                question_real[_str[i + 1] - 'a'] = random.Next(3, 100);
                                break;
                        }
                        break;
                    case '×':
                        switch (_str[i])
                        {
                            case '÷':
                                question_real[_str[i - 1] - 'a'] = random.Next(3, 10);
                                question_real[_str[i - 3] - 'a'] = random.Next(3, 15) * question_real[_str[i - 1] - 'a'];
                                question_real[_str[i + 1] - 'a'] = random.Next(3, 10);
                                break;
                            case '+':
                                question_real[_str[i - 1] - 'a'] = random.Next(3, 100);
                                question_real[_str[i - 3] - 'a'] = random.Next(3, 100);
                                question_real[_str[i + 1] - 'a'] = random.Next(3, 10);
                                break;
                        }
                        break;
                    case '÷':
                        switch (_str[i])
                        {
                            case '+':
                                question_real[_str[i + 1] - 'a'] = random.Next(2, 10);
                                int temp = random.Next(2, 15);
                                int temp2 = random.Next(1, temp);
                                question_real[_str[i - 3] - 'a'] = (temp - temp2) * question_real[_str[i + 1] - 'a'];
                                question_real[_str[i - 1] - 'a'] = (temp2) * question_real[_str[i + 1] - 'a'];
                                break;
                            case '×':
                                question_real[_str[i + 1] - 'a'] = random.Next(2, 10);
                                break;
                        }
                        break;
                }
            }
            if (_str[i] == '+')
            {
                a = a + b;
                i++; i++;
                if (_str.Length > i)
                {
                    b = question_real[_str[i - 1] - 'a'];
                }
            }
            else if (_str[i] == '-')
            {
                a = a - b;
                i++; i++;
                if (_str.Length > i)
                {
                    b = question_real[_str[i - 1] - 'a'];
                }
            }
            else if (_str[i] == '×')
            {
                a = a * b;
                i++; i++;
                if (_str.Length > i)
                {
                    b = question_real[_str[i - 1] - 'a'];
                }
            }
            else if (_str[i] == '÷')
            {
                a = a / b;
                i++; i++;
                if (_str.Length > i)
                {
                    b = question_real[_str[i - 1] - 'a'];
                }
            }
            else
                i++;
        }
        return a;
    }
    public void InfixMaker(int _count = -1, int _num = -1)
    {
        calculationer.Clear();
        calculationer_origin.Clear();
        int gwalhocount = 0;
        int gihocount = 0;

        int[] randoming = { 0, 1, 2, 3 };
        for (int j = 0; j < 10; j++)
        {
            int rand = random.Next(0, 4);
            int temp = randoming[0];
            randoming[0] = randoming[rand];
            randoming[rand] = temp;
        }
        int[,] randbeam = new int[24, 4];
        int count0 = 0;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    List<int> randy = new List<int>();
                    randy.Add(0);
                    randy.Add(1);
                    randy.Add(2);
                    randy.Add(3);
                    randbeam[count0, 0] = randy[i];
                    randy.RemoveAt(i);
                    randbeam[count0, 1] = randy[j];
                    randy.RemoveAt(j);
                    randbeam[count0, 2] = randy[k];
                    randy.RemoveAt(k);
                    randbeam[count0, 3] = randy[0];
                    randy.RemoveAt(0);
                    count0++;
                }
            }
        }
        for (int i = 0; i < question_real.Length; i++)
        {
            if (gwalhocount < gwalhoStart.Length && i == gwalhoStart[gwalhocount])
            {
                calculationer.Add("(");
            }
            calculationer.Add(((char)('a' + i)).ToString());
            if (gwalhocount < gwalhoStart.Length && i == gwalhoEnd[gwalhocount])
            {
                calculationer.Add(")");
                gwalhocount++;
            }
            if (i != question_real.Length - 1)
            {
                if (_count != -1)
                {
                    calculationer.Add(giho[randbeam[_count, gihocount]]);
                    if (_num != -1)
                    {
                        threegihos[_num, gihocount] = giho[randbeam[_count, gihocount]];
                    }
                    gihocount++;
                }
                else
                {
                    calculationer.Add(giho[randoming[gihocount]]);
                    gihocount++;
                }
                //todo: 기호 랜덤 한번씩 생성토록할것.
            }
        }
        calculationer_origin = calculationer.ToList();
    }
    public void ReplaceRealnum()
    {
        for (int i = 0; i < question_real.Length; i++)
        {
            calculationer[calculationer_origin.IndexOf(((char)('a' + i)).ToString())] = question_real[i].ToString();
        }
    }
    public (bool fail, List<int> answer) Calculation()
    {
        answers.Clear();
        List<List<string>> calc = new List<List<string>>();
        List<string> calculator = calculationer.ToList();
        int count = 0;
        for (int i = 0; i < calculator.Count;)
        {
            if (calculator[i] == "(")
            {
                calculator.RemoveAt(i);
                List<string> fix = new List<string>();
                while (calculator[i] != ")")
                {
                    fix.Add(calculator[i]);
                    calculator.RemoveAt(i);
                }
                calculator.RemoveAt(i);
                calculator.Insert(i, "On" + count.ToString());
                count++;
                calc.Add(fix);
            }
            else
                i++;
        }
        List<List<int>> order = new List<List<int>>();
        for (int i = 0; i < calc.Count; i++)
        {
            order.Add(new List<int>());
            int count2 = 0;
            for (int j = 0; j < calc[i].Count;)
            {
                if (calc[i][j] == "×")
                {
                    int a = int.Parse(calc[i][j - 1]);
                    int b = int.Parse(calc[i][j + 1]);
                    calc[i].RemoveAt(j - 1);
                    calc[i].RemoveAt(j - 1);
                    calc[i].RemoveAt(j - 1);
                    calc[i].Insert(j - 1, (a * b).ToString());
                    answers.Add((a * b));
                    if (a * b > 150)
                        return (true, null);
                }
                else if (calc[i][j] == "÷")
                {
                    int a = int.Parse(calc[i][j - 1]);
                    int b = int.Parse(calc[i][j + 1]);
                    calc[i].RemoveAt(j - 1);
                    calc[i].RemoveAt(j - 1);
                    calc[i].RemoveAt(j - 1);
                    calc[i].Insert(j - 1, (a / b).ToString());
                    if (a % b != 0 || a / b < 2)
                        return (true, null);
                    answers.Add((a / b));
                }
                else
                    j++;
            }
            for (int j = 0; j < calc[i].Count;)
            {
                if (calc[i][j] == "+")
                {
                    int a = int.Parse(calc[i][j - 1]);
                    int b = int.Parse(calc[i][j + 1]);
                    calc[i].RemoveAt(j - 1);
                    calc[i].RemoveAt(j - 1);
                    calc[i].RemoveAt(j - 1);
                    calc[i].Insert(j - 1, (a + b).ToString());
                    answers.Add((a + b));
                    if (a + b > 300)
                        return (true, null);
                }
                else if (calc[i][j] == "-")
                {
                    int a = int.Parse(calc[i][j - 1]);
                    int b = int.Parse(calc[i][j + 1]);
                    calc[i].RemoveAt(j - 1);
                    calc[i].RemoveAt(j - 1);
                    calc[i].RemoveAt(j - 1);
                    calc[i].Insert(j - 1, (a - b).ToString());
                    answers.Add((a - b));
                    if (a - b < 1)
                        return (true, null);
                }
                else
                    j++;
            }
            int temp = calculator.IndexOf("On" + i.ToString());
            calculator.RemoveAt(temp);
            calculator.Insert(temp, answers[answers.Count - 1].ToString());
        }
        List<int> tempList = new List<int>();
        //여기서부터 다시 인덱싱 필요
        for (int i = 0; i < calculator.Count;)
        {
            if (calculator[i] == "×")
            {
                int a = int.Parse(calculator[i - 1]);
                int b = int.Parse(calculator[i + 1]);
                calculator.RemoveAt(i - 1);
                calculator.RemoveAt(i - 1);
                calculator.RemoveAt(i - 1);
                calculator.Insert(i - 1, (a * b).ToString());
                answers.Add((a * b));
                if (a * b > 150)
                    return (true, null);
            }
            else if (calculator[i] == "÷")
            {
                int a = int.Parse(calculator[i - 1]);
                int b = int.Parse(calculator[i + 1]);
                calculator.RemoveAt(i - 1);
                calculator.RemoveAt(i - 1);
                calculator.RemoveAt(i - 1);
                calculator.Insert(i - 1, (a / b).ToString());
                answers.Add((a / b));
                if (a % b != 0 || a / b < 3)
                    return (true, null);
            }
            else
                i++;
        }
        for (int i = 0; i < calculator.Count;)
        {
            if (calculator[i] == "+")
            {
                int a = int.Parse(calculator[i - 1]);
                int b = int.Parse(calculator[i + 1]);
                calculator.RemoveAt(i - 1);
                calculator.RemoveAt(i - 1);
                calculator.RemoveAt(i - 1);
                calculator.Insert(i - 1, (a + b).ToString());
                answers.Add((a + b));
                if (a + b > 300)
                    return (true, null);
            }
            else if (calculator[i] == "-")
            {
                int a = int.Parse(calculator[i - 1]);
                int b = int.Parse(calculator[i + 1]);
                calculator.RemoveAt(i - 1);
                calculator.RemoveAt(i - 1);
                calculator.RemoveAt(i - 1);
                calculator.Insert(i - 1, (a - b).ToString());
                answers.Add((a - b));
                if (a - b < 1)
                    return (true, null);
            }
            else
                i++;
        }
        return (false, answers.ToList());
    }
    public List<int> Getindex()
    {
        orderList.Clear();
        int i = 0;
        for (int j = 0; j < gwalhoStart.Length; j++)
        {
            while (i < calculationer_origin.Count && calculationer_origin[i] != "(")
            {
                i++;
            }
            int starter = i;
            while (i < calculationer_origin.Count && calculationer_origin[i] != ")")
            {
                if (calculationer_origin[i] == "×")
                {
                    orderList.Add(i);
                }
                else if (calculationer_origin[i] == "÷")
                {
                    orderList.Add(i);
                }
                i++;
            }
            i = starter;
            while (i < calculationer_origin.Count && calculationer_origin[i] != ")")
            {
                if (calculationer_origin[i] == "+")
                {
                    orderList.Add(i);
                }
                else if (calculationer_origin[i] == "-")
                {
                    orderList.Add(i);
                }
                i++;
            }
            i++;
        }
        for (i = 0; i < calculationer_origin.Count; i++)
        {
            if (calculationer_origin[i] == "(")
            {
                while (i < calculationer_origin.Count && calculationer_origin[i] != ")")
                    i++;
                i++;
            }
            if (i < calculationer_origin.Count)
            {
                if (calculationer_origin[i] == "×")
                {
                    orderList.Add(i);
                }
                else if (calculationer_origin[i] == "÷")
                {
                    orderList.Add(i);
                }
            }
        }
        for (i = 0; i < calculationer_origin.Count; i++)
        {
            if (calculationer_origin[i] == "(")
            {
                while (i < calculationer_origin.Count && calculationer_origin[i] != ")")
                    i++;
                i++;
            }
            if (i < calculationer_origin.Count)
            {
                if (calculationer_origin[i] == "+")
                {
                    orderList.Add(i);
                }
                else if (calculationer_origin[i] == "-")
                {
                    orderList.Add(i);
                }
            }
        }
        return orderList;
    }
    public string Getstring()
    {
        string infix_str = "";
        for (int i = 0; i < calculationer.Count; i++)
        {
            infix_str += calculationer[i];
        }
        return infix_str;
    }
    public string InfixToPostfix()
    {
        Stack<string> postfix_stack = new Stack<string>();
        string postfix_str = "";
        for (int i = 0; i < calculationer_origin.Count; i++)
        {
            if (calculationer_origin[i] == "(")
            {
                postfix_stack.Push(calculationer_origin[i]);
            }
            else if (calculationer_origin[i] == "+" || calculationer_origin[i] == "-")
            {
                string temp;
                while (postfix_stack.Count > 0)
                {
                    temp = postfix_stack.Pop();
                    if (temp == "(")
                    {
                        postfix_stack.Push(temp);
                        break;
                    }
                    else
                    {
                        postfix_str += temp;
                    }
                }
                postfix_stack.Push(calculationer_origin[i]);
            }
            else if (calculationer_origin[i] == ")")
            {
                string temp;
                while (true)
                {
                    temp = postfix_stack.Pop();
                    if (temp != "(")
                    {
                        postfix_str += temp;
                    }
                    else
                        break;
                }
            }
            else if (calculationer_origin[i] == "÷" || calculationer_origin[i] == "×")
            {
                postfix_stack.Push(calculationer_origin[i]);
            }
            else
            {
                postfix_str += calculationer_origin[i];
            }
        }
        while (postfix_stack.Count > 0)
        {
            postfix_str += postfix_stack.Pop();
        }
        return postfix_str;
    }



    /// <summary>
    /// 숫자 만들어서 구하기
    /// </summary>
    /// <param name="N">분모 최댓값</param>
    /// <param name="N1">분모 최솟값 ,기본값은 2</param>
    /// <param name="N2">분수자연수 최댓값,기본값은3</param>
    /// <param name="B">대분수인지 아닌지체크,true면 대분수,false면 자연수,기본값flase</param>
    public void MakeNum(int N, int N1 = 2, int N2 = 3, bool B = false)
    {
        if (!B)
        {
            do
            {
                bunmonum = random.Next(N1, N);
                bunjanum = random.Next(1, bunmonum);
                num = random.Next(2, 31);
                var a = multAnswernum(bunmonum, bunjanum, 0, num);
                AnswerMo = a.Item1;
                AnswerNum = a.Item2 / a.Item1;
                AnswerJa = a.Item2 % a.Item1;
            } while (AnswerJa == 0);
        }
        else
        {
            do
            {
                bunmonum = random.Next(N1, N);
                bunjanum = random.Next(1, bunmonum);
                num = random.Next(2, 31);
                bunNum = random.Next(1, N2);
                var a = multAnswernum(bunmonum, bunjanum, bunNum, num);
                AnswerMo = a.Item1;
                AnswerNum = a.Item2 / a.Item1;
                AnswerJa = a.Item2 % a.Item1;
            } while (AnswerJa == 0);
        }
    }
    /// <summary>
    /// 약분이 안되는숫자만나오기
    /// </summary>
    /// <param name="N"></param>
    /// <param name="N1"></param>
    /// <param name="N2"></param>
    /// <param name="B"></param>
    public void MakeNum1(int N, int N1 = 2, int N2 = 3, bool B = false)
    {
        if (!B)
        {

            do
            {
                bool b = false;
                bool b1 = false;
                List<int> list = new List<int>();
                List<int> listnum = new List<int>();
                bunmonum = random.Next(N1, N);
                bunjanum = random.Next(1, bunmonum);
                num = random.Next(2, 31);
                for (int i = 2; i <= bunmonum; i++)
                {
                    if (bunmonum % i == 0)
                    {
                        list.Add(i);
                    }
                }
                for (int i = 2; i <= num; i++)
                {
                    if (num % i == 0)
                    {
                        listnum.Add(i);
                    }
                }
                for (int i = 0; i < list.Count; i++)
                {
                    if (bunjanum % list[i] == 0)
                    {
                        b = true;
                        break;
                    }
                }
                for (int i = 0; i < list.Count; i++)
                {
                    for (int y = 0; y < listnum.Count; y++)
                    {
                        if (list[i] == listnum[y])
                        {
                            b1 = true;
                            break;
                        }
                    }
                }
                while (b)
                {
                    b = false;
                    bunjanum = random.Next(1, bunmonum);
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (bunjanum % list[i] == 0)
                        {
                            b = true;
                        }
                    }
                }
                while (b1)
                {
                    listnum.Clear();
                    b1 = false;
                    num = random.Next(2, 31);
                    for (int i = 2; i <= num; i++)
                    {
                        if (num % i == 0)
                        {
                            listnum.Add(i);
                        }
                    }
                    for (int i = 0; i < list.Count; i++)
                    {
                        for (int y = 0; y < listnum.Count; y++)
                        {
                            if (listnum[y] % list[i] == 0)
                            {
                                b1 = true;
                                break;
                            }
                        }
                    }
                }

                var a = multAnswernum(bunmonum, bunjanum, 0, num);
                AnswerMo = a.Item1;
                AnswerNum = a.Item2 / a.Item1;
                AnswerJa = a.Item2 % a.Item1;
            } while (AnswerJa == 0);
        }
        else
        {

            do
            {
                bool b = false;
                bool b1 = false;
                bool b2 = false;
                List<int> list = new List<int>();
                List<int> listnum = new List<int>();
                bunmonum = random.Next(N1, N);
                bunjanum = random.Next(1, bunmonum);
                num = random.Next(2, 31);
                for (int i = 2; i <= bunmonum; i++)
                {
                    if (bunmonum % i == 0)
                    {
                        list.Add(i);
                    }
                }
                for (int i = 2; i <= num; i++)
                {
                    if (num % i == 0)
                    {
                        listnum.Add(i);
                    }
                }
                for (int i = 0; i < list.Count; i++)
                {
                    if (bunjanum % list[i] == 0)
                    {
                        b = true;
                        break;
                    }
                }
                for (int i = 0; i < list.Count; i++)
                {
                    for (int y = 0; y < listnum.Count; y++)
                    {
                        if (listnum[y] % list[i] == 0)
                        {
                            b1 = true;
                            break;
                        }
                    }
                }
                while (b)
                {
                    b = false;
                    bunjanum = random.Next(1, bunmonum);
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (bunjanum % list[i] == 0)
                        {
                            b = true;
                        }
                    }
                }
                while (b1)
                {
                    listnum.Clear();
                    b1 = false;
                    num = random.Next(2, 31);
                    for (int i = 2; i <= num; i++)
                    {
                        if (num % i == 0)
                        {
                            listnum.Add(i);
                        }
                    }
                    for (int i = 0; i < list.Count; i++)
                    {
                        for (int y = 0; y < listnum.Count; y++)
                        {
                            if (listnum[y] % list[i] == 0)
                            {
                                b1 = true;
                                break;
                            }
                        }
                    }
                }

                bunNum = random.Next(1, N2);


                var a = multAnswernum(bunmonum, bunjanum, bunNum, num);
                AnswerMo = a.Item1;
                AnswerNum = a.Item2 / a.Item1;
                AnswerJa = a.Item2 % a.Item1;
            } while (AnswerJa == 0);
        }
    }
    /// <summary>
    /// 약분되는숫자 한개이상
    /// </summary>
    /// <param name="N"></param>
    /// <param name="N1"></param>
    /// <param name="N2"></param>
    /// <param name="B"></param>
    public void MakeNum2(int N, int N1 = 2, int N2 = 3, bool B = false)
    {
        if (!B)
        {

            do
            {
                int returnnum = 0;
                bool Abb = false;
                bunmonum = random.Next(N1, N);
                bunjanum = random.Next(1, bunmonum);
                num = random.Next(2, 31);
                List<int> list = new List<int>();
                List<int> listnum = new List<int>();

                for (int i = 2; i <= bunmonum; i++)
                {
                    if (bunmonum % i == 0)
                    {
                        list.Add(i);
                    }
                }
                for (int i = 2; i <= num; i++)
                {
                    if (num % i == 0)
                    {
                        listnum.Add(i);
                    }
                }

                for (int i = 0; i < list.Count; i++)
                {
                    if (bunjanum % list[i] == 0)
                    {
                        Abb = true;
                        break;
                    }
                }
                while (!Abb)
                {
                    returnnum++;
                    if (returnnum > 100)
                    {
                        MakeNum2(N, N1, N2, B);
                        return;
                    }
                    listnum.Clear();
                    num = random.Next(2, 31);
                    for (int i = 2; i <= num; i++)
                    {
                        if (num % i == 0)
                        {
                            listnum.Add(i);
                        }
                    }
                    for (int i = 0; i < list.Count; i++)
                    {
                        for (int y = 0; y < listnum.Count; y++)
                        {
                            if (listnum[y] % list[i] == 0)
                            {
                                Abb = true;
                                break;
                            }
                        }
                    }
                }


                var a = multAnswernum(bunmonum, bunjanum, 0, num);
                AnswerMo = a.Item1;
                AnswerNum = a.Item2 / a.Item1;
                AnswerJa = a.Item2 % a.Item1;
            } while (AnswerJa == 0);
        }
        else
        {

            do
            {
                int returnnum = 0;
                bool Abb = false;
                bunmonum = random.Next(N1, N);
                bunjanum = random.Next(1, bunmonum);
                num = random.Next(2, 31);
                List<int> list = new List<int>();
                List<int> listnum = new List<int>();

                for (int i = 2; i <= bunmonum; i++)
                {
                    if (bunmonum % i == 0)
                    {
                        list.Add(i);
                    }
                }
                for (int i = 2; i <= num; i++)
                {
                    if (num % i == 0)
                    {
                        listnum.Add(i);
                    }
                }

                for (int i = 0; i < list.Count; i++)
                {
                    if (bunjanum % list[i] == 0)
                    {
                        Abb = true;
                        break;
                    }
                }
                while (!Abb)
                {
                    returnnum++;
                    if (returnnum > 100)
                    {
                        MakeNum2(N, N1, N2, B);
                        return;
                    }
                    Abb = false;
                    listnum.Clear();
                    num = random.Next(2, 31);
                    for (int i = 2; i <= num; i++)
                    {
                        if (num % i == 0)
                        {
                            listnum.Add(i);
                        }
                    }
                    for (int i = 0; i < list.Count; i++)
                    {
                        for (int y = 0; y < listnum.Count; y++)
                        {
                            if (listnum[y] % list[i] == 0)
                            {
                                Abb = true;
                                break;
                            }
                        }
                    }
                }

                bunNum = random.Next(1, N2);

                var a = multAnswernum(bunmonum, bunjanum, bunNum, num);
                AnswerMo = a.Item1;
                AnswerNum = a.Item2 / a.Item1;
                AnswerJa = a.Item2 % a.Item1;
            } while (AnswerJa == 0);
        }
    }
    /// <summary>
    /// 한쪽만 약분되기
    /// </summary>
    /// <param name="N"></param>
    /// <param name="N1"></param>
    /// <param name="N2"></param>
    /// <param name="ja">true면 분자가 false면 자연수가</param>
    /// <param name="B"></param>
    public void MakeNum3(int N, int N1 = 2, int N2 = 3, bool ja = false, bool B = false)
    {
        if (!B)
        {

            do
            {
                int returnnum = 0;
                bool b = false;
                bool b1 = false;
                bunmonum = random.Next(N1, N);
                bunjanum = random.Next(1, bunmonum);
                num = random.Next(2, 31);
                List<int> list = new List<int>();
                List<int> listnum = new List<int>();

                for (int i = 2; i <= bunmonum; i++)
                {
                    if (bunmonum % i == 0)
                    {
                        list.Add(i);
                    }
                }
                for (int i = 2; i <= num; i++)
                {
                    if (num % i == 0)
                    {
                        listnum.Add(i);
                    }
                }
                if (ja)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (bunjanum % list[i] == 0)
                        {
                            b = true;
                            break;
                        }
                    }
                    while (!b)
                    {
                        returnnum++;
                        if (returnnum > 100)
                        {
                            MakeNum3(N, N1, N2, ja, B);
                            return;
                        }
                        b = false;
                        bunjanum = random.Next(1, bunmonum);
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (bunjanum % list[i] == 0)
                            {
                                b = true;
                                break;
                            }
                        }
                    }
                    returnnum = 0;
                    for (int i = 0; i < list.Count; i++)
                    {
                        for (int y = 0; y < listnum.Count; y++)
                        {
                            if (listnum[y] % list[i] == 0)
                            {
                                b1 = true;
                                break;
                            }
                        }
                    }
                    while (b1)
                    {
                        returnnum++;
                        if (returnnum > 100)
                        {
                            MakeNum3(N, N1, N2, ja, B);
                            return;
                        }
                        b1 = false;
                        listnum.Clear();
                        num = random.Next(2, 31);
                        for (int i = 2; i <= num; i++)
                        {
                            if (num % i == 0)
                            {
                                listnum.Add(i);
                            }
                        }
                        for (int i = 0; i < list.Count; i++)
                        {
                            for (int y = 0; y < listnum.Count; y++)
                            {
                                if (listnum[y] % list[i] == 0)
                                {
                                    b1 = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (bunjanum % list[i] == 0)
                        {
                            b = true;
                            break;
                        }
                    }
                    while (b)
                    {
                        returnnum++;
                        if (returnnum > 100)
                        {
                            MakeNum3(N, N1, N2, ja, B);
                            return;
                        }
                        b = false;
                        bunjanum = random.Next(1, bunmonum);
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (bunjanum % list[i] == 0)
                            {
                                b = true;
                                break;
                            }
                        }
                    }
                    returnnum = 0;
                    for (int i = 0; i < list.Count; i++)
                    {
                        for (int y = 0; y < listnum.Count; y++)
                        {
                            if (listnum[y] % list[i] == 0)
                            {
                                b1 = true;
                                break;
                            }
                        }
                    }
                    while (!b1)
                    {
                        returnnum++;
                        if (returnnum > 100)
                        {
                            MakeNum3(N, N1, N2, ja, B);
                            return;
                        }
                        b1 = false;
                        listnum.Clear();
                        num = random.Next(2, 31);
                        for (int i = 2; i <= num; i++)
                        {
                            if (num % i == 0)
                            {
                                listnum.Add(i);
                            }
                        }
                        for (int i = 0; i < list.Count; i++)
                        {
                            for (int y = 0; y < listnum.Count; y++)
                            {
                                if (listnum[y] % list[i] == 0)
                                {
                                    b1 = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                var a = multAnswernum(bunmonum, bunjanum, 0, num);
                AnswerMo = a.Item1;
                AnswerNum = a.Item2 / a.Item1;
                AnswerJa = a.Item2 % a.Item1;
            } while (AnswerJa == 0);
        }
        else
        {

            do
            {
                int returnnum = 0;
                bool b = false;
                bool b1 = false;
                bunmonum = random.Next(N1, N);
                bunjanum = random.Next(1, bunmonum);
                num = random.Next(2, 31);
                List<int> list = new List<int>();
                List<int> listnum = new List<int>();

                for (int i = 2; i <= bunmonum; i++)
                {
                    if (bunmonum % i == 0)
                    {
                        list.Add(i);
                    }
                }
                for (int i = 2; i <= num; i++)
                {
                    if (num % i == 0)
                    {
                        listnum.Add(i);
                    }
                }

                if (ja)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (bunjanum % list[i] == 0)
                        {
                            b = true;
                            break;
                        }
                    }
                    while (!b)
                    {
                        returnnum++;
                        if (returnnum > 100)
                        {
                            MakeNum3(N, N1, N2, ja, B);
                            return;
                        }
                        b = false;
                        bunjanum = random.Next(1, bunmonum);
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (bunjanum % list[i] == 0)
                            {
                                b = true;
                                break;
                            }
                        }
                    }
                    returnnum = 0;
                    for (int i = 0; i < list.Count; i++)
                    {
                        for (int y = 0; y < listnum.Count; y++)
                        {
                            if (listnum[y] % list[i] == 0)
                            {
                                b1 = true;
                                break;
                            }
                        }
                    }
                    while (b1)
                    {
                        returnnum++;
                        if (returnnum > 100)
                        {
                            MakeNum3(N, N1, N2, ja, B);
                            return;
                        }
                        b1 = false;
                        listnum.Clear();
                        num = random.Next(2, 31);
                        for (int i = 2; i <= num; i++)
                        {
                            if (num % i == 0)
                            {
                                listnum.Add(i);
                            }
                        }
                        for (int i = 0; i < list.Count; i++)
                        {
                            for (int y = 0; y < listnum.Count; y++)
                            {
                                if (listnum[y] % list[i] == 0)
                                {
                                    b1 = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (bunjanum % list[i] == 0)
                        {
                            b = true;
                            break;
                        }
                    }
                    while (b)
                    {
                        returnnum++;
                        if (returnnum > 100)
                        {
                            MakeNum3(N, N1, N2, ja, B);
                            return;
                        }
                        b = false;
                        bunjanum = random.Next(1, bunmonum);
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (bunjanum % list[i] == 0)
                            {
                                b = true;
                                break;
                            }
                        }
                    }
                    returnnum = 0;
                    for (int i = 0; i < list.Count; i++)
                    {
                        for (int y = 0; y < listnum.Count; y++)
                        {
                            if (listnum[y] % list[i] == 0)
                            {
                                b1 = true;
                                break;
                            }
                        }
                    }
                    while (!b1)
                    {
                        returnnum++;
                        if (returnnum > 100)
                        {
                            MakeNum3(N, N1, N2, ja, B);
                            return;
                        }
                        b1 = false;
                        listnum.Clear();
                        num = random.Next(2, 31);
                        for (int i = 2; i <= num; i++)
                        {
                            if (num % i == 0)
                            {
                                listnum.Add(i);
                            }
                        }
                        for (int i = 0; i < list.Count; i++)
                        {
                            for (int y = 0; y < listnum.Count; y++)
                            {
                                if (listnum[y] % list[i] == 0)
                                {
                                    b1 = true;
                                    break;
                                }
                            }
                        }
                    }
                }

                bunNum = random.Next(1, N2);

                var a = multAnswernum(bunmonum, bunjanum, bunNum, num);
                AnswerMo = a.Item1;
                AnswerNum = a.Item2 / a.Item1;
                AnswerJa = a.Item2 % a.Item1;
            } while (AnswerJa == 0);
        }
    }


    public System.Tuple<int, int> multAnswernum(int bottom, int top, int num = 0, int num1 = 1)
    {
        top += bottom * num;
        top *= num1;
        int a = bottom;
        int b = top;
        List<int> list = new List<int>();
        List<int> list1 = new List<int>();
        List<int> answerList = new List<int>();
        for (int i = 2; i <= a; i++)
        {
            if (a % i == 0)
                list.Add(i);

        }
        for (int i = 2; i <= b; i++)
        {
            if (b % i == 0)
                list1.Add(i);

        }
        for (int i = 0; i < list.Count; i++)
        {
            for (int y = 0; y < list1.Count; y++)
            {
                if (list[i] == list1[y])
                    answerList.Add(list1[y]);
            }
        }

        int temp = 1;
        int temp1 = 1;
        for (int i = 0; i < answerList.Count; i++)
        {
            if (answerList[i] > temp)
                temp = answerList[i];
        }

        a /= temp;
        b /= temp;

        return new System.Tuple<int, int>(a, b);
    }
    public System.Tuple<int, int> multAnswer(int bottom, int top, int bottom1, int top1, int num = 0, int num1 = 0)
    {
        top += bottom * num;
        top1 += bottom1 * num1;
        int a = bottom * bottom1;
        int b = top * top1;
        List<int> list = new List<int>();
        List<int> list1 = new List<int>();
        List<int> answerList = new List<int>();
        for (int i = 2; i <= a; i++)
        {
            if (a % i == 0)
                list.Add(i);

        }
        for (int i = 2; i <= b; i++)
        {
            if (b % i == 0)
                list1.Add(i);

        }
        for (int i = 0; i < list.Count; i++)
        {
            for (int y = 0; y < list1.Count; y++)
            {
                if (list[i] == list1[y])
                    answerList.Add(list1[y]);
            }
        }

        int temp = 1;
        int temp1 = 1;
        for (int i = 0; i < answerList.Count; i++)
        {
            if (answerList[i] > temp)
                temp = answerList[i];
        }

        a /= temp;
        b /= temp;

        return new System.Tuple<int, int>(a, b);
    }
    public System.Tuple<int, int> diviAnswer(int bottom, int top, int bottom1, int top1, int num = 0, int num1 = 0)
    {
        top += bottom * num;
        top1 += bottom1 * num1;
        int a = bottom * top1;
        int b = bottom1 * top;

        List<int> list = new List<int>();
        List<int> list1 = new List<int>();
        List<int> answerList = new List<int>();

        for (int i = 2; i <= a; i++)
        {
            if (a % i == 0)
                list.Add(i);

        }
        for (int i = 2; i <= b; i++)
        {
            if (b % i == 0)
                list1.Add(i);

        }
        for (int i = 0; i < list.Count; i++)
        {
            for (int y = 0; y < list1.Count; y++)
            {
                if (list[i] == list1[y])
                    answerList.Add(list1[y]);
            }
        }

        int temp = 1;
        int temp1 = 1;
        for (int i = 0; i < answerList.Count; i++)
        {
            if (answerList[i] > temp)
                temp = answerList[i];
        }
        a /= temp;
        b /= temp;

        return new System.Tuple<int, int>(a, b);
    }
    /// <summary>
    /// 기약분수인지아닌지 체크 Item1이 true이면 약분,통분값과 같음 false이면 자체가틀림,Item2가 true이면 기약분수 false이면 약분,통분은되엇지만 기약분수가아님
    /// </summary>
    /// <param name="resultbottom">입력받을 분모</param>
    /// <param name="resulttop">입력받을 분자</param>
    /// <param name="answerbottom">정답 분모</param>
    /// <param name="answertop">정답 분자</param>
    /// <returns></returns>
    public System.Tuple<bool, bool> Abbreviation(int resultbottom, int resulttop, int answerbottom, int answertop)
    {
        bool abbreviation = false;
        bool reduced = false;

        List<int> list = new List<int>();
        List<int> list1 = new List<int>();
        List<int> answerList = new List<int>();

        List<int> answerlist = new List<int>();
        List<int> answerlist1 = new List<int>();
        List<int> answeranswerList = new List<int>();

        for (int i = 2; i <= answerbottom; i++)
        {
            if (answerbottom % i == 0)
                answerlist.Add(i);

        }
        for (int i = 2; i <= answertop; i++)
        {
            if (answertop % i == 0)
                answerlist1.Add(i);

        }
        for (int i = 0; i < answerlist.Count; i++)
        {
            for (int y = 0; y < answerlist1.Count; y++)
            {
                if (answerlist[i] == answerlist1[y])
                    answeranswerList.Add(answerlist1[y]);
            }
        }


        int answertemp = 1;
        for (int i = 0; i < answeranswerList.Count; i++)
        {
            if (answeranswerList[i] > answertemp)
                answertemp = answeranswerList[i];
        }


        answerbottom /= answertemp;
        answertop /= answertemp;



        if (resultbottom % answerbottom != 0 || resulttop % answertop != 0)
            abbreviation = false;
        else
            abbreviation = true;
        if (!abbreviation)
            reduced = false;
        else
        {
            if (resultbottom / answerbottom == 1 && resulttop / answertop == 1)
                reduced = true;
        }

        return new System.Tuple<bool, bool>(abbreviation, reduced);
    }
}
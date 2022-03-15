using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using System;
using System.Linq;
using Random = UnityEngine.Random;
using TMPro;
namespace Funtion_CWS
{
    public static class Funtion_CWS
    {

        public const string FontSize = "<size={0}>{1}</size>";
        public const string FontColor = "<size={0}>{1}<color={2}>{3}</color></size>";
        public static void Tostring(this string msg, int size = 20)
        {
#if UNITY_EDITOR
            string s = string.Format(FontSize, size, msg);
            Debug.Log(s);
#endif
        }
        public static void Tostring1(this string msg, string msg1, int size = 20, string color = "red")
        {
#if UNITY_EDITOR
            string s = string.Format(FontColor, size, msg, color, msg1);
            Debug.Log(s);
#endif
        }
        public static float AnimatonLength(this Animation _A, string s = null)
        {
            if (s != null)
            {
                Animation _a = _A;
                AnimationClip c = null;
                foreach (AnimationState state in _a)
                {
                    if (state.name == s)
                    {
                        c = state.clip;
                    }

                }
                return c.length;
            }
            else
            {
                return _A.clip.length;
            }
        }
        /// <summary>
        /// 특정숫자를 제외한 랜덤숫자뽑기
        /// </summary>
        /// <param name="_contain">배열안에포함된숫자제외</param>
        /// <param name="_min">제일작은숫자</param>
        /// <param name="_max">제일큰숫자</param>
        /// <returns></returns>
        public static int GetRandom(this int[] _contain, int _min, int _max)
        {
            HashSet<int> exclude = new HashSet<int>();
            for (int i = 0; i < _contain.Length; i++)
            {
                exclude.Add(_contain[i]);
            }
            int rand = Random.Range(_min, _max);
            do
            {
                rand = Random.Range(_min, _max);
            } while (exclude.Contains(rand));
            return rand;
        }
        /// <summary>
        /// 특정숫자를 제외한 랜덤숫자뽑기
        /// </summary>
        /// <param name="_contain">배열안에포함된숫자제외</param>
        /// <param name="_min">제일작은숫자</param>
        /// <param name="_max">제일큰숫자</param>
        /// <returns></returns>
        public static int GetRandom(this List<int> _contain, int _min, int _max)
        {
            int rand = Random.Range(_min, _max);
            do
            {
                rand = Random.Range(_min, _max);
            } while (_contain.Contains(rand));
            return rand;
        }
        /// <summary>
        /// 특정숫자를 제외한 랜덤숫자뽑기
        /// </summary>
        /// <param name="_contain">배열안에포함된숫자제외</param>
        /// <param name="_min">제일작은숫자</param>
        /// <param name="_max">제일큰숫자</param>
        /// <returns></returns>
        public static int GetRandom(this int _contain, int _min, int _max)
        {
            int rand = Random.Range(_min, _max);
            do
            {
                rand = Random.Range(_min, _max);
            } while (rand == _contain);
            return rand;
        }
        /// <summary>
        /// 정답후 박스 움직이기
        /// </summary>
        /// <param name="rect">움직일오브젝트</param>
        /// <param name="rect2">답칸박스</param>
        /// <param name="_speed">몇초만에움직일지</param>
        /// <returns></returns>
        public static IEnumerator AnswerTextMove(this RectTransform rect, RectTransform rect2, float _speed)
        {
            float f = (rect2.sizeDelta.x - rect.sizeDelta.x) / 2;
            LayoutRebuilder.ForceRebuildLayoutImmediate(rect.GetComponent<RectTransform>());
            yield return rect.MoveToVector(new Vector3(rect.anchoredPosition3D.x - Mathf.Abs(f), rect.anchoredPosition3D.y), _speed);
        }
        public static void MSAA_bool(this Transform _T)
        {
            List<CompedVectorImage> a = new List<CompedVectorImage>();
            a = _T.GetComponentsInChildren<CompedVectorImage>(true).ToList();
            Camera.main.allowMSAA = a.Count > 0;
        }
        /// <summary>
        /// html언어로 색을 바꿧을경우 text안에서 글자색 바꾸는 함수
        /// </summary>
        /// <param name="t"></param>
        /// <param name="s">#을제외한 6글자</param>
        public static void LocalizationColor(this TextMeshProUGUI t, string s)
        {
            string originname = t.text;
            int num = s.Length;
            if (originname.Contains("color"))
            {
                char[] c = originname.ToCharArray();
                char[] col = s.ToCharArray();
                for (int i = 0; i < originname.Length; i++)
                {
                    if (originname.Substring(i, 1) == "#")
                    {
                        for (int y = 1; y < num + 1; y++)
                        {
                            c[i + y] = col[y - 1];
                        }
                        originname = new string(c);
                        break;
                    }
                }
            }

            t.text = originname;

        }
        /// <summary>
        /// Color색을 Html언어로 바꿧을경우 text안에서 글자색 바꾸는 함수
        /// </summary>
        /// <param name="t"></param>
        /// <param name="s">#을제외한 6글자</param>
        public static void LocalizationColor(this TextMeshProUGUI t, Color color)
        {
            string originname = t.text;
            string s = ColorToStr(color);
            Debug.Log(s);
            int num = s.Length;
            if (originname.Contains("color"))
            {
                char[] c = originname.ToCharArray();
                char[] col = s.ToCharArray();
                for (int i = 0; i < originname.Length; i++)
                {
                    if (originname.Substring(i, 1) == "#")
                    {
                        for (int y = 1; y < num + 1; y++)
                        {
                            c[i + y] = col[y - 1];
                        }
                        originname = new string(c);
                        break;
                    }
                }
            }

            t.text = originname;

        }
        public static System.Tuple<string, string, string> ToSosu(this int left, int right)
        {
            string a = "";
            string b = "";
            string c = "";
            return new System.Tuple<string, string, string>(a, b, c);
        }
        public static string ColorToStr(Color color)
        {
            Color blend = Color.white;
            int alpha = (int)color.a;
            int r = (int)((alpha / 255) * color.r + (1 - alpha / 255) * blend.r);
            int g = (int)((alpha / 255) * color.g + (1 - alpha / 255) * blend.g);
            int b = (int)((alpha / 255) * color.b + (1 - alpha / 255) * blend.b);
            string result = string.Format("{0}{1}{2}", r, g, b);

            return result;
        }

        public static IEnumerator PopText(this TextMeshProUGUI _T, int size, Color color, float time = 0.5f)
        {
            string originname = _T.text;
            int num = size.ToString().Length;
            if (originname.Contains("size"))
            {
                char[] c = originname.ToCharArray();
                for (int i = 0; i < originname.Length; i++)
                {
                    if (originname.Substring(i, 5) == "size=")
                    {
                        float t = 0;
                        int originnum = int.Parse(originname.Substring(i + 5, 2));
                        int s = int.Parse(originname.Substring(i + 5, 2));
                        int minus = size - s;
                        int cursize = int.Parse(originname.Substring(i + 5, 2));
                        int Z = cursize + minus;
                        float f = (time * 0.5f) / (float)minus;
                        while (cursize < Z)
                        {
                            cursize++;
                            char[] col = cursize.ToString().ToCharArray();
                            c[i + 5] = col[0];
                            c[i + 6] = col[1];
                            _T.text = new string(c);
                            yield return new WaitForSeconds(f);
                        }
                        while (originnum < cursize)
                        {
                            cursize--;
                            char[] col = cursize.ToString().ToCharArray();
                            c[i + 5] = col[0];
                            c[i + 6] = col[1];
                            _T.text = new string(c);
                            yield return new WaitForSeconds(f);
                        }
                        break;
                    }
                }
            }
        }
        public static void NumAdd(this List<int> _a, int _num)
        {
            int rand = Random.Range(0, _num);
            for (int i = 0; i < _num;)
            {
                if (_a.Contains(rand))
                {
                    rand = Random.Range(0, _num);
                    continue;
                }
                else
                {
                    _a.Add(rand);
                    i++;
                }
            }
        }
        public static float Tososu(this int num, int bottom, int top)
        {
            return num + ((float)top / (float)bottom);
        }
        public static void SortFloat(this float[] _F)
        {
            float temp = 0;
            for (int i = 0; i < _F.Length; i++)
            {
                for (int j = i + 1; j < _F.Length; j++)
                {
                    if (_F[i] > _F[j])
                    {
                        temp = _F[i];
                        _F[i] = _F[j];
                        _F[j] = temp;
                    }
                }
            }
        }

        public static string Sosu(this int a, int b)
        {
            string s = "";
            int r = 0;
            int length = 0;
            if (a == 0 || b == 0)
            {
                return "0이 있습니다.";
            }
            if (a % b == 0)
            {
                s = (a / b).ToString();
            }
            else
            {
                r = (a * (1000000)) / b;

                length = Mathf.Abs(b.ToString().Length - a.ToString().Length);

                string temp = (r % 1000000).ToString();
                char[] p = temp.ToCharArray();
                for (int i = temp.Length - 1; i > 0; i--)
                {

                    if (temp.Substring(i, 1) != "0")
                    {
                        break;
                    }
                    else
                        p[i] = ' ';
                }
                temp = new string(p).Trim();
                s = $"{r / 1000000}.{int.Parse(temp).ToString("D" + length)}";
            }


            char[] phrase = s.ToCharArray();
            if (s.Contains("."))
            {
                for (int i = s.Length - 1; i > 0; i--)
                {
                    if (s.Substring(i, 1) != "0")
                    {
                        if (s.Substring(i, 1) == ".")
                        {
                            phrase[i] = ' ';
                            break;
                        }
                        break;
                    }
                    else
                        phrase[i] = ' ';
                }
            }
            s = new string(phrase).Trim();
            int num = 0;
            if (s.Contains("."))
            {
                num = s.Length - s.IndexOf(".") - 1;
            }
            if (num > 3)
            {
                return null;
            }
            return s;
        }
        public static void SortInt(this int[] _F)
        {
            int temp = 0;
            for (int i = 0; i < _F.Length; i++)
            {
                for (int j = i + 1; j < _F.Length; j++)
                {
                    if (_F[i] > _F[j])
                    {
                        temp = _F[i];
                        _F[i] = _F[j];
                        _F[j] = temp;
                    }
                }
            }
        }

        /// <summary>
        /// 화영님용
        /// </summary>
        /// <param name="I"></param>
        /// <param name="B"></param>
        public static void ImageChange(this Image I, bool B = true)
        {
            if (B)
            {
                Color c;
                ColorUtility.TryParseHtmlString("#E90006", out c);
                I.color = c;
            }
            else
            {
                Color c;
                ColorUtility.TryParseHtmlString("#A5A5A5", out c);
                I.color = c;
            }
        }
        /// <summary>
        /// 가람님용
        /// </summary>
        /// <param name="I"></param>
        /// <param name="B"></param>
        public static void ImageChange1(this Image I, bool B = true)
        {
            if (B)
            {
                Color c;
                ColorUtility.TryParseHtmlString("#E90006", out c);
                I.color = c;
            }
            else
            {
                Color c;
                ColorUtility.TryParseHtmlString("#888888", out c);
                I.color = c;
            }
        }
        public static void EnableBox(GameObject _g)
        {
            _g.transform.gameObject.SetActive(!_g.transform.gameObject.active);
        }
        public static void EnableBox(Transform _g)
        {
            _g.gameObject.SetActive(!_g.gameObject.active);
        }
        /// <summary>
        /// 투명도조절
        /// </summary>
        /// <param name="_g">오브젝트</param>
        /// <param name="_f">감소수치</param>
        /// <param name="b">true일때 감소 false증가</param>
        public static IEnumerator AlphaControl(this Transform _g, float _f, bool b)
        {
            if (b)
            {
                while (_g.GetComponent<CanvasGroup>().alpha > 0)
                {
                    _g.GetComponent<CanvasGroup>().alpha -= _f * Time.deltaTime;
                    yield return null;
                }
            }
            else
            {
                while (_g.GetComponent<CanvasGroup>().alpha < 1)
                {
                    _g.GetComponent<CanvasGroup>().alpha += _f * Time.deltaTime;
                    yield return null;
                }
            }

        }
        /// <summary>
        /// 투명도조절
        /// </summary>
        /// <param name="_g">오브젝트</param>
        /// <param name="_f">감소수치</param>
        /// <param name="b">true일때 감소 false증가</param>
        public static IEnumerator AlphaControl1(this Transform _g, float a, float _f = 0.5f)
        {
            bool b;
            if (_g.GetComponent<CanvasGroup>().alpha < a)
                b = true;
            else
                b = false;
            if (b)
            {
                while (_g.GetComponent<CanvasGroup>().alpha < a)
                {
                    _g.GetComponent<CanvasGroup>().alpha += _f * Time.deltaTime;
                    yield return null;
                }
            }
            else
            {
                while (_g.GetComponent<CanvasGroup>().alpha > a)
                {
                    _g.GetComponent<CanvasGroup>().alpha -= _f * Time.deltaTime;
                    yield return null;
                }
            }

        }
        /// <summary>
        /// 틀렸을때 상자흔들림
        /// </summary>
        /// <param name="target">어떤게흔들릴지</param>
        /// <param name="time">흔들릴시간</param>
        /// <param name="power">크기</param>
        /// <returns></returns>
        public static IEnumerator ShakeObjectByAnchored3D(this RectTransform target, float time = 1f, float power = 20f, float width = 0.5f)
        {
            Vector3 origin = target.anchoredPosition3D;

            float curWidth = width;

            float timer = 0f;

            float curPower = power;

            var wait = new WaitForSeconds(0.01f);

            while (timer < time)
            {
                yield return wait;

                float posX = Random.Range(-curWidth, curWidth) * curPower;
                float posY = Random.Range(-curWidth, curWidth) * curPower;

                target.anchoredPosition3D = origin + new Vector3(posX, 0, 0);

                timer += Time.deltaTime * 2f;

                if (curPower > 0)
                    curPower -= curWidth;

            }

            target.anchoredPosition3D = origin;

        }

        public static IEnumerator Barfill(this Transform _g, bool b, float f = 0.1f)
        {
            if (b)
            {
                while (_g.GetComponent<Image>().fillAmount < 1)
                {
                    _g.GetComponent<Image>().fillAmount += f * Time.deltaTime;
                    yield return null;
                }
            }
            else
            {
                while (_g.GetComponent<Image>().fillAmount > 0)
                {
                    _g.GetComponent<Image>().fillAmount -= f * Time.deltaTime;
                    yield return null;
                }
            }
        }
        /// <summary>
        /// 특정 name부터 num개를 list에 넣을지
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <param name="num"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T FindGameObjectByName<T>(this Transform parent, string name, int num, List<T> list) where T : Component
        {

            int index = 0;
            var childList = new List<T>();
            parent.GetComponentsInChildren<T>(true, childList);

            foreach (var child in childList)
            {
                for (int i = 0; i < childList.Count; i++)
                {
                    if (childList[i].name == name)
                    {
                        index = i;
                        break;
                    }
                }
                for (int i = 0; i < num; i++)
                {
                    list.Add(childList[index + i]);
                }
                break;

            }
            return null;
        }
        /// <summary>
        /// num에 갯수까지 하되 end랑 이름이같으면 종료
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <param name="end"></param>
        /// <param name="names"></param>
        /// <param name="num"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T FindGameObjectByNameend<T>(this Transform parent, string name, string end, string names, int num, List<T> list) where T : Component
        {

            int index = 0;
            var childList = new List<T>();
            parent.GetComponentsInChildren<T>(true, childList);

            foreach (var child in childList)
            {
                for (int i = 0; i < childList.Count; i++)
                {
                    if (childList[i].name == name)
                    {
                        index = i;
                        break;
                    }
                }
                for (int i = 0; i < num; i++)
                {
                    if (childList[index + i].name == names)
                        continue;
                    else
                    if (childList[index + i].name == end)
                    {
                        list.Add(childList[index + i]);
                        break;
                    }
                    list.Add(childList[index + i]);
                }
                break;

            }
            return null;
        }

        /// <summary>
        /// num에 갯수까지 하되 end랑 이름이같으면 종료
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <param name="end"></param>
        /// <param name="names"></param>
        /// <param name="num"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T FindGameObjectByNameend<T>(this Transform parent, string name, string end, string names, string namess, int num, List<T> list) where T : Component
        {

            int index = 0;
            var childList = new List<T>();
            parent.GetComponentsInChildren<T>(true, childList);

            foreach (var child in childList)
            {
                for (int i = 0; i < childList.Count; i++)
                {
                    if (childList[i].name == name)
                    {
                        index = i;
                        break;
                    }
                }
                for (int i = 0; i < num; i++)
                {
                    if (childList[index + i].name == namess)
                        continue;
                    if (childList[index + i].name == names)
                        continue;
                    else
                    if (childList[index + i].name == end)
                    {
                        list.Add(childList[index + i]);
                        break;
                    }
                    list.Add(childList[index + i]);
                }
                break;

            }
            return null;
        }
        /// <summary>
        /// 특정 name으로부터 names를제외한이름에 num개를 list에넣는다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <param name="names"></param>
        /// <param name="num"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T FindGameObjectByNames<T>(this Transform parent, string name, string names, int num, List<T> list) where T : Component
        {
            int index = 0;
            var childList = new List<T>();
            parent.GetComponentsInChildren<T>(true, childList);

            foreach (var child in childList)
            {
                for (int i = 0; i < childList.Count; i++)
                {
                    if (childList[i].name == name)
                    {
                        index = i;
                        break;
                    }
                }
                for (int i = 0; i < num; i++)
                {
                    if (childList[index + i].name == names)
                        continue;
                    else
                        list.Add(childList[index + i]);
                }
                break;
            }
            return null;
        }
        /// <summary>
        /// 특정 name으로부터 names와namess를제외한이름에 num개를 list에넣는다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <param name="names"></param>
        /// <param name="namess"></param>
        /// <param name="num"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T FindGameObjectByNamess<T>(this Transform parent, string name, string names, string namess, int num, List<T> list) where T : Component
        {
            int index = 0;
            var childList = new List<T>();
            parent.GetComponentsInChildren<T>(true, childList);

            foreach (var child in childList)
            {
                for (int i = 0; i < childList.Count; i++)
                {
                    if (childList[i].name == name)
                    {
                        index = i;
                        break;
                    }
                }
                for (int i = 0; i < num; i++)
                {
                    if (childList[index + i].name == names || childList[index + i].name == namess)
                        continue;
                    else
                        list.Add(childList[index + i]);
                }
                break;
            }
            return null;
        }
        /// <summary>
        /// 특정 name으로부터 names와namess를제외한이름에 num개를 list에넣는다.(names와 같아지면 거기서 add를멈춘다.)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <param name="names"></param>
        /// <param name="namess"></param>
        /// <param name="num"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T FindGameObjectByNamesss<T>(this Transform parent, string name, string array, string names, string namess, int num, List<T> list) where T : Component
        {
            int index = 0;
            var childList = new List<T>();
            parent.GetComponentsInChildren<T>(true, childList);

            foreach (var child in childList)
            {
                for (int i = 0; i < childList.Count; i++)
                {
                    if (childList[i].name == name)
                    {
                        index = i;
                        break;
                    }
                }
                for (int i = 0; i < num; i++)
                {
                    if (childList[index + i].name == array)
                        break;
                    if (childList[index + i].name == names || childList[index + i].name == namess)
                        continue;
                    else
                        list.Add(childList[index + i]);
                }
                break;
            }
            return null;
        }
        public static IEnumerator BoxPapPin(Transform _T, float _Num = 1.1f, float _Num1 = 1f)
        {

            float x = _T.GetComponent<RectTransform>().localScale.x;
            float y = _T.GetComponent<RectTransform>().localScale.y;
            if (x == 1)
            {
                while (_T.GetComponent<RectTransform>().localScale.x <= _Num)
                {
                    x += _Num1 * Time.deltaTime;
                    y += _Num1 * Time.deltaTime;
                    _T.GetComponent<RectTransform>().localScale = new Vector2(x, y);
                    yield return null;
                }
                _T.GetComponent<RectTransform>().localScale = new Vector2(_Num, _Num);
                while (_T.GetComponent<RectTransform>().localScale.x >= 1)
                {
                    x -= _Num1 * Time.deltaTime;
                    y -= _Num1 * Time.deltaTime;
                    _T.GetComponent<RectTransform>().localScale = new Vector2(x, y);
                    yield return null;
                }
                _T.GetComponent<RectTransform>().localScale = new Vector2(1, 1);
            }
        }
        public static IEnumerator SetScaleByAnimationCurve1(this Transform transform, AnimationCurve curve, float speed = 1, bool isSetLastValue = false)
        {
            float targetTime = curve.GetPlayTime();
            float curTime = 0;
            Vector3 originScale = transform.localScale;

            while (curTime <= targetTime)
            {
                float scalar = curve.Evaluate(curTime);
                float scale = scalar * originScale.x;
                float scale1 = scalar * originScale.y;

                transform.localScale = new Vector2(scale, scale1);

                yield return null;

                curTime += Time.deltaTime * speed;
            }

            transform.localScale = isSetLastValue ? Vector3.one * curve.GetLastValue() : originScale;
        }
        public static float GetPlayTime(this AnimationCurve curve)
        {
            if (curve.length > 0)
                return curve.keys[curve.length - 1].time;
            else
                return 0;
        }

        public static float GetLastValue(this AnimationCurve curve)
        {
            if (curve.length > 0)
                return curve.Evaluate(curve.GetPlayTime());
            else
                return 0;

        }
        public static IEnumerator SoundBoxPapPin(Transform _T, float _Num = 1.1f, float _Num1 = 1f)
        {

            float x = _T.GetComponent<RectTransform>().localScale.x;
            float y = _T.GetComponent<RectTransform>().localScale.y;
            if (x == 1)
            {
                while (_T.GetComponent<RectTransform>().localScale.x <= _Num)
                {
                    x += _Num1 * Time.deltaTime;
                    y += _Num1 * Time.deltaTime;
                    _T.GetComponent<RectTransform>().localScale = new Vector2(x, y);
                    yield return null;
                }
                _T.GetComponent<RectTransform>().localScale = new Vector2(_Num, _Num);
                while (_T.GetComponent<RectTransform>().localScale.x >= 1)
                {
                    x -= _Num1 * Time.deltaTime;
                    y -= _Num1 * Time.deltaTime;
                    _T.GetComponent<RectTransform>().localScale = new Vector2(x, y);
                    yield return null;
                }
                _T.GetComponent<RectTransform>().localScale = new Vector2(1, 1);
            }
        }
        /// <summary>
        /// 스케일커지기
        /// </summary>
        /// <param name="_T">커질 오브젝트</param>
        /// <param name="_B">True면 커지기false면 작아지기</param>
        /// <param name="_S">속도</param>
        /// <returns></returns>
        public static IEnumerator ScaleControal(this Transform _T, bool _B, float _S = 2.5f, float _A = 1)
        {
            if (_B)
            {
                float x = _T.GetComponent<RectTransform>().localScale.x;
                float y = _T.GetComponent<RectTransform>().localScale.y;
                while (_T.GetComponent<RectTransform>().localScale.x < _A || _T.GetComponent<RectTransform>().localScale.y < _A)
                {
                    x += _S * Time.deltaTime;
                    y += _S * Time.deltaTime;
                    _T.GetComponent<RectTransform>().localScale = new Vector2(x, y);
                    yield return null;
                }
                _T.GetComponent<RectTransform>().localScale = new Vector2(_A, _A);
            }
            else
            {
                float x = _T.GetComponent<RectTransform>().localScale.x;
                float y = _T.GetComponent<RectTransform>().localScale.y;
                while (_T.GetComponent<RectTransform>().localScale.x > 0)
                    while (_T.GetComponent<RectTransform>().localScale.x > 0 || _T.GetComponent<RectTransform>().localScale.y > 0)
                    {
                        x -= _S * Time.deltaTime;
                        y -= _S * Time.deltaTime;
                        _T.GetComponent<RectTransform>().localScale = new Vector2(x, y);
                        yield return null;
                    }
                _T.GetComponent<RectTransform>().localScale = new Vector2(0, 0);
            }
        }
        /// <summary>
        /// X축 회전
        /// </summary>
        /// <param name="_T">움직일 오브젝트</param>
        /// <param name="_F">몇도 움직일지</param>
        /// <param name="f">속도</param>
        /// <returns></returns>
        public static IEnumerator RotationX(this Transform _T, float _F, float f = 10)
        {
            float p = _T.GetComponent<RectTransform>().eulerAngles.x + _F;
            float x = _T.GetComponent<RectTransform>().eulerAngles.x;
            float y = _T.GetComponent<RectTransform>().eulerAngles.y;
            float z = _T.GetComponent<RectTransform>().eulerAngles.z;
            if (x < _F)
            {
                while (x < p)
                {
                    x += f * Time.deltaTime;
                    _T.GetComponent<RectTransform>().rotation = Quaternion.Euler(x, y, z);
                    yield return null;
                }
                _T.GetComponent<RectTransform>().eulerAngles = new Vector3(p, y, z);
            }
            else
            {
                while (x > p)
                {
                    x -= f * Time.deltaTime;
                    _T.GetComponent<RectTransform>().eulerAngles = new Vector3(x, y, z);
                    yield return null;
                }
                _T.GetComponent<RectTransform>().eulerAngles = new Vector3(p, y, z);
            }
        }
        /// <summary>
        /// Y축 회전
        /// </summary>
        /// <param name="_T">움직일 오브젝트</param>
        /// <param name="_F">몇도 움직일지</param>
        /// <param name="f">속도</param>
        /// <returns></returns>
        public static IEnumerator RotationY(this Transform _T, float _F, float f = 10)
        {
            float p = _T.GetComponent<RectTransform>().eulerAngles.y + _F;
            float x = _T.GetComponent<RectTransform>().eulerAngles.x;
            float y = _T.GetComponent<RectTransform>().eulerAngles.y;
            float z = _T.GetComponent<RectTransform>().eulerAngles.z;
            if (y < _F)
            {
                while (y < p)
                {
                    y += f * Time.deltaTime;
                    _T.GetComponent<RectTransform>().rotation = Quaternion.Euler(x, y, z);
                    yield return null;
                }
                _T.GetComponent<RectTransform>().eulerAngles = new Vector3(x, p, z);
            }
            else
            {
                while (y > p)
                {
                    y -= f * Time.deltaTime;
                    _T.GetComponent<RectTransform>().eulerAngles = new Vector3(x, y, z);
                    yield return null;
                }
                _T.GetComponent<RectTransform>().eulerAngles = new Vector3(x, p, z);
            }
        }

        /// <summary>
        /// 부드럽게커지기
        /// </summary>
        /// <param name="transform">커질오브젝트</param>
        /// <param name="targetScale">목표 크기</param>
        /// <param name="targetTime">몇초동안할지</param>
        /// <returns></returns>
        public static IEnumerator SmoothExpandScale(this Transform transform, Vector3 targetScale, float targetTime = 0.5f)
        {
            var localScale = transform.localScale;

            var addScaleFerSecond = (targetScale - localScale);

            Vector3 direction = (targetScale - localScale).normalized;

            Vector3 originDirection = direction;

            float curTime = 0;

            while (curTime < targetTime)
            {
                direction = targetScale - (transform.localScale + addScaleFerSecond * Time.deltaTime / targetTime);
                direction.Normalize();

                if (//Vector3.Distance(targetScale, transform.localScale + addScaleFerSecond * Time.deltaTime / targetTime) > threshold ||
                    Vector3.Angle(originDirection, direction) < 175f)
                {
                    transform.localScale += addScaleFerSecond * Time.deltaTime / targetTime;
                }
                else
                {
                    transform.localScale = targetScale;
                    break;
                }

                yield return null;
                curTime += Time.deltaTime;
            }

            transform.localScale = targetScale;
        }
        public static IEnumerator LateSound(this int _N, float _time = 0.1f)
        {
            yield return new WaitForSeconds(_time);
            SFX_Prefab_Script.Instance.PlayOneShot(_N);
        }
        public static IEnumerator AnimationEnd(this Animator _A, string s, float f = 1f)
        {
            yield return new WaitUntil(() => _A.GetCurrentAnimatorStateInfo(0).IsName(s) && _A.GetCurrentAnimatorStateInfo(0).normalizedTime >= f);
        }

        /// <summary>
        /// 이미지 fillamount 시간으로 계산
        /// </summary>
        /// <param name="i"></param>
        /// <param name="b"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static IEnumerator ImageFill(this Image i, bool b = true, float f = 1)
        {
            float time = 0;
            if (b)
            {
                while (time < 1)
                {
                    time += Time.deltaTime / f;
                    yield return null;
                    i.fillAmount = time;
                }
            }
            else
            {
                while (time < 1)
                {
                    time += Time.deltaTime / f;
                    yield return null;
                    i.fillAmount = 1 - (time);
                }
            }
        }
        public static IEnumerator AnimationEnd(this Animation _A, string s = null)
        {
            if (s != null)
            {
                Animation _a = _A;
                AnimationClip c = null;
                foreach (AnimationState state in _a)
                {
                    if (state.name == s)
                    {
                        c = state.clip;
                    }
                }
                _A.Play(s);
                yield return new WaitForSeconds(c.length);
            }
            else
            {
                _A.Play();
                yield return new WaitForSeconds(_A.clip.length);
            }
        }
        /// <summary>
        /// 정해진시간동안 목표물로 이동
        /// </summary>
        /// <param name="transform">움직일 오브젝트</param>
        /// <param name="p">이동할위치</param>
        /// <param name="timeToMove">몇초동안갈지</param>
        /// <returns></returns>
        public static IEnumerator MoveToPosition(this Transform transform, Transform p, float timeToMove = 1)
        {
            var currentPos = transform.GetComponent<RectTransform>().anchoredPosition3D;
            var t = 0f;
            Vector3 v = p.GetComponent<RectTransform>().anchoredPosition3D;
            RectTransform rect = transform.GetComponent<RectTransform>();
            while (t < 1)
            {
                rect.anchoredPosition3D = Vector3.Lerp(currentPos, v, t);
                yield return null;
                t += Time.deltaTime / timeToMove;
            }
            rect.anchoredPosition3D = v;
        }
        /// <summary>
        /// 정해진시간동안 목표물로 이동
        /// </summary>
        /// <param name="transform">움직일 오브젝트</param>
        /// <param name="p">이동할위치</param>
        /// <param name="timeToMove">몇초동안갈지</param>
        /// <returns></returns>
        public static IEnumerator MoveToVector(this Transform transform, Vector3 p, float timeToMove = 1)
        {
            var currentPos = transform.GetComponent<RectTransform>().anchoredPosition3D;
            var t = 0f;
            RectTransform rect = transform.GetComponent<RectTransform>();
            while (t < 1)
            {
                rect.anchoredPosition3D = Vector3.Lerp(currentPos, p, t);
                yield return null;
                t += Time.deltaTime / timeToMove;
            }
            rect.anchoredPosition3D = p;
        }
        /// <summary>
        /// 정해진시간동안 목표물로 이동
        /// </summary>
        /// <param name="transform">움직일 오브젝트</param>
        /// <param name="p">이동할위치</param>
        /// <param name="timeToMove">몇초동안갈지</param>
        /// <returns></returns>
        public static IEnumerator MoveToVector_Curve(this Transform transform, AnimationCurve curve, Vector3 p, float timeToMove = 1)
        {
            var currentPos = transform.GetComponent<RectTransform>().anchoredPosition3D;
            var t = 0f;
            RectTransform rect = transform.GetComponent<RectTransform>();
            while (t < 1)
            {
                float x = curve.Evaluate(t) * p.x;
                float y = curve.Evaluate(t) * p.y;
                rect.anchoredPosition3D = Vector3.Lerp(currentPos, new Vector3(x, y), t);
                yield return null;
                t += Time.deltaTime / timeToMove;
            }
            rect.anchoredPosition3D = p;
        }

        /// <summary>
        /// 정해진시간동안 목표물로 이동
        /// </summary>
        /// <param name="transform">움직일 오브젝트</param>
        /// <param name="p">이동할위치</param>
        /// <param name="timeToMove">몇초동안갈지</param>
        /// <returns></returns>
        public static IEnumerator MoveToVector_curve(this Transform transform, AnimationCurve curve, Vector3 p, float timeToMove = 1)
        {
            var currentPos = transform.GetComponent<RectTransform>().anchoredPosition3D;
            var t = 0f;
            RectTransform rect = transform.GetComponent<RectTransform>();
            while (t < 1)
            {
                float x = curve.Evaluate(t) * p.x;
                float y = curve.Evaluate(t) * p.y;
                rect.anchoredPosition3D = Vector3.Lerp(currentPos, new Vector3(x, y), t);
                yield return null;
                t += Time.deltaTime / timeToMove;
            }
            rect.anchoredPosition3D = p;
        }

        /// <summary>
        /// 정해진시간동안 정해진위치까지 월드좌표계가이동
        /// </summary>
        /// <param name="transform">움직일 오브젝트</param>
        /// <param name="p">이동할위치</param>
        /// <param name="timeToMove">몇초동안갈지</param>
        /// <returns></returns>
        public static IEnumerator MoveToWorldPos(this Transform transform, Transform p, float timeToMove = 1)
        {
            var currentPos = transform.position;
            Vector3 pos = p.position;
            var t = 0f;
            while (t < 1)
            {
                transform.position = Vector3.Lerp(currentPos, pos, t);
                yield return null;
                t += Time.deltaTime / timeToMove;
            }
            transform.position = pos;
        }
        /// <summary>
        /// 정해진시간동안 정해진위치까지 월드좌표계가이동
        /// </summary>
        /// <param name="transform">움직일 오브젝트</param>
        /// <param name="p">이동할위치</param>
        /// <param name="timeToMove">몇초동안갈지</param>
        /// <returns></returns>
        public static IEnumerator MoveToWorldPos_curve(this Transform transform, AnimationCurve curve, Transform p, float timeToMove = 1)
        {
            var currentPos = transform.position;
            Vector3 pos = p.position;
            var t = 0f;
            while (t < 1)
            {
                float x = curve.Evaluate(t) * p.position.x;
                float y = curve.Evaluate(t) * p.position.y;
                transform.position = Vector3.Lerp(currentPos, new Vector3(x, y), t);
                yield return null;
                t += Time.deltaTime / timeToMove;
            }
            transform.position = pos;
        }
        /// <summary>
        /// 정해진시간동안 정해진위치까지 월드좌표계가이동
        /// </summary>
        /// <param name="transform">움직일 오브젝트</param>
        /// <param name="p">이동할위치</param>
        /// <param name="timeToMove">몇초동안갈지</param>
        /// <returns></returns>
        public static IEnumerator MoveToWorldPos1(this Transform transform, Transform p, float speedToMove = 1)
        {
            Transform rect = transform;
            while (rect.position != p.position)
            {
                rect.position = Vector3.MoveTowards(rect.position, p.position, speedToMove * Time.deltaTime);
                yield return null;
            }
            transform.position = p.position;
        }

        /// <summary>
        /// 캔버스 그룹의 알파값을 조절.
        /// </summary>
        /// <param name="cg">대상이 되는 캔버스그룹</param>
        /// <param name="isOn">목표 알파값</param>
        /// <param name="speed">알파값이 변하는 속도</param>
        /// <returns></returns>
        public static IEnumerator SetAlphaCanvasGroup_num(this CanvasGroup cg, float _num = 1, float speed = 3f)
        {

            var alpha = cg.alpha;

            while (alpha <= _num)
            {
                cg.alpha = alpha;
                yield return null;
                alpha += Time.deltaTime * speed;
            }
            cg.alpha = _num;
        }
        public static IEnumerator SetColorLerp(this Transform _T, Color c, float _time = 1f)
        {
            if (_T.GetComponent<TextMeshProUGUI>() != null)
            {
                TextMeshProUGUI t;
                t = _T.GetComponent<TextMeshProUGUI>();
                Color originColor = t.color;
                float time = 0;
                while (time < 0)
                {
                    t.color = Color.Lerp(originColor, c, time);
                    yield return null;
                    time += Time.deltaTime / _time;
                }
                t.color = c;
            }
            else if (_T.GetComponent<Image>() != null)
            {
                Image t;
                t = _T.GetComponent<Image>();
                Color originColor = t.color;
                float time = 0;
                while (time < 0)
                {
                    t.color = Color.Lerp(originColor, c, time);
                    yield return null;
                    time += Time.deltaTime / _time;
                }
                t.color = c;
            }

            yield break;
        }
        /// <summary>
        /// 캔버스 그룹의 알파값을 정해진 수치와 시간으로조정
        /// </summary>
        /// <param name="cg">대상이 되는 캔버스그룹</param>
        /// <param name="isOn">목표 알파값</param>
        /// <param name="speed">알파값이 변하는 시간</param>
        /// <returns></returns>
        public static IEnumerator SetAlphaCanvasGroup_numTime(this CanvasGroup cg, float _num = 1, float speed = 1f)
        {

            float alpha = cg.alpha;
            float time = 0;
            float disfloat = Mathf.Abs(alpha - _num);
            if (alpha <= _num)
            {
                while (time <= 1f)
                {
                    cg.alpha = alpha + (disfloat * time);
                    yield return null;
                    time += Time.deltaTime / speed;
                }
            }
            else
            {
                while (time <= 1f)
                {
                    cg.alpha = alpha - (disfloat * time);
                    yield return null;
                    time += Time.deltaTime / speed;
                }
            }




            cg.alpha = _num;
        }


        /// <summary>
        /// 목표까지움직이기
        /// </summary>
        /// <param name="_T">움직일 오브젝트</param>
        /// <param name="_P">목표</param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static IEnumerator MoveToPosition1(this Transform _T, Transform _P, float time = 1)
        {
            RectTransform rect = _T.GetComponent<RectTransform>();
            RectTransform rect1 = _P.GetComponent<RectTransform>();
            while (_T.GetComponent<RectTransform>().anchoredPosition3D != _P.GetComponent<RectTransform>().anchoredPosition3D)
            {
                rect.anchoredPosition3D = Vector3.MoveTowards(rect.anchoredPosition3D, rect1.anchoredPosition3D, time * Time.deltaTime);
                yield return null;
            }
            _T.GetComponent<RectTransform>().anchoredPosition3D = _P.GetComponent<RectTransform>().anchoredPosition3D;
        }
        /// <summary>
        /// 목표까지움직이기
        /// </summary>
        /// <param name="_T">움직일 오브젝트</param>
        /// <param name="_P">목표</param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static IEnumerator MovetoVector1(this Transform _T, Vector3 _P, float time = 1)
        {

            RectTransform rect = _T.GetComponent<RectTransform>();
            while (_T.GetComponent<RectTransform>().anchoredPosition3D != _P)
            {
                rect.anchoredPosition3D = Vector3.MoveTowards(rect.anchoredPosition3D, _P, time * Time.deltaTime);
                yield return null;
            }
            _T.GetComponent<RectTransform>().anchoredPosition3D = _P;
        }
        /// <summary>
        /// 정해진시간동안 스케일변환
        /// </summary>
        /// <param name="transform">움직일 오브젝트</param>
        /// <param name="p">이동할위치</param>
        /// <param name="timeToMove">몇초동안갈지</param>
        /// <returns></returns>
        public static IEnumerator TimeScale(this Transform transform, float X, float Y, float timeToMove = 1)
        {
            Vector3 p = new Vector3(X, Y, 1);
            var currentPos = transform.GetComponent<RectTransform>().localScale;
            var t = 0f;
            while (t < 1)
            {
                transform.GetComponent<RectTransform>().localScale = Vector3.Lerp(currentPos, p, t);
                yield return null;
                t += Time.deltaTime / timeToMove;
            }
            transform.GetComponent<RectTransform>().localScale = new Vector3(X, Y, 1);
        }

        /// <summary>
        /// Z축 회전
        /// </summary>
        /// <param name="_T">움직일 오브젝트</param>
        /// <param name="_F">몇도 움직일지</param>
        /// <param name="f">속도</param>
        /// <returns></returns>
        public static IEnumerator RotationZ(this Transform _T, float _F, float f = 10)
        {
            float p = _T.GetComponent<RectTransform>().eulerAngles.z + _F;
            float x = _T.GetComponent<RectTransform>().eulerAngles.x;
            float y = _T.GetComponent<RectTransform>().eulerAngles.y;
            float z = _T.GetComponent<RectTransform>().eulerAngles.z;
            if (z < _F)
            {
                while (z < p)
                {
                    z += f * Time.deltaTime;
                    _T.GetComponent<RectTransform>().rotation = Quaternion.Euler(x, y, z);
                    yield return null;
                }
                //_T.GetComponent<RectTransform>().eulerAngles = new Vector3(x, y, p);
            }
            else
            {
                while (z > p)
                {
                    z -= f * Time.deltaTime;
                    _T.GetComponent<RectTransform>().eulerAngles = new Vector3(x, y, z);
                    yield return null;
                }
                //_T.GetComponent<RectTransform>().eulerAngles = new Vector3(x, y, p);
            }
        }
        static int Precede(char c)
        {
            switch (c)
            {
                case '(':
                case ')':
                    return 0;
                case '+':
                case '-':
                    return 1;
                case '*':
                case '/':
                    return 2;
            }
            return -1;
        }
        public static string InfixTopostfix(string s)
        {
            Stack<char> infixToPostfixstack = new Stack<char>();

            char[] ca = s.ToCharArray();
            char popChar;
            string t = "";
            for (int i = 0, count = ca.Length; i < count; i++)
            {
                switch (ca[i])
                {
                    case '+':
                    case '-':
                    case '*':
                    case '/':
                        do
                        {
                            if (infixToPostfixstack.Count == 0)
                            {
                                break;
                            }
                            popChar = infixToPostfixstack.Pop();
                            if (Precede(popChar) >= Precede(ca[i]))
                            {
                                t += " " + popChar;
                            }
                            else
                            {
                                infixToPostfixstack.Push(popChar);
                                break;
                            }
                        } while (true);

                        t += " ";
                        infixToPostfixstack.Push(ca[i]);
                        break;

                    case '(':
                        infixToPostfixstack.Push(ca[i]);
                        break;
                    case ')':
                        do
                        {
                            popChar = infixToPostfixstack.Pop();
                            if (popChar == '(')
                            {
                                break;
                            }
                            t += " " + popChar;
                        } while (true);
                        break;

                    default:
                        t += ca[i];
                        break;

                }
            }
            for (int i = 0, count = infixToPostfixstack.Count; i < count; i++)
            {
                t += " " + infixToPostfixstack.Pop();
            }
            return t;
        }
        public static double PostfixProcess(string s, int skillLevel, int tileLevel)
        {
            Stack<double> stack = new Stack<double>();

            double value = 0;
            double op1, op2;

            string[] sa = s.Split(' ');

            if (s.Length == 0)
            {
                throw new System.ArgumentException("Parameter cannot be null", "string");
            }
            else if (s.Length == 1)
            {
                value = Convert.ToDouble(sa[0]);
                return value;
            }
            for (int i = 0, count = sa.Length; i < count; i++)
            {
                if (sa[i] != "+" && sa[i] != "-" && sa[i] != "*" && sa[i] != "/")
                {
                    if (sa[i] == "sLv")
                    {
                        stack.Push(skillLevel);
                    }
                    else if (sa[i] == "atk")
                    {
                        stack.Push(0);
                    }
                    else if (sa[i] == "tLv")
                    {
                        stack.Push(tileLevel);
                    }
                    else
                    {
                        stack.Push((Convert.ToDouble(sa[i])));
                    }
                    continue;
                }
                op2 = stack.Pop();
                op1 = stack.Pop();

                switch (sa[i])
                {
                    case "+": value = op1 + op2; break;
                    case "-": value = op1 - op2; break;
                    case "*": value = op1 * op2; break;
                    case "/": value = op1 / op2; break;
                }
                stack.Push(value);
            }
            value = stack.Pop();
            return value;
        }
        /// <summary>
        /// 수식풀기
        /// </summary>
        /// <param name="_S">수식</param>
        public static int Equation(string _S)
        {
            string St = _S;
            char[] phraseAsChars = St.ToCharArray();
            for (int i = 0; i < St.Length; i++)
            {
                if (St.Substring(i, 1) == "×")
                {
                    //int b = St.IndexOf("×");
                    phraseAsChars[i] = '*';
                }
            }
            for (int i = 0; i < St.Length; i++)
            {
                if (St.Substring(i, 1) == "÷")
                {
                    //int c = St.IndexOf("÷");
                    phraseAsChars[i] = '/';
                }
            }
            for (int i = 0; i < St.Length; i++)
            {
                if (St.Substring(i, 1) == "=")
                {
                    //int d = St.IndexOf("=");
                    phraseAsChars[i] = ' ';
                    phraseAsChars[i].ToString().Replace(" ", "");
                }
            }
            int a = 0;
            for (int i = 0; i < St.Length; i++)
            {
                if (St.Substring(i, 1) == "-")
                {
                    if (i == 0)
                        return -9999;
                    else
                    {
                        bool b = int.TryParse(St.Substring(i - 1, 1), out a);
                        if (!b)
                        {
                            if (St.Substring(i - 1, 1) != ")")
                                return -9999;
                        }
                    }
                }
            }
            St = new string(phraseAsChars).Replace(" ", "");
            St = new string(phraseAsChars).Trim();
            string sTemp = St;

            string sTemp2 = string.Empty;
            double dTemp;
            sTemp2 = InfixTopostfix(sTemp);
            dTemp = PostfixProcess(sTemp2, 0, 0);

            return (int)dTemp;
        }
        public static float Equation2(this string _S)
        {
            string St = _S;
            char[] phraseAsChars = St.ToCharArray();
            for (int i = 0; i < St.Length; i++)
            {
                if (St.Substring(i, 1) == "×")
                {
                    //int b = St.IndexOf("×");
                    phraseAsChars[i] = '*';
                }
            }
            for (int i = 0; i < St.Length; i++)
            {
                if (St.Substring(i, 1) == "÷")
                {
                    //int c = St.IndexOf("÷");
                    phraseAsChars[i] = '/';
                }
            }
            for (int i = 0; i < St.Length; i++)
            {
                if (St.Substring(i, 1) == "=")
                {
                    //int d = St.IndexOf("=");
                    phraseAsChars[i] = ' ';
                    phraseAsChars[i].ToString().Replace(" ", "");
                }
            }
            //int a = 0;
            //for (int i = 0; i < St.Length; i++)
            //{
            //    if (St.Substring(i, 1) == "-")
            //    {
            //        if (i == 0)
            //            return -9999;
            //        else
            //        {
            //            bool b = int.TryParse(St.Substring(i - 1, 1), out a);
            //            if (!b)
            //            {
            //                return -9999;
            //            }
            //        }
            //    }
            //}
            St = new string(phraseAsChars).Replace(" ", "");
            string sTemp = St;

            string sTemp2 = string.Empty;
            double dTemp;
            sTemp2 = InfixTopostfix(sTemp);
            dTemp = PostfixProcess(sTemp2, 0, 0);

            return (float)dTemp;
        }
    }
}
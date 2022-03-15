using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class Spoid : MonoBehaviour
{
    public Image i;
    public Color c;
    Vector3 mpos;
    // Update is called once per frame
    void Update()
    {
        mpos = Input.mousePosition;
        if (Input.GetMouseButton(0))
            StartCoroutine(a());
    }
    private void OnEnable()
    {
        Study_1_GameManager.Instance.Draw_Clear();
    }
    IEnumerator a()
    {
        Texture2D t = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        yield return new WaitForEndOfFrame();
        t.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        t.Apply();

        //추출된 색
        c = t.GetPixel((int)mpos.x, (int)mpos.y);

        i.color = c;
        string s = "#"+ColorUtility.ToHtmlStringRGB(c);
#if UNITY_EDITOR
        EditorGUIUtility.systemCopyBuffer = s;
#endif
        Study_1_GameManager.Instance.SetPenColor(c);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ImageFill : MonoBehaviour
{
    public static ImageFill instance = null;
    [Header("배터리 채워져 있는 이미지")]
    public Sprite fullbty;
    [Header("배터리 라인")]
    public Sprite btyline;
    [Header("배터리 채울 배경")]
    public Sprite btybg;
    [Header("무슨색으로할지")]
    public Color color;
    Imagemask im;
    [Header("스크린샷 몇번인지")]
    public int gaze;
    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        instance = this;
        gaze = Stun.stunCount;
        GameObject s = new GameObject();
        s.AddComponent<Image>();
        s.transform.parent = transform;
        s.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        s.GetComponent<RectTransform>().localScale = Vector3.one;
        im = s.gameObject.AddComponent<Imagemask>();
    }

    public void Fill(int N, float f = 0.5f)
    {
        im.FillGaze(N, f);
    }
    public void FillPerfect(int n)
    {
        im.FillPerfect(n);
    }
}

[RequireComponent(typeof(Mask))]

public class Imagemask : MonoBehaviour
{
    public Image img;
    public RectTransform imgrect;
    public RectTransform imgrect2;
    Image rectimg;
    Image rectimg2;
    RectTransform rect;
    float gaze;
    public float minus;
    public float plus;
    Mask m;
    private void Awake()
    {
        gaze = (float)ImageFill.instance.gaze;
        GameObject s = new GameObject();
        s.AddComponent<Image>();
        s.transform.parent = transform;
        s.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        s.GetComponent<RectTransform>().localScale = Vector3.one;

        GameObject s1 = new GameObject();
        s1.AddComponent<Image>();
        s1.transform.parent = transform;
        s1.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        s1.GetComponent<RectTransform>().localScale = Vector3.one;


        m = GetComponent<Mask>();
        m.showMaskGraphic = false;
        rect = GetComponent<RectTransform>();
        img = GetComponent<Image>();
        img.sprite = ImageFill.instance.fullbty;
        imgrect = transform.GetChild(0).GetComponent<RectTransform>();
        rectimg = imgrect.GetComponent<Image>();
        rectimg.sprite = ImageFill.instance.btyline;
        imgrect2 = transform.GetChild(1).GetComponent<RectTransform>();
        rectimg2 = imgrect2.GetComponent<Image>();
        rectimg2.sprite = ImageFill.instance.btybg;
        SetSize();
        ManagerCS.MInstance.BtyFill = rectimg2;
    }
    public void SetSize()
    {
        img.SetNativeSize();
        imgrect.sizeDelta = rect.sizeDelta;
        imgrect2.sizeDelta = rect.sizeDelta;
        rectimg2.type = Image.Type.Filled;
        rectimg2.fillMethod = Image.FillMethod.Horizontal;
        rectimg2.fillOrigin = (int)Image.OriginHorizontal.Right;
        rectimg2.color = ImageFill.instance.color;
        rectimg2.fillAmount = 1;
    }
    public void FillGaze(int n,float f1=0.5f)
    {
        StartCoroutine(Fill(n,f1));
    }
    public void FillPerfect(int n)
    {
        float f = (1 / gaze) * n;
        rectimg2.fillAmount = f;
    }
    IEnumerator Fill(int n,float f1)
    {
        float time = 0;
        float originf = rectimg2.fillAmount;
        float f = (1 / gaze)*n;
        minus = originf - f;
        plus = Mathf.Abs(originf - f);
        if (f<originf)
        {
            
            while (time < 1)
            {
                
                time += Time.deltaTime / f1;
                rectimg2.fillAmount = originf - (minus*time);
                yield return null;
            }
        }
        else
        {
            while (time < 1)
            {
                time += Time.deltaTime / f1;
                rectimg2.fillAmount = originf + (plus * time);
                yield return null;
            }
        }
    }
}

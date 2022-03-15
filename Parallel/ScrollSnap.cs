using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollSnap : MonoBehaviour
{
    public RectTransform panel;
    public Button[] btn;
    public RectTransform center;

   
    public float[] distance;
    public float[] distReposition;
    private bool dragging = false;
    private int btndistance;
    public static int minButtonNum;
    private int btnLength;

   
    void Start()
    {
        
        int btnLength = btn.Length;
        distance = new float[btnLength];
        distReposition = new float[btnLength];

        btndistance = (int)Mathf.Abs(btn[1].GetComponent<RectTransform>().anchoredPosition.x 
            - btn[0].GetComponent<RectTransform>().anchoredPosition.x);
        //btndistance = 두번째버튼의 x좌표 - 첫번쨰버튼의 x좌표 의 절댓값
     
    }
    void Update()
    {
        for (int i=0; i<btn.Length; i++)
        {
            distReposition[i] = center.GetComponent<RectTransform>().position.x
                - btn[i].GetComponent<RectTransform>().position.x;
            //center :960 변동 없음
            //distReposition[i] = 센터의 x좌표 - 버튼[i]의 x좌표    
            distance[i] = Math.Abs(distReposition[i]);

           if(distReposition[i]>=-50 && distReposition[i]<=50)
            {
                btn[i].GetComponent<RectTransform>().localScale =
                   Vector2.Lerp(btn[i].GetComponent<RectTransform>().localScale,
                   new Vector2(1.2f, 1.2f), 10f*Time.deltaTime);

            }
            else 
            {
                btn[i].GetComponent<RectTransform>().localScale =
                   Vector2.Lerp(btn[i].GetComponent<RectTransform>().localScale,
                   new Vector2(0.8f, 0.8f), 10f * Time.deltaTime);
                
            }
            if (distReposition[i] > 450)
            {
                
                float curX = btn[i].GetComponent<RectTransform>().anchoredPosition.x;                
                float curY = btn[i].GetComponent<RectTransform>().anchoredPosition.y;                
                Vector2 newAnchoredPos = new Vector2(curX + (btn.Length * btndistance), curY);
           
                btn[i].GetComponent<RectTransform>().anchoredPosition = newAnchoredPos;
               
            }

            if (distReposition[i] < -450)
            {
                float curX = btn[i].GetComponent<RectTransform>().anchoredPosition.x;
                float curY = btn[i].GetComponent<RectTransform>().anchoredPosition.y;

                Vector2 newAnchoredPos = new Vector2(curX - (btn.Length * btndistance), curY);
                btn[i].GetComponent<RectTransform>().anchoredPosition = newAnchoredPos;

                btn[i].GetComponent<RectTransform>().localScale =
                    Vector2.Lerp(btn[i].GetComponent<RectTransform>().localScale,
                    new Vector2(0.8f, 0.8f), 1f);

            }

        }

        float minDistance = Mathf.Min(distance);

        for(int a=0; a<btn.Length; a++)
        {
            if(minDistance == distance[a])
            {
                minButtonNum = a;
            }
        } 

        if(!dragging)
        {
            //LerpToBtn(minButtonNum * -btndistance);
            LerpToBtn(-btn[minButtonNum].GetComponent<RectTransform>().anchoredPosition.x);
        }
    }

    public void ClickLeftRight()
    {
        if (EventSystem.current.currentSelectedGameObject.GetComponent<RectTransform>().anchoredPosition.x > btn[minButtonNum].GetComponent<RectTransform>().anchoredPosition.x)
        {
            Debug.Log("qwe");
            for(int i=0; i<btn.Length; i++)
            {
                btn[i].transform.Translate(-150, 0, 0);
            }
           
        }
        if (EventSystem.current.currentSelectedGameObject.GetComponent<RectTransform>().anchoredPosition.x <btn[minButtonNum].GetComponent<RectTransform> ().anchoredPosition.x)
        {
            Debug.Log("asd");
            for (int i = 0; i < btn.Length; i++)
            {
                btn[i].transform.Translate(150, 0, 0);
            }
           
        }
    }

     void LerpToBtn(float position)
     {
        float newX = Mathf.Lerp(panel.anchoredPosition.x, position, Time.deltaTime * 10f);
        Vector2 newPosition = new Vector2(newX, panel.anchoredPosition.y);

        panel.anchoredPosition = newPosition;

    }

    public void StartDrag()
    {
        dragging = true;
    }

    public void EndDrag()
    {
        dragging = false;
    }
     


}

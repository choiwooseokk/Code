using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(LineRenderer))]
public class rope : MonoBehaviour
{
    public Transform t;
    public List<Transform> moveobejct = new List<Transform>();
    public LineRenderer lineRenderer;
    public int segmentCount = 15;
    public int constrairtLoop = 15;
    public float segmentLength = 0.1f;
    public float ropeWidth = 0.1f;
    public Vector2 gravity = new Vector2(0f, -9.81f);
    public float ImageLength = 2;
    public GameObject image;
    public float screenx;
    public float screeny;
    //public float SizeY;
    [Space(10f)] //처음위치
    public Transform startTransform;

    private List<Segment> segments = new List<Segment>();

    private void Reset()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.white;

    }

    private void Awake()
    {
        lineRenderer.useWorldSpace = false;
        Camera camera = Camera.main;
        Vector3 Temp = Camera.main.ViewportToWorldPoint(Vector3.one) - Camera.main.ViewportToWorldPoint(Vector3.zero);
        if (Temp.x > Temp.y / 9 * 16)
            Temp.x = Temp.y / 9 * 16;
        else
            Temp.y = Temp.x * 9 / 16;
        screenx = Temp.x;
        screeny = Temp.y;

        float SizeY = (screeny / 2) / Camera.main.orthographicSize;
        ropeWidth *= SizeY;
        if (t != null)
        {
            Transform[] g = t.GetComponentsInChildren<Transform>();
            foreach (Transform ga in g)
            {
                if (ga != t)
                    moveobejct.Add(ga);
            }
        segmentCount = moveobejct.Count;
        }
        startTransform.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(startTransform.GetComponent<RectTransform>().anchoredPosition3D.x, 
            startTransform.GetComponent<RectTransform>().anchoredPosition3D.y);
        //segmentLength *= SizeY;
        //시작할때 점 위치지정
        Vector2 segmentPos = new Vector2(startTransform.GetComponent<RectTransform>().anchoredPosition3D.x, startTransform.GetComponent<RectTransform>().anchoredPosition3D.y);
        for(int i=0;i<segmentCount;i++)
        {
            segments.Add(new Segment(segmentPos));
            segmentPos.y -= segmentLength; //계속 아래로
        }
    }
    
    private void FixedUpdate()
    {
        UpdateSegemts();
        for(int i=0;i<constrairtLoop;i++)
        {
            ApplyConstraint();
        }
        DrawRope();
    }

    /// <summary>
    /// 라인렌더러로 점 그려줌
    /// </summary>
    private void DrawRope()
    {
        if (t == null)
        {
            lineRenderer.startWidth = ropeWidth;
            lineRenderer.endWidth = ropeWidth;
        }
        Vector3[] segmentPositions = new Vector3[segments.Count];
        for (int i=0;i<segments.Count;i++)
        {
            //segmentPositions[i] = segments[i].position;
            segmentPositions[i] = new Vector3(segments[i].position.x, segments[i].position.y, 0);
            if (t != null)
            {
                moveobejct[i].transform.position = segments[i].position;
            }
        }
        Vector3 imageLength = new Vector3(segments[segments.Count - 1].position.x, segments[segments.Count - 1].position.y, 0) - new Vector3(0, ImageLength, 0);
        if (image!=null)
        image.transform.GetComponent<RectTransform>().anchoredPosition3D= imageLength;

        if (t == null)
        {
            lineRenderer.positionCount = segmentPositions.Length;
            lineRenderer.SetPositions(segmentPositions);
        }
    }

    private void UpdateSegemts()
    {
        for(int i=0;i<segments.Count;i++)
        {
            segments[i].velocity = segments[i].position - segments[i].previousPos;
            segments[i].previousPos = segments[i].position;
            segments[i].position += gravity * Time.fixedDeltaTime * Time.fixedDeltaTime;
            segments[i].position += segments[i].velocity;
        }
    }

    private void ApplyConstraint()
    {
        segments[0].position = startTransform.GetComponent<RectTransform>().anchoredPosition3D;
        for(int i=0;i<segments.Count-1;i++)
        {
            float distance = (segments[i].position - segments[i + 1].position).magnitude;
            float difference = segmentLength - distance;
            Vector2 dir =(segments[i + 1].position - segments[i].position).normalized;

            Vector2 movement = dir * difference;
            if (i == 0)
                segments[i + 1].position += movement;
            else
            {
                segments[i].position -= movement*0.5f;
                segments[i + 1].position += movement * 0.5f;
            }
        }
    }

    public class Segment
    {
        public Vector2 previousPos;
        public Vector2 position;
        public Vector2 velocity;

        

        

        public Segment(Vector2 _position)
        {
            previousPos = _position;
            position = _position;
            velocity = Vector2.zero;
        }
    }
}

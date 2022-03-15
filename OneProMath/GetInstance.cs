using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExpandFuncs_Common;
public abstract class GetInstance<T> : IStage2 where T : GetInstance<T>
{
    protected static T instance;
    // Start is called before the first frame update
    public static T Instance()
    {
        if (instance == null)
            instance = FindObjectOfType<T>();
        return instance;
    }
    protected virtual void Awake()
    {

        if (instance == null)
        {
            instance = this as T;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            StartCoroutine(TrueAnswer());
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            for (int i = 0; i < (maxQno - curQno) - 1; i++)
            {
                Study_1_GameManager.Instance.next_question_bar();
                Study_1_GameManager.Instance.next_question_number();
            }
            Study_1_GameManager.Instance.next_question_bar();
            Study_1_GameManager.Instance.StopTimer();
            GameObject outro_anim = transform.FindGameObjectByName("Outro");
            if(outro_anim!=null)
            {
                outro_anim.SetActive(!outro_anim.activeSelf);
            }
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            GameObject intro_anim = transform.FindGameObjectByName("Intro");
            if (intro_anim != null)
            {
                if (intro_anim.activeSelf)
                {
                    Intro();
                    intro_anim.SetActive(!intro_anim.activeSelf);
                }
                else
                {
                    intro_anim.SetActive(!intro_anim.activeSelf);
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.C))
        {
            GameObject PenObject = transform.FindGameObjectByName("SetPenColor");
            if (PenObject != null)
            {
                PenObject.SetActive(!PenObject.activeSelf);
            }
        }
    }
#endif
}

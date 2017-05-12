using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIBehaviour : MonoBehaviour
{

    private Text[] textFields;
    private float Timer;
    private bool timeEnd;
    private bool countdown;
    public float uiTimer;

    // Reference to the UI's health bar.
    // Use this for initialization
    void Start()
    {
        Timer = -99f;
        timeEnd = false;
        textFields = GetComponentsInChildren<Text>();
        uiTimer = 30;
        getText("Timer").text = uiTimer.ToString();

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        Timer -= Time.deltaTime;

        if (Timer < 0 && timeEnd)
        {
            
            StartCoroutine(FadeTextToZeroAlpha(1f, getText("Instructions")));
            timeEnd = false;
            uiTimer = 30f;
            startTimer();
        }
       
        if (countdown && uiTimer > 0)
        {
            uiTimer -= Time.deltaTime;
            getText("Timer").color = new Color(1, (uiTimer / 30), (uiTimer / 30));
            getText("Timer").text = Mathf.CeilToInt(uiTimer).ToString();
        }
        
    }

    private Text getText(string systemName)
    {
        foreach (Text tf in textFields)
        {
            if (tf.name == systemName)
            {
                return tf;
            }
        }
        return null;
    }

    private Image getImage(string systemName)
    {
        //foreach (Image tf in images)
        // {
        //    if (tf.name == systemName)
        //   {
        //     return tf;
        //   }
        // }
        return null;
    }

    public void setInstructions(string t)
    {
        getText("Instructions").text = t;
        Timer = 5f;
        timeEnd = true;
        StartCoroutine(FadeTextToFullAlpha(1f, getText("Instructions")));

    }

    public void startTimer()
    {
        countdown = true;
    }

    public IEnumerator FadeTextToFullAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }

    public IEnumerator FadeTextToZeroAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }
}
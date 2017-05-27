﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIBehaviour : MonoBehaviour
{

    private Text[] textFields;
    private Image[] images;
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
        images = GetComponentsInChildren<Image>();
        uiTimer = float.MaxValue;
        //getText("Timer").text = "Timer";

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
            //uiTimer = 30f;
            //startTimer();
        }
       
        if (countdown)
        {
            if (uiTimer >= 0)
            {
                uiTimer -= Time.deltaTime;
                getText("Timer").color = new Color(1, (uiTimer / 30), (uiTimer / 30));
                getText("Timer").text = Mathf.CeilToInt(uiTimer).ToString();
            } else
            {
                getText("Timer").color = new Color(1, 1, 1);
                getText("Timer").text = "END!";
                getText("tIcon").text = "";
                getText("Timer").transform.localPosition = new Vector3(0, 244, 0);
            }
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
        foreach (Image tf in images)
        {
            if (tf.name == systemName)
            {
                return tf;
            }
        }
        return null;
    }

    public void showOpeningInstructions()
    {
        Timer = 30;
        timeEnd = true;
        getText("openInstructions").enabled = true;
        getImage("bImage").enabled = true;
        StartCoroutine(FadeTextToFullAlpha(3f, getText("openInstructions")));
 
    }

    public void showClosingResults(int oneScore, int twoScore, int threeScore, int fourScore)
    {
        Timer = 30;
        timeEnd = true;
        getText("closingResults").enabled = true;
        getText("closingResults").text = "Gold Count: \n \n Player 1: " + oneScore + " \n Player 2: " + twoScore + "\n Player 3: " + threeScore + " \n Player 4: " + fourScore;
        getImage("bImage").enabled = true;
        StartCoroutine(FadeTextToFullAlpha(3f, getText("closingResults")));

    }


    public void setInstructions(string t, float time = 5.0f)
    {
        getText("Instructions").text = t;
        Timer = time;
        timeEnd = true;
        StartCoroutine(FadeTextToFullAlpha(1f, getText("Instructions")));

    }

    public void startTimer(float limit = 30f)
    {
        uiTimer = limit;
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
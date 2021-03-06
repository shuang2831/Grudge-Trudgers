﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IceLevelLogic : MonoBehaviour
{
    private GameObject[] players;
    private PlayerController[] playerControllers;
    private AudioSource[] sounds;
    private bool isCutscene;
    private bool isClosing;
    private float openTimer;
    private float closingTimer;
    private string uiState;
    private UIBehaviour UIcanvas;
    private bool[] freezing;
    private float[] fTimer;
    private bool[] frozen;

    // Use this for initialization
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");

        UIcanvas = GameObject.FindGameObjectWithTag("UI").GetComponent<UIBehaviour>();

        sounds = GetComponents<AudioSource>();

        isCutscene = true;
        openTimer = 5f;
        closingTimer = 5f;
        openingScene();
        uiState = "begin";
        freezing = new bool[] { false, false, false, false };
        fTimer = new float[] { 3.0f, 3.0f, 3.0f, 3.0f };
        frozen = new bool[] { false, false, false, false };


    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("goal").GetComponent<GoalLogic>().finish)
        {
            UIcanvas.uiTimer = -1;
        }

        foreach (GameObject player in players)
        {
            if (player.transform.position.z > -13 && (player.transform.position.z < 13))
            {
                if (player.GetComponent<Rigidbody>().velocity.x < 0.5 && player.GetComponent<Rigidbody>().velocity.z < 0.5 && player.GetComponent<Rigidbody>().velocity.x > -0.5 && player.GetComponent<Rigidbody>().velocity.z > -0.5)
                {
                    //player.GetComponent<Rigidbody>().isKinematic = false;
                    //player.GetComponent<PlayerController>().enabled = true;
                    player.GetComponent<PlayerController>().slipping = false;
                }
                else
                {
                    //player.GetComponent<Rigidbody>().isKinematic = true;
                    //player.GetComponent<PlayerController>().enabled = false;
                    player.GetComponent<PlayerController>().slipping = true;
                    player.GetComponent<Rigidbody>().velocity = player.GetComponent<Rigidbody>().velocity;
                }
            }
            else
            {
                //player.GetComponent<Rigidbody>().isKinematic = false;
                //player.GetComponent<PlayerController>().enabled = true;
                player.GetComponent<PlayerController>().slipping = false;
            }
        }

    }
    void FixedUpdate()
    {



        if (isClosing && uiState == "punish")
        {


            foreach (GameObject player in players)
            {



            }

            uiState = "endgame";
        }

        if (uiState == "gameplay")
        {
            
        }


        if (isCutscene)
        {
            


            openTimer -= Time.deltaTime;

            if (openTimer < 3 && uiState == "begin")
            {
                uiState = "text1";
                UIcanvas.setInstructions("Slide the ice to the goal!");

            }

            //else if (openTimer < 4 && uiState == "text1")
            //{
            //    uiState = "text2";
            //    UIcanvas.setInstructions("Be left alone and you'll freeze!");

            //}

            else if (openTimer < 0 && uiState == "text1")
            {
                isCutscene = false;
                foreach (GameObject player in players)
                {
                    //player.GetComponent<PlayerController>().lightning.enabled = false;
                    player.GetComponent<PlayerController>().enabled = true;

                }
                UIcanvas.startTimer(45f);
                uiState = "gameplay";
                sounds[0].Play();


            }
        }

        if (UIcanvas.uiTimer <= 0 && !isClosing && uiState == "gameplay")
        {
            isClosing = true;
            uiState = "punish";
            sounds[0].Stop();

        }

        if (isClosing)
        {
            closingTimer -= Time.deltaTime;
        }

        if (closingTimer < 0 && isClosing)
        {
            
            uiState = "nextLevel";
            isClosing = false;
            ScoreBehavior.levels.RemoveAt(0);
            if (ScoreBehavior.levels.Count == 0) { Initiate.Fade("End Level", Color.black, 2f); }
            else
            {
                string nextLevel = ScoreBehavior.levels[0];
                Initiate.Fade(nextLevel, Color.black, 2f);
            }
        }
    }
    private void openingScene()
    {
        foreach (GameObject player in players)
        {
            //player.GetComponent<PlayerController>().lightning.enabled = false;
            player.GetComponent<PlayerController>().enabled = false;

            //enemy.GetComponent<BlobController>().enabled = false;

        }

    }
}

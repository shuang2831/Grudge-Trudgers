using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HuddleLevelLogic : MonoBehaviour
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
            foreach (GameObject player in players)
            {

                if (!frozen[player.GetComponent<PlayerController>().playerNum - 1])
                {
                    
                    int numClose = 1;

                    for (int i = 0; i < 4; i++)
                    {
                        if (player.GetComponent<PlayerController>().isClose[i] && !frozen[i])
                        {
                            numClose = numClose + 1;
                        }
                    }

                    if (numClose < 2)
                    {
                        freezing[player.GetComponent<PlayerController>().playerNum - 1] = true;
                    }
                    else
                    {
                        freezing[player.GetComponent<PlayerController>().playerNum - 1] = false;

                    }

                    if (freezing[player.GetComponent<PlayerController>().playerNum - 1])
                    {
                        fTimer[player.GetComponent<PlayerController>().playerNum - 1] -= Time.deltaTime;
                        Color newColor = new Color(fTimer[player.GetComponent<PlayerController>().playerNum - 1] / 2f, fTimer[player.GetComponent<PlayerController>().playerNum - 1] / 2f, 1f);
                        player.GetComponentInChildren<SkinnedMeshRenderer>().material.SetColor("_Color", newColor);
                    }
                    else
                    {
                        fTimer[player.GetComponent<PlayerController>().playerNum - 1] = 3.0f;
                        player.GetComponentInChildren<SkinnedMeshRenderer>().material.SetColor("_Color", Color.white);
                    }

                    if (fTimer[player.GetComponent<PlayerController>().playerNum - 1] < 0)
                    {
                        player.GetComponent<PlayerController>().punishPlayer();
                        frozen[player.GetComponent<PlayerController>().playerNum - 1] = true;
                        player.transform.Find("Cube").gameObject.GetComponent<MeshRenderer>().enabled = true;
                        player.GetComponent<PlayerController>().enabled = false;
                        //player.GetComponent<PlayerController>().
                    }

                }

            }
        }


        if (isCutscene)
        {
            if (openTimer > 3)
            {
                foreach (GameObject player in players)
                {
                    player.transform.Translate(Vector3.forward * Time.deltaTime * 2.5f);

                }
            }


            openTimer -= Time.deltaTime;

            //if (openTimer < 8 && uiState == "begin")
            //{
            //    uiState = "text1";
            //    UIcanvas.setInstructions("It's a cold day outside, huddle for warmth!");

            //}

            if (openTimer < 3 && uiState == "begin")
            {
                uiState = "text2";
                UIcanvas.setInstructions("Huddle for warmth!");

            }

            else if (openTimer < 0 && uiState == "text2")
            {
                isCutscene = false;
                foreach (GameObject player in players)
                {
                    //player.GetComponent<PlayerController>().lightning.enabled = false;
                    player.GetComponent<PlayerController>().enabled = true;

                }
                UIcanvas.startTimer(30f);
                uiState = "gameplay";
                sounds[0].Play();


            }
        }

        if (UIcanvas.uiTimer <= 0 && !isClosing && uiState == "gameplay")
        {
            isClosing = true;
            uiState = "punish";

        }

        if (isClosing)
        {
            closingTimer -= Time.deltaTime;
        }

        if (closingTimer < 0 && isClosing)
        {
            uiState = "nextLevel";
            isClosing = false;
            sounds[0].Stop();
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

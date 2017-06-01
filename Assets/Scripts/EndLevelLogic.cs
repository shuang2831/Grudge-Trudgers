using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndLevelLogic : MonoBehaviour
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
    private int side;

    private int[] places;
    private int winner;
    private int maxScore;


    // Use this for initialization
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");

        UIcanvas = GameObject.FindGameObjectWithTag("UI").GetComponent<UIBehaviour>();

        sounds = GetComponents<AudioSource>();

        isCutscene = true;
        openTimer = 44f;
        closingTimer = 5f;
        openingScene();
        uiState = "begin";
        side = 0;
        maxScore = 0;
        

        places = new int[] { 0, 0, 0, 0 };


        for (int i = 0; i < 4; i++)
        {
            if (ScoreBehavior.PlayerScores[i] > maxScore)
            {
                maxScore = ScoreBehavior.PlayerScores[i];
                winner = i + 1;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {



    }
    void FixedUpdate()
    {

        if (isClosing && uiState == "punish")
        {




            uiState = "endgame";
        }

        if (uiState == "gameplay")
        {

        }


        if (isCutscene)
        {
            if (openTimer > 42)
            {
                foreach (GameObject player in players)
                {
                    player.transform.Translate(Vector3.forward * Time.deltaTime * 2.5f);

                }
            }


            openTimer -= Time.deltaTime;

            if (openTimer < 42 && uiState == "begin")
            {
                uiState = "text2";
                UIcanvas.setInstructions("Now for the results");

            }

            else if (openTimer < 38 && uiState == "text2")
            {
                uiState = "text3";
                UIcanvas.setInstructions("The winner is...");

            }

            else if (openTimer < 34 && uiState == "text3")
            {
                uiState = "text4";
                UIcanvas.setInstructions("Player " + winner + " with " + maxScore + " gold!");

            }

            else if (openTimer < 30 && uiState == "text4")
            {
                UIcanvas.showClosingResults(ScoreBehavior.PlayerScores[0], ScoreBehavior.PlayerScores[1], ScoreBehavior.PlayerScores[2], ScoreBehavior.PlayerScores[3]);
                isCutscene = false;
                foreach (GameObject player in players)
                {
                    //player.GetComponent<PlayerController>().lightning.enabled = false;
                    player.GetComponent<PlayerController>().enabled = true;

                }


                uiState = "gameplay";
                UIcanvas.startTimer(60f);
                sounds[0].Play();
            }
        }

        if (UIcanvas.uiTimer <= 0 && !isClosing)
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
            
            Initiate.Fade("Main Menu", Color.black, 2f);
            sounds[0].Play();
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

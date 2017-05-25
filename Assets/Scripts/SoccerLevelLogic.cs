using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure; //Custom Xinput Wrapper because Unity's sucks


public class SoccerLevelLogic : MonoBehaviour
{

    private GameObject[] players;
    public bool[] playerBehind;
    private int[] numNotNear;
    private bool isCutscene;
    private bool isClosing;
    private float openTimer;
    private float closingTimer;
    private string uiState;
    private UIBehaviour UIcanvas;

    private GameObject[] goals;
    private bool[] playerPunished;

    // Use this for initialization
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        goals = GameObject.FindGameObjectsWithTag("playerLight");

        UIcanvas = GameObject.FindGameObjectWithTag("UI").GetComponent<UIBehaviour>();

        playerPunished = new bool[] { false, false, false, false };

        isCutscene = true;
        openTimer = 2f;
        closingTimer = 10f;
        openingScene();
        uiState = "begin";
    }

    // Update is called once per frame
    void Update()
    {

        if (uiState == "gameplay")
        {

            foreach (GameObject goal in goals)
            {

                if (goal.GetComponent<SoccerGoalBehaviour>().scored && !playerPunished[goal.GetComponent<SoccerGoalBehaviour>().player.GetComponent<PlayerController>().playerNum - 1])
                {
                    goal.GetComponent<SoccerGoalBehaviour>().player.GetComponent<PlayerController>().lightning.enabled = true;
                    goal.GetComponent<SoccerGoalBehaviour>().player.GetComponent<PlayerController>().punishPlayerLight();
                    playerPunished[goal.GetComponent<SoccerGoalBehaviour>().player.GetComponent<PlayerController>().playerNum - 1] = true;
                }

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
            foreach (GameObject player in players)
            {







            }
        }


        if (isCutscene)
        {
            if (openTimer > 8)
            {
                foreach (GameObject player in players)
                {
                    player.transform.Translate(Vector3.forward * Time.deltaTime * 2.5f);

                }
            }


            openTimer -= Time.deltaTime;

            if (openTimer < 8 && uiState == "begin")
            {
                uiState = "text1";
                UIcanvas.setInstructions("Stab your peers' back to steal some gold.");

            }

            else if (openTimer < 4 && uiState == "text1")
            {
                uiState = "text2";
                UIcanvas.setInstructions("Press the button when the knife appears!");

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
            Initiate.Fade("PickSides Level", Color.black, 2f);
            isClosing = false;
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

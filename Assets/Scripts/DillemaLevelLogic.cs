using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using XInputDotNetPure; //Custom Xinput Wrapper because Unity's sucks

public class DillemaLevelLogic : MonoBehaviour
{
    private GameObject[] players;
    private PlayerController[] playerControllers;
    private bool isCutscene;
    private bool isClosing;
    private float openTimer;
    private float closingTimer;
    private string uiState;
    private UIBehaviour UIcanvas;
    private int side;

    private int[] scores = new int[] { 0, 0, 0, 0 };
    private bool[] chosen = new bool[] { false, false, false, false };


    // Use this for initialization
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        UIcanvas = GameObject.FindGameObjectWithTag("UI").GetComponent<UIBehaviour>();

        isCutscene = true;
        openTimer = 10f;
        closingTimer = 10f;
        openingScene();
        uiState = "begin";
        side = 0;

    }

    // Update is called once per frame
    void Update()
    {


        if (uiState == "gameplay")
        {
            foreach (GameObject player in players)
            {
                if (!chosen[player.GetComponent<PlayerController>().playerNum - 1])
                {
                    if (player.GetComponent<PlayerController>().prevState.Buttons.X == ButtonState.Released && player.GetComponent<PlayerController>().state.Buttons.X == ButtonState.Pressed)
                    {
                        scores[0] = scores[0] + 1;
                        chosen[player.GetComponent<PlayerController>().playerNum - 1] = true;
                    }
                    if (player.GetComponent<PlayerController>().prevState.Buttons.Y == ButtonState.Released && player.GetComponent<PlayerController>().state.Buttons.Y == ButtonState.Pressed)
                    {
                        scores[1] = scores[1] + 1;
                        chosen[player.GetComponent<PlayerController>().playerNum - 1] = true;
                    }
                    if (player.GetComponent<PlayerController>().prevState.Buttons.B == ButtonState.Released && player.GetComponent<PlayerController>().state.Buttons.B == ButtonState.Pressed)
                    {
                        scores[2] = scores[2] + 1;
                        chosen[player.GetComponent<PlayerController>().playerNum - 1] = true;
                    }
                    if (player.GetComponent<PlayerController>().prevState.Buttons.A == ButtonState.Released && player.GetComponent<PlayerController>().state.Buttons.A == ButtonState.Pressed)
                    {
                        scores[3] = scores[3] + 1;
                        chosen[player.GetComponent<PlayerController>().playerNum - 1] = true;
                    }
                }

            }
        }


    }
    void FixedUpdate()
    {


        if (isClosing && uiState == "punish")
        {
            int maxVotes = 0;

            for (int i = 0; i < 4; i++)
            {

                if (scores[i] > maxVotes)
                {
                    maxVotes = scores[i];
                }

            }

            foreach (GameObject player in players)
            {

                if (scores[player.GetComponent<PlayerController>().playerNum - 1] == maxVotes && maxVotes != 0)
                {
                    //player.GetComponent<PlayerController>().lightning.enabled = true;
                    player.GetComponent<PlayerController>().rewardPlayer(10);
                }

            }

            uiState = "endgame";
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
                UIcanvas.setInstructions("Dillema time! Pick either 'cooperate' or 'betray' your peer.");

            }

            else if (openTimer < 4 && uiState == "text1")
            {
                uiState = "text2";
                UIcanvas.setInstructions("If you all cooperate, noone is punished. If anyone betrays, only those that cooperated will be punished. But if you all betray, then you all will be punished. Choose wisely.");

            }

            else if (openTimer < 0 && uiState == "text2")
            {
                isCutscene = false;
                foreach (GameObject player in players)
                {
                    //player.GetComponent<PlayerController>().lightning.enabled = false;
                    player.GetComponent<PlayerController>().enabled = true;

                }

                UIcanvas.startTimer(15);
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

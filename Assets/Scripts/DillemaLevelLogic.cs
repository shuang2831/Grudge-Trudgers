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
    private AudioSource[] sounds;
    private bool isCutscene;
    private bool isClosing;
    private float openTimer;
    private float closingTimer;
    private string uiState;
    private UIBehaviour UIcanvas;


    private int[] choices = new int[] { 0, 0 };
    public bool[] chosen = new bool[] { false, false, false, false };
    private bool[] betray = new bool[] { false, false, false, false };


    // Use this for initialization
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        UIcanvas = GameObject.FindGameObjectWithTag("UI").GetComponent<UIBehaviour>();
        sounds = GetComponents<AudioSource>();
        isCutscene = true;
        openTimer = 10f;
        closingTimer = 5f;
        openingScene();
        uiState = "begin";


    }

    // Update is called once per frame
    void Update()
    {

        if (chosen[0] && chosen[1] && chosen[2] && chosen[3])
        {
            UIcanvas.uiTimer = -1;
        }

        if (uiState == "gameplay")
        {
            foreach (GameObject player in players)
            {
                if (!chosen[player.GetComponent<PlayerController>().playerNum - 1])
                {
                    if (player.GetComponent<PlayerController>().prevState.Buttons.X == ButtonState.Released && player.GetComponent<PlayerController>().state.Buttons.X == ButtonState.Pressed)
                    {
                        choices[0] = choices[0] + 1;
                        chosen[player.GetComponent<PlayerController>().playerNum - 1] = true;
                    }
                    
                    if (player.GetComponent<PlayerController>().prevState.Buttons.B == ButtonState.Released && player.GetComponent<PlayerController>().state.Buttons.B == ButtonState.Pressed)
                    {
                        choices[1] = choices[1] + 1;
                        chosen[player.GetComponent<PlayerController>().playerNum - 1] = true;
                        betray[player.GetComponent<PlayerController>().playerNum - 1] = true;
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

            if (choices[0] == 4)
            {

                foreach (GameObject player in players)
                {

                    
                    player.GetComponent<PlayerController>().rewardPlayer(5);
                    

                }
            }
            else if (choices[1] == 4)
            {

                foreach (GameObject player in players)
                {

                    player.GetComponent<PlayerController>().lightning.enabled = true;
                    player.GetComponent<PlayerController>().punishPlayerLight();


                }
            }
            else if (choices[1] == 0 && choices[0] == 0)
            {

                
            }
            else
            {
                foreach (GameObject player in players)
                {

                    if (!betray[player.GetComponent<PlayerController>().playerNum - 1])
                    {
                        player.GetComponent<PlayerController>().punishPlayerLight();
                    }
                    


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
                UIcanvas.setInstructions("Dillema time! All pick cooperate to get rewards.");

            }

            else if (openTimer < 4 && uiState == "text1")
            {
                uiState = "text2";
                UIcanvas.setInstructions("Betray, and anyone who didn't will be punished. All betray, and you're all punished.");

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeLevelLogic : MonoBehaviour {

    private GameObject[] players;
    private Light[] lights;
    private AudioSource[] sounds;
    private PlayerController[] playerControllers;
    private bool isCutscene;
    private bool isClosing;
    private float openTimer;
    private float closingTimer;
    private string uiState;
    private UIBehaviour UIcanvas;


    // Use this for initialization
    void Start () {
        players = GameObject.FindGameObjectsWithTag("Player");
        //lights = Light.FindGameObjectsWithTag("playerLight");
        UIcanvas = GameObject.FindGameObjectWithTag("UI").GetComponent<UIBehaviour>();
        sounds = GetComponents<AudioSource>();
        isCutscene = true;
        openTimer = 8f;
        closingTimer = 5f;
        openingScene();
        uiState = "begin";

        //foreach (GameObject player in players)
        //{
        //    player.GetComponent<PlayerController>().isClose = new bool[] { true, true, true, true };
            

        //}
    }
	
	// Update is called once per frame
	void Update () {
        if (GameObject.FindGameObjectWithTag("goal").GetComponent<GoalLogic>().finish)
        {
            UIcanvas.uiTimer = -1;
        }
    }

    void FixedUpdate()
    {
        foreach (GameObject player in players)
        {
            
            int numClose = 1;
            
            for (int i = 0; i < 4; i++)
            {
                if (player.GetComponent<PlayerController>().isClose[i])
                {
                    numClose = numClose + 1;
                }
            }

            player.GetComponentInChildren<Light>().range = numClose * 5;

        }

        if (isClosing && uiState == "punish")
        {


            foreach (GameObject player in players)
            {

                player.GetComponent<PlayerController>().enabled = false;

            }

            uiState = "endgame";
        }

        if (uiState == "gameplay")
        {
            
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
                UIcanvas.setInstructions("Get to the goal up top to get a reward.");

            }

            else if (openTimer < 4 && uiState == "text1")
            {
                uiState = "text2";
                UIcanvas.setInstructions("But remember, you all shine brighter when together.");

            }

            else if (openTimer < 0 && uiState == "text2")
            {
                isCutscene = false;
                foreach (GameObject player in players)
                {
                    //player.GetComponent<PlayerController>().lightning.enabled = false;
                    player.GetComponent<PlayerController>().enabled = true;

                }
                UIcanvas.startTimer(60f);
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
           // UIcanvas.uiTimer = 45;

            //enemy.GetComponent<BlobController>().enabled = false;

        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IceLevelLogic : MonoBehaviour
{
    private GameObject[] players;
    private PlayerController[] playerControllers;
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

        isCutscene = true;
        openTimer = 10f;
        closingTimer = 10f;
        openingScene();
        uiState = "begin";
        freezing = new bool[] { false, false, false, false };
        fTimer = new float[] { 3.0f, 3.0f, 3.0f, 3.0f };
        frozen = new bool[] { false, false, false, false };


    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject player in players)
        {
            if (player.transform.position.z > -10 && (player.transform.position.z < 10))
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
            if (openTimer > 8)
            {
                foreach (GameObject player in players)
                {
                    //player.transform.Translate(Vector3.forward * Time.deltaTime * 2.5f);

                }
            }


            openTimer -= Time.deltaTime;

            if (openTimer < 8 && uiState == "begin")
            {
                uiState = "text1";
                UIcanvas.setInstructions("It's a cold day outside, huddle for warmth!");

            }

            else if (openTimer < 4 && uiState == "text1")
            {
                uiState = "text2";
                UIcanvas.setInstructions("Be left alone and you'll freeze!");

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

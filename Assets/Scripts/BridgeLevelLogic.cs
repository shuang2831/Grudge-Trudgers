using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BridgeLevelLogic : MonoBehaviour
{

    private GameObject[] players;
    private PlayerController[] playerControllers;
    bool uDied = false;
    private bool isCutscene;
    private bool isClosing;
    private float openTimer;
    private float closingTimer;
    private string uiState;
    private UIBehaviour UIcanvas;
   
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

    }

    // Update is called once per frame
    void Update()
    {

        int total = 0;
        foreach (GameObject player in players)
        {
            if (player.transform.position.y < -4)
            {
                total++;
            }
        }

        if (total >= 4)
        {
            uDied = true;
            //endfunction something
        }
    }

    private void FixedUpdate()
    {

        if (isCutscene)
        {
            if (openTimer > 5)
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
                UIcanvas.setInstructions("Watch out for the blob! It will grow and grow while chasing the closest player. ");

            }

            else if (openTimer < 4 && uiState == "text1")
            {
                uiState = "text2";
                UIcanvas.setInstructions("But you can make him shrink from shyness if at least 2 of you stare at him! You have 30 seconds to survive.");

            }

            else if (openTimer < 0 && uiState == "text2")
            {
                isCutscene = false;
                foreach (GameObject player in players)
                {
                    //player.GetComponent<PlayerController>().lightning.enabled = false;
                    player.GetComponent<PlayerController>().enabled = true;

                }


            }
        }

       if (UIcanvas.uiTimer <= 0 && !isClosing)
        {
            isClosing = true;
        }
      
        if (isClosing)
        {
            closingTimer -= Time.deltaTime;
        }

        if (closingTimer < 0 && isClosing)
        {
            SceneManager.LoadScene("PickSides Level");
        }


    }


    private void OnGUI()
    {
        string endText = "You all died";
        if (uDied)
        {
            GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, Screen.width - 20, 30), endText);
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
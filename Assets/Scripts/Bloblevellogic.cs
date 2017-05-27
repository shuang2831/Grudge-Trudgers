using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Bloblevellogic : MonoBehaviour
{

    private GameObject[] players;
    private PlayerController[] playerControllers;
    private GameObject enemy;
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
        enemy = GameObject.FindGameObjectWithTag("enemy");
        UIcanvas = GameObject.FindGameObjectWithTag("UI").GetComponent<UIBehaviour>();
        
        isCutscene = true;
        openTimer = 5f;
        closingTimer = 5f;
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
            //    UIcanvas.setInstructions("Watch out for the blob! It will grow and grow while chasing the closest player. ");

            //}

            if (openTimer < 3 && uiState == "begin")
            {
                uiState = "text2";
                UIcanvas.setInstructions("Don't get eaten by the bashful blob!");

            }

            else if (openTimer < 0 && uiState == "text2")
            {
                isCutscene = false;
                foreach (GameObject player in players)
                {
                    //player.GetComponent<PlayerController>().lightning.enabled = false;
                    player.GetComponent<PlayerController>().enabled = true;

                }

                enemy.GetComponent<BlobController>().enabled = true;

                uiState = "gameplay";
                UIcanvas.startTimer(30f);

            }
        }

       if (UIcanvas.uiTimer <= 0 && !isClosing && uiState == "gameplay")
        {
            isClosing = true;
            enemy.GetComponent<BlobController>().die();
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
            enemy.GetComponent<BlobController>().enabled = false;
            enemy.GetComponent<Rigidbody>().velocity = Vector3.zero;
            //enemy.GetComponent<BlobController>().enabled = false;

        }

    }
}
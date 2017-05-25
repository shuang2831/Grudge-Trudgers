using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using XInputDotNetPure; //Custom Xinput Wrapper because Unity's sucks

public class ButtonLevelLogic : MonoBehaviour
{
    private GameObject[] players;
    private GameObject[] enemies;
   
    private bool isCutscene;
    private bool isClosing;
    private GameObject enemy;
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
        //enemy = GameObject.FindGameObjectWithTag("enemy");
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
            
        }


    }
    void FixedUpdate()
    {


        if (isClosing && uiState == "punish")
        {
           

            foreach (GameObject player in players)
            {

                if (player.transform.position.z < -5 || player.transform.position.z  > 5 || player.transform.position.x < -5 || player.transform.position.x > 5)
                {
                    //player.GetComponent<PlayerController>().lightning.enabled = true;
                    player.GetComponent<PlayerController>().punishPlayerLight();
                }

            }

            enemies = GameObject.FindGameObjectsWithTag("enemy");

            foreach (GameObject enemy in enemies)
            {

                enemy.GetComponent<LittleBlobController>().die();

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
                UIcanvas.setInstructions("Don't let this little guy push you off for a reward!");

            }

            else if (openTimer < 4 && uiState == "text1")
            {
                uiState = "text2";
                UIcanvas.setInstructions("I wonder what this button does...");

            }

            else if (openTimer < 0 && uiState == "text2")
            {
                isCutscene = false;
                foreach (GameObject player in players)
                {
                    //player.GetComponent<PlayerController>().lightning.enabled = false;
                    player.GetComponent<PlayerController>().enabled = true;

                }
                //enemy.GetComponent<BlobController>().enabled = true;
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

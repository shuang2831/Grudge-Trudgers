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
    private AudioSource[] sounds;
   
    private bool isCutscene;
    private bool isClosing;
    private GameObject enemy;
    private float openTimer;
    private float closingTimer;
    private string uiState;
    private UIBehaviour UIcanvas;
    private int side;

    private int i = 0;

    private int[] scores = new int[] { 0, 0, 0, 0 };
    private bool[] chosen = new bool[] { false, false, false, false };


    // Use this for initialization
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        //enemy = GameObject.FindGameObjectWithTag("enemy");
        UIcanvas = GameObject.FindGameObjectWithTag("UI").GetComponent<UIBehaviour>();

        sounds = GetComponents<AudioSource>();


        isCutscene = true;
        openTimer = 5f;
        closingTimer = 10f;
        openingScene();
        uiState = "begin";
        side = 0;

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
            //    UIcanvas.setInstructions("Don't let this little guy push you off for a reward!");

            //}

            if (openTimer < 3 && uiState == "begin")
            {
                uiState = "text2";
                UIcanvas.setInstructions("Just stay within the yellow lines...");

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
                UIcanvas.startTimer(30);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PickSidesLevelLogic : MonoBehaviour {
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
    private GameObject[] planes;

    // Use this for initialization
    void Start () {
        players = GameObject.FindGameObjectsWithTag("Player");
        planes = GameObject.FindGameObjectsWithTag("plane");
        sounds = GetComponents<AudioSource>();
        planes[0].GetComponent<MeshRenderer>().enabled = false;
        planes[1].GetComponent<MeshRenderer>().enabled = false;
        UIcanvas = GameObject.FindGameObjectWithTag("UI").GetComponent<UIBehaviour>();

        isCutscene = true;
        openTimer = 10f;
        closingTimer = 10f;
        openingScene();
        uiState = "begin";
        side = 0;
        
    }

    // Update is called once per frame
    void Update () {

       

        side = 0;

        foreach (GameObject player in players)
        {
            if (player.transform.position.x <= 0)
            {
                side = side - 1;
            }
            else
            {
                side = side + 1;
            }

        }

        if (side == 4 || side == -4)
        {
            side = 0;
        }

		
	}
    void FixedUpdate()
    {

        if (isClosing && uiState == "punish")
        {


            foreach (GameObject player in players)
            {

                if (side == 0)
                {
                    player.GetComponent<PlayerController>().lightning.enabled = true;
                    player.GetComponent<PlayerController>().punishPlayerLight();
                }
                else if (side < 0)
                {
                    if (player.transform.position.x > 0)
                    {

                        player.GetComponent<PlayerController>().lightning.enabled = true;
                        player.GetComponent<PlayerController>().punishPlayerLight();
                    }
                }
                else
                {
                    if (player.transform.position.x <= 0)
                    {
                        player.GetComponent<PlayerController>().lightning.enabled = true;
                        player.GetComponent<PlayerController>().punishPlayerLight();
                    }
                }

            }

            uiState = "endgame";
        }

        if (uiState == "gameplay")
        {
            if (side < 0)
            {
                planes[0].GetComponent<MeshRenderer>().enabled = false;
                planes[1].GetComponent<MeshRenderer>().enabled = true;
            }
            else if (side > 0)
            {
                planes[1].GetComponent<MeshRenderer>().enabled = false;
                planes[0].GetComponent<MeshRenderer>().enabled = true;
            }
            else if (side == 0 || side == 4)
            {
                planes[1].GetComponent<MeshRenderer>().enabled = true;
                planes[0].GetComponent<MeshRenderer>().enabled = true;
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
                UIcanvas.setInstructions("3 to 1 divided by the yellow line, only the 1 is punished.  ");

            }

            else if (openTimer < 4 && uiState == "text1")
            {
                uiState = "text2";
                UIcanvas.setInstructions("Anything else, and you're all punished.");

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

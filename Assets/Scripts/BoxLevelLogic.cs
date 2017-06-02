using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure; //Custom Xinput Wrapper because Unity's sucks


public class BoxLevelLogic : MonoBehaviour
{

    private GameObject[] players;
    private AudioSource[] sounds;
    public bool[] playerBehind;
    private int[] numNotNear;
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
        sounds = GetComponents<AudioSource>();
        UIcanvas = GameObject.FindGameObjectWithTag("UI").GetComponent<UIBehaviour>();

        isCutscene = true;
        openTimer = 4f;
        closingTimer = 5f;
        openingScene();
        uiState = "begin";
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("goal").GetComponent<GoalLogic>().finish)
        {
            UIcanvas.uiTimer = -1;
        }

        if (uiState == "gameplay")
        {

            foreach (GameObject player in players)
            {

                RaycastHit hit;

                if (Physics.Raycast(player.transform.position, (player.transform.forward), out hit, 2f))
                {
                    //Debug.Log(hit.collider.transform.parent.gameObject.tag);
                    //Debug.Log(player.transform.forward);
                    if (hit.collider.transform.parent != null)
                    {
                        Debug.Log(hit.collider.transform.parent.gameObject);

                        if (hit.collider.transform.parent.gameObject.tag == "box")
                        {
                           
                            if (player.GetComponent<PlayerController>().prevState.Buttons.A == ButtonState.Released && player.GetComponent<PlayerController>().state.Buttons.A == ButtonState.Pressed)
                            {
                                if (Vector3.Dot(player.transform.forward, Vector3.right) > 0.3f)
                                {
                                    Debug.Log("right");
                                    hit.collider.transform.parent.gameObject.GetComponent<BoxBehaviour>().pushBox("right");
                                }
                                if (Vector3.Dot(player.transform.forward, Vector3.left) > 0.3f)
                                {
                                    Debug.Log("left");
                                    hit.collider.transform.parent.gameObject.GetComponent<BoxBehaviour>().pushBox("left");
                                }
                                if (Vector3.Dot(player.transform.forward, Vector3.forward) > 0.3f)
                                {
                                    Debug.Log("up");
                                    hit.collider.transform.parent.gameObject.GetComponent<BoxBehaviour>().pushBox("up");
                                }
                                if (Vector3.Dot(player.transform.forward, Vector3.back) > 0.3f)
                                {
                                    Debug.Log("down");
                                    hit.collider.transform.parent.gameObject.GetComponent<BoxBehaviour>().pushBox("down");
                                }

                            }
                        }
                    }
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
            //if (openTimer > 3)
            //{
            //    foreach (GameObject player in players)
            //    {
            //        player.transform.Translate(Vector3.forward * Time.deltaTime * 2.5f);

            //    }
            //}


            openTimer -= Time.deltaTime;

            //if (openTimer < 8 && uiState == "begin")
            //{
            //    uiState = "text1";
            //    UIcanvas.setInstructions("Stab your peers' back to steal some gold.");

            //}

            if (openTimer < 3 && uiState == "begin")
            {
                uiState = "text2";
                UIcanvas.setInstructions("Reach the yellow goal up north!");

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

            //enemy.GetComponent<BlobController>().enabled = false;

        }

    }
}

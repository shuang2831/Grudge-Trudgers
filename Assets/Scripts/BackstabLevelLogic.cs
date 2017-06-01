using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure; //Custom Xinput Wrapper because Unity's sucks


public class BackstabLevelLogic : MonoBehaviour
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

    private AudioClip stab;
    private AudioClip footsteps;

    // Use this for initialization
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        sounds = GetComponents<AudioSource>();
        playerBehind = new bool[] { false, false, false, false };
        numNotNear = new int[] { 3, 3, 3, 3 };

        UIcanvas = GameObject.FindGameObjectWithTag("UI").GetComponent<UIBehaviour>();

        isCutscene = true;
        openTimer = 5f;
        closingTimer = 5f;
        uiState = "begin";

        stab = (AudioClip)Resources.Load("Sound_Effects/stab");
        footsteps = (AudioClip)Resources.Load("Sound_Effects/foot2");

        openingScene();
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



            }

            uiState = "endgame";
        }

        if (uiState == "gameplay")
        {
            foreach (GameObject player in players)
            {

                RaycastHit hit;

                if (Physics.Raycast(player.transform.position, (player.transform.forward * -1), out hit, 1))
                {
                    if (hit.transform.gameObject.tag == "Player" && playerBehind[player.GetComponent<PlayerController>().playerNum - 1] && player.GetComponent<PlayerController>().isActive)
                    {
                        if (hit.transform.gameObject.GetComponent<PlayerController>().prevState.Buttons.A == ButtonState.Released && hit.transform.gameObject.GetComponent<PlayerController>().state.Buttons.A == ButtonState.Pressed)
                        {
                           
                            sounds[1].PlayOneShot(stab);
                            hit.transform.gameObject.GetComponent<PlayerController>().isActive = false;
                            player.GetComponent<PlayerController>().punishPlayer();
                            player.GetComponent<PlayerController>().enabled = false;
                            player.GetComponent<Rigidbody>().isKinematic = true;
                            player.transform.Translate(0, -95, 0);
                            player.transform.GetChild(0).transform.localPosition = new Vector3(0, 100, 0);
                            player.transform.GetChild(0).GetComponent<Rigidbody>().useGravity = true;


                        }
                    }
                }

            }
        }

        if (uiState == "gameplay")
        {
            foreach (GameObject player in players)
            {
                
                RaycastHit hit;

                if (Physics.Raycast(player.transform.position, (player.transform.forward * -1), out hit, 1))
                {
                    if (hit.transform.gameObject.tag == "Player" && Vector3.Dot(hit.transform.forward, player.transform.forward) > 0.5)
                    {
                        playerBehind[player.GetComponent<PlayerController>().playerNum - 1] = true;
                    }
                }
                else
                {
                    playerBehind[player.GetComponent<PlayerController>().playerNum - 1] = false;
                }




            }
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
            //    UIcanvas.setInstructions("Stab your peers' back to steal some gold.");

            //}

            if (openTimer < 3 && uiState == "begin")
            {
                uiState = "text2";
                UIcanvas.setInstructions("Watch your back...");
                sounds[1].Stop();

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
            Debug.Log(footsteps);
            sounds[1].PlayOneShot(footsteps);

            //enemy.GetComponent<BlobController>().enabled = false;

        }

    }
}

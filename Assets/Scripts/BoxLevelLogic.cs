using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure; //Custom Xinput Wrapper because Unity's sucks


public class BoxLevelLogic : MonoBehaviour
{

    private GameObject[] players;
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

        UIcanvas = GameObject.FindGameObjectWithTag("UI").GetComponent<UIBehaviour>();

        isCutscene = true;
        openTimer = 2f;
        closingTimer = 10f;
        openingScene();
        uiState = "begin";
    }

    // Update is called once per frame
    void Update()
    {

        if (uiState == "gameplay")
        {

            foreach (GameObject player in players)
            {

                RaycastHit hit;

                if (Physics.Raycast(player.transform.position, (player.transform.forward), out hit, 2))
                {
                    //Debug.Log(hit.collider.transform.parent.gameObject.tag);
                    //Debug.Log(player.transform.forward);
                    if (hit.collider.transform.parent != null)
                    {
                        Debug.Log(hit.collider.transform.parent.gameObject);

                        if (hit.collider.transform.parent.gameObject.tag == "box")
                        {
                            Debug.Log("seen");
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
                UIcanvas.setInstructions("Stab your peers' back to steal some gold.");

            }

            else if (openTimer < 4 && uiState == "text1")
            {
                uiState = "text2";
                UIcanvas.setInstructions("Press the button when the knife appears!");

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

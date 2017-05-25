using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PaintLevelLogic : MonoBehaviour
{
    private GameObject[] players;
    private PlayerController[] playerControllers;
    private bool isCutscene;
    private bool isClosing;
    private float openTimer;
    private float closingTimer;
    private string uiState;
    private UIBehaviour UIcanvas;
    private int side;
    private GameObject[] planes;

    public Terrain terrain;
    private TerrainData terr;

    private Color32[] colors1;
    private Color32[] colors2;
    private Color32[] colors3;
    private Color32[] colors4;
    private Color32[][] colors;
    private Color32[] defaultColors;
    private int[] scores;



    // Use this for initialization
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        planes = GameObject.FindGameObjectsWithTag("plane");
        scores = new int[] { 0, 0, 0, 0 };

        UIcanvas = GameObject.FindGameObjectWithTag("UI").GetComponent<UIBehaviour>();

        isCutscene = true;
        openTimer = 10f;
        closingTimer = 10f;
        openingScene();
        uiState = "begin";
        side = 0;

        terr = terrain.terrainData;
     
        colors1 = new Color32[100];
        for (int i = 0; i < 100; i++)
        {
            colors1[i] = Color.blue;
        }
        colors2 = new Color32[100];
        for (int i = 0; i < 100; i++)
        {
            colors2[i] = Color.red;
        }
        colors3 = new Color32[100];
        for (int i = 0; i < 100; i++)
        {
            colors3[i] = Color.green;
        }
        colors4 = new Color32[100];
        for (int i = 0; i < 100; i++)
        {
            colors4[i] = Color.cyan;
        }

        colors = new Color32[][] { colors1, colors2, colors3, colors4 };

        defaultColors = new Color32[256 * 256];
        for (int i = 0; i < (256 * 256); i++)
        {
            defaultColors[i] = Color.white;
        }
        terr.splatPrototypes[0].texture.SetPixels32(defaultColors);

    }

    private void OnDestroy()
    {
        terr.splatPrototypes[0].texture.SetPixels32(defaultColors);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void FixedUpdate()
    {

        if (isClosing && uiState == "punish")
        {

            Color32[] texColors = terr.splatPrototypes[0].texture.GetPixels32();

            foreach (Color32 col in texColors)
            {
                if (col == Color.blue)
                {
                    scores[0] = scores[0] + 1;
                }
                if (col == Color.red)
                {
                    scores[1] = scores[1] + 1;
                }
                if (col == Color.green)
                {
                    scores[2] = scores[2] + 1;
                }
                if (col == Color.cyan)
                {
                    scores[3] = scores[3] + 1;
                }
            }
            int min = int.MaxValue;
            int iter = -1;
            for (int i = 0; i < 4; i++)
            {
                if (scores[i] < min)
                {
                    min = scores[i];
                    iter = i;
                }
            }

            foreach (GameObject player in players)
            {

                if (iter == player.GetComponent<PlayerController>().playerNum - 1)
                {
                    player.GetComponent<PlayerController>().lightning.enabled = true;
                    player.GetComponent<PlayerController>().punishPlayerLight();
                }

            }

            uiState = "endgame";
        }


        foreach (GameObject player in players)
        {
            int x = (int)((player.transform.position.x * 10) - 5);
            int y = (int)((player.transform.position.z * 10) - 5);

            terr.splatPrototypes[0].texture.SetPixels32(x, y, 10, 10, colors[player.GetComponent<PlayerController>().playerNum - 1]);
          
        }

        terr.splatPrototypes[0].texture.Apply();


        
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
                UIcanvas.setInstructions("It's time to paint the ground with your color!");

            }

            else if (openTimer < 4 && uiState == "text1")
            {
                uiState = "text2";
                UIcanvas.setInstructions("Paint the least, and you'll be punished. You have 30 seconds.");

            }

            else if (openTimer < 0 && uiState == "text2")
            {
                isCutscene = false;
                foreach (GameObject player in players)
                {
                    
                    player.GetComponent<PlayerController>().enabled = true;

                }
                UIcanvas.startTimer(30);
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
            player.GetComponent<PlayerController>().enabled = false;

        }

    }

    
}

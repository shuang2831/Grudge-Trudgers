using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainPaintLogic : MonoBehaviour {

    public Terrain terrain;
    private TerrainData terr;
    private GameObject[] players;
    private Color32[] colors1;
    private Color32[] colors2;
    private Color32[] colors3;
    private Color32[] colors4;
    private Color32[][] colors;
    private Color32[] defaultColors;
    private int factor;

    // Use this for initialization
    void Start () {
        terr = terrain.terrainData;
        players = GameObject.FindGameObjectsWithTag("Player");
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

        defaultColors = new Color32[256*256];
        for (int i = 0; i < (256 * 256); i++)
        {
            defaultColors[i] = Color.white;
        }
        terr.splatPrototypes[0].texture.SetPixels32(defaultColors);

    }
	
	// Update is called once per frame
	void Update () {
        //paint();
	}

    private void OnDestroy()
    {
        terr.splatPrototypes[0].texture.SetPixels32(defaultColors);
    }

    private void FixedUpdate()
    {
        int playerNum = 0;
        foreach (GameObject player in players)
        {
            int x = (int) ((player.transform.position.x * 10) - 5);
            int y = (int) ((player.transform.position.z * 10) - 5);

            terr.splatPrototypes[0].texture.SetPixels32(x, y, 10, 10, colors[playerNum]);
            playerNum = playerNum + 1;
        }

        terr.splatPrototypes[0].texture.Apply();
    }

    private void paint()
    {
        SplatPrototype[] sp = new SplatPrototype[4];

        // create the splat types here
        sp[0] = new SplatPrototype();
        sp[0].texture = Resources.Load("grassTex2") as Texture2D;

        sp[1] = new SplatPrototype();
        sp[1].texture = Resources.Load("grassTex2") as Texture2D;

        sp[2] = new SplatPrototype();
        sp[2].texture = Resources.Load("grassTex2") as Texture2D;

        sp[3] = new SplatPrototype();
        sp[3].texture = Resources.Load("grassTex2") as Texture2D;

        terr.splatPrototypes = sp;
        float[,,] alphamaps = new float[0, 0, sp.Length];

        // set the actual textures in each tile here.
        // first two indexes are coordinates, the last is the alpha blend of this particular layer.

        terr.SetAlphamaps(0, 0, alphamaps);
    }
}

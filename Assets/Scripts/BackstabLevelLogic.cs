using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackstabLevelLogic : MonoBehaviour {

    private GameObject[] players;
    public bool[] playerBehind;
    private int[] numNotNear;

    // Use this for initialization
    void Start() {
        players = GameObject.FindGameObjectsWithTag("Player");
        playerBehind = new bool[] { false, false, false, false };
        numNotNear = new int[] { 3, 3, 3, 3 };
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {
        foreach (GameObject player in players)
        {
            Debug.Log(player.GetComponent<PlayerController>().isClose);
            int numClose = 0;

            for (int i = 0; i < 4; i++)
            {
                if (player.GetComponent<PlayerController>().isClose[i])
                {
                    numClose = numClose + 1;
                }

            }

            if (numClose > 0)
            {
                playerBehind[player.GetComponent<PlayerController>().playerNum - 1] = true;
            }
            else
            {
                playerBehind[player.GetComponent<PlayerController>().playerNum - 1] = false;
            }

            

        }
    }
}

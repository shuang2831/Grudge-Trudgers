using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure; //Custom Xinput Wrapper because Unity's sucks

public class StartBehaviour : MonoBehaviour {

    private GameObject[] players;
    // Use this for initialization
    void Start () {
        players = GameObject.FindGameObjectsWithTag("Player");
    }
	
	// Update is called once per frame
	void Update () {
        foreach (GameObject player in players)
        {
            if (player.GetComponent<PlayerController>().prevState.Buttons.Start == ButtonState.Released && player.GetComponent<PlayerController>().state.Buttons.Start == ButtonState.Pressed)
            {
                Initiate.Fade("Opening Scene", Color.black, 2f);
            }

        }
    }
}

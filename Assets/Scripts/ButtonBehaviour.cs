using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure; //Custom Xinput Wrapper because Unity's sucks


public class ButtonBehaviour : MonoBehaviour {

    private GameObject[] players;
    private bool nearby;
    public GameObject enemy;                // The enemy prefab to be spawned.

    // Use this for initialization
    void Start () {
        nearby = false;
        players = GameObject.FindGameObjectsWithTag("Player");
    }
	
	// Update is called once per frame
	void Update () {

        foreach (GameObject player in players)
        {

                if (nearby && player.GetComponent<PlayerController>().isActive)
                {
                    if (player.GetComponent<PlayerController>().prevState.Buttons.A == ButtonState.Released && player.gameObject.GetComponent<PlayerController>().state.Buttons.A == ButtonState.Pressed)
                    {
                        Vector3 p = new Vector3(Random.Range(-10.0f, 10.0f), 0, Random.Range(-10.0f, 10.0f));
                        Instantiate (enemy, p, Quaternion.identity);
                    }
                }
            
    
        }

    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            nearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            nearby = false;
        }
    }
}

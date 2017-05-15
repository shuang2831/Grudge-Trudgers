using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalLogic : MonoBehaviour {

    bool[] goalReached;

	// Use this for initialization
	void Start () {

        goalReached = new bool[] { false, false, false, false };
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if(!goalReached[other.GetComponent<PlayerController>().playerNum - 1])
            {
                goalReached[other.GetComponent<PlayerController>().playerNum - 1] = true;
                other.GetComponent<PlayerController>().rewardPlayer(10);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalLogic : MonoBehaviour {

    bool[] goalReached;
    public bool finish;

	// Use this for initialization
	void Start () {
        finish = false;
        goalReached = new bool[] { false, false, false, false };
	}
	
	// Update is called once per frame
	void Update () {
		if (goalReached[0] && goalReached[1] && goalReached[2] && goalReached[3])
        {
            finish = true;
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if(!goalReached[other.GetComponent<PlayerController>().playerNum - 1])
            {
                goalReached[other.GetComponent<PlayerController>().playerNum - 1] = true;
                other.GetComponent<PlayerController>().rewardPlayer(10);
                GetComponent<AudioSource>().PlayOneShot((AudioClip)Resources.Load("Sound_Effects/win"));
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoccerGoalBehaviour : MonoBehaviour {


    public bool scored;
    public GameObject player;
	// Use this for initialization
	void Start () {
        scored = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "box")
        {
            scored = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBehaviour : MonoBehaviour {

    public GameObject[] players;
    public GameObject[] enemies;
    private float dist;
    private float coinSpeed;
    private bool isColliding;

    // Use this for initialization
    void Start () {
        players = GameObject.FindGameObjectsWithTag("Player");
        dist = 2.0f;
        coinSpeed = 10.0f;
        enemies = GameObject.FindGameObjectsWithTag("enemy");
        foreach (GameObject player in players)
        {
            Physics.IgnoreCollision(player.GetComponent<CapsuleCollider>(), GetComponent<BoxCollider>());
        }
        foreach (GameObject enemy in enemies)
        {
            Physics.IgnoreCollision(enemy.GetComponent<CapsuleCollider>(), GetComponent<BoxCollider>());
            Physics.IgnoreCollision(enemy.GetComponent<BoxCollider>(), GetComponent<BoxCollider>());
        }
        isColliding = false;

        
    }
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0, 100 * Time.deltaTime, 0);
        foreach (GameObject player in players)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < 2 && player.GetComponent<PlayerController>().isActive)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * 7);
            }
        }
        isColliding = false;
    }

    void OnTriggerEnter(Collider player)
    {
        if (isColliding || player.isTrigger)
        {
            return;
        }
        isColliding = true;
        if(player.gameObject.tag == "Player" && player.GetComponent<PlayerController>().isActive && !player.GetComponent<Rigidbody>().isKinematic)
        {
            GetComponent<AudioSource>().Play();
            ScoreBehavior.PlayerScores[player.gameObject.GetComponent<PlayerController>().playerNum - 1] = ScoreBehavior.PlayerScores[player.gameObject.GetComponent<PlayerController>().playerNum - 1] + 1;
            Debug.Log(ScoreBehavior.PlayerScores[player.gameObject.GetComponent<PlayerController>().playerNum - 1]);
            Destroy(gameObject, 0.2f);

        }
    }

    
}

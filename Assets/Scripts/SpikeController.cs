using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeController : MonoBehaviour {
    private GameObject[] players;
    private GameObject[] spikes;
    //private PlayerController[] playerControllers;
    private Rigidbody rb;
    int state = 0;
    private float delay;
    bool isActive;
    float cycleTime = 0f;
    float delayTime = 4f;
    float restTime = 6f;
    float spikeTime = 1f;
    private float state0 = -4.05f;
    private float state1 = -2.8f;
    private float state2 = -1.35f;
    // Use this for initialization
    Coroutine coCycle;
    void Start () {
        players = GameObject.FindGameObjectsWithTag("Player");
        rb = GetComponent<Rigidbody>();
        isActive = false;
        delay = Random.Range(1, 12);
    }
	
	// Update is called once per frame
	void Update () {
        cycleTime = cycleTime + Time.deltaTime;
        if (cycleTime >= delay && !isActive)
        {
            startCycle();
            isActive = true;
        }
    }

    private void FixedUpdate()
    {
        if(state == 0)
        {
            if(transform.position.y <= state0)
            {
                transform.Translate(new Vector3(0, 0, 0));
                transform.position = new Vector3(transform.position.x, state0, transform.position.z);

            }
            else
            {
                transform.Translate(new Vector3(0, 0, -0.18f));
            }
        }
        else if (state == 1)
        {
            if(transform.position.y >= state1)
            {
                transform.Translate(new Vector3(0, 0, 0));
                transform.position = new Vector3(transform.position.x, state1, transform.position.z);

            }
            else
            {
                transform.Translate(new Vector3(0, 0, 0.05f));

            }
        }
        else if (state == 2)
        {
            
            if (transform.position.y >= state2)
            {
                transform.Translate(new Vector3(0, 0, 0));
                transform.position = new Vector3(transform.position.x, state2, transform.position.z);

            }
            else
            {
                transform.Translate(new Vector3(0, 0, 0.09f));
            }

        }


    }


    public void startCycle()
    {
        coCycle = StartCoroutine(cycle(restTime, delayTime, spikeTime));
    }

    IEnumerator cycle(float time, float time1, float time2)
    {
        int index = 0;
        while (true)
        {
            state = index % 3;
            if (state == 0)
            {
                //transform.position = new Vector3(transform.position.x, -2.7f, transform.position.z);
                yield return new WaitForSeconds(time);

            }
            else if (state == 1)
            {
                //transform.position = new Vector3(transform.position.x, -2.0f, transform.position.z);
                yield return new WaitForSeconds(time1);

            }
            else if (state == 2)
            {
                //transform.position = new Vector3(transform.position.x, -0.5f, transform.position.z);
                yield return new WaitForSeconds(time2);

            }
            index++;

        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().punishPlayer();
            collision.gameObject.transform.Translate(0, -95, 0);
            collision.gameObject.GetComponent<PlayerController>().enabled = false;
            collision.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            collision.gameObject.transform.GetChild(0).Translate(0, -95, 0);
            collision.gameObject.transform.GetChild(0).GetComponent<Rigidbody>().useGravity = true;
        }
    }
}

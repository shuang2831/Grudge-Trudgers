using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeController : MonoBehaviour {
    private GameObject[] players;
    private GameObject[] spikes;
    //private PlayerController[] playerControllers;
    private Rigidbody rb;
    public Transform[] ts = GameObject.GetComponentsInChildren<Transform>();
    bool active = false;
    public List<GameObject> children;
    int state = 0;
    float cycleTime;
    float delayTime;
    // Use this for initialization
    Coroutine coCycle;
    void Start () {
        ts = GetComponentInChildren<Transform>();
        foreach(Transform child in transform)
        {
            if(child.tag == "Spike")
            {
                children.Add(child.gameObject);
            }
        }
        active = false;

        startCycle();
    }
	
	// Update is called once per frame
	void Update () {
		if(state == 0)
        {

        }
	}

    void spikeCycle(float IntervalTime)
    {

    }

    public void startCycle()
    {
        coCycle = StartCoroutine(cycle(3f));
    }

    IEnumerator cycle(float time)
    {
        int index = 0;
        while (true)
        {
            state = index % 3;
            index++;
            yield return new WaitForSeconds(time);
        }
    }
}

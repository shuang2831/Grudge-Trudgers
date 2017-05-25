﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindController : MonoBehaviour {

    private GameObject[] players;
    private Transform[] targets;

    private float TimeSinceStart = 0;
    private float TimeToCycle = 5f;
    //private Rigidbody rb;


    private ParticleSystem ps;

    Coroutine coDelay, coStart;
   
    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        Debug.Log(string.Format("Started"));
        ps.Stop();
        //Physics.IgnoreCollision(GetComponent<CapsuleCollider>(), GameObject.FindGameObjectWithTag("Player").GetComponent<CapsuleCollider>());
        
    }
    // Use this for initialization
    void Start () {
      
        players = GameObject.FindGameObjectsWithTag("Player");

        targets = new Transform[players.Length];

        for (int i = 0; i < targets.Length; ++i)
        {
            targets[i] = players[i].transform;
        }

        //rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        TimeSinceStart = TimeSinceStart + Time.deltaTime;
        if(TimeSinceStart > TimeToCycle)
        {
            if(ps.isPlaying)
            {
                ps.Stop();
            }
            else
            {
                ps.Play();
            }
            TimeSinceStart = 0;
        }
	}

    private void FixedUpdate()
    {
    }

    private void startRandDelay()
    {
        coStart = StartCoroutine(RandDelay());
    }

    IEnumerator RandDelay()
    {
        Debug.Log(string.Format("Random Delay Started"));
        yield return new WaitForSeconds(Random.Range(10, 17));

    }

    public void startDelay()
    {
        coDelay = StartCoroutine(delayCycle(ps));
    }

    IEnumerator delayCycle(ParticleSystem ps)
    {
        if(ps.isPlaying)
        {
            ps.Stop();
            //Debug.Log(string.Format( "Cannon Stopped"));
            Debug.Log(string.Format("IsPlaying?: {0}", ps.isPlaying));
        }
        else
        {
            ps.Play();
            //Debug.Log(string.Format("Cannon Started"));
            Debug.Log(string.Format("IsPlaying?: {0}", ps.isPlaying));
            
        }
        yield return new WaitForSecondsRealtime(20f);
    }

    private void OnTriggerStay(Collider collision)
    {
        if(collision.gameObject.tag == "Player" && ps.isPlaying)
        {
           
            collision.gameObject.transform.Translate(-Vector3.forward * Time.deltaTime * 5.0f,Space.World);
            //Debug.Log(-Vector3.forward);
           
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindController : MonoBehaviour {

    private GameObject[] players;
    private Transform[] targets;
    private bool isActive;
    private float TimeSinceStart = 0;
    public float TimeToCycle;
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
        isActive = false;

        for (int i = 0; i < targets.Length; ++i)
        {
            targets[i] = players[i].transform;
        }


        //rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        TimeSinceStart = TimeSinceStart + Time.deltaTime;
        if(TimeSinceStart >= TimeToCycle && !isActive)
        {
            isActive = true;
            startDelay();
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

        while(true)
        {
            if (ps.isPlaying)
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
            yield return new WaitForSecondsRealtime(3.5f);
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if(collision.gameObject.tag == "Player" && ps.isPlaying)
        {
           
            collision.gameObject.transform.Translate(-Vector3.forward * Time.deltaTime * 9.0f,Space.World);
            //Debug.Log(-Vector3.forward);
           
        }
    }
}

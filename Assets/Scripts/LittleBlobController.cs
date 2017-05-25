using UnityEngine;
using System.Collections;

public class LittleBlobController : MonoBehaviour
{

    public float Distance;
    private GameObject[] players;
    private Transform[] targets;
    private float lookAtDistance;
    private float chaseRange = 10.0f;
    private float moveSpeed;
    private int rotationSpeed = 5;
    private Rigidbody rb;
    //Animator anim;
    public bool isMoving;
    public bool isKnockback;

    public Texture[] textures;

    // private EnemyHealth enemyHealth;

    Vector3 dir;
    float timeLimit = 2.2f; // 10 seconds.

    public new SkinnedMeshRenderer renderer;
    private Color[] colors = { Color.white, new Color(1.0f, 0.4f, 0.7f, 1.0f) };
    public Color chosenColor;
    public int chosenIdx; // green, yellow, blue, white, pink
    private Color[] blobColors = { new Color(0.53f, 1.0f, 0.3f, 1.0f), new Color(1.0f, 1.0f, 0.5f, 1.0f), new Color(0.3f, 0.79f, 1.0f, 1.0f), Color.white };
    Coroutine coFlash;
    float savedTime;
    float timeLeft;
    private ParticleSystem ps;

    //public Material mat;

    private Transform currentTarget;

    void Awake()
    {
        renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        ps = GetComponent<ParticleSystem>();
        chosenIdx = Random.Range(0, blobColors.Length);
        chosenColor = blobColors[chosenIdx];
        renderer.material.color = chosenColor;
        colors[0] = chosenColor;
        Physics.IgnoreCollision(GetComponent<CapsuleCollider>(), GameObject.FindGameObjectWithTag("ground").GetComponent<BoxCollider>());

        switch (chosenIdx)
        {

            default:
                moveSpeed = 3.0f;
                lookAtDistance = 10.0f;
                break;
        }
        
    }

    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");

        targets = new Transform[players.Length];

        for (int i = 0; i < targets.Length; ++i)
        {
            targets[i] = players[i].transform;
        }

        rb = GetComponent<Rigidbody>();
        //anim = GetComponent<Animator>();
        rb.freezeRotation = true;
        savedTime = Time.time;
        timeLeft = 3.0f;


        transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);

        //target = GameObject.Find("Player").transform;
    }

    void Update()
    {
        if (transform.position.y < -10)
        {
            Destroy(gameObject, 5.0f);
        }
        yellowLogic();




    }



    // Turn to face the player.
    void lookAt()
    {
        // Rotate to look at player.
        Quaternion rotation = Quaternion.LookRotation(currentTarget.position - transform.position);
        rotation.z = 0;
        rotation.x = 0;

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        //transform.LookAt(Target); alternate way to track player replaces both lines above.
    }


    void wander()
    {

        if (Time.time - savedTime <= 2.2 && transform.position.y < 30)
        {
            //transform.rotation = Random.rotation;
            transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
        }
        else if (Time.time - savedTime >= 2.2 && Time.time - savedTime <= 5)
        {

        }
        else if (Time.time - savedTime > 5)
        {
            Vector3 randomLook = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            transform.rotation = Quaternion.LookRotation(randomLook.normalized, Vector3.up);
            savedTime = Time.time;
        }
    }


    private void yellowLogic()
    {
        float min = float.PositiveInfinity;
        int numFacing = 0;

        foreach (Transform target in targets)
        {
            if (Vector3.Distance(target.position, transform.position) < min && target.position.y > -5)
            {
                currentTarget = target;
                min = Vector3.Distance(target.position, transform.position);

            }

            float angle = 30;
            if (Vector3.Angle(target.transform.forward, transform.position - target.position) < angle && (transform.position - target.position).magnitude < 15f)
            {
                numFacing++;
            }

        }



        // AI begins tracking player.
        if (Distance < lookAtDistance)
        {
            lookAt();

        }


        // Attack! Chase the player until/if player leaves attack range.
        if (Distance < chaseRange && transform.position.y < 30)
        {


            Vector3 temp = transform.forward;
            temp.y = 0;
            transform.position += temp * moveSpeed * Time.deltaTime;
            //rb.velocity = temp * moveSpeed;



        }



        else
        {
            wander();
            //chargeSpeed = moveSpeed;
            // rb.AddForce(Vector3.zero, ForceMode.VelocityChange);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetComponent<PlayerController>().isActive)
            {

                collision.gameObject.GetComponent<Rigidbody>().AddForce((collision.gameObject.transform.forward * -1) * 100f);
            }
        }
    }

    public void die()
    {

        ps.Emit(5);
        transform.position = new Vector3(0, -10, 0);
        Destroy(gameObject, 3f);

    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxBehaviour : MonoBehaviour {

    private Rigidbody rb;
    Vector3 dirVector;
    bool moving;
    float timer;

    bool boxRight;
    bool boxLeft;
    bool boxUp;
    bool boxDown;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        moving = false;
        timer = 0;
        Physics.IgnoreCollision(GameObject.FindGameObjectWithTag("ground").GetComponent<BoxCollider>(), GetComponent<BoxCollider>());
        //pushBox("right");
        boxRight = false;
        boxLeft = false;
        boxUp = false;
        boxDown = false;
    }
    void FixedUpdate()
    {
        //rb.MovePosition(transform.position + transform.forward * Time.deltaTime);
        if (moving)
        {
            rb.MovePosition(transform.position + dirVector * Time.deltaTime * 5);
            
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                moving = false;
            }
        }

    }

    // Update is called once per frame
    void Update () {
        //rb.MovePosition(dirVector * Time.deltaTime);
        //if (Input.GetKeyDown("space"))
        //{

        //    pushBox("right");
        //}
        RaycastHit uhit;

        if (Physics.Raycast(transform.position, Vector3.forward, out uhit, 0.8f))
        {
            if (uhit.collider.gameObject.tag == "box")
            {
                boxUp = true;
            }
        }
        else
        {
            boxUp = false;
        }

        RaycastHit rhit;

        if (Physics.Raycast(transform.position, Vector3.right, out rhit, 0.8f))
        {
            if (rhit.collider.gameObject.tag == "box")
            {
                boxRight = true;
            }
        }
        else
        {
            boxRight = false;
        }

        RaycastHit dhit;

        if (Physics.Raycast(transform.position, Vector3.back, out dhit, 0.8f))
        {
            if (dhit.collider.gameObject.tag == "box")
            {
                boxDown = true;
            }
        }
        else
        {
            boxDown = false;
        }

        RaycastHit lhit;

        if (Physics.Raycast(transform.position, Vector3.left, out lhit, 0.8f))
        {
            if (lhit.collider.gameObject.tag == "box")
            {
                boxLeft = true;
            }
        }
        else
        {
            boxLeft = false;
        }
    }

    public void pushBox(string dir)
    {
        switch (dir)
        {   
            case "right":
                if (boxRight)
                {
                    Debug.Log("box is right");
                    break;
                }
                GetComponent<AudioSource>().Play();
                dirVector = new Vector3(1.5f, 0, 0) ;
                moving = true;
                timer = 0.1875f;
                break;
            case "up":
                if (boxUp)
                {
                    Debug.Log("box is right");
                    break;
                }
                GetComponent<AudioSource>().Play();
                dirVector = new Vector3(0, 0, 1.5f);
                moving = true;
                timer = 0.1875f;
                break;
            case "down":
                if (boxDown)
                {
                    Debug.Log("box is right");
                    break;
                }
                GetComponent<AudioSource>().Play();
                dirVector = new Vector3(0, 0, -1.5f);
                moving = true;
                timer = 0.1875f;
                break;
            case "left":
                if (boxLeft)
                {
                    Debug.Log("box is right");
                    break;
                }
                GetComponent<AudioSource>().Play();
                dirVector = new Vector3(-1.5f, 0, 0);
                moving = true;
                timer = 0.1875f;
                break;
            //case "right":
            //    transform.translate
            default:
                dirVector = new Vector3(1.5f, 0, 0) + transform.position;
                moving = true;
                break;
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "box" || other.gameObject.tag == "plane")
        {

            
            //    if (Mathf.Abs(other.transform.position.x - transform.position.x) > Mathf.Abs(other.transform.position.z - transform.position.z))
            //    {

            //        if (other.transform.position.x > transform.position.x)
            //        {
            //            boxRight = true;

            //        }
            //        else
            //        {
            //            boxLeft = true;

            //        }
            //    }
            //    else
            //    {

            //        if (other.transform.position.z > transform.position.z)
            //        {
            //            boxUp = true;

            //        }
            //        else
            //        {
            //            boxDown = true;

            //        }
            //    }

            //}
            //else
            //{
            //    boxRight = false;
            //    boxLeft = false;
            //    boxUp = false;
            //    boxDown = false;

        }
    }
}

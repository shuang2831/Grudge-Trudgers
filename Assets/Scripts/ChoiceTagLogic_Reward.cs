using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChoiceTagLogic_Reward : MonoBehaviour
{
    Transform cameraTransform;
    Quaternion rotation;
    public Camera camera;
    public GameObject player;
    public GameObject bll;

    private void Awake()
    {
        rotation = transform.rotation;
        GetComponent<Text>().enabled = false;
        //camera = GetComponent<Camera>();
    }

    void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        //Vector3 lk = transform.position - cameraTransform.position;
        //lk.z = 0;
        //transform.rotation = Quaternion.LookRotation(lk);
        Vector3 screenPoint = camera.WorldToScreenPoint(player.transform.position) + new Vector3(-5, 50, 0);
        transform.position = screenPoint;
        if (bll.GetComponent<RewardLevelLogic>().chosen[player.GetComponent<PlayerController>().playerNum - 1])
        {
            GetComponent<Text>().enabled = true;
        }
        else
        {
            GetComponent<Text>().enabled = false;
        }
    }

    private void LateUpdate()
    {
        transform.rotation = rotation;
    }
}
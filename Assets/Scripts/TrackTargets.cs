using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackTargets : MonoBehaviour
{

    [SerializeField]
    private Transform[] targets;
    GameObject[] players;

    [SerializeField]
    float boundingBoxPadding = 20f;

    [SerializeField]
    float minimumOrthographicSize = 8f;

    [SerializeField]
    float zoomSpeed = 20f;

    Camera camera;
    private Vector3 offset;
    private Rigidbody rb;
    private float minFov;

    void Awake()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
      
        targets = new Transform[players.Length];

        for (int i = 0; i < targets.Length; ++i)
        {
            targets[i] = players[i].transform;
        }
        camera = GetComponent<Camera>();
        //camera.orthographic = true;
        //camera.orthographic = true;
    }
    void Start()
    {
        //offset = transform.position - player.transform.position;
        minFov = Camera.main.fieldOfView;
    }

    void LateUpdate()
    {

        Rect bb = CalculateTargetsBoundingBox();
        Vector3 pos = CalculateCameraPosition(bb);

        if (float.IsNaN(pos.x) || float.IsNaN(pos.z))
        {
            transform.position = new Vector3(0, 20, 0);
            Camera.main.fieldOfView = 100;
        }
        else
        {
            transform.position = pos;
        }

        Vector3 min = camera.WorldToScreenPoint(new Vector3(bb.min.x, 0, bb.min.y));
        Vector3 max = camera.WorldToScreenPoint(new Vector3(bb.max.x, 0, bb.max.y));

        //Debug.Log(bb.size);
        //Debug.Log("min: " + min);
        //Debug.Log("max: " + max);

        if (min.x < 100 || min.y < 100 || max.x > Screen.width - 100 || max.y > Screen.height - 100)
        {

            Camera.main.fieldOfView += 0.2f;

        }
        else if (min.x > 600 || min.y > 600 || max.x < Screen.width - 600 || max.y < Screen.height - 600)
        {
            if (Camera.main.fieldOfView > minFov)
            {
               Camera.main.fieldOfView -= 0.2f;

            }

        }
    }

    void Update()
    {
        //Debug.Log(CalculateTargetsBoundingBox().center);
       
        

    }
    

    /// <summary>
    /// Calculates a bounding box that contains all the targets.
    /// </summary>
    /// <returns>A Rect containing all the targets.</returns>
    Rect CalculateTargetsBoundingBox()
    {
        float minX = Mathf.Infinity;
        float maxX = Mathf.NegativeInfinity;
        float minZ = Mathf.Infinity;
        float maxZ = Mathf.NegativeInfinity;

        foreach (Transform target in targets)
        {
            Vector3 position = target.position;

            if (position.y > -5)
            {
                minX = Mathf.Min(minX, position.x);
                minZ = Mathf.Min(minZ, position.z);
                maxX = Mathf.Max(maxX, position.x);
                maxZ = Mathf.Max(maxZ, position.z);
            }
        }

        return Rect.MinMaxRect(minX - boundingBoxPadding, minZ - boundingBoxPadding, maxX + boundingBoxPadding, maxZ + boundingBoxPadding);
    }

    /// <summary>
    /// Calculates a camera position given the a bounding box containing all the targets.
    /// </summary>
    /// <param name="boundingBox">A Rect bounding box containg all targets.</param>
    /// <returns>A Vector3 in the center of the bounding box.</returns>
    Vector3 CalculateCameraPosition(Rect boundingBox)
    {
        Vector2 boundingBoxCenter = boundingBox.center;
        
        if (boundingBoxCenter.x == float.NaN || boundingBoxCenter.y == float.NaN)
        {
            return new Vector3(0, camera.transform.position.y, 0);
        }
        return new Vector3(boundingBoxCenter.x, camera.transform.position.y, boundingBoxCenter.y - 5);
    }

    float CalculateOrthographicSize(Rect boundingBox)
    {
        float orthographicSize = camera.orthographicSize;
        Vector3 topRight = new Vector3(boundingBox.x + boundingBox.width, 0f, boundingBox.y);
        Vector3 topRightAsViewport = camera.WorldToViewportPoint(topRight);

        if (topRightAsViewport.x >= topRightAsViewport.y)
            orthographicSize = Mathf.Abs(boundingBox.width) / camera.aspect / 2f;
        else
            orthographicSize = Mathf.Abs(boundingBox.height) / 2f;

        return Mathf.Clamp(Mathf.Lerp(camera.orthographicSize, orthographicSize, Time.deltaTime * zoomSpeed), minimumOrthographicSize, Mathf.Infinity);
    }

}
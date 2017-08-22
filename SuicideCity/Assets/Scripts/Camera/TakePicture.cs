﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class TakePicture : MonoBehaviour {

    [Header("Interest Area Project Vars")]
    [Tooltip("Extent of box")]
    public Vector3 extentRange;
    private Vector3 halfExtentRange;
    [Tooltip("Box position")]
    public Vector3 boxPos;
    [Tooltip("Focus change")]
    [Range(1.0f, 2.0f)]
    public float focusChange = 1.5f;

    [Header("Camera Vars")]
    [Tooltip("Camera Zoom")]
    [Range(-10, 10)]
    public float cameraZoom = 0.0f;
    [Tooltip("The number of intervals between the minimum and maximum zoom")]
    public float zoomIntervals = 1.0f;
    [Tooltip("Camera zoom min")]
    public float cameraZoomMin = -10.0f;
    [Tooltip("Camera zoom max")]
    public float cameraZoomMax = 10.0f;

    //[Header("Camera Arm Object")]
    //[Tooltip("Camera arm object ref")]
    //public GameObject cameraArmPos;

    [Header("Input Axis")]
    [Tooltip("Name of mousewheel axis")]
    public string mouseWheelAxisName = "Mouse ScrollWheel";

    [Header("Interest Object Tag")]
    [Tooltip("Tag for objects of interest")]
    public string interestObjectTag = "Interest";

    //camera ref
    private Camera mainCamera;
    //camera pos
    private Transform cameraPos;

    //Frustrum planes of camera, for view port definition
    private Plane[] viewPortPlanes;

    //Collider array of all captured within interest area
    private Collider[] objectsInArea;
    //List of gameobjects that are of interest
    private List<GameObject> objectsInterestList = new List<GameObject>();

    // Use this for initialization
    void Start() {
        mainCamera = Camera.main;
        halfExtentRange = extentRange / 2;

        //GetFrustumPlanes();
        //CreateFrustumPlanes();
    }

    // Update is called once per frame
    void Update() {
        CameraZoom();

        //debug
        if (Input.GetKeyDown(KeyCode.E))
        {
            RunCameraFuncs();
            PrintObjectsInList();
        }
    }

    //handle camera zoom before processing any actions per frame
    private void CameraZoom()
    {
        //get input from scroll wheel
        if (Input.GetAxis(mouseWheelAxisName) > 0.0f)
        {
            if (cameraZoom > cameraZoomMin)
            {
                //change the camera zoom
                cameraZoom -= zoomIntervals;
                //alter the position of the interest box to match zoom
                boxPos.z += zoomIntervals;
                //move the camera
                MoveCamera(cameraZoom);
            }
        }
        else if (Input.GetAxis(mouseWheelAxisName) < 0.0f)
        {
            if (cameraZoom < cameraZoomMax)
            {
                //change the camera zoom
                cameraZoom += zoomIntervals;
                //alter the position of the interest box to match zoom
                boxPos.z -= zoomIntervals;
                //move the camera
                MoveCamera(cameraZoom);
            }
        }
    }

    //move the camera using tweening
    public void MoveCamera(float _Zoom)
    {
        //create a new vector for camera to move to
        Vector3 newCameraPosition = new Vector3(0.0f, 0.0f, -_Zoom);
        //move the main camera
        mainCamera.transform.DOLocalMove(newCameraPosition, 0.5f);
    }

    //Get the frustum planes upon input <- only when called for
    private void GetFrustumPlanes()
    {
        viewPortPlanes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
    }

    //project a box to capture all the colliders in which is within interest range
    private void ProjectInterestArea()
    {
        //get location to place box ahead of camera, half the z ahead of forward
        Vector3 boxCenterLocation = transform.position + (transform.forward * boxPos.z / focusChange);
        //place the box down and get all in box
        objectsInArea = Physics.OverlapBox(boxCenterLocation, halfExtentRange, transform.rotation);
    }

    //find all objects of interest currently in the area in view of camera
    private void FilterObjectsOfInterest()
    {
        //for all the objects in the area
        for (int i = 0; i < objectsInArea.Length; i++)
        {
            //check if the object is of interest
            if (objectsInArea[i].gameObject.CompareTag(interestObjectTag))
            {
                //add to the list
                objectsInterestList.Add(objectsInArea[i].gameObject);
            }
        }
    }

    //check if the objects of interest found are also within the camera's view port
    private void CheckInterestObjectwithinViewPort()
    {
        //Get frustum planes from camera
        GetFrustumPlanes();
        //for all objects in list
        for (int i = 0; i < objectsInterestList.Count; i++)
        {
            //check if an objects collider is within the gotton frustrum planes
            //if not, remove from the list
            if (!GeometryUtility.TestPlanesAABB(viewPortPlanes, objectsInterestList[i].GetComponent<Collider>().bounds))
            {
                objectsInterestList.Remove(objectsInterestList[i]);
            }
        }
    }

    //check if the objects of interest in area are in view of the camera
    private void CheckInterestObjectInViewPerspective()
    {
        //for all objects in list
        for (int i = 0; i < objectsInterestList.Count; i++)
        {
            //get the direction from self to object
            Vector3 directionToTarget = (objectsInterestList[i].transform.position - mainCamera.transform.position).normalized;
            //cast a ray, if it hits, check if the other object is the object you are looking for
            //otherwise, there was something obstructing the view
            RaycastHit hitInfo;
            if (Physics.Raycast(mainCamera.transform.position, directionToTarget, out hitInfo, Mathf.Infinity))
            {
                print("Ray hit " + hitInfo.transform.gameObject);
                //if ray hit something, check what it hit
                //if the thing that was hit was not the object in question, object not in view
                //if so, discard that object from the list
                if (hitInfo.transform.gameObject != objectsInterestList[i])
                {
                    objectsInterestList.Remove(objectsInterestList[i]);
                }
            }
        }
    }

    //run camera functions in sequence
    private void RunCameraFuncs()
    {
        ProjectInterestArea();
        FilterObjectsOfInterest();
        CheckInterestObjectwithinViewPort();
        CheckInterestObjectInViewPerspective();
        //ScreenCapture.CaptureScreenshot()
    }
    
    //debug funcs
    private void CreateFrustumPlanes()
    {
        for (int i = 0; i < viewPortPlanes.Length; i++)
        {
            GameObject p = GameObject.CreatePrimitive(PrimitiveType.Plane);
            p.name = "Plane " + i.ToString();
            p.transform.position = -viewPortPlanes[i].normal * viewPortPlanes[i].distance;
            p.transform.rotation = Quaternion.FromToRotation(Vector3.up, viewPortPlanes[i].normal);
        }
    }

    //prints out all objects in the list <- after all functions for checking if something is in camera view
    private void PrintObjectsInList()
    {
        //for all objects captured in original array
        for (int i = 0; i < objectsInArea.Length; i++)
        {
            print(objectsInArea[i] + " in interest area");
        }

        //for all objects in list
        for (int i = 0; i < objectsInterestList.Count; i++)
        {
            //print out the object with name
            print(objectsInterestList[i] + " in interest list");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + (transform.forward * boxPos.z / focusChange), extentRange);
    }
}

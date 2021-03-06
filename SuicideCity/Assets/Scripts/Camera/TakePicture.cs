﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TakePicture : MonoBehaviour {

    [Header("Interest Area Project Vars")]
    [Tooltip("Extent of box")]
    public Vector3 extentRange;
    private Vector3 halfExtentRange;
    [Tooltip("Box position")]
    public Vector3 boxPos;
    [Tooltip("Lens image")]
    public Image lensImage;
    [Tooltip("Lens movement time")]
    [Range(0.1f, 0.5f)]
    public float lensMovementTime = 0.2f;
    [Tooltip("Focus change")]
    [Range(1.0f, 2.0f)]
    public float focusChange = 1.5f;
    [Tooltip("Zoom change duration")]
    public float zoomChangeDuration = 0.3f;

    [Header("Camera Vars")]
    [Tooltip("Camera Zoom")]
    [Range(-10, 10)]
    public float cameraZoom = 0.0f;
    [Tooltip("The number of intervals between the minimum and maximum zoom")]
    public float zoomIntervals = 1.0f;
    [Tooltip("Camera zoom min")]
    public float cameraZoomMin = 0.0f;
    [Tooltip("Camera zoom max")]
    public float cameraZoomMax = 10.0f;
    [Tooltip("Lens movement")]
    public float lensMovement = 90.0f;
    //starting fov
    private float startFOV = 60.0f;

    [Header("Input Axis")]
    [Tooltip("Name of mousewheel axis")]
    public string mouseWheelAxisName = "Mouse ScrollWheel";

    [Header("Interest Object Tag")]
    [Tooltip("Tag for objects of interest")]
    public string interestObjectTag = "Interest";

    [Header("Resolution")]
    [Tooltip("Resolution Width")]
    public int resolutionWidth = 1980;
    [Tooltip("Resolution Height")]
    public int resolutionHeight = 1020;

    [Header("Photo Object")]
    [Tooltip("The photo object to base photos off")]
    public GameObject photoObject;

    //camera ref
    private Camera mainCamera;
    //camera pos
    private Transform cameraPos;

    //Frustrum planes of camera, for view port definition
    private Plane[] viewPortPlanes;

    //Collider array of all captured within interest area
    private Collider[] objectsInArea;
    //List of gameobjects that are of interest
    public List<GameObject> objectsInterestList = new List<GameObject>();

    [Header("Photobook")]
    [Tooltip("Photobook object held by player")]
    public PhotobookBehaviour photobook;

    [Header("Description Manager")]
    [Tooltip("Description manager script ref")]
    public DescriptionManager descriptionManager;


    public KeyCode photoCaptureKeyCode;

    //control bools
    private bool isCameraOn = false;
    //tween control
    private TweenCallback tweenCallback;

    // Use this for initialization
    void Start() {
        mainCamera = Camera.main;
        halfExtentRange = extentRange / 2;
        startFOV = mainCamera.fieldOfView;
        DOTween.Init();
        //GetFrustumPlanes();
        //CreateFrustumPlanes();
    }

    // Update is called once per frame
    void Update() {
        if (isCameraOn)
        {
            CameraZoom();
            if (Input.GetMouseButtonDown(0))
            {
                RunCameraFuncs();
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            TurnCameraOnOff();
        }
    }

    //move camera screen into view
    private void TurnCameraOnOff()
    {
        if (isCameraOn)
        {
            //set camera on to false
            isCameraOn = false;
            //move lens
            lensImage.rectTransform.DOLocalRotate(new Vector3(-lensMovement, 0, 0), lensMovementTime, RotateMode.Fast);
            //set fov back to preset
            mainCamera.fieldOfView = startFOV;
            //set zoom back to 0
            cameraZoom = 0.0f;
        }
        else if (!isCameraOn)
        {
            isCameraOn = true;
            lensImage.rectTransform.DOLocalRotate(new Vector3(0, 0, 0), lensMovementTime, RotateMode.Fast);
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
                cameraZoom--;
                //move interest box
                boxPos.z += zoomIntervals / 2.5f;
                //alter the fov
                mainCamera.DOFieldOfView(mainCamera.fieldOfView - zoomIntervals, zoomChangeDuration);
            }
        }
        else if (Input.GetAxis(mouseWheelAxisName) < 0.0f)
        {
            
            if (cameraZoom < cameraZoomMax)
            {
                //increase current zoom
                cameraZoom++;
                //move interest box
                boxPos.z -= zoomIntervals / 2.5f;
                //change the field of view by interval
                mainCamera.DOFieldOfView(mainCamera.fieldOfView + zoomIntervals, zoomChangeDuration);
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
        //clear the list before every picture
        objectsInterestList.Clear();
        //run camera funcs
        ProjectInterestArea();
        FilterObjectsOfInterest();
        CheckInterestObjectwithinViewPort();
        CheckInterestObjectInViewPerspective();
        //ScreenCapture.CaptureScreenshot()
        //create the photo
        CreatePhoto();
    }

    //creates the screenshots name
    private string ScreenShotName()
    {
        return string.Format("{0}/Screenshot/{1}.png", Application.dataPath, System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }

    //create the screenshot as 2d texture
    private void CurrentScreenToPicture(PhotoBehaviour _Photo)
    {
        //make a render texture to hold the new image
        RenderTexture newRenderTexture = new RenderTexture(resolutionWidth, resolutionHeight, 24);
        mainCamera.targetTexture = newRenderTexture;
        //make the new texture
        Texture2D newTexture = new Texture2D(resolutionWidth, resolutionHeight, TextureFormat.RGB24, false);
        //have camera render
        mainCamera.Render();
        //sets the active render texture
        RenderTexture.active = newRenderTexture;
        //read pixels into texture
        newTexture.ReadPixels(new Rect(0, 0, resolutionWidth, resolutionHeight), 0, 0);
        newTexture.Apply();
        //deactivate jobs
        mainCamera.targetTexture = null;
        RenderTexture.active = null;
        //destroy the render texture object
        Destroy(newRenderTexture);

        //convert the new texture into a sprite
        Sprite newSprite = Sprite.Create(newTexture, new Rect(0.0f, 0.0f, newTexture.width, newTexture.height), new Vector2(0.5f, 0.5f));
        //give image to photo behaviour object
        _Photo.SetPhotoImage(newSprite);

        ////write the texture to a file
        //byte[] bytes = newTexture.EncodeToPNG();
        //string filename = ScreenShotName();
        //System.IO.File.WriteAllBytes(filename, bytes);
    }

    //get all the interest points in the photo and compile into photo details <- called after all filtering has occured
    private void GetAllInterstPointsInPhoto(PhotoBehaviour _Photo)
    {
        //for all objects of interest inside the photo area
        for (int i = 0; i < objectsInterestList.Count; i++)
        {
            
                _Photo.AddInterestBehaviour(objectsInterestList[i]);
            
        }
    }

    //get the object closest to center and set as focus of the camera
    private void FindFocusObject(PhotoBehaviour _Photo)
    {
        //set initialize values to compare against
        GameObject focusObject = null;
        float minDistance = 2.0f;
        //for all the interest objects in list
        for (int i = 0; i < objectsInterestList.Count; i++)
        {
            //get the transform of this object as a rect transform
            Vector2 rectTransform = mainCamera.WorldToViewportPoint(objectsInterestList[i].transform.position);
            //compare the distance to middle of viewport
            float distance = Vector2.Distance(rectTransform, new Vector2(0.5f, 0.5f));
            //if this object is closer
            if (distance < minDistance)
            {
                //set this object to focus
                focusObject = objectsInterestList[i];
                //set min distance to this distance
                minDistance = distance;
            }
        }
        //set the focus object of this photo
        _Photo.SetFocusObject(focusObject);
    }

    //Set up the photo's description
    private void SetDescriptionOnPhoto(PhotoBehaviour _Photo)
    {
        if (_Photo.mainFocusInterestObject)
        {
            _Photo.description = descriptionManager.GetDescription(_Photo.mainFocusInterestObject.GetComponent<InterestBehaviour>());
        }
        else
        {
            _Photo.description = descriptionManager.GetNoInterestDescription();
        }
    }

    //create the photo object and run it through the neccesary functions
    private void CreatePhoto()
    {
        //create the photo
        GameObject photoClone = Instantiate(photoObject, photobook.transform);
        //run it through all funcs for photo details
        CurrentScreenToPicture(photoClone.GetComponent<PhotoBehaviour>());
        GetAllInterstPointsInPhoto(photoClone.GetComponent<PhotoBehaviour>());
        FindFocusObject(photoClone.GetComponent<PhotoBehaviour>());
        SetDescriptionOnPhoto(photoClone.GetComponent<PhotoBehaviour>());
        photobook.AddPhotoToBook(photoClone);
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
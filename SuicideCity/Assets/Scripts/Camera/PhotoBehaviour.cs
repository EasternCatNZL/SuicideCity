using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoBehaviour : MonoBehaviour {

    [Header("Interest objects list")]
    [Tooltip("List of interest objects captured in the photo")]
    public List<GameObject> interestObjectsList = new List<GameObject>();
    //the main focus point of the photo
    public GameObject mainFocusInterestObject;

    //the image created when taking screenshot
    public Sprite photo;

	// Use this for initialization
	void Start () {
        //interestObjectsList.Clear();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //set the sprite of this photo
    public void SetPhotoImage(Sprite newImage)
    {
        photo = newImage;
    }

    //add interest behaviour script details to the photo
    public void AddInterestBehaviour(GameObject newInterestObject)
    {
        interestObjectsList.Add(newInterestObject);
    }

    //set the focus of this photo
    public void SetFocusObject(GameObject focusObject)
    {
        mainFocusInterestObject = focusObject;
    }
}

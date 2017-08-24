using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoBehaviour : MonoBehaviour {

    [Header("Interest objects list")]
    [Tooltip("List of interest objects captured in the photo")]
    public List<InterestBehaviour> interestObjectsList = new List<InterestBehaviour>();
    //the main focus point of the photo
    public InterestBehaviour mainFocusInterestObject;

    //the image created when taking screenshot
    private Sprite photo;

	// Use this for initialization
	void Start () {
		
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
    public void AddInterestBehaviour(InterestBehaviour newInterestBehaviour)
    {
        interestObjectsList.Add(newInterestBehaviour);
    }

    //set the focus of this photo
    public void SetFocusObject(InterestBehaviour focusBehaviour)
    {
        mainFocusInterestObject = focusBehaviour;
    }
}

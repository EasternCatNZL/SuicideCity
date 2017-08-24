using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotobookBehaviour : MonoBehaviour {

    //the photos currently taken, in list format
    public List<PhotoBehaviour> photoList = new List<PhotoBehaviour>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddPhotoToBook(PhotoBehaviour newPhoto)
    {
        photoList.Add(newPhoto);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotobookBehaviour : MonoBehaviour {

    //the photos currently taken, in list format
    public List<Sprite> photoList = new List<Sprite>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddPhotoToBook(Sprite newPhoto)
    {
        photoList.Add(newPhoto);
    }
}

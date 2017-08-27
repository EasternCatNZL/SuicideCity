using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightPhotoButton : MonoBehaviour {

    public PhotobookBehaviour photobook;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //button logic
    public void PressButton()
    {
        photobook.ChangePhotoNext();
    }
}

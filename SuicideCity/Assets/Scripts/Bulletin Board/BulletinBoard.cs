using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletinBoard : MonoBehaviour {

    public string NextScene;
    public Color FadeColor = Color.black;
    public float FadeSpeed = 0.5f;

    public Text Hint;

    private bool CheckInput = false;
    private bool InMenu = false;

    private Transform InfoSheet;
    private Camera MainCamera;




	// Use this for initialization
	void Start () {
        InfoSheet = GameObject.Find("InfoSheet").transform;
        MainCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
        ZoomOnBoard();

	}

    void FadeScene()
    {
        print("calling FadeScene");
        InitiateFadeObject.Fade(NextScene, FadeColor, FadeSpeed);
    }

    private void OnTriggerEnter(Collider Collision)
    {
        print("Board Trigger");
        Hint.enabled = true;
        CheckInput = true;
    }

    private void OnTriggerExit()
    {
        Hint.enabled = false;
        CheckInput = false;
    }

    void ZoomOnBoard()
    {
        if(CheckInput)
        {
            if(InMenu == false && Input.GetKeyDown(KeyCode.F))
            {
                MainCamera.transform.LookAt(InfoSheet);
                MainCamera.fieldOfView = Mathf.Lerp(MainCamera.fieldOfView,20.0f,1.0f);
                CameraController.LockCamera();
                PlayerController.LockPlayer();
                Hint.enabled = false;
                InMenu = true;
            }
            else if(InMenu == true && Input.GetKeyDown(KeyCode.F))
            {
                CameraController.UnlockCamera();
                PlayerController.UnlockPlayer();
                FadeScene();                
            }
        }
    }
}

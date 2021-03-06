﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    Vector2 MouseLook;
    Vector2 SmoothVector;

    public float Sensitivity = 2.0f;
    public float Smoothing = 2.0f;
    static private bool Lock = false;

    GameObject Player;

	// Use this for initialization
	void Start () {
        Player = this.transform.parent.gameObject;		
	}
	
	// Update is called once per frame
	void Update () {
        
        if (!Lock)
        {
            MouseMovement();
            if (Input.GetKeyUp(KeyCode.Escape)) { Lock = false; Cursor.lockState = CursorLockMode.Locked; }
            else if (Input.GetKeyDown(KeyCode.Escape)) { Lock = true; Cursor.lockState = CursorLockMode.None; }
        }
	}

    void MouseMovement()
    {
        var MouseDirection = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        MouseDirection = Vector2.Scale(MouseDirection, new Vector2(Sensitivity * Smoothing, Sensitivity * Smoothing));
        SmoothVector.x = Mathf.Lerp(SmoothVector.x, MouseDirection.x, 1.0f / Smoothing);
        SmoothVector.y = Mathf.Lerp(SmoothVector.y, MouseDirection.y, 1.0f / Smoothing);

        MouseLook += SmoothVector;
        MouseLook.y = Mathf.Clamp(MouseLook.y, -90.0f, 90.0f);

        transform.localRotation = Quaternion.AngleAxis(-MouseLook.y, Vector3.right);
        Player.transform.localRotation = Quaternion.AngleAxis(MouseLook.x, Player.transform.up);
    }

    static public void LockCamera()
    {
        Lock = true;
    }

    static public void UnlockCamera()
    {
        Lock = false;
    }
}

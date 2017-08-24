using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float MoveSpeed = 5.0f;
    private float Move;
    private float Straffe;
    static private bool Lock = false;

	// Use this for initialization
	void Start () {
        Cursor.lockState = CursorLockMode.Locked;
    }
	
	// Update is called once per frame
	void Update () {
        if (!Lock)
        {
            Movement();
        }
	}

    void Movement()
    {
        Move = Input.GetAxis("Vertical") * MoveSpeed;
        Straffe = Input.GetAxis("Horizontal") * MoveSpeed;

        Move *= Time.deltaTime;
        Straffe *= Time.deltaTime;

        transform.Translate(Straffe, 0, Move);       
    }

    static public void LockPlayer()
    {
        Cursor.lockState = CursorLockMode.None;
        Lock = true;
    }

    static public void UnlockPlayer()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Lock = false;
    }
}

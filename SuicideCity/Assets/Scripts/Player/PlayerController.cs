using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float MoveSpeed = 5.0f;
    private float Move;
    private float Straffe;

	// Use this for initialization
	void Start () {
        Cursor.lockState = CursorLockMode.Locked;
    }
	
	// Update is called once per frame
	void Update () {
        Movement();
	}

    void Movement()
    {
        Move = Input.GetAxis("Vertical") * MoveSpeed;
        Straffe = Input.GetAxis("Horizontal") * MoveSpeed;

        Move *= Time.deltaTime;
        Straffe *= Time.deltaTime;

        transform.Translate(Straffe, 0, Move);       
    }
}

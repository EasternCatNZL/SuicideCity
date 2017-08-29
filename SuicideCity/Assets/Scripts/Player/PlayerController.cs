using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float MoveSpeed = 5.0f;
    private float Move;
    private float Straffe;
    private Animator Anim;
    static private bool Lock = false;
    private Rigidbody Rigid;
    

	// Use this for initialization
	void Start () {
        Rigid = GetComponent<Rigidbody>();
        Anim = GetComponent<Animator>();
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
        if (Input.GetAxis("Vertical") != 0f || Input.GetAxis("Horizontal") != 0f)
        {
            Anim.SetBool("Walking", true);
            Move = Input.GetAxis("Vertical") * MoveSpeed;
            Straffe = Input.GetAxis("Horizontal") * MoveSpeed;

            Move *= Time.deltaTime;
            Straffe *= Time.deltaTime;

            transform.Translate(Straffe, 0, Move);
            //Rigid.MovePosition(transform.position + new Vector3(Straffe, 0, Move));
        }
        else
        {
            Anim.SetBool("Walking", false);
        }
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

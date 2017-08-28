using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActManager : MonoBehaviour {

    public enum Act
    {
        ThePark
    }

    [Header("Act")]
    [Tooltip("Which act is this scene?")]
    public Act thisAct;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //
}

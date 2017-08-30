﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActOneVictimLogic : MonoBehaviour {

    //ref back to act one
    [HideInInspector]
    public ActOneLogic actOneLogic;

    [Header("Tags")]
    [Tooltip("Floor tag")]
    public string floorTag = "Floor";

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        //when making contact with the floor end act
        if (collision.gameObject.CompareTag(floorTag))
        {
            actOneLogic.victimActor.GetComponent<InterestBehaviour>().progress = InterestBehaviour.ActProgress.AfterAct;
            actOneLogic.sceneObject.GetComponent<InterestBehaviour>().progress = InterestBehaviour.ActProgress.AfterAct;
        }
    }
}

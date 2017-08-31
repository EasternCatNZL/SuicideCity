using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterestBehaviour : MonoBehaviour {

    //[Header("Details of interest")]
    //[Tooltip("Name of this thing")]
    //public string interestName;

    //enum of interest type
    public enum InterestType
    {
        Victim,
        Scene,
        Tool,
        Evidence
    }

    //enum of Act progress
    public enum ActProgress
    {
        BeforeAct,
        InAct,
        AfterAct
    }

    [Header("Interest Type")]
    [Tooltip("What is this interest point?")]
    public InterestType interestType;
    [Tooltip("What state is this interest point in?")]
    public ActProgress progress;

	// Use this for initialization
	void Start () {
        //debug
        //print(Camera.main.WorldToViewportPoint(transform.position));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //progress change functionality
    public void ChangeToBeforeAct()
    {
        progress = ActProgress.BeforeAct;
    }

    public void ChangeToInAct()
    {
        progress = ActProgress.InAct;
    }

    public void ChangeToAfterAct()
    {
        progress = ActProgress.AfterAct;
    }
}

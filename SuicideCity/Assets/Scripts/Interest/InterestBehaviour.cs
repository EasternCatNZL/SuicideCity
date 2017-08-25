using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterestBehaviour : MonoBehaviour {

    [Header("Details of interest")]
    [Tooltip("Name of this thing")]
    public string interestName;
    [Tooltip("Details of interest")]
    public string[] interestDetailsArray = new string[0];

    //enum of interest type
    public enum InterestType
    {
        Victim,
        Weapon,
        Evidence
    }

    [Header("Interest Type")]
    [Tooltip("What is this interest point?")]
    public InterestType interesttype;

	// Use this for initialization
	void Start () {
        //debug
        //print(Camera.main.WorldToViewportPoint(transform.position));
	}
	
	// Update is called once per frame
	void Update () {
		
	}


}

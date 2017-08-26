using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActOneLogic : MonoBehaviour {

    [Header("Actors")]
    [Tooltip("The victim")]
    public GameObject victimObject;
    [Tooltip("The location")]
    public GameObject locationObject;
    [Tooltip("The tool")]
    public GameObject toolObject;

    [Header("Tags")]
    [Tooltip("Player tag")]
    public string playerTag = "Player";

    //control bools
    private bool beginAct = false; //checks to see if player is within range to begin act

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //begin act when player approaches
    private void OnTriggerEnter(Collider other)
    {
        //check if other is the player
        if (other.CompareTag(playerTag))
        {
            beginAct = true;
        }
    }
}

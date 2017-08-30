using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActOneLogic : MonoBehaviour {

    [Header("References to actors")]
    [Tooltip("The victim in the act")]
    public GameObject victimActor;
    [Tooltip("The scene of the crime")]
    public GameObject sceneObject;

    [Header("Tags")]
    [Tooltip("Tag for player")]
    public string playerString = "Player";

    private bool isStartAct = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //coroutine that runs after player has entered area
    private IEnumerator ActCoroutine(){
        //wait for flag to start
        yield return isStartAct;


    }

    //start coroutine using trigger
    private void OnTriggerEnter(Collider other)
    {
        //if other is player, start coroutine
        if (other.CompareTag(playerString))
        {
            isStartAct = true;
        }
    }
}

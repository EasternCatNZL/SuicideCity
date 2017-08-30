using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ActOneLogic : MonoBehaviour {

    [Header("References to actors")]
    [Tooltip("The victim in the act")]
    public GameObject victimActor;
    [Tooltip("The scene of the crime")]
    public GameObject sceneObject;

    [Header("Tags")]
    [Tooltip("Tag for player")]
    public string playerString = "Player";

    [Header("Act control")]
    [Tooltip("Jump force by actor")]
    public float actorJumpForce = 4.0f;
    [Tooltip("Forward jump distance by actor")]
    public float actorJumpDistance = 5.0f;
    [Tooltip("Tween jump time (Arc of jump)")]
    public float arcJumpTime = 3.0f;

    private bool isStartAct = false;

	// Use this for initialization
	void Start () {
        victimActor.GetComponent<ActOneVictimLogic>().actOneLogic = this;
        StartCoroutine(ActCoroutine());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //coroutine that runs after player has entered area
    private IEnumerator ActCoroutine(){
        //wait for flag to start
        yield return isStartAct;

        //change victim and scene to in act
        victimActor.GetComponent<InterestBehaviour>().progress = InterestBehaviour.ActProgress.InAct;
        sceneObject.GetComponent<InterestBehaviour>().progress = InterestBehaviour.ActProgress.InAct;

        //get end of jump arc
        Vector3 endOfArcPos = victimActor.transform.position + victimActor.transform.forward * actorJumpDistance;
        //have victim jump
        victimActor.transform.DOJump(endOfArcPos, actorJumpForce, 1, arcJumpTime);
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

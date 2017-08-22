using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicAI : MonoBehaviour {

    public PathWaypoint Destination;
    public Vector3 PrevDestination = Vector3.zero;

    static float Arrived = 2.0f;

    private NavMeshAgent Agent;
    private Vector3 Direction;

    private void Start()
    {

        Agent = GetComponent<NavMeshAgent>();
        Agent.SetDestination(Destination.transform.position);
        Agent.angularSpeed = 180;
    }
	
	// Update is called once per frame
	void Update () {
        if(Agent.remainingDistance < Arrived)
        {
            Vector3 TempPos = Destination.transform.position;

            print("Prev Dest: " + PrevDestination.ToString());
            Destination = Destination.GetOppositeWaypoint(PrevDestination);

            PrevDestination = TempPos;

            Agent.SetDestination(Destination.transform.position);
        }


        Direction = Agent.nextPosition - transform.position;

        Direction.Normalize();

        //Player Rotations
        if (Vector3.Dot(transform.right, Direction) < 0.0f || Vector3.Dot(transform.right, Direction) > 0.0f)
        {
            Agent.speed = 0;
        }
        else
        {
            Agent.speed = 3.5f;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + Direction);
    }
}

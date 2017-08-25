using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicAI : MonoBehaviour
{

    public PathWaypoint Destination;
    public Vector3 PrevDestination = Vector3.zero;

    public GameObject TargetPointOfInterest = null;

    static float Arrived = 2.0f;

    private NavMeshAgent Agent;
    private Vector3 Direction;

    private void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        Agent.SetDestination(Destination.transform.position);
        Agent.angularSpeed = 90;
    }

    // Update is called once per frame
    void Update()
    {
        if (Agent.remainingDistance < Arrived && Destination)
        {
            if (TargetPointOfInterest)
            {
                Vector3 TempPos = Destination.transform.position;

                Destination = Destination.FindPointOfInterest(TargetPointOfInterest);
                if (!Destination) Agent.isStopped = true;
                else
                {
                    PrevDestination = TempPos;

                    Agent.SetDestination(Destination.transform.position);
                }
            }
            else
            {
                Vector3 TempPos = Destination.transform.position;

                Destination = Destination.GetOppositeWaypoint(PrevDestination);

                PrevDestination = TempPos;

                Agent.SetDestination(Destination.transform.position);
            }
        }

        //Rotations
        Direction = Agent.nextPosition - transform.position;

        Direction.Normalize();


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

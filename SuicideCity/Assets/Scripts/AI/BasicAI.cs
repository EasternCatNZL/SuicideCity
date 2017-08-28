using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicAI : MonoBehaviour
{
    public float MinSpeed = 1f;
    public float MaxSpeed = 1.5f;
    public PathWaypoint Destination;
    public Vector3 PrevDestination = Vector3.zero;

    public GameObject TargetPointOfInterest = null;

    static float Arrived = 1.0f;

    private float Speed;
    private NavMeshAgent Agent;
    private Animator Anim;
    private Vector3 Direction;

    private void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        Anim = GetComponent<Animator>();
        Speed = Random.Range(MinSpeed, MaxSpeed);
        Agent.SetDestination(Destination.transform.position);
        Agent.angularSpeed = 90;
    }

    // Update is called once per frame
    void Update()
    {
        if(Agent.destination != null) Debug.DrawLine(transform.position, Agent.destination);
        if (Agent.remainingDistance < Arrived && Destination)
        {
            Anim.speed = Speed;
            Anim.SetBool("Walking", true);
            if (TargetPointOfInterest)
            {
                Vector3 TempPos = Destination.transform.position;

                Destination = Destination.FindPointOfInterest(TargetPointOfInterest);
                if (!Destination) Agent.isStopped = true;
                else
                {
                    PrevDestination = TempPos;

                    Agent.SetDestination(Destination.GetRandomPoint());
                    
                }
            }
            else
            {
                Vector3 TempPos = Destination.transform.position;

                Destination = Destination.GetOppositeWaypoint(PrevDestination);

                PrevDestination = TempPos;

                Agent.SetDestination(Destination.GetRandomPoint());
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
            Agent.speed = Speed;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + Direction);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    public bool Turning = false;
    public float TurnRate = 0.75f;
    public float Dot = 0f;
    public float Angle = 0f;
    public float MinSpeed = 1f;
    public float MaxSpeed = 1.5f;
    public PathWaypoint Destination;
    public Vector3 PrevDestination = Vector3.zero;

    public GameObject TargetPointOfInterest = null;

    static float Arrived = 2.0f;

    private bool Stop;
    private float Speed;
    private float TurningAngle;
    private Animator Anim;
    private Rigidbody Rigid;
    private Vector3 Direction;
    private Vector3 DestinationPos;

    private void Start()
    {
        Anim = GetComponent<Animator>();
        Rigid = GetComponent<Rigidbody>();
        Speed = Random.Range(MinSpeed, MaxSpeed);
        TurnRate = Random.Range(1f, 1.5f);
        DestinationPos = Destination.GetRandomPoint();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(transform.position, DestinationPos);
        if (Vector3.Magnitude(DestinationPos - transform.position) < Arrived && Destination)
        {
            Anim.speed = Speed;
            Anim.SetBool("Walking", true);
            if (TargetPointOfInterest)
            {
                Vector3 TempPos = Destination.transform.position;

                Destination = Destination.FindPointOfInterest(TargetPointOfInterest);
                if (!Destination) Stop = true;
                else
                {
                    PrevDestination = TempPos;

                    DestinationPos = Destination.GetRandomPoint();

                }
            }
            else
            {
                Vector3 TempPos = Destination.transform.position;

                Destination = Destination.GetOppositeWaypoint(PrevDestination);

                PrevDestination = TempPos;

                DestinationPos = Destination.GetRandomPoint();
            }
        }

        //Rotations
        Direction = DestinationPos - transform.position;

        Direction.Normalize();

        Turning = false;
        
        if (Vector3.Dot(transform.right, Direction) < -0.01f)
        {
            if (TurningAngle == 0) TurningAngle = Vector3.Angle(transform.forward, Direction);
            Turning = true;
            Dot = Vector3.Dot(transform.right, Direction);
            transform.Rotate(0.0f, (-TurningAngle) * Time.deltaTime, 0.0f);
            Angle = (-Vector3.Angle(transform.forward, Direction) * TurnRate);
        }
        else if (Vector3.Dot(transform.right, Direction) > 0.01f)
        {
            if (TurningAngle == 0) TurningAngle = Vector3.Angle(transform.forward, Direction);
            Turning = true;
            Dot = Vector3.Dot(transform.right, Direction);
            transform.Rotate(0.0f, TurningAngle * Time.deltaTime, 0.0f);
            Angle = (-Vector3.Angle(transform.forward, Direction) * TurnRate);
        }
        else
        {
            TurningAngle = 0f;
        }
        Rigid.MovePosition(transform.position + transform.forward * Speed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;       
        Gizmos.DrawLine(transform.position, transform.position + Direction);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward);
    }
}

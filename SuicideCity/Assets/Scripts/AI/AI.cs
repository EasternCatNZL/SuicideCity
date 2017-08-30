using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class AI : MonoBehaviour
{
    public bool TurningLeft = false;
    public bool TurningRight = false;
    [Range(0, 1f)]
    public float TurnSlow = 0.7f;
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
    private float CurrentSpeed;
    public float TurningAngle;
    private Animator Anim;
    private Rigidbody Rigid;
    private Vector3 Direction;
    private Vector3 DestinationPos;

    private void Start()
    {
        Anim = GetComponent<Animator>();
        Rigid = GetComponent<Rigidbody>();
        Speed = Random.Range(MinSpeed, MaxSpeed);
        CurrentSpeed = Speed;
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
        Direction = new Vector3(DestinationPos.x, 0f, DestinationPos.z) - new Vector3(transform.position.x, 0f, transform.position.z);

        Direction.Normalize();

        TurningLeft = false;
        TurningRight = false;

        Angle = (Vector3.Angle(transform.forward, Direction));
        if (Angle > 0f)
        {
            Rigid.angularVelocity = Vector3.zero; //Stop spinouts from happening

            Dot = Vector3.Dot(transform.right, Direction);

            if (Dot < 0.0f) //Check if the AI needs to turn left
            {
                if(Dot < -0.01f) CurrentSpeed = Speed * 0.7f;
                else CurrentSpeed = Speed;

                TurningLeft = true;
                               
                Angle = -Angle; //Flip the angle because Vector3.Angle() only returns absolutes

                if (-TurnRate * Time.deltaTime < Angle)
                {
                    transform.Rotate(0.0f, Angle * Time.deltaTime, 0.0f);
                }
                else
                {
                    transform.Rotate(0.0f, -TurnRate * Time.deltaTime, 0.0f);
                }

            }
            else if (Dot > 0.0f) //Check if the AI needs to turn right
            {
                if (Dot > 0.01f) CurrentSpeed = Speed * 0.7f;
                else CurrentSpeed = Speed;

                TurningRight = true;
               
                if (TurnRate  * Time.deltaTime > Angle)
                {
                    transform.Rotate(0.0f, Angle * Time.deltaTime, 0.0f);
                }
                else
                {
                    transform.Rotate(0.0f, TurnRate * Time.deltaTime, 0.0f);
                }

            }
            else //This else statement is probably redundant. Not sure.
            {               
                transform.Rotate(0.0f, Angle * Time.deltaTime, 0.0f);
            }
        }
        else
        {
            
        }
        Rigid.MovePosition(transform.position + transform.forward * CurrentSpeed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;       
        Gizmos.DrawLine(transform.position, transform.position + Direction);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward + new Vector3(0f, 0.1f, 0f));
        Handles.DrawSolidArc(transform.position, transform.up, transform.forward, Angle, 1f);
    }

    private void OnDrawGizmosSelected()
    {
        Handles.BeginGUI();
        Handles.Label(transform.position + transform.up * 3, "Turning Direction:\nLeft: " + TurningLeft + "\nRight: " +TurningRight + "\nSpeed: " + CurrentSpeed);
        
    }
}

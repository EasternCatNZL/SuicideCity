using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathWaypoint : MonoBehaviour {

    public PathWaypoint[] Waypoint;
  

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //Return a waypoint that isn't the one you came from
    public PathWaypoint GetOppositeWaypoint(Vector3 _From)
    {
        if (_From == Vector3.zero) return GetRandomWaypoint();
        for(int i = 0; i < Waypoint.Length; ++i)
        {
            if(Waypoint[i].transform.position != _From)
            {
                print(_From.ToString());
                print("To " + Waypoint[i].name);
                print(Waypoint[i].transform.position.ToString());
                return Waypoint[i];
            }
        }
        return Waypoint[0];
    }

    //Return a completely random waypoint
    public PathWaypoint GetRandomWaypoint()
    {
        return Waypoint[Random.Range(0, Waypoint.Length)];
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(transform.position, 0.5f);
        for (int i = 0; i < Waypoint.Length; ++i)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, Waypoint[i].transform.position);
        }
    }
}

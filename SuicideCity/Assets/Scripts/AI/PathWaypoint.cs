using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathWaypoint : MonoBehaviour
{
    [Tooltip("The radius of the area a position can be given")]
    public float Radius = 1f;
    public PathWaypoint[] Waypoint;
    public GameObject PointOfInterest = null;
    private Dictionary<GameObject, int> POIs = new Dictionary<GameObject, int>();
    public bool Mapped = false;
    private bool Mapping = false;
    public bool DebugBoolean;
    private List<int> temp;
    // Use this for initialization
    void Start()
    {
        if (PointOfInterest)
        {
            print("POI Spreading");
            SpreadPointsOfInterest(PointOfInterest, gameObject.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (DebugBoolean)
        {
            foreach (KeyValuePair<GameObject, int> entry in POIs)
            {
                print("POI Value: " + entry.Key.name + " Path: " + entry.Value);
            }
            DebugBoolean = false;
        }
    }

    //Returns a Waypoint in the direction to a point of interest
    public PathWaypoint FindPointOfInterest(GameObject _POI)
    {
        if (PointOfInterest == _POI) return null;
        int WaypointIndex = -1;
        if (POIs.TryGetValue(_POI, out WaypointIndex))
        {
            return Waypoint[WaypointIndex];
        }
        return null;
    }

    //Propergate the knowledge of a point of interest
    public void SpreadPointsOfInterest(GameObject _POI, string _PointName)
    {
        if (POIs.ContainsKey(_POI))
        {
            return;
        }
        else
        {
            for (int i = 0; i < Waypoint.Length; ++i)
            {
                if (Waypoint[i].name == _PointName)
                {
                    POIs.Add(_POI, i);
                }
            }
            for (int i = 0; i < Waypoint.Length; ++i)
            {
                Waypoint[i].SpreadPointsOfInterest(_POI, gameObject.name);
            }
        }
    }

    //Return a waypoint that isn't the one you came from
    public PathWaypoint GetOppositeWaypoint(Vector3 _From)
    {
        if (_From == Vector3.zero) return GetRandomWaypoint();
        float weight = Random.Range(0f, 1f);
        float chance = 1f / (Waypoint.Length - 1f);

        for (int i = 0; i < Waypoint.Length; ++i)
        {
            if (Waypoint[i].transform.position != _From)
            {
                if (weight < chance)
                {
                    return Waypoint[i];
                }
                else
                {
                    chance += 1f / (Waypoint.Length - 1f);
                }
            }
        }
        if (Waypoint[0].transform.position != _From || Waypoint.Length == 1)
        {
            return Waypoint[0];
        }
        else
        {
            return Waypoint[1];
        }

    }

    //Return a completely random waypoint
    public PathWaypoint GetRandomWaypoint()
    {
        return Waypoint[Random.Range(0, Waypoint.Length)];
    }

    public Vector3 GetRandomPoint()
    {
        float RandX = Random.Range(-Radius, Radius);
        float RandZ = Random.Range(-Radius, Radius);

        Vector3 RandVector = new Vector3(transform.position.x + RandX, transform.position.y, transform.position.z + RandZ);

        return RandVector;
    }

    public Vector3 GetRandomPoint(Vector3 Offset)
    {
        return Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(transform.position, 0.2f);
        Gizmos.color = Color.red;
        UnityEditor.Handles.color = Color.cyan;
        UnityEditor.Handles.Label(transform.position + transform.up, gameObject.name);
        UnityEditor.Handles.DrawWireDisc(transform.position, transform.up, Radius);
        for (int i = 0; i < Waypoint.Length; ++i)
        {
            Vector3 dir = Waypoint[i].transform.position - transform.position;
            dir.Normalize();
            
            Gizmos.DrawLine(transform.position + Vector3.Cross(dir, transform.up) / 2, Waypoint[i].transform.position);                     
        }
        if(PointOfInterest)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, PointOfInterest.transform.position);
        }
    }
}

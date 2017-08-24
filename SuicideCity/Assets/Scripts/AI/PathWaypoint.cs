using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathWaypoint : MonoBehaviour
{

    public PathWaypoint[] Waypoint;
    public GameObject PointOfInterest = null;
    private Dictionary<GameObject, int> POIs = new Dictionary<GameObject, int>();
    public bool Mapped = false;
    private bool Mapping = false;
    public bool DebugBoolean;
    public List<int> temp;
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

    public GameObject GetPointOfInterest()
    {
        return PointOfInterest;
    }

    public bool MapPointsOfInterest()
    {
        if (Mapped) return true;
        if (Mapping) return false;
        Mapping = true;
        for (int i = 0; i < Waypoint.Length; ++i)
        {
            print(gameObject.name);
            if (Waypoint[i].MapPointsOfInterest())
            {
                if (Waypoint[i].GetPointOfInterest() != null)
                {
                    POIs.Add(Waypoint[i].GetPointOfInterest(), i);
                }
            }
        }
        print("\n");
        print(gameObject.name);
        print(POIs.Keys.ToString());
        print(POIs.Values.ToString());
        Mapped = true;
        return true;
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
                print("Weight: " + weight + " - Chance: " + chance);
                if (weight < chance)
                {
                    print("From " + gameObject.name);
                    print("To " + Waypoint[i].name);
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

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(transform.position, 0.2f);
        for (int i = 0; i < Waypoint.Length; ++i)
        {
            Vector3 dir = Waypoint[i].transform.position - transform.position;
            dir.Normalize();
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position + Vector3.Cross(dir, transform.up) / 2, Waypoint[i].transform.position);
            UnityEditor.Handles.Label(Waypoint[i].transform.position + transform.up, Waypoint[i].name);
        }
    }
}

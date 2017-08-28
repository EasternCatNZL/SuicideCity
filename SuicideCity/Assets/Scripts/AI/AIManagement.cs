using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManagement : MonoBehaviour {

    public GameObject AIPrefab;
    GameObject[] Waypoints;

	// Use this for initialization
	void Start () {
        SpawnAI();
	}
	
	public void SpawnAI()
    {
        Waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        foreach (GameObject it in Waypoints)
        {
            GameObject temp = Instantiate(AIPrefab, it.transform.position, Quaternion.identity);
            temp.GetComponent<AI>().Destination = it.GetComponent<PathWaypoint>();
        }
    }
}

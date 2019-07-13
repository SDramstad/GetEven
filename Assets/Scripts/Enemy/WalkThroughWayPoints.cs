using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class WalkThroughWayPoints : MonoBehaviour {
    
    public GameObject[] _walkWaypoints;

    private int currentTargetWaypoint;

    // Use this for initialization
    void Start()
    {
        currentTargetWaypoint = Random.Range(0, _walkWaypoints.Length);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, _walkWaypoints[currentTargetWaypoint].transform.position) > 2f)
        {
            GetComponent<NavMeshAgent>().destination = _walkWaypoints[currentTargetWaypoint].transform.position;
            //Debug.Log("En route to Waypoint " + currentWaypoint);
        }
        else
        {
            //Debug.Log("Changing Mind: Moving to " + currentWaypoint);
            currentTargetWaypoint = Random.Range(0, _walkWaypoints.Length);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainBackground : MonoBehaviour {

    [SerializeField]
    private float speed;
    //private GameObject _waypoint1;
    //private GameObject _waypoint2;
    private bool switchDirection;
    //true = forward, false = backward

    private Vector3 _startLocation;
    private Vector3 _endLocation;

    private const int _distance = 800;

	// Use this for initialization
	void Start () {
        _startLocation = transform.position;
        _endLocation = new Vector3(transform.position.x, transform.position.y, transform.position.z + _distance);
        //switchDirection = true;
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(Vector3.Distance(_startLocation, transform.position));

        if (Vector3.Distance(_startLocation, transform.position) == _distance)
        {
            switchDirection = false;
        }

        if (Vector3.Distance(_startLocation, transform.position) == 0)
        {
            switchDirection = true;
        }

        if (switchDirection)
        {
            transform.position = Vector3.MoveTowards(transform.position, _endLocation, speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, _startLocation, speed * Time.deltaTime);
        }
	}
}

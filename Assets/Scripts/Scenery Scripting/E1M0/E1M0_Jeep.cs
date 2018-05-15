using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1M0_Jeep : MonoBehaviour {

    private Vector3 _startLocation;
    private Vector3 _endLocation;

    public float speed;
    public bool xPos;

    // Use this for initialization
    void Start () {
        _startLocation = transform.position; 
        if (xPos)
        {
            _endLocation = new Vector3(transform.position.x + 120, transform.position.y, transform.position.z);
        }
        else
        {
            _endLocation = new Vector3(transform.position.x - 120, transform.position.y, transform.position.z);

        }
    }
	
	// Update is called once per frame
	void Update () {

        if (Vector3.Distance(transform.position, _endLocation) < 1f) 
        {
            transform.position = _startLocation;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, _endLocation, speed * Time.deltaTime);
        }
    }
}

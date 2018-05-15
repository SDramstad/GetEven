using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObjects : MonoBehaviour {

    private Vector3 _startLocation;
    private Vector3 _endLocation;

    public float speed;
    public int distance;

    //x = 0, y = 1, z = 2
    public int x_y_or_z;
    public bool Pos;



    // Use this for initialization
    void Start () {
        _startLocation = transform.position;
        float xPosition = transform.position.x;
        float yPosition = transform.position.y;
        float zPosition = transform.position.z;

        switch (x_y_or_z)
        {
            case 0:
                if (Pos)
                {
                    _endLocation = new Vector3(xPosition + distance, yPosition, zPosition);
                }
                else
                {
                    _endLocation = new Vector3(xPosition - distance, yPosition, zPosition);
                }
                break;
            case 1:
                if (Pos)
                {
                    _endLocation = new Vector3(xPosition, yPosition + distance, zPosition);
                }
                else
                {
                    _endLocation = new Vector3(xPosition, yPosition - distance, zPosition);
                }
                break;
            case 2:
                if (Pos)
                {
                    _endLocation = new Vector3(xPosition, yPosition, zPosition + distance);
                }
                else
                {
                    _endLocation = new Vector3(xPosition, yPosition, zPosition - distance);
                }
                break;
            default:
                break;
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

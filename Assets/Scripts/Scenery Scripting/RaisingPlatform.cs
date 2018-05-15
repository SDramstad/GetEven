using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaisingPlatform : MonoBehaviour {

    Transform platform;

    [SerializeField]
    private float startPointY = 0f;

    [SerializeField]
    private float startPointX = 0f;

    [SerializeField]
    private float startPointZ = 0f;

    [SerializeField]
    private float endPointY = 0f;

    [SerializeField]
    private float endPointX = 0f;

    [SerializeField]
    private float endPointZ = 0f;

    [SerializeField]
    private float speed = 1f;

    private Vector3 targetPosition;

    private Vector3 endPosition;

    private Vector3 startPosition;

    // Use this for initialization
    void Start () {
        platform = GetComponent<Transform>();
        startPosition = new Vector3(startPointX, startPointY, startPointZ);
        endPosition = new Vector3(endPointX, endPointY, endPointZ);
        targetPosition = new Vector3(endPointX, endPointY, endPointZ);
    }
    
	// Update is called once per frame
	void Update () {

        if (Vector3.Distance(transform.position, endPosition) < .1f)
        {
            targetPosition = startPosition;
        }
        
        if (Vector3.Distance(transform.position, startPosition) < .1f)
        {
            targetPosition = endPosition;
        } 

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
	}
}

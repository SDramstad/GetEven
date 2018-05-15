using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerToStartLocation : MonoBehaviour {

    
    private GameObject startLocation;
	// Use this for initialization
	void Start () {
        startLocation = this.gameObject;
        GameObject player = GameObject.Find("Player");
        player.transform.position = startLocation.transform.position;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

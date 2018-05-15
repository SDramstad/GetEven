using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterHereSpawnThere : I_EnterTrigger {

    //private List<GameObject> activateObjects;
    [SerializeField]
    private GameObject _spawnObjectParent;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (checkBounds())
        {
            _spawnObjectParent.SetActive(true);
            Destroy(this);
        }
	}
}

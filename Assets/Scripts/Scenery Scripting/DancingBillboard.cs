using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DancingBillboard : MonoBehaviour {

    private SpriteRenderer _spriteRenderer;
    private Random random;
	// Use this for initialization
	void Start () {
        float randomDanceSpeed = Random.Range(0.3f, 1.2f);
        InvokeRepeating("Dance", 1f, randomDanceSpeed);
	}

    void Dance()
    {
        GetComponentInChildren<SpriteRenderer>().flipX = !GetComponentInChildren<SpriteRenderer>().flipX;
    }
	
}

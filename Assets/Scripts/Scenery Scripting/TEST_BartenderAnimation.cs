using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_BartenderAnimation : MonoBehaviour {

    private SpriteRenderer _spriteRenderer;
    private Random random;
    private bool isFirstSprite;

    [SerializeField]
    private Sprite firstSprite;
    [SerializeField]
    private Sprite secondSprite;

    // Use this for initialization
    void Start () {
        float randomDanceSpeed = Random.Range(0.3f, 1.2f);
        InvokeRepeating("Dance", 1f, randomDanceSpeed);
        isFirstSprite = true;
    }

    void Dance()
    {
        if (isFirstSprite)
        {
            GetComponentInChildren<SpriteRenderer>().sprite = secondSprite;
            isFirstSprite = false;
        }
        else
        {
            GetComponentInChildren<SpriteRenderer>().sprite = firstSprite;
            isFirstSprite = true;
        }
    }
	
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_Spin : MonoBehaviour
{
    [SerializeField]
    private int spinFactor;
    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(Vector3.right * spinFactor *  Time.deltaTime);
    }
}

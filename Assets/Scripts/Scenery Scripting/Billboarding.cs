using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboarding : MonoBehaviour
{

    public Transform CameraTrans;
    public Transform ThisTrans;
    public bool alignNotLook = true;

    // Use this for initialization
    void Start()
    {
        ThisTrans = this.transform;
        CameraTrans = Camera.main.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (alignNotLook)
        {
            ThisTrans.forward = CameraTrans.forward;
        }
        else
        {
            ThisTrans.LookAt(CameraTrans, Vector3.up);
        }
    }
}
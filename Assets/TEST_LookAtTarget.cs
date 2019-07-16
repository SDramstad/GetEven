using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_LookAtTarget : MonoBehaviour
{
    public void TakeALook(Transform target)
    {
        transform.LookAt(target);
    }
}

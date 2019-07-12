using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_GenericEvent : MonoBehaviour
{
    private bool hasRun = false;

    public virtual void Run()
    {
        hasRun = true;
    }

    public bool HasRun()
    {
        return hasRun;
    }

}

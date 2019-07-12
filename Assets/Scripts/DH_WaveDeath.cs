using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DH_WaveDeath : A_GenericEvent
{
    public override void Run()
    {
        if (GameObject.Find("WaveManager"))
        {
            GameObject.Find("WaveManager").GetComponent<WaveManager>().enemyTakenDown();
        }
        base.Run();
    }
}

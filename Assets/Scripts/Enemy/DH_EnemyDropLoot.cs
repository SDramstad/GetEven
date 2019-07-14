using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DH_EnemyDropLoot : A_GenericEvent
{
    public override void Run()
    {
        if (Random.Range(1,3) == 1)
        {
            Debug.Log("Item spawned.");
            Instantiate(GlobalControl.Instance.RandomLootTable[Random.Range(0, GlobalControl.Instance.RandomLootTable.Count)], gameObject.transform);
        }
        base.Run();
    }
}

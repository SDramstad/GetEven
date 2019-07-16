using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DH_EnemyDropLoot : A_GenericEvent
{
    [SerializeField]
    private float force = 1000f;

    public override void Run()
    {
        if (Random.Range(1,3) == 1)
        {
            //Debug.Log("Item spawned.");
            int randomRoll = Random.Range(0, GlobalControl.Instance.RandomLootTable.Count - 1);
            Debug.Log("Range is 0 to GlobalControl.Instance.RandomLootTable.Count. Our roll is " + randomRoll);
            GameObject item = Instantiate(GlobalControl.Instance.RandomLootTable[randomRoll], gameObject.transform.position, gameObject.transform.rotation);
            //item.GetComponent<Rigidbody>().AddForceAtPosition(force * transform.forward, transform.position);
        }
        base.Run();
    }
}

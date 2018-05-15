using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unarmed : Weapon {


    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (Input.GetMouseButtonDown(0) && (Time.time - lastFireTime) > fireRate)
        {
            lastFireTime = Time.time;
            //Attack();
            Debug.Log("No weapon equipped. This should be displayed to the user.");
        }
    }

}


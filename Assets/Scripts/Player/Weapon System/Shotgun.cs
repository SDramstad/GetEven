using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{

    protected override void Update()
    {
        base.Update();

        if (Input.GetMouseButtonDown(0) && (Time.time - lastFireTime) > fireRate && !GlobalGame.pauseMenuActive)
        {
            lastFireTime = Time.time;
            Attack();
        }
    }
}

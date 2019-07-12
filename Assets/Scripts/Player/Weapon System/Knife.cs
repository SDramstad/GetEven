using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : Weapon {


    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (Input.GetMouseButtonDown(0) && (Time.time - lastFireTime) > fireRate && !GlobalGame.pauseMenuActive)
        {
            lastFireTime = Time.time;
            GetComponent<Animator>().Play("Knife_Attack");
            Attack();
        }
    }

}


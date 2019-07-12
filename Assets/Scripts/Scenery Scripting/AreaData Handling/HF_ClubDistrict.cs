using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HF_ClubDistrict : A_HandleFlags {

    /// <summary>
    /// This is the first Handle Flags implementation.
    /// 
    /// Handle Flags will, on area start, implement all flag changes the player has triggered on the level.
    /// May be a better idea to move this over to integers instead of boolean.
    /// 
    /// </summary>
    /// 

    [SerializeField]
    private GameObject destructableGameObject;

    //private GameObject gameManager;
    
    public override void handleFlags()
    {
        if (GetComponent<Game>().localAreaData.flags[0])
        {
            destructableGameObject.SetActive(false);
        }
    }

    
}

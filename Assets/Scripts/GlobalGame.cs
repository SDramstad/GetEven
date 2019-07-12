using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalGame {
      

    private static int e1m1_powerbox_count;

    public static int e1m1_powerboxes
    {
        get { return e1m1_powerbox_count; }
        set { e1m1_powerbox_count = value; }
    }

    //Determines whether or not the pause menu is active, and if so, should certain controls be affected
    public static bool pauseMenuActive;

    //Determines which entry a character uses when going through a transition.
    public static int entryId;

    
}

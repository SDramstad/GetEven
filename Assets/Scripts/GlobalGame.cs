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

    public static bool pauseMenuActive;
}

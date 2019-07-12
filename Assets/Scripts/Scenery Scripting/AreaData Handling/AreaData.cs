using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaData {

    //local flags for persisting data between scene transitions
    public bool[] flags = new bool[20];

    //area name for save/load game stuff
    public string areaName;

    //area id
    public int areaId;

    public AreaData()
    {
        areaName = "Default";
        areaId = 0;
        for (int i = 0; i < flags.Length; i++)
        {
            flags[i] = false;
        }
    }

    public AreaData(string sceneName, int sceneId)
    {
        areaName = sceneName;
        areaId = sceneId;
    }
}

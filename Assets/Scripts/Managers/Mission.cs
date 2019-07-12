using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for containing data regarding mission variables.
/// </summary>
public class Mission : MonoBehaviour {

    public string Name { get; set; }
    public string ShortDescription { get; set; }
    public string FullDescription { get; set; }
    public int[] missionVars { get; set; } = new int[20];

    public Mission()
    {
        foreach (var item in missionVars)
        {
            missionVars[item] = 0;
        }
        Name = "Test Mission";
        ShortDescription = "This is an example mission.";
        FullDescription = "Go to the SimulWorld and Kill Examples.";
    }

}

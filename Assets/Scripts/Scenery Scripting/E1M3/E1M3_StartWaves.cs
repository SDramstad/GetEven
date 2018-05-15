using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1M3_StartWaves : I_EnterTrigger
{
    private bool hasRun;

    void Start()
    {
        hasRun = false;
    }

    void Update()
    {
        if (checkBounds() && !hasRun)
        {
            GameObject.Find("WaveManager").GetComponent<WaveManager>().SendMessage("triggerStart");
            GameObject.Find("UIManager").GetComponent<UIManager>().toggleScoreboardVisibility();
            hasRun = true;
        }
    }
    

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchFlag : MonoBehaviour {

	public void flagSwitch(int flag)
    {
        int buildIndex = SceneManager.GetActiveScene().buildIndex;
        Game gameManager = GameObject.Find("GameManager").GetComponent<Game>();
        gameManager.localAreaData.flags[flag] = true;
    }
}

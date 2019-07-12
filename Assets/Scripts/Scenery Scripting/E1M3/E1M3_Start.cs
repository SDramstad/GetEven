using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class E1M3_Start : MonoBehaviour {

    [SerializeField]
    private UIManager _ui;
    
    void Awake()
    {
        //sets up the player with maximized stats
        GlobalControl.Instance.savedPlayerData = new PlayerData(true);
    }
	// Use this for initialization
	void Start ()
    {
        GameObject player = GameObject.Find("Player");
        string startText = "SCENE 3\n\nGladiator\n\n" + "Captured by a bad guy and forced to fight in an arena against your will.\n\n" + "Beat a wave in less than 30 seconds to score bonus points.\n\n" + "Make it through Wave 4 to get out of here.\n\n" + "Press E to begin E1M3.";
        _ui.SetFullscreenBlack(startText.ToUpper());

    }
	
}

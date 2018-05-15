using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class E1M0_Start : MonoBehaviour {

    [SerializeField]
    private UIManager _ui;
	// Use this for initialization
	void Start () {
        GameObject player = GameObject.Find("Player");
        player.GetComponent<FirstPersonController>().enabled = false;
        string startText = "SCENE 0\n\nTutorial\n\n" + "IT'S YOUR FIRST DAY AT THE BIG RESISTANCE HQ" + "\n\n" + "DO YOUR BEST NOT TO GET MADE FUN OF" + "\n\n" + "GOOD LUCK\n\nPress E to Begin";
        _ui.SetFullscreenBlack(startText.ToUpper());
        player.GetComponent<FirstPersonController>().enabled = true;

    }
	
}

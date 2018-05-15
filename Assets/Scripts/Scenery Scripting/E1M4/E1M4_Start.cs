using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1M4_Start : MonoBehaviour {

    private UIManager _ui;
	// Use this for initialization
	void Start () {
        _ui = GameObject.Find("UIManager").GetComponent<UIManager>();
        GameObject player = GameObject.Find("Player");
        string startText = "SCENE 4\n\nTechnoir\n\n" + "You've made it home to the Technoir. Chill out and have a nice time." + "Press E to begin E1M3.";
        _ui.SetFullscreenBlack(startText.ToUpper());
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1M4_Start : MonoBehaviour {

    private UIManager _ui;

    public int entryId;
	// Use this for initialization
	void Start () {
        _ui = GameObject.Find("UIManager").GetComponent<UIManager>();
        GameObject player = GameObject.Find("Player");
        string startText = "Section 7\nThe TechNoir\n\n" + "You've made it home to the TechNoir. Chill out and have a nice time." + "Press Interact to continue.";
        _ui.SetFullscreenBlack(startText.ToUpper());
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class E1M1_Start : MonoBehaviour {

    [SerializeField]
    private UIManager _ui;

    public static int e1m1_powerBoxesActivated;

	// Use this for initialization
	void Start () {
        GameObject player = GameObject.Find("Player");
        string startText = "SCENE 1\n\nQuarantined Streets\n\n" + "STUFF GOES BAD AND YOU GET SHOT IN THE HEAD." + "\n\n" + "I will probably put a scene in here later." + "\n\n" + "You have to find your way home to the Technoir and recover while you plan your next moves.\n\nPress E to Begin";
        _ui.SetFullscreenBlack(startText.ToUpper());

    }
	
}

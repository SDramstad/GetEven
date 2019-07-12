using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class E1M0_Start : MonoBehaviour {

    [SerializeField]
    [TextArea(15,20)]
    private string startText;

    private UIManager _ui;

    void Awake()
    {
        GlobalControl.Instance.savedPlayerData = new PlayerData();
    }

    // Use this for initialization
    void Start () {
        GameObject player = GameObject.Find("Player");
        _ui = GameObject.Find("UIManager").GetComponent<UIManager>();

        player.GetComponent<FirstPersonController>().enabled = false;
        startText = "SCENE 0\n\nTutorial\n\n" + "IT'S YOUR FIRST DAY AT THE BIG RESISTANCE HQ" + "\n\n" + "DO YOUR BEST NOT TO GET MADE FUN OF" + "\n\n" + "GOOD LUCK\n\nPress E to Begin";
        _ui.SetFullscreenBlack(startText.ToUpper());
        player.GetComponent<FirstPersonController>().enabled = true;

    }
	
}

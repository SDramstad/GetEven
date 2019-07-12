using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class TestingAreaStart : MonoBehaviour {

    private UIManager _ui;

    [SerializeField]
    [TextArea(15, 20)]
    private string startText;

    GameObject player;
    

    void Awake()
    {
        player = GameObject.Find("Player");

        //set starting equipment to full
        GlobalControl.Instance.savedPlayerData = new PlayerData(true);
    }
    
    // Use this for initialization
    void Start()
    {

        _ui = GameObject.Find("UIManager").GetComponent<UIManager>();

        player.GetComponent<FirstPersonController>().enabled = false;
        _ui.SetFullscreenBlack(startText.ToUpper());
        player.GetComponent<FirstPersonController>().enabled = true;


    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class AreaStart : MonoBehaviour {

    private UIManager _ui;

    [SerializeField]
    [TextArea(15, 20)]
    private string startText;

    [SerializeField]
    private GameObject[] entryPoints;

    [SerializeField]
    private bool DebugModeMaxxedPlayer = false;
    
    void Awake()
    {
        if (DebugModeMaxxedPlayer)
        {
            GlobalControl.Instance.savedPlayerData = new PlayerData(true);
        }
    }
    
    // Use this for initialization
    void Start()
    {
        GameObject player = GameObject.Find("Player");

        //disable RotateView in FPC to allow for setting it here
        //player.GetComponent<FirstPersonController>().ToggleRotateView();
        player.GetComponent<FirstPersonController>().enabled = false;

        //Sets the character's location to be at a certain entry point.
        player.transform.SetPositionAndRotation(entryPoints[GlobalGame.entryId].transform.position, entryPoints[GlobalGame.entryId].transform.rotation);

        //re-enable RotateView in FPC to allow for mouse movement
        //player.GetComponent<FirstPersonController>().ToggleRotateView();
        player.GetComponent<FirstPersonController>().enabled = true;

        _ui = GameObject.Find("UIManager").GetComponent<UIManager>();

        player.GetComponent<FirstPersonController>().enabled = false;
        _ui.SetFullscreenBlack(startText.ToUpper());
        player.GetComponent<FirstPersonController>().enabled = true;


    }
    

    public GameObject[] getEntryPoints()
    {
        return entryPoints;
    }
}

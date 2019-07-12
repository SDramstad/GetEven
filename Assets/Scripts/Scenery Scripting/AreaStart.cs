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
    
    // Use this for initialization
    void Start()
    {
        GameObject player = GameObject.Find("Player");
        //Sets the character's location to be at a certain entry point.
        player.transform.SetPositionAndRotation(entryPoints[GlobalGame.entryId].transform.position, entryPoints[GlobalGame.entryId].transform.rotation);
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

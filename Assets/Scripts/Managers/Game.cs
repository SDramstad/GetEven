using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Game : MonoBehaviour {

    public UIManager uiManager;
    public WeaponSwitcher weaponSwitcher;
    public Player player;

	// Use this for initialization
	void Start () {
		//public GameObject deathPanel = GameObject.Find("DeathPanel").GetComponent<
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GameOver()
    {
        Time.timeScale = 0.0f;
        GameObject.Find("Player").GetComponent<FirstPersonController>().enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        uiManager.DisplayDeathMenu();

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;
using System;

public class Game : MonoBehaviour {

    public UIManager uiManager;
    public WeaponSwitcher weaponSwitcher;
    public Player player;
    public AreaData localAreaData;

    //true to allow for alternate death conditions, such as losing a fight but the story still progressing
    public bool alternateGameOver;

	// Use this for initialization
	void Start () {

        localAreaData = GlobalControl.Instance.savedAreaData[SceneManager.GetActiveScene().buildIndex];
        if (GetComponent<A_HandleFlags>() != null)
        {
            GetComponent<A_HandleFlags>().handleFlags();
        }
	}
	
    public void GameOver()
    {
        Time.timeScale = 0.0f;
        GameObject.Find("Player").GetComponent<FirstPersonController>().enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        uiManager.DisplayDeathMenu();
    }




    internal void SaveArea()
    {
        GlobalControl.Instance.savedAreaData[SceneManager.GetActiveScene().buildIndex] = localAreaData;
    }
}

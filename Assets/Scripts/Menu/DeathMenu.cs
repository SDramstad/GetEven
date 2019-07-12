using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public class DeathMenu : MonoBehaviour {

    public void RetryLevel()
    {
        Time.timeScale = 1f;
        GameObject.Find("Player").GetComponent<Player>().localPlayerData = GlobalControl.Instance.savedPlayerData;
        Scene thisScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(thisScene.buildIndex);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}

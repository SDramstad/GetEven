using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    [SerializeField]
    private GameObject _LorePanel;
    void Start()
    {
        GlobalControl.Instance.savedPlayerData = new PlayerData();
        GlobalControl.Instance.savedAreaData.Clear();
        
    }
    public void StartMainGame()
    {
        SceneManager.LoadScene("E1M1");
    }

    public void ShowLorePanel()
    {
        _LorePanel.SetActive(true);
    }

    public void HideLorePanel()
    {
        _LorePanel.SetActive(false);
    }

    public void StartTutorial()
    {
        SceneManager.LoadScene("E1M0");
    }
        
    public void Exit()
    {
        Application.Quit();
    }
}

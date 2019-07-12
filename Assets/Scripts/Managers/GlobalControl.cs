using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalControl : MonoBehaviour {

    public static GlobalControl Instance;
    
    public PlayerData savedPlayerData = new PlayerData();
    public List<AreaData> savedAreaData = new List<AreaData>();
    public List<Mission> MissionList = new List<Mission>();

    public string[] tips = { "Make sure not to die.", "Try to avoid getting shot.", "Guns require ammofood. Make sure to feed your guns.","Aren't loading screens fun?","Molemen can be killed by a knife without getting hurt. If you're good.","Death is a learning experience. Make sure to die in enlightening ways." };

    void Awake()
    {      
        if (Instance == null)
        {
            BuildAreaDataList();
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void BuildAreaDataList()
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < sceneCount; i++)
        {
            savedAreaData.Add(new AreaData(SceneManager.GetSceneByBuildIndex(i).name, SceneManager.GetSceneByBuildIndex(i).buildIndex));
        }
    }
}

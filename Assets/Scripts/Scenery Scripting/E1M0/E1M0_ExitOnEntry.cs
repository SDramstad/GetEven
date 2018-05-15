using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class E1M0_ExitOnEntry : I_EnterTrigger {

    public string nextMapString;

    bool startedLoading;

    public GameObject player;
	
	// Update is called once per frame
	void Update () {
		if (checkBounds())
        {
            //SceneManager.MoveGameObjectToScene(GameObject.Find("Player"), "E1M1");
            //if (!startedLoading)
            //{
            //    StartCoroutine(LoadScene());
            //}
            SceneManager.LoadScene("E1M1");
        }
	}

    

    //IEnumerator LoadScene()
    //{
    //    startedLoading = true;

    //    Scene currentScene = SceneManager.GetActiveScene();

    //    AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextMapString, LoadSceneMode.Additive);

    //    while (!asyncLoad.isDone)
    //    {
    //        yield return null;
    //    }

    //    SceneManager.MoveGameObjectToScene(player, SceneManager.GetSceneByName(nextMapString));

    //    SceneManager.UnloadSceneAsync(currentScene);
    //}
}

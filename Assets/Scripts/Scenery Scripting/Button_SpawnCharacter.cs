using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_SpawnCharacter : I_EnterTrigger {

    [SerializeField]
    private GameObject spawnLocation;
    [SerializeField]
    private GameObject characterToSpawn;
    [SerializeField]
    private AudioClip buttonSound;

    protected void Update()
    {
        if (base.checkBounds())
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                GetComponent<AudioSource>().PlayOneShot(buttonSound);
                spawnCharacter();
            }
        }

    }

    private void spawnCharacter()
    {
        Instantiate(characterToSpawn, spawnLocation.transform);
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (isInBounds)
    //    {
    //        //otherwise we can take input
    //        if (Input.GetKeyDown(KeyCode.E))
    //        {
    //            Debug.Log("Power box activated.");
    //            //GetComponent<AudioSource>().PlayOneShot(speechNoise);
    //            //_ui.SetConversationText(characterName, characterDialogue);

    //        }
    //    }
    //}
}

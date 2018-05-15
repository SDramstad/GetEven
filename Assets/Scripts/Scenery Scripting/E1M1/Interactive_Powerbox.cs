using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive_PowerBox : I_EnterTrigger {

    void Update()
    {
        if (checkBounds())
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Key press accepted.");
            }
        }

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

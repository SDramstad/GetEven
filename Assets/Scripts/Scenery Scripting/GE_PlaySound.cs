using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GE_PlaySound : A_GenericInteractEvent {

    [SerializeField]
    AudioClip audio;

	public override void Run()
    {
        GetComponent<AudioSource>().PlayOneShot(audio);
    }
}

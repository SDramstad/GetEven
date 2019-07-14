using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Civilian : A_TakesDamage {

    private UIManager _ui;

    [SerializeField]
    private string characterName;
    [SerializeField]
    private string characterDialogue;
    [SerializeField]
    private List<string> dialogueList = new List<string>();
    [SerializeField]
    private string painDialogue;
    [SerializeField]
    private float durationDialogue;
    [SerializeField]
    private float durationPainDialogue;
    [SerializeField]
    private AudioClip speechNoise;
    [SerializeField]
    private AudioClip painSound;

    private bool _isInBounds;

    private int currentDialogueIndex = 0;

    //public ParticleSystem painGFX;


    void Start()
    {
        _ui = GameObject.Find("UIManager").GetComponent<UIManager>();
        dialogueList.Insert(0, characterDialogue);

    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Player")
        {            
            _isInBounds = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            
            _isInBounds = false;
        }
    }

    // Update is called once per frame
    protected void Update () {
        if (_isInBounds)
        {
            //otherwise we can take input
            if (Input.GetButtonDown("Interact"))
            {
                //Debug.Log("Conversation starts here.");
                GetComponent<AudioSource>().PlayOneShot(speechNoise);
                Talk();

            }
        }
    }

    private void Talk()
    {
        if (dialogueList.Count == 1)
        {
            _ui.SetConversationText(characterName, characterDialogue, durationDialogue);
        }
        else
        {            
            
            if (currentDialogueIndex > dialogueList.Count-1)
            {
                currentDialogueIndex = 0;
                _ui.SetConversationText(characterName, dialogueList[currentDialogueIndex], durationDialogue);
            }
            else
            {
                _ui.SetConversationText(characterName, dialogueList[currentDialogueIndex], durationDialogue);
                currentDialogueIndex++;
            }
        }
    }

    //public void PlayDialogue(string header, string dialogue)
    //{
    //    GetComponent<AudioSource>().PlayOneShot(speechNoise);
    //    _ui.SetConversationText(header, dialogue);
    //}

    //public void PlayDialogue(string header, string dialogue, AudioClip audio)
    //{
    //    GetComponent<AudioSource>().PlayOneShot(audio);
    //    _ui.SetConversationText(header, dialogue);
    //}

    public override void TakeDamage(int damage = 5)
    {
        GetComponent<AudioSource>().PlayOneShot(painSound);
        _ui.SetConversationText(characterName, painDialogue);
    }

}

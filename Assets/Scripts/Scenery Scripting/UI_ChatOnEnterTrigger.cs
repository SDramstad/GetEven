using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ChatOnEnterTrigger : I_EnterTrigger {

    public UIManager _ui;

    [SerializeField]
    private string _name;
    [SerializeField]
    private string _chattext;
    [SerializeField]
    private AudioClip _uiNoise;

    private bool playOnce;
		
    void Start()
    {
        playOnce = false;
        _ui = GameObject.Find("UIManager").GetComponent<UIManager>();
    }
	// Update is called once per frame
	void Update () {
		
        if (checkBounds() && !playOnce)
        {
            GetComponent<AudioSource>().PlayOneShot(_uiNoise);
            _ui.SetConversationText(_name, _chattext);            
            playOnce = true;
            StartCoroutine("DeathTimer");
        }
	}

    IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}

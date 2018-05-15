using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class UIManager : MonoBehaviour {

    [SerializeField]
    private Image _dialoguePanel;
    [SerializeField]
    private Text _dialogueNameText;
    [SerializeField]
    private Text _dialogueText;
    [SerializeField]
    private Image _pdaPanel;
    [SerializeField]
    private Text _pdaHeader;
    [SerializeField]
    private Text _pdaText;
    [SerializeField]
    private Image _fullScreen;
    [SerializeField]
    private Text _fullScreenText;
    [SerializeField]
    private Text _promptText;
    [SerializeField]
    private Text _healthText;
    [SerializeField]
    private GameObject _damageScreen;
    [SerializeField]
    private GameObject _deathPanel;
    [SerializeField]
    private Image _crosshair;

    private GameObject player;

    private GameObject _wavePanel;
    private Text _waveCounter;
    private Text _enemiesLeft;
    private Text _score;
    private Text _bonusTimer;

    //Pause Menu
    private GameObject _pauseMenu;

    //crosshair info
    [SerializeField]
    private Sprite crosshair_target;
    [SerializeField]
    private Sprite crosshair_notarget;



    // Use this for initialization
    void Start () {
        _dialoguePanel.GetComponent<Image>().enabled = false;
        _pdaPanel.GetComponent<Image>().enabled = false;
        _pauseMenu = GameObject.Find("PauseMenuPanel");

        player = GameObject.Find("Player");

        //must go after player is declared
        togglePauseMenuVisibility();

        if (GameObject.Find("WavePanel"))
        {
            _wavePanel = GameObject.Find("WavePanel");
            _waveCounter = GameObject.Find("txt_waveCounter").GetComponent<Text>();
            _enemiesLeft = GameObject.Find("txt_enemiesLeft").GetComponent<Text>();
            _score = GameObject.Find("txt_score").GetComponent<Text>();
            _bonusTimer = GameObject.Find("txt_bonustimer").GetComponent<Text>();
            toggleScoreboardVisibility();
        }
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            togglePauseMenuVisibility();
        }
    }

    public void CrosshairTargetInSight(bool targetPresent)
    {
        if (targetPresent)
        {
            _crosshair.sprite = crosshair_target;
        }
        else
        {
            _crosshair.sprite = crosshair_notarget;
        }

    }

    public void SetConversationText(string charName, string charDialogue, float duration)
    {

        _dialoguePanel.GetComponent<Image>().enabled = false;
        _dialogueNameText.GetComponent<Text>().enabled = false;
        _dialogueText.GetComponent<Text>().enabled = false;

        //make sure they're enabled
        _dialoguePanel.GetComponent<Image>().enabled = true;
        _dialogueNameText.GetComponent<Text>().enabled = true;
        _dialogueText.GetComponent<Text>().enabled = true;

        //display the dialogue and name
        _dialogueNameText.text = charName.ToUpper();
        _dialogueText.text = charDialogue.ToUpper();

        if (duration == 0)
        {
            duration = 5f;
        }

        //begin countdown to hide
        StopCoroutine("HidePickUpText");
        StopCoroutine("HideConversation");
        StartCoroutine("HideConversation");            
    }

    public void SetConversationText(string charName, string charDialogue)
    {
        SetConversationText(charName, charDialogue, 5f);
    }

    public void SetPickUpText(string displayText)
    {
        _dialoguePanel.GetComponent<Image>().enabled = true;
        _dialogueText.GetComponent<Text>().enabled = true;
        _dialogueNameText.GetComponent<Text>().enabled = true;

        _dialogueNameText.text = "";
        _dialogueText.text = displayText.ToUpper();

        StopCoroutine("HidePickUpText");
        StopCoroutine("HideConversation");
        StartCoroutine("HidePickUpText");
    }

    IEnumerator HidePickUpText()
    {
        yield return new WaitForSeconds(5f);
        _dialoguePanel.GetComponent<Image>().enabled = false;
        _dialogueNameText.GetComponent<Text>().enabled = false;
        _dialogueText.GetComponent<Text>().enabled = false;
    }

    public void SetPromptText(string text)
    {
        _promptText.color = Color.white;
        _promptText.text = text.ToUpper();
    }

    public void HidePromptText()
    {
        _promptText.color = Color.clear;
    }



    IEnumerator HideConversation()
    {
        yield return new WaitForSeconds(5f);
        _dialoguePanel.GetComponent<Image>().enabled = false;
        _dialogueNameText.GetComponent<Text>().enabled = false;
        _dialogueText.GetComponent<Text>().enabled = false;
    }

    public void SetPDAText(string charName, string charDialogue, float duration)
    {


        //make sure they're enabled
        _pdaPanel.GetComponent<Image>().enabled = true;
        _pdaHeader.GetComponent<Text>().enabled = true;
        _pdaText.GetComponent<Text>().enabled = true;

        //display the dialogue and name
        _pdaHeader.text = charName;
        _pdaText.text = charDialogue;

        if (duration == 0)
        {
            duration = 5f;
        }
        //Unity requires extra work to get a duration to go for this. I might want to switch over to instantiating a new UI element each time, but that might be awful.

        //begin countdown to hide
        StopCoroutine("HidePDA");
        StartCoroutine("HidePDA");
    }

    public void SetPDAText(string charName, string charDialogue)
    {
        SetPDAText(charName, charDialogue, 5f);
    }



    IEnumerator HidePDA()
    {
        yield return new WaitForSeconds(5f);
        _pdaPanel.GetComponent<Image>().enabled = false;
        _pdaHeader.GetComponent<Text>().enabled = false;
        _pdaText.GetComponent<Text>().enabled = false;
    }

    public void SetFullscreenBlack(string text)
    {
        Time.timeScale = 0;
        _fullScreen.GetComponent<Image>().enabled = true;   
        _fullScreen.GetComponent<Image>().color = Color.black;
        _fullScreenText.color = Color.white;
        _fullScreenText.text = text;
        //player.GetComponent<FirstPersonController>().enabled = false;

        StartCoroutine("HideFullscreen");
    }

    IEnumerator HideFullscreen()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
        _fullScreen.GetComponent<Image>().color = Color.clear;
        _fullScreenText.color = Color.clear;
        _fullScreenText.text = "";
        _fullScreen.GetComponent<Image>().enabled = false;
        Time.timeScale = 1;
        //player.GetComponent<FirstPersonController>().enabled = true;
    }

    public void DamageFlash()
    {
        //_fullScreen.GetComponent<Image>().enabled = true;
        //var tempColor = _fullScreen.color;
        //tempColor = new Color(233, 0, 0);
        //tempColor.a = 25f;
        //_fullScreen.color = tempColor;
        ////_fullScreen.color = new Color(233, 0, 0, 25);
        //StartCoroutine("HideDamageFlash");
        

        //StartCoroutine(HideDamageFlash());
    }

    IEnumerator HideDamageFlash()
    {
        //yield return new WaitForSeconds(.1f);
        //var tempColor = _fullScreen.color;
        //tempColor = new Color(0, 0, 0);
        //tempColor.a = 0f;
        //_fullScreen.color = tempColor;
        //_fullScreen.GetComponent<Image>().enabled = false;
        
        for (int i = 0; i < 2; i++)
        {
            yield return new WaitForSeconds(.1f);
            _damageScreen.SetActive(true);

            yield return new WaitForSeconds(.1f);
            _damageScreen.SetActive(false);
        }
    }


    public void SetHealth(int health)
    {
        _healthText.text = health.ToString();
    }

    internal void DisplayDeathMenu()
    {
        _deathPanel.SetActive(true);
    }

    public void SetWaveCounter(int wave) 
    {
        _waveCounter.text = wave.ToString();
    }

    public void SetEnemiesLeft(int enemiesLeft)
    {
        _enemiesLeft.text = enemiesLeft.ToString();
    }

    public void SetScore(int score)
    {
        _score.text = score.ToString();
    }

    public void SetBonusTimer(int timer)
    {
        _bonusTimer.text = timer.ToString();
    }

    public void toggleScoreboardVisibility()
    {
        if (_wavePanel.activeSelf)
        {
            _wavePanel.SetActive(false);
        }
        else
        {
            _wavePanel.SetActive(true);
        }
    }

    public void togglePauseMenuVisibility()
    {
        if (_pauseMenu.activeSelf)
        {
            //hide the pause menu
            GlobalGame.pauseMenuActive = false;
            Time.timeScale = 1.0f;

            player.GetComponent<FirstPersonController>().m_MouseLook.lockCursor = true;
            player.GetComponent<FirstPersonController>().enabled = true;
            player.GetComponent<CharacterController>().enabled = true;
            Cursor.visible = false;
            //Cursor.lockState = CursorLockMode.None;
            Cursor.lockState = CursorLockMode.Confined;
            
            _pauseMenu.SetActive(false);
        }
        else
        {
            //show the pause menu
            Time.timeScale = 0.0f;


            GlobalGame.pauseMenuActive = true;

            player.GetComponent<FirstPersonController>().enabled = false;
            player.GetComponent<CharacterController>().enabled = false;
            player.GetComponent<FirstPersonController>().m_MouseLook.lockCursor = false;
            Cursor.visible = true;
            //Cursor.lockState = CursorLockMode.None;
            Cursor.lockState = CursorLockMode.None;

            _pauseMenu.SetActive(true);
        }
    }

}

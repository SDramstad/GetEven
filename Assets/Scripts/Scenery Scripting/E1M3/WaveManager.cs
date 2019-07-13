using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveManager : MonoBehaviour {

    private int _currentWave;
    private int _currentWaveTimer;
    private bool _waveActive;

    private UIManager _uiManager;

    public int score;

    public int numberOfEnemies;

    private const int TIME_TO_COMPLETE_WAVE_AND_STILL_GET__BONUS = 50;
    private const int REST_TIME_BETWEEN_WAVES = 10;

    [SerializeField]
    private AudioClip _cheeringSound;

    [SerializeField]
    private GameObject _wave1;
    [SerializeField]
    private GameObject _wave2;
    [SerializeField]
    private GameObject _wave3;
    [SerializeField]
    private GameObject _wave4;

    [SerializeField]
    private GameObject[] _itemSpawnPoints;


    [SerializeField]
    private GameObject[] _itemsToSpawn;

    /// <summary>
    /// HOW TO USE:
    /// Enemies should have an OnDeath script fire when they die, if a condition is true. (Likely, gameobject.find to look for a death handler script)
    /// Then they should send a message to it to reduce number of living enemies.
    /// 
    /// This should allow the rest of the script to run correctly. 
    /// </summary>


    // Use this for initialization
    void Start () {
        _currentWave = 0;
        _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        _uiManager.SetScore(0);
        _uiManager.SetWaveCounter(0);
        _uiManager.SetEnemiesLeft(0);        
        _uiManager.SetBonusTimer(0);
        spawnPickUps();
    }

    private void endOfWave()
    {
        GetComponent<AudioSource>().PlayOneShot(_cheeringSound);
        if (_currentWave >= 4)
        {
            _uiManager.SetConversationText("Spicy Announcer", "Wow! I'm so mad we just lost our best pilot! ALRIGHT, I GUESS'LL WE'LL JUST RELEASE EVERYTHING WE HAVE! Wait, what do you mean we have nothing left?");        
       
        }
        else
        {
            _uiManager.SetConversationText("Spicy Announcer", "Hey, our hero's not dead! Congrats! Get ready for another round!");        
        }
        Debug.Log("Current time: " + _currentWaveTimer);

        if (_currentWave > 0)
        {
            handleWaveRewards();
        }

        spawnPickUps();

        Invoke("nextWave", REST_TIME_BETWEEN_WAVES);
    }

    private void spawnPickUps()
    {        
        foreach (var spawnPoint in _itemSpawnPoints)
        {
            GameObject pickup = Instantiate(_itemsToSpawn[UnityEngine.Random.Range(0, _itemsToSpawn.Length)]);
            pickup.transform.position = spawnPoint.transform.position;
            pickup.transform.parent = spawnPoint.transform.parent;
        }

    }

    //this should be on an invoke in REST_TIME_BETWEEN_WAVES seconds
    private void nextWave()
    {
        Debug.Log("Next wave");
        _currentWave++;
        _uiManager.SetWaveCounter(_currentWave);

        //shouldn't run if you're past wave 4
        if(!(_currentWave >= 5))
        {
            _currentWaveTimer = TIME_TO_COMPLETE_WAVE_AND_STILL_GET__BONUS;
            Invoke("Timer", 1f);
            spawnWave();
            _uiManager.SetEnemiesLeft(numberOfEnemies);
        }
        else
        {
            handleEndOfWaves();
        }
    }

    private void handleWaveRewards()
    {

        Debug.Log("Handle Wave Rewards");
        if (_currentWaveTimer > 0)
        {
            score += (_currentWaveTimer * 10);
            _currentWaveTimer = 0;
        }

        score += _currentWave * 75;

        _uiManager.SetScore(score);
    }

    private void spawnWave()
    {

        Debug.Log("Spawn wave");
        switch (_currentWave)
        {
            case 1:
                _uiManager.SetConversationText("Spicy Announcer", "Here we go, it's Wave 1! We hope you won't dissapoint the darling citizen of Section 7!");
                numberOfEnemies = 5;
                _wave1.SetActive(true);
                break;
            case 2:
                _uiManager.SetConversationText("Spicy Announcer", "Wave 2, get ready for the Molemen-Robot Tagteam!");
                numberOfEnemies = 12;
                _wave2.SetActive(true);
                break;
            case 3:
                _uiManager.SetConversationText("Spicy Announcer", "You know it, you love it, it's the MOLEMAN WAVE");
                numberOfEnemies = 15;
                _wave3.SetActive(true);
                break;
            case 4:
                _uiManager.SetConversationText("Spicy Announcer", "My friend, it's been fun, but it's time for our ace pilot to mow you down with his COMBAT CHOPPER!");
                numberOfEnemies = 1;
                _wave4.SetActive(true);
                break;
            default:
                break;
        }
    }


    public void enemyTakenDown()
    {

        Debug.Log("Enemy Taken Down");
        numberOfEnemies--;
        _uiManager.SetEnemiesLeft(numberOfEnemies);
        if (numberOfEnemies == 0 )
        {
            _waveActive = false;
            endOfWave();
        }
    }

    void Timer()
    {        
        if (_currentWaveTimer > 0)
        {
            //Debug.Log("Timer decrementing: " + _currentWaveTimer);
            _currentWaveTimer -= 1;
            _uiManager.SetBonusTimer(_currentWaveTimer);
        }
        else
        {
            _uiManager.SetBonusTimer(0);
        }

        Invoke("Timer", 1f);
    }

    public void triggerStart()
    {

        Debug.Log("triggered Start");
        nextWave();
    }

    private void handleEndOfWaves()
    {
        _uiManager.toggleScoreboardVisibility();
        _uiManager.SetPDAText("Spicy Announcer", "I can't believe it! The madman did it! Won with " + score + " points!");
        StartCoroutine("EndDemo");
    }

    IEnumerator EndDemo()
    {
        yield return new WaitForSeconds(5f);
        string endText = "End of demo\n\nPress E to return to the main menu.";
        _uiManager.SetFullscreenBlack(endText.ToUpper());
        StartCoroutine("ReturnToMainMenu");
    }

    IEnumerator ReturnToMainMenu()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("MainMenu");
    }
}

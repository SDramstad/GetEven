using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ALLY_TestC : BASE_EnemySoldier

{
    protected SpriteRenderer _spriteRenderer;

    [SerializeField]
    protected Sprite _idleSprite;
    [SerializeField]
    protected Sprite _moveSprite1;
    [SerializeField]
    protected Sprite _moveSprite2;
    [SerializeField]
    protected Sprite _fireSprite1;
    [SerializeField]
    protected Sprite _fireSprite2;
    [SerializeField]
    protected Sprite _dieSprite1;
    [SerializeField]
    protected Sprite _dieSprite2;
    [SerializeField]
    protected Sprite _painSprite;

    //handles sprite swapping for movement
    protected const float TIME_TO_DISSAPEAR = 30f;
    protected float time_to_swap_sprite = 1f;
    protected float _lastSwitched;
    protected bool isMoveSprite1 = true;
    protected bool isAttackAnim = false;

    //The current goal of the ally.
    private enum Goal
    {
        SearchForThreats,
        FollowPlayer,
        Idle
    }

    private Goal goal;
    private GameObject player;
    private UIManager uiManager;

    protected float baseSpeed;
    protected float baseAccel;

    private bool IsHired = false;
    private bool _isInBounds = false;

    // DIALOGUE
    [SerializeField]
    private AudioClip[] banterList = { };

    [SerializeField]
    private AudioClip joinDialogue;
    [SerializeField]
    private AudioClip leaveDialogue;
    [SerializeField]
    private AudioClip meleeDialogue;

    private float nextTimeToBanter = 0f;

    // MELEE STUFF
    //self defense in close quarters
    private float _timeLastMeleed;
    [SerializeField]
    private float _meleeFireRate;
    [SerializeField]
    private int _meleeDamage;
    [SerializeField]
    private AudioClip meleeSFX;
    [SerializeField]
    private GameObject meleeGFX;

    protected override void Start()
    {
        base.Start();
        time_to_swap_sprite = UnityEngine.Random.Range(0.15f, 0.2f);
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _lastSwitched = 0f;
        goal = Goal.Idle;
        player = GameObject.Find("Player");
        baseSpeed = agent.speed;
        baseAccel = agent.acceleration;
        nextTimeToBanter += UnityEngine.Random.Range(5, 20);
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
    }

    protected override void Update()
    {
        base.Update();

        OnUpdatePlayerFollowRoutine();
        InTriggerChatRange();

        //if moving, check to swap side of sprite
        if (_state == SoldierState.MovingTo || _state == SoldierState.Repositioning)
        {
            if (Time.time - _lastSwitched > time_to_swap_sprite)
            {
                _lastSwitched = Time.time;
                if (isMoveSprite1)
                {
                    _spriteRenderer.sprite = _moveSprite2;
                    isMoveSprite1 = false;
                }
                else
                {
                    _spriteRenderer.sprite = _moveSprite1;
                    isMoveSprite1 = true;
                }
            }
        }
    }

    private void InTriggerChatRange()
    {
        if (_isInBounds)
        {
            //otherwise we can take input
            if (Input.GetButtonDown("Interact"))
            {
                if (IsHired)
                {
                    FireAlly();
                }
                else
                {
                    HireAlly();
                }

            }
        }
    }

    private void HireAlly()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(joinDialogue);
        }
        uiManager.SetConversationText("Freedom Fighter", "He says something about joining you.");
        IsHired = true;

    }

    private void FireAlly()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(leaveDialogue);
        }
        uiManager.SetConversationText("Freedom Fighter", "He grumbles something about staying here and waiting.");
        IsHired = false;
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

    private void OnUpdatePlayerFollowRoutine()
    {
        if (goal == Goal.FollowPlayer)
        {
            //overrides base orders if the goal is to follow the player
            float distance = Vector3.Distance(transform.position, player.transform.position);

            //if too close, stop moving
            if (distance < 4.5f)
            {
                _state = SoldierState.Idle;
                agent.isStopped = true;
            }
            else
            {
                if (distance > 25f)
                {
                    //if too far, speed up to hang with your buddy
                    agent.speed = baseSpeed * 2;
                    agent.acceleration = baseAccel * 2;
                }
                else
                {
                    agent.speed = baseSpeed;
                    agent.acceleration = baseAccel;
                }
                //otherwise, 
                _state = SoldierState.MovingTo;
                agent.isStopped = false;
                agent.SetDestination(player.transform.position);
            }
        }
    }

    protected override void RecheckState()
    {
        //die
        if (_hp <= 0)
        {
            _state = SoldierState.Death;
            return;
        }

        CheckGoal();

        //only bother to do this when Aware
        if (_awareness == AlertState.Aware)
        {
            if (goal == Goal.SearchForThreats)
            {
                //generic AI set up
                //CONSIDER: should check if flinching?
                if (_flinchTime > 0)
                {
                    _state = SoldierState.Pain;
                    return;
                }

                if (!_state.Equals(SoldierState.Repositioning))
                {
                    hasRepositionGoal = false;
                }

                //if within firing range
                float distance = Vector3.Distance(target.transform.position, gameObject.transform.position);

                //if not within firing range,
                //check if in personal space bubble of target 
                if (distance <= TOO_CLOSE_DISTANCE)
                {
                    _state = SoldierState.Repositioning;
                }
                else if (distance < _fireRange)
                {
                    if (CheckClearLineOfFire())
                    {
                        _state = SoldierState.Firing;
                    }
                    else
                    {
                        _state = SoldierState.Repositioning;
                    }
                }
                else if (distance > TOO_CLOSE_DISTANCE)
                {
                    _state = SoldierState.MovingTo;
                }

            }
            else if (goal == Goal.Idle)
            {
                _state = SoldierState.Idle;
            }

        }
    }



    protected override void DoIdle()
    {
        base.DoIdle();
        BanterCheck();
        agent.isStopped = true;
        _spriteRenderer.sprite = _idleSprite;
    }

    private void BanterCheck()
    {
        if (Time.time >= nextTimeToBanter)
        {
            nextTimeToBanter = Time.time + UnityEngine.Random.Range(5, 20);
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(banterList[UnityEngine.Random.Range(0, banterList.Length)]);
            }
        }
    }

    protected override void DoFire()
    {
        //
        if (isAttackAnim)
        {
            _spriteRenderer.sprite = _fireSprite2;
        }
        else
        {
            _spriteRenderer.sprite = _fireSprite1;
        }

        base.DoFire();
    }

    private void CheckGoal()
    {
        if (IsHired)
        {
            float playerDistance = Vector3.Distance(transform.position, player.transform.position);
            float targetDistance;
            try
            {
                targetDistance = Vector3.Distance(transform.position, target.transform.position);

            }
            catch (NullReferenceException)
            {
                //if no targets on map, assume they're miles away
                targetDistance = 5000f;
                throw;
            }

            //if player too far away, go for them
            if (playerDistance > 25)
            {
                goal = Goal.FollowPlayer;
            }
            // else, if enemies within short range, fight them
            else if (targetDistance < 25)
            {
                goal = Goal.SearchForThreats;
            }
            // else, just follow player
            else if (playerDistance > 3)
            {
                goal = Goal.FollowPlayer;
            }
            else
            {
                goal = Goal.Idle;
            }
        }
        else
        {
            goal = Goal.Idle;
        }

    }

    protected IEnumerator ResetAttackAnim()
    {
        isAttackAnim = true;
        yield return new WaitForSeconds(0.5f);
        isAttackAnim = false;
    }

    protected override void DoPain()
    {
        CheckFlinchStatus();
        _spriteRenderer.sprite = _painSprite;
    }

    protected override IEnumerator Die()
    {
        isDying = true;

        //disable collider 
        GetComponent<CapsuleCollider>().enabled = false;
        audioSource.PlayOneShot(_deathSound);


        //animator.Play("Die");
        _spriteRenderer.sprite = _dieSprite1;

        //stop moving
        agent.isStopped = true;

        //add a degree of randomization to body piles
        if (UnityEngine.Random.Range(1, 3) == 2)
        {
            _spriteRenderer.flipX = true;
        }

        yield return new WaitForSeconds(TIME_TO_DIE);
        animator.enabled = false;
        _spriteRenderer.sprite = _dieSprite2;

        yield return new WaitForSeconds(TIME_TO_DISSAPEAR);
        isDying = false;
        Destroy(gameObject);
    }
}


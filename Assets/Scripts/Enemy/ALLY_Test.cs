﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ALLY_Test : BASE_EnemySoldier
{

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
                    if (Time.time - _timeLastMeleed < _meleeFireRate)
                    {
                        DoMelee();
                    }
                    else
                    {
                        _state = SoldierState.Repositioning;
                    }
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

    private void DoMelee()
    {
        Debug.Log("SLAPPA YO FACE");
        //set time last fired to now
        _timeLastMeleed = Time.time;

        //raycasting for determining the line to shoot
        Vector3 direction = target.transform.position - transform.position;
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hit;

        Debug.DrawRay(transform.position, direction, Color.red);

        if (Physics.Raycast(ray, out hit, _fireRange))
        {
            _weaponExitPoint.transform.LookAt(hit.transform);
            GameObject _tempParticleSystem = Instantiate(meleeGFX, _weaponExitPoint.transform.position, _weaponExitPoint.transform.rotation);
            Destroy(_tempParticleSystem, 3f);

            processHit(hit.collider.gameObject, hit);

            GetComponent<AudioSource>().PlayOneShot(_fireSound);
        }
    }
    private void processHit(GameObject hitObject, RaycastHit hit)
    {
        if (hitObject.GetComponent<A_TakesDamage>() != null)
        {
            hitObject.GetComponent<A_TakesDamage>().TakeDamage(_meleeDamage);
            ParticleSystem _tempParticleSystem = Instantiate(hitObject.GetComponent<A_TakesDamage>().painGFX, hit.point, Quaternion.identity);
            Destroy(_tempParticleSystem, 2f);
        }
    }

    protected override void DoIdle()
    {
        base.DoIdle();
        BanterCheck();
        agent.isStopped = true;
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
}

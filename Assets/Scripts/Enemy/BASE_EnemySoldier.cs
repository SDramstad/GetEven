using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// The base class for State security.
/// </summary>
public class BASE_EnemySoldier : A_TakesDamage
{

    [SerializeField]
    protected int _maxHp;
    protected int _hp;

    [SerializeField]
    protected int _sightRange;
    [SerializeField]
    protected int _fireRange;
    [SerializeField]
    protected float _fireRate;


    protected float _flinchTime;
    protected float _fireCooldown;
    protected float _timeLastFired;
    [SerializeField]
    protected float _flinchTimeModifier;

    protected const float RANGE_TO_ALERT = 20f;
    protected const float FLINCH_TIME = .4f;
    protected const float TOO_CLOSE_DISTANCE = 5f;
    protected const float REPOSITION_DISTANCE = 15f;
    protected const float TIME_TO_DIE = .9f;

    protected bool isDying;
    protected bool isRepositioning;
    protected bool isFiring;

    protected GameObject target;
    protected bool hasRepositionGoal;
    protected NavMeshHit _repositionGoal;
    protected NavMeshAgent agent;
    protected AudioSource audioSource;
    public Animator animator;

    //inspector prefabs
    [SerializeField]
    protected GameObject _weaponExitPoint;
    [SerializeField]
    protected GameObject _projectile;
    [SerializeField]
    protected GameObject _muzzleFlashEffect;
    [SerializeField]
    protected AudioClip _fireSound;
    [SerializeField]
    protected AudioClip _painSound;
    [SerializeField]
    protected AudioClip _deathSound;

    protected enum SoldierState
    {
        Idle,
        Pain,
        MovingTo,
        Repositioning,
        Firing,
        Death
    }

    protected enum AlertState
    {
        Unaware,
        Aware,
        Panic
    }

    [SerializeField]
    protected SoldierState _state;
    [SerializeField]
    protected AlertState _awareness;

    public override void TakeDamage(int damage)
    {
        //shout for help
        Alert();

        //take damage
        _hp -= damage;

        if (_hp <= 0)
        {
            _state = SoldierState.Death;
        }
        else
        {
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(_painSound);
            }
            //set up rules for how much damage to cause pain, how long stun state should be
            _flinchTime = (FLINCH_TIME + (float)(damage * _flinchTimeModifier));
            Debug.Log("Adding flinctime " + _flinchTime);
        }

    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _hp = _maxHp;
        _flinchTimeModifier = 0.1f;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        //_awareness = AlertState.Unaware;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //figure out who to target
        SelectTarget();

        //Debug.Log("AWARENESS: " + _awareness);

        //actions chosen based on the current state
        switch (_awareness)
        {
            case AlertState.Unaware:

                //run detection stuff if target is within a certain radius
                DetectionCheck();

                //do idle stuff
                DoIdle();


                break;
            case AlertState.Aware:
                //Debug.Log("I'm " + _awareness + " and " + _state + "!");

                //figure out which state is appropriate
                RecheckState();

                //perform actions
                switch (_state)
                {
                    case SoldierState.Idle:
                        DoIdle();
                        break;
                    case SoldierState.Pain:
                        DoPain();
                        break;
                    case SoldierState.MovingTo:
                        DoMove();
                        break;
                    case SoldierState.Repositioning:
                        DoReposition();
                        break;
                    case SoldierState.Firing:
                        DoFire();
                        break;
                    case SoldierState.Death:
                        DoDeath();
                        break;
                    default:
                        break;
                }
                break;
            case AlertState.Panic:
                Debug.Log("ERROR: Soldier's Alert state should not be in PANIC, as it is not implemented.");
                break;
            default:
                break;
        }
        
    }

    protected void DetectionCheck()
    {
        //if within sight range
        if (Vector3.Distance(transform.position, target.transform.position) < _sightRange)
        {
            //Debug.Log("In range.");            
            //if clear line of sight
            if (CheckClearLineOfSight())
            {
                //Debug.Log("In Line of sight.");
                Alert();
            }
        }
    }

    protected void CheckFlinchStatus()
    {
        if (_flinchTime > 0)
        {
            //reduces flinch time 
            _flinchTime -= Time.deltaTime;
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;

        }
    }

    /// <summary>
    /// Check what state we should be in right now.
    /// </summary>
    protected virtual void RecheckState()
    {
        if (_hp < 0)
        {
            _state = SoldierState.Death;
            return;
        }

        //only bother to do this when Aware
        if (_awareness == AlertState.Aware)
        {
            //generic AI set up
            //CONSIDER: should check if flinching?
            if (_flinchTime > 0)
            {
                _state = SoldierState.Pain;
                return;

            }

            //RESET REPO GOAL if not currently in REPO state
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
                //Debug.Log("Too close.");
                _state = SoldierState.Repositioning;
            }
            else if (distance < _fireRange)
            {
                //Debug.Log("Within fire range.");
                if (CheckClearLineOfFire())
                {
                    _state = SoldierState.Firing;
                }
                else
                {
                    //Debug.Log("REPOSITION.");
                    _state = SoldierState.Repositioning;
                }
            }
            else if (distance > TOO_CLOSE_DISTANCE)
            {
                _state = SoldierState.MovingTo;
            }

            if (agent.isOnOffMeshLink)
            {
                //use jump sprite
                //Debug.Log("Is jumping.");
            }
            


        }
        else
        {
            Debug.Log("RecheckState was called from an Unaware Soldier. This should not happen.");
        }
    }

    //Check there's actually a line of fire
    protected bool CheckClearLineOfFire()
    {
        bool isClear = false;
        RaycastHit hit;
        
        //Transform of target + 1 Y

        Debug.DrawLine((target.transform.position + new Vector3(0, .5f)), _weaponExitPoint.transform.position);

        if (Physics.Raycast(transform.position, ((target.transform.position + new Vector3(0, .5f)) - transform.position), out hit, _fireRange)) {            
            if (hit.transform.name == target.name)
            {
                isClear = true;
            }
        }

        return isClear;
    }

    //Check there's actually a line of sight
    protected bool CheckClearLineOfSight()
    {
        bool isClear = false;
        RaycastHit hit;
        Debug.DrawLine(target.transform.position, _weaponExitPoint.transform.position);
        
        if (Physics.Raycast(transform.position, (target.transform.position - transform.position), out hit, _sightRange))
        {
            if (hit.transform.name == target.name)
            {
                isClear = true;
            }
        }

        return isClear;
    }

    /// <summary>
    /// Selects the target that affects the rest of the character's actions. 
    /// TODO: Actually allow for infighting, faction conflict, so on.
    /// </summary>
    protected void SelectTarget()
    {
        target = GameObject.Find("Player");
    }

    protected virtual void DoIdle()
    {

        //animator.Play("Idle");
        //play idle animation   
        //play idle sounds     
    }

    protected virtual void DoPain()
    {
        CheckFlinchStatus();
    }

    protected virtual void DoMove()
    {
        //transform.LookAt(target.transform);
        agent.isStopped = false;
        //animator.Play("Running");
        agent.SetDestination(target.transform.position);
    }

    protected virtual void DoReposition()
    {
        agent.isStopped = false;

        //if no reposition target is readied, ready one
        if (!hasRepositionGoal)
        {
            //Debug.Log("Getting REPO goal.");
            //distance to run
            int distance = 6;

            //pick random direction
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;
            randomDirection += transform.position;

            NavMeshHit navHit;

            NavMesh.SamplePosition(randomDirection, out navHit, distance, -1);

            _repositionGoal = navHit;

            hasRepositionGoal = true;
        }
        //if at goal, clear goal
        if (agent.remainingDistance < 2)
        {
            //Debug.Log("At REPO goal.");
            hasRepositionGoal = false;
        }


        //Debug.Log("Enroute to REPO goal.");
        agent.SetDestination(_repositionGoal.position);

        ////repositition till done
        //if (!isRepositioning)
        //{
        //    StartCoroutine(Reposition());
        //}
    }

    protected IEnumerator Reposition()
    {
        isRepositioning = true;


        //animator.Play("Running");

        //distance to run
        int distance = 5;

        //pick random direction
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;
        randomDirection += transform.position;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, -1);

        //run towards randomized position
        agent.SetDestination(navHit.position);

        //run until you arrive
        //EDIT don't do that if you change state for any reason itll be stuck
        yield return new WaitForSeconds(2f);        
        isRepositioning = false;
    }

    protected virtual void DoFire()
    {
        //check if can fire again
        if ((Time.time - _timeLastFired) > _fireRate)
        {
            //can fire

            //animator

            //animator.Play("Fire");

            //set time last fired to now
            _timeLastFired = Time.time;

            //raycasting for determining the line to shoot
            Vector3 direction = target.transform.position - transform.position;
            Ray ray = new Ray(transform.position, direction);
            RaycastHit hit;

            Debug.DrawRay(transform.position, direction, Color.red);

            if (Physics.Raycast(ray, out hit, _fireRange))
            {
                _weaponExitPoint.transform.LookAt(hit.transform);
                Instantiate(_projectile, _weaponExitPoint.transform.position, _weaponExitPoint.transform.rotation);

                GameObject _tempParticleSystem = Instantiate(_muzzleFlashEffect, _weaponExitPoint.transform.position, _weaponExitPoint.transform.rotation);
                Destroy(_tempParticleSystem, 3f);
                //soldierAnimations.Play("Fire");
                audioSource.PlayOneShot(_fireSound);
            }
        }
        else
        {
            //wait
        }
    }
   
    protected virtual void DoDeath()
    {
        //run if not dying yet
        if (!isDying)
        {
            StartCoroutine(Die());
        }

        if (GetComponent<A_GenericEvent>() != null)
        {
            if (!GetComponent<A_GenericEvent>().HasRun())
            {
                GetComponent<A_GenericEvent>().Run();
            }
        }
    }

    protected virtual IEnumerator Die()
    {
        isDying = true;

        //disable collider 
        GetComponent<CapsuleCollider>().enabled = false;
        audioSource.PlayOneShot(_deathSound);


        animator.Play("Die");

        //stop moving
        agent.isStopped = true;


        Debug.Log(gameObject.name + " : I'M DYING!!!!");
        yield return new WaitForSeconds(TIME_TO_DIE);
        isDying = false;
        Destroy(gameObject);
    }

    // Calls out to all nearby friends and gets them to attack the player.
    // Additionally, sets the enemy as able to attack the player.
    public void Alert()
    {
        Collider[] inRadiusList = Physics.OverlapSphere(transform.position, RANGE_TO_ALERT);

        foreach (var gameObject in inRadiusList)
        {
            if (gameObject.GetComponent<BASE_EnemySoldier>() != null)
            {
                if (!gameObject.GetComponent<BASE_EnemySoldier>().isAlerted())
                {
                    gameObject.GetComponent<BASE_EnemySoldier>().alertSelf();
                }
            }
            else if(gameObject.GetComponent<CO_SpawnCloset>() != null)
            {
                gameObject.GetComponent<CO_SpawnCloset>().StartCloset();
            }
        }

        _awareness = AlertState.Aware;
    }

    public bool isAlerted()
    {
        bool returnValue;
        if (_awareness == AlertState.Aware)
        {
            returnValue = true;
        }
        else
        {
            returnValue = false;
        }
        return returnValue;
    }
    public void alertSelf()
    {
        _awareness = AlertState.Aware;
    }

}

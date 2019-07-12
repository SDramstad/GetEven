using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// The class for Moleman melee units.
/// </summary>
public class MOL_ManAnim : BASE_EnemySoldier
{
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private int _meleeMinDamage;
    [SerializeField]
    private int _meleeMaxDamage;
    [SerializeField]
    private Sprite _idleSprite;
    [SerializeField]
    private Sprite _moveSprite1;
    [SerializeField]
    private Sprite _moveSprite2;
    [SerializeField]
    private Sprite _fireSprite1;
    [SerializeField]
    private Sprite _fireSprite2;
    [SerializeField]
    private Sprite _dieSprite1;
    [SerializeField]
    private Sprite _dieSprite2;
    [SerializeField]
    private Sprite _painSprite;

    private const float TIME_TO_DISSAPEAR = 30f;
    //handles sprite swapping for movement
    private float time_to_swap_sprite = 1f;
    private float _lastSwitched;
    private bool isMoveSprite1 = true;
    private bool isAttackAnim = false;

    protected override void Start()
    {
        base.Start();
        _flinchTimeModifier = 0f;
        agent.speed = UnityEngine.Random.Range(6.5f, 8.5f);
        time_to_swap_sprite = UnityEngine.Random.Range(0.6f, 1.3f);
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _lastSwitched = 0f;
    }


    protected override void Update()
    {
        base.Update();
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
  

    /// <summary>
    /// Check what state we should be in right now.
    /// </summary>
    protected override void RecheckState()
    {
        //Debug.Log("I am " + _state + " and " + _awareness);
        if (_hp < 0)
        {
            _state = SoldierState.Death;
            return;
        }

        //only bother to do this when Aware
        if (_awareness == AlertState.Aware)
        {
            //generic AI set up

           // Debug.Log(_flinchTime);
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
            if (distance < _fireRange)
            {
                //Debug.Log("Within fire range.");
                if (CheckClearLineOfFire())
                {
                    //Debug.Log("FIRE.");
                    _state = SoldierState.Firing;
                    return;
                }
                else
                {
                    //Debug.Log("REPOSITION.");
                    _state = SoldierState.Repositioning;
                    return;
                }
            }
            else
            {
                _state = SoldierState.MovingTo;
                return;
            }

        }
        else
        {
            Debug.Log("RecheckState was called from an Unaware Soldier. This should not happen.");
        }
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


        Debug.Log(gameObject.name + " : I'M DYING!!!!");

        //add a degree of randomization to body piles
        if (UnityEngine.Random.Range(1, 3) == 2)
        {
            Debug.Log("Flipping");
            _spriteRenderer.flipX = true;
        }

        yield return new WaitForSeconds(TIME_TO_DIE);
        animator.enabled = false;
        _spriteRenderer.sprite = _dieSprite2;

        yield return new WaitForSeconds(TIME_TO_DISSAPEAR);
        isDying = false;
        Destroy(gameObject);
    }

    protected override void DoIdle()
    {
        base.DoIdle();
        _spriteRenderer.sprite = _idleSprite;
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

        //check if can fire again
        if ((Time.time - _timeLastFired) > _fireRate)
        {
            //can fire
            StartCoroutine("ResetAttackAnim");


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
                GameObject _tempParticleSystem = Instantiate(_muzzleFlashEffect, _weaponExitPoint.transform.position, _weaponExitPoint.transform.rotation);
                Destroy(_tempParticleSystem, 3f);

                processHit(hit.collider.gameObject, hit);

                GetComponent<AudioSource>().PlayOneShot(_fireSound);
            }
        }
        else
        {
            //wait
        }
    }

    private IEnumerator ResetAttackAnim()
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

    private void processHit(GameObject hitObject, RaycastHit hit)
    {
        if (hitObject.GetComponent<A_TakesDamage>() != null)
        {
            int damage = UnityEngine.Random.Range(_meleeMinDamage, _meleeMaxDamage);
            hitObject.GetComponent<A_TakesDamage>().TakeDamage(damage);
            ParticleSystem _tempParticleSystem = Instantiate(hitObject.GetComponent<A_TakesDamage>().painGFX, hit.point, Quaternion.identity);
            Destroy(_tempParticleSystem, 2f);
        }
    }
}

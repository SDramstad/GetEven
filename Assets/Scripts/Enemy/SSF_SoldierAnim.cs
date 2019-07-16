using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSF_SoldierAnim : BASE_EnemySoldier
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
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        time_to_swap_sprite = UnityEngine.Random.Range(0.15f, 0.2f);
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

    protected override void RecheckState()
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




        }
        else
        {
            Debug.Log("RecheckState was called from an Unaware Soldier. This should not happen.");
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

    protected override void DoIdle()
    {
        base.DoIdle();
        _spriteRenderer.sprite = _idleSprite;
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

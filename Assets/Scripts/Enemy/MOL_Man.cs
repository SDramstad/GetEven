using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// The class for Moleman melee units.
/// </summary>
public class MOL_Man : BASE_EnemySoldier
{
    [SerializeField]
    private int _meleeMinDamage;
    [SerializeField]
    private int _meleeMaxDamage;

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
            //set up rules for how much damage to cause pain, how long stun state should be
            _flinchTime += (FLINCH_TIME + (float)(damage * 0.015));
        }
    }    

    /// <summary>
    /// Check what state we should be in right now.
    /// </summary>
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

            //if within firing range
            float distance = Vector3.Distance(target.transform.position, gameObject.transform.position);
            //if not within firing range,
            //check if in personal space bubble of target 
            if (distance < _fireRange)
            {
                //Debug.Log("Within fire range.");
                if (CheckClearLineOfFire())
                {
                    Debug.Log("FIRE.");
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

    protected override void DoDeath()
    {
        base.DoDeath();
    }





    protected override void DoFire()
    {
        //check if can fire again
        if ((Time.time - _timeLastFired) > _fireRate)
        {
            //can fire
            
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

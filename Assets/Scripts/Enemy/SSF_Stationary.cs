using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Pistoleer class of State Security Forces. Shouldn't actually be any different, but any future differences should be overriden here.
/// </summary>
public class SSF_Stationary : BASE_EnemySoldier
{
    //[SerializeField]
    //private GameObject laserSight;
    [SerializeField]
    private GameObject lastKnownPositionOfTarget;

    protected override void Start()
    {
        base.Start();
    }
    


    protected override void Update()
    {
        base.Update();
        
        //LASER SIGHTS TEST
        //laserSight.SetActive(true);
        //laserSight.GetComponent<LineRenderer>().SetPosition(0, _weaponExitPoint.transform.position);
        //laserSight.GetComponent<LineRenderer>().SetPosition(1, (target.transform.position + new Vector3(0,.5f)));
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
            
            //if within firing range
            float distance = Vector3.Distance(target.transform.position, gameObject.transform.position);
            //if not within firing range,
            if (distance < _fireRange)
            {
                //Debug.Log("Within fire range.");
                if (CheckClearLineOfFire())
                {
                    Debug.Log(Time.time + " CAN FIRE");
                    _state = SoldierState.Firing;
                }
                else
                {
                    Debug.Log(Time.time + " CANNOT FIRE");
                    _state = SoldierState.Idle;
                }
                
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

    protected override void DoMove()
    {
        //base.DoMove();
        //Debug.Log("Would be moving.");
    }

    protected override void DoReposition()
    {
        //base.DoReposition();
        //Debug.Log("Would be moving.");
    }

}

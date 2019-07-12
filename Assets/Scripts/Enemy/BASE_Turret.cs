using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BASE_Turret : A_TakesDamage
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

    protected float _fireCooldown;
    protected float _timeLastFired;

    protected const float RANGE_TO_ALERT = 50f;
    protected const float TIME_TO_DIE = .9f;

    protected GameObject target;
    protected bool isDying;

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
    [SerializeField]
    protected AudioClip _alertSound;

    protected AudioSource audioSource;

    [SerializeField]
    private GameObject turretBody;

    protected enum TurretState
    {
        Idle,
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
    protected TurretState _state;
    [SerializeField]
    protected AlertState _awareness;

    // Start is called before the first frame update
    void Start()
    {
        _hp = _maxHp;
        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        SelectTarget();
        DecideActions();

    }
    public override void TakeDamage(int damage)
    {
        //shout for help
        Alert();

        //take damage
        _hp -= damage;

        if (_hp <= 0)
        {
            _state = TurretState.Death;
        }
        else
        {
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(_painSound);
            }
        }

    }

    private void SelectTarget()
    {
        target = GameObject.Find("Player");
    }

    private void DecideActions()
    {
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
                //figure out which state is appropriate
                RecheckState();

                //is turret, rotate head towards target
                Vector3 targetPosition = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
                turretBody.transform.LookAt(targetPosition);

                //perform actions
                switch (_state)
                {
                    case TurretState.Idle:
                        DoIdle();
                        break;
                    case TurretState.Firing:
                        DoFire();
                        break;
                    case TurretState.Death:
                        DoDeath();
                        break;
                    default:
                        break;
                }
                break;
            case AlertState.Panic:
                Debug.Log("ERROR: Turret's Alert state should not be in PANIC, as it is not implemented.");
                break;
            default:
                break;
        }
    }

    protected virtual void RecheckState()
    {
        if (_hp < 0)
        {
            _state = TurretState.Death;
            return;
        }

        //only bother to do this when Aware
        if (_awareness == AlertState.Aware)
        {
            //generic AI set up

            //if within firing range
            float distance = Vector3.Distance(target.transform.position, gameObject.transform.position);

            if (distance < _fireRange)
            {
                if (CheckClearLineOfFire())
                {
                    _state = TurretState.Firing;
                }
                else
                {
                    _state = TurretState.Idle;
                }
            }
            else
            {
                _state = TurretState.Idle;
            }

        }
        else
        {
            Debug.Log("RecheckState was called from an Unaware Soldier. This should not happen.");
        }
    }

    private void DetectionCheck()
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

    private bool CheckClearLineOfSight()
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

    private bool CheckClearLineOfFire()
    {
        bool isClear = false;
        RaycastHit hit;

        //Transform of target + 1 Y

        Debug.DrawLine((target.transform.position + new Vector3(0, .5f)), _weaponExitPoint.transform.position);

        if (Physics.Raycast(transform.position, ((target.transform.position + new Vector3(0, .5f)) - transform.position), out hit, _fireRange))
        {
            if (hit.transform.name == target.name)
            {
                isClear = true;
            }
        }

        return isClear;
    }

    private void DoIdle()
    {
        //nothing here
    }

    private void DoFire()
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
            //Vector3 randomizedDirection = new Vector3(direction.x + Random.Range(-1,1), direction.y + Random.Range(-1, 1), direction.z);
            Ray ray = new Ray(transform.position, direction);
            RaycastHit hit;

            Debug.DrawRay(transform.position, direction, Color.red);

            if (Physics.Raycast(ray, out hit, _fireRange))
            {
                _weaponExitPoint.transform.LookAt(hit.transform);
                Instantiate(_projectile, _weaponExitPoint.transform.position, _weaponExitPoint.transform.rotation);

                GameObject _tempParticleSystem = Instantiate(_muzzleFlashEffect, _weaponExitPoint.transform.position, _weaponExitPoint.transform.rotation);
                Destroy(_tempParticleSystem, 3f);
                audioSource.PlayOneShot(_fireSound);
            }
        }
        else
        {
            //wait
        }
    }

    private void DoDeath()
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
        GetComponent<BoxCollider>().enabled = false;
        audioSource.PlayOneShot(_deathSound);        

        Debug.Log(gameObject.name + " : I'M DYING!!!!");
        yield return new WaitForSeconds(TIME_TO_DIE);
        isDying = false;
        Destroy(gameObject);
    }

    private void Alert()
    {
        _awareness = AlertState.Aware;
        //play alert sound
    }
}

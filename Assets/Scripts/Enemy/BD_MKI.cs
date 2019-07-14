using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// 1/10/2019
/// 
/// BattleDroid MK I
/// 
/// The class for Battledroid units. Also a test for the animation system.
/// 
/// Should be stronger healthwise, and less capable of stunning.
/// 
/// </summary>
public class BD_MKI : BASE_EnemySoldier
{
    private SpriteRenderer _spriteRenderer;
   // private Sprite _spriteRenderer;

    [SerializeField]
    private Sprite _idleSprite;
    [SerializeField]
    private Sprite _moveSprite1;
    [SerializeField]
    private Sprite _moveSprite2;
    [SerializeField]
    private Sprite _fireSprite;
    [SerializeField]
    private Sprite _dieSprite;
    [SerializeField]
    private Sprite _painSprite;
    [SerializeField]
    private GameObject _deathGFX;

    [SerializeField]
    private AudioClip[] banterList = { };
    [SerializeField]
    private AudioClip[] attackChatterList = { };

    private float nextTimeToBanter = 0f;


    private bool isMoveSprite1 = true;
    private bool isRunningSwapSprite = false;
    private float _lastSwitched;
    private const float TIME_TO_SWAP_SPRITE = 1f;

    protected override void Start()
    {
        base.Start();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _lastSwitched = 0f;
    }

    protected override void Update()
    {
        base.Update();
        //if moving, check to swap side of sprite
        if (_state == SoldierState.MovingTo || _state == SoldierState.Repositioning)
        {
            if (Time.time - _lastSwitched > TIME_TO_SWAP_SPRITE)
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

    protected override void DoIdle()
    {
        //ResetAnimation();
        base.DoIdle();
        BanterCheck();
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
        agent.isStopped = true;
        //ResetAnimation();
        base.DoFire();
        CombatChatterCheck();
        _spriteRenderer.sprite = _fireSprite;

    }

    private void CombatChatterCheck()
    {
        if (Time.time >= nextTimeToBanter)
        {
            nextTimeToBanter = Time.time + UnityEngine.Random.Range(5, 20);
            audioSource.PlayOneShot(attackChatterList[UnityEngine.Random.Range(0, attackChatterList.Length)]);
        }
    }

    protected override void DoMove()
    {
        base.DoMove();
        //InvokeRepeating("SwapAnim", .4F, .4F);
    }

    protected override void DoPain()
    {
        //ResetAnimation();
        base.DoPain();
        _spriteRenderer.sprite = _painSprite;
    }

    protected override void DoDeath()
    {
        //ResetAnimation();
        GameObject _tempGFX = Instantiate(_deathGFX, transform.position, transform.rotation);
        Destroy(_tempGFX, 3f);
        _spriteRenderer.sprite = _dieSprite;

        

        base.DoDeath();
    }

    protected override void DoReposition()
    {
        base.DoReposition();
        //InvokeRepeating("SwapAnim", .4F, .4F);
    }

    //private void SwapAnim()
    //{
    //    Debug.Log("SwapAnim is running! isMoveSprite1?" + isMoveSprite1);
    //    if (isMoveSprite1)
    //    {
    //        _spriteRenderer.sprite = _moveSprite2;
    //        isMoveSprite1 = false;
    //    }
    //    else
    //    {
    //        _spriteRenderer.sprite = _moveSprite1;
    //        isMoveSprite1 = true;
    //    }
    

    ///// <summary>
    ///// Needs to be called for every DoState 
    ///// </summary>
    //private void ResetAnimation()
    //{
    //    CancelInvoke();
    //}


}

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
        _spriteRenderer.sprite = _idleSprite;
    }

    protected override void DoFire()
    {
        agent.isStopped = true;
        //ResetAnimation();
        base.DoFire();
        _spriteRenderer.sprite = _fireSprite;

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

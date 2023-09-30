using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 電腦操控角色動畫管理
/// </summary>
public class SubCharacterState : ScriptableObject, IState
{
    float stateStartTime;

    protected float currentSpeedx;
    protected float currentSpeedy;

    protected PlayerInput playerInput;
    protected PlayerCharacterStats characterStats;
    protected PlayerCharacterSwitch characterSwitch;
    protected Animator animator;
    protected SubCharacterController subCharacterController;
    protected SubCharacterStateMachine stateMachine;
    protected SubCharacterSwitch subCharacterSwitch;


    /// <summary>
    /// 動畫結束標示
    /// </summary>
    protected bool IsAnimationFinished => StateDuration >= animator.GetCurrentAnimatorStateInfo(0).length;
    /// <summary>
    /// 狀態持續時間
    /// </summary>
    protected float StateDuration => Time.time - stateStartTime;
    /// <summary>
    /// 當前狀態時間
    /// </summary>
    protected float CurrentStateTime => animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

    private void OnDestroy()
    {
        this.characterStats.hpZeroEvent -= PartnerHPZeroEvent;
        this.characterSwitch.DownSwitchEnd -= DownEventEnd;
    }
    public void Initialize(PlayerInput playerInput,PlayerCharacterStats characterStats,PlayerCharacterSwitch characterSwitch,SubCharacterController subCharacterController, Animator animator, SubCharacterStateMachine stateMachine , SubCharacterSwitch subCharacterSwitch)
    {
        this.playerInput = playerInput;
        this.characterStats = characterStats;
        this.characterSwitch = characterSwitch;
        this.subCharacterController = subCharacterController;
        this.animator = animator;
        this.stateMachine = stateMachine;
        this.subCharacterSwitch = subCharacterSwitch;

        this.characterStats.hpZeroEvent += PartnerHPZeroEvent;
        this.characterSwitch.DownSwitchEnd += DownEventEnd;
        this.characterSwitch.SwitchGameOverEvent += GameOverEvent;
    }
    /// <summary>
    /// 切換角色同時切換Animator
    /// </summary>
    public virtual void ChangeAnimator(Animator animator)
    {
        this.animator = animator;
    }
    public virtual void Enter()
    {
        stateStartTime = Time.time;
    }

    public virtual void Exit()
    {

    }

    public virtual void LogicUpdate()
    {
        
    }

    public virtual void PhysicUpdate()
    {
        
    }
    /// <summary>
    /// 戰鬥模式我方角色HP歸零事件
    /// </summary>
    public void PartnerHPZeroEvent()
    {
        stateMachine.SwitchState(typeof(SubCharacterState_PartnerDown));
    }
    public void DownEventEnd()
    {
        stateMachine.SwitchState(typeof(SubCharacterState_Idle));
    }
    public void GameOverEvent()
    {
        stateMachine.SwitchState(typeof(SubCharacterState_Down));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �q���ޱ�����ʵe�޲z
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
    /// �ʵe�����Х�
    /// </summary>
    protected bool IsAnimationFinished => StateDuration >= animator.GetCurrentAnimatorStateInfo(0).length;
    /// <summary>
    /// ���A����ɶ�
    /// </summary>
    protected float StateDuration => Time.time - stateStartTime;
    /// <summary>
    /// ��e���A�ɶ�
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
    /// ��������P�ɤ���Animator
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
    /// �԰��Ҧ��ڤ訤��HP�k�s�ƥ�
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

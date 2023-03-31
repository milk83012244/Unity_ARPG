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

    public void Initialize(PlayerInput playerInput,SubCharacterController subCharacterController, Animator animator, SubCharacterStateMachine stateMachine , SubCharacterSwitch subCharacterSwitch)
    {
        this.playerInput = playerInput;
        this.subCharacterController = subCharacterController;
        this.animator = animator;
        this.stateMachine = stateMachine;
        this.subCharacterSwitch = subCharacterSwitch;
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
}

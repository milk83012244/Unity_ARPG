using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家狀態數值
/// </summary>
public class PlayerState : ScriptableObject, IState
{
    //[SerializeField] string stateName;
    //[SerializeField, Range(0f, 1f)] float transitionDuration = 0.1f; 

    //int stateHash;

    float stateStartTime;

    protected float currentSpeedx;
    protected float currentSpeedy;

    protected Animator animator;

    protected PlayerController player;

    protected PlayerInput input;

    protected PlayerStateMachine stateMachine;
    /// <summary>
    /// 動畫結束標示
    /// </summary>
    protected bool IsAnimationFinished => StateDuration >= animator.GetCurrentAnimatorStateInfo(0).length;
    /// <summary>
    /// 狀態持續時間
    /// </summary>
    protected float StateDuration => Time.time - stateStartTime;

    private void OnEnable()
    {
        //stateHash = Animator.StringToHash(stateName);
    }

    public void Initialize(PlayerController player, Animator animator,PlayerStateMachine stateMachine , PlayerInput input)
    {
        this.player = player;
        this.animator = animator;
        this.stateMachine = stateMachine;
        this.input = input;
    }
    public virtual void Enter()
    {
        //animator.CrossFade(stateName, transitionDuration);
        stateStartTime = Time.time;
    }

    public virtual void Exit()
    {
        
    }
    /// <summary>
    /// 切換角色同時切換Animator
    /// </summary>
    public virtual void ChangeAnimator(Animator animator)
    {
        this.animator = animator;
    }

    public virtual void LogicUpdate()
    {
       
    }

    public virtual void PhysicUpdate()
    {
       
    }
}

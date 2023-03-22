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
    protected bool  isInitial = true;
    protected float currentSpeedx;
    protected float currentSpeedy;

    protected Animator animator;

    protected PlayerController player;
    protected PlayerCharacterSwitch playerCharacterSwitch;

    protected PlayerInput input;

    protected PlayerStateMachine stateMachine;

    /// <summary>
    /// 當前遊戲狀態
    /// </summary>
    protected int CurrentGameState => (int)GameManager.GetInstance().CurrentGameState;
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
    private void OnEnable()
    {
        isInitial = true;
        //stateHash = Animator.StringToHash(stateName);
    }

    public void Initialize(PlayerController player,PlayerCharacterSwitch playerCharacterSwitch, Animator animator,PlayerStateMachine stateMachine , PlayerInput input)
    {
        this.player = player;
        this.playerCharacterSwitch = playerCharacterSwitch;
        this.animator = animator;
        this.stateMachine = stateMachine;
        this.input = input;
    }
    public virtual void Enter()
    {
        //animator.CrossFade(stateName, transitionDuration);
        stateStartTime = Time.time; //動畫持續時間開始計時
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

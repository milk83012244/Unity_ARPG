using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���a���A�ƭ�
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
    /// ��e�C�����A
    /// </summary>
    protected int CurrentGameState => (int)GameManager.GetInstance().CurrentGameState;
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
        stateStartTime = Time.time; //�ʵe����ɶ��}�l�p��
    }

    public virtual void Exit()
    {
        
    }
    /// <summary>
    /// ��������P�ɤ���Animator
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

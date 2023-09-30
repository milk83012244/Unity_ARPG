using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// 玩家狀態機
/// 負責:玩家移動,撥放動畫與傳遞事件,改變標示,不傳遞數值
/// </summary>
public class PlayerState : SerializedScriptableObject, IState
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
    protected PlayerCooldownController playerCooldownController;
    protected PlayerEffectSpawner playerEffectSpawner;

    protected PlayerStateMachine stateMachine;
    protected PlayerCharacterStats characterStats;

    /// <summary>
    /// 當前遊戲狀態
    /// </summary>
    protected int CurrentGameState => (int)GameManager.Instance.CurrentGameState;
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

    private void OnDestroy()
    {
        this.characterStats.hpZeroEvent -= HPZeroState;
    }
    public void Initialize(PlayerController player,PlayerCharacterSwitch playerCharacterSwitch, Animator animator,PlayerStateMachine stateMachine , PlayerInput input,PlayerCooldownController playerCooldownController, PlayerEffectSpawner playerEffectSpawner, PlayerCharacterStats characterStats)
    {
        this.player = player;
        this.playerCharacterSwitch = playerCharacterSwitch;
        this.animator = animator;
        this.stateMachine = stateMachine;
        this.input = input;
        this.playerCooldownController = playerCooldownController;
        this.playerEffectSpawner = playerEffectSpawner;
        this.characterStats = characterStats;

        this.characterStats.hpZeroEvent += HPZeroState;
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
        //animator.CrossFade(stateName, transitionDuration);
        stateStartTime = Time.time; //動畫持續時間開始計時

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

    public virtual void UseDamage()
    {

    }
    /// <summary>
    /// 共用使用技能的狀態判斷
    /// </summary>
    public virtual void UseSkill()
    {
        //技能有瞄準功能
        if (GameManager.Instance.CurrentGameState == GameState.Battle && playerCharacterSwitch.GetSkillManager().skills[0].hasFiring)
        {
            if (input.PressingSkill1)
            {
                stateMachine.SwitchState(typeof(PlayerState_Firing));
            }
            else if (input.PressSkill1Release)
            {
                stateMachine.SwitchState(typeof(PlayerState_Skill1));
            }
        }
        else
        {
            if (input.PressSkill1)
            {
                stateMachine.SwitchState(typeof(PlayerState_Skill1));
            }
        }
        if (GameManager.Instance.CurrentGameState == GameState.Battle && playerCharacterSwitch.GetSkillManager().skills[1].hasFiring)
        {
            if (input.PressingSkill2)
            {
                stateMachine.SwitchState(typeof(PlayerState_Firing));
            }
            else if (input.PressSkill2Release)
            {
                stateMachine.SwitchState(typeof(PlayerState_Skill2));
            }
        }
        else
        {
            if (input.PressSkill2)
            {
                stateMachine.SwitchState(typeof(PlayerState_Skill2));
            }
        }
        if (input.PressUSkill && characterStats.CurrnetUSkillValue==100)
        {
            stateMachine.SwitchState(typeof(PlayerState_USkill));
        }
    }
    /// <summary>
    /// 狀態進行時角色切換開關
    /// </summary>
    public virtual void SwitchCharacterState(bool canSwitch)
    {
        playerCharacterSwitch.characterSwitchButtons.characterSwitchSlotCanUseForStateAction.Invoke(canSwitch);
    }
    /// <summary>
    /// 共用輸入功能按鈕
    /// </summary>
    public virtual void InputFunctionButtons()
    {
        switch (playerCharacterSwitch.currentControlCharacterNamesSB.ToString())
        {
            case "Lia":
                if (input.SwitchFunctionkey1 && LiaElementSwitch.canSwitch)
                {
                    stateMachine.SwitchState(typeof(PlayerState_Function));
                }
                else if (input.SwitchFunctionkey2 && LiaElementSwitch.canSwitch)
                {
                    stateMachine.SwitchState(typeof(PlayerState_Function));
                }
                else if (input.SwitchFunctionkey3 && LiaElementSwitch.canSwitch)
                {
                    stateMachine.SwitchState(typeof(PlayerState_Function));
                }
                else if (input.SwitchFunctionkey4 && LiaElementSwitch.canSwitch)
                {
                    stateMachine.SwitchState(typeof(PlayerState_Function));
                }
                break;
        }
        //if (input.SwitchFunctionkey1)
        //{
        //    stateMachine.SwitchState(typeof(PlayerState_Function));
        //}
        //else if (input.SwitchFunctionkey2)
        //{
        //    stateMachine.SwitchState(typeof(PlayerState_Function));
        //}
        //else if (input.SwitchFunctionkey3)
        //{
        //    stateMachine.SwitchState(typeof(PlayerState_Function));
        //}
        //else if (input.SwitchFunctionkey4)
        //{
        //    stateMachine.SwitchState(typeof(PlayerState_Function));
        //}
    }
    /// <summary>
    /// HP為0狀態
    /// </summary>
    public virtual void HPZeroState()
    {
        stateMachine.SwitchState(typeof(PlayerState_Down));
    }
}

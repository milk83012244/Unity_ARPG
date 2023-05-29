using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家狀態機 管理玩家的狀態
/// </summary>
public class PlayerStateMachine : StateMachine
{
   [SerializeField] private PlayerState[] states;

    PlayerController player;
    PlayerCharacterSwitch characterSwitch;
    PlayerInput input;
    PlayerCooldownController playerCooldownController;
    PlayerEffectSpawner playerEffectSpawner;
    PlayerCharacterStats characterStats;

    Animator animator;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();

        player = GetComponent<PlayerController>();
        characterSwitch = GetComponent<PlayerCharacterSwitch>();
        input = GetComponent<PlayerInput>();
        playerCooldownController = GetComponent<PlayerCooldownController>();
        playerEffectSpawner = GetComponent<PlayerEffectSpawner>();
        characterStats = GetComponent<PlayerCharacterStats>();

        stateDic = new Dictionary<System.Type, IState>(states.Length);

        foreach (PlayerState state in states)
        {
            state.Initialize(player, characterSwitch, animator, this , input, playerCooldownController, playerEffectSpawner, characterStats);
            stateDic.Add(state.GetType(), state);
        }

    }
    private void Start()
    {
        SwitchOn(stateDic[typeof(PlayerState_Idle)]);
    }
    public void ReIbitialize()
    {
        animator = GetComponentInChildren<Animator>();
        stateDic.Clear();
        foreach (PlayerState state in states)
        {
            state.Initialize(player, characterSwitch, animator, this, input, playerCooldownController,playerEffectSpawner, characterStats);
            stateDic.Add(state.GetType(), state);
        }
    }
}

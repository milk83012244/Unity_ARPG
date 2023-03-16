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

    PlayerInput input;

    Animator animator;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();

        player = GetComponent<PlayerController>();
        input = GetComponent<PlayerInput>();

        stateDic = new Dictionary<System.Type, IState>(states.Length);

        foreach (PlayerState state in states)
        {
            state.Initialize(player,animator, this , input);
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
            state.Initialize(player, animator, this, input);
            stateDic.Add(state.GetType(), state);
        }
    }
}

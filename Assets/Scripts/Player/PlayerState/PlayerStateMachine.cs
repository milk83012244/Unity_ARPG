using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���a���A�� �޲z���a�����A
/// </summary>
public class PlayerStateMachine : StateMachine
{
   [SerializeField] private PlayerState[] states;

    PlayerController player;

    PlayerCharacterSwitch characterSwitch;

    PlayerInput input;

    Animator animator;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();

        player = GetComponent<PlayerController>();
        characterSwitch = GetComponent<PlayerCharacterSwitch>();
        input = GetComponent<PlayerInput>();

        stateDic = new Dictionary<System.Type, IState>(states.Length);

        foreach (PlayerState state in states)
        {
            state.Initialize(player, characterSwitch, animator, this , input);
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
            state.Initialize(player, characterSwitch, animator, this, input);
            stateDic.Add(state.GetType(), state);
        }
    }
}

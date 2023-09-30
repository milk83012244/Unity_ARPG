using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubCharacterStateMachine : StateMachine
{
    [SerializeField] private SubCharacterState[] states;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerCharacterStats characterStats;
    [SerializeField] private PlayerCharacterSwitch characterSwitch;

    SubCharacterController subCharacterController;
    SubCharacterSwitch subCharacterSwitch;

    Animator animator;

    private void OnDestroy()
    {
        
    }
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();

        subCharacterController = GetComponent<SubCharacterController>();
        subCharacterSwitch = GetComponent<SubCharacterSwitch>();

        stateDic = new Dictionary<System.Type, IState>(states.Length);

        foreach (SubCharacterState state in states)
        {
            state.Initialize(playerInput, characterStats, characterSwitch, subCharacterController, animator, this, subCharacterSwitch);
            stateDic.Add(state.GetType(), state);
        }
    }
    private void Start()
    {
        SwitchOn(stateDic[typeof(SubCharacterState_Idle)]);
    }
    public void ReIbitialize()
    {
        animator = GetComponentInChildren<Animator>();
        stateDic.Clear();
        foreach (SubCharacterState state in states)
        {
            state.Initialize(playerInput, characterStats, characterSwitch,subCharacterController, animator, this, subCharacterSwitch);
            stateDic.Add(state.GetType(), state);
        }
    }
}

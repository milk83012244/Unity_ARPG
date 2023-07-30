using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// ���A���޲z
/// 1.�޲z�P�������������A��
/// 2.�t�d���A��s
/// </summary>
public class StateMachine : SerializedMonoBehaviour
{
    /// <summary>
    /// ��e���A
    /// </summary>
    [SerializeField] IState currentState;
    /// <summary>
    /// ���A�r��
    /// </summary>
    protected Dictionary<System.Type, IState> stateDic;

    private void Update()
    {
        currentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        currentState.PhysicUpdate();
    }
    /// <summary>
    /// �i�J�s�����A
    /// </summary>
    protected void SwitchOn(IState newState)
    {
        currentState = newState;
        newState.Enter();
    }
    /// <summary>
    /// ������s�����A
    /// </summary>
    public void SwitchState(IState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        SwitchOn(newState);
    }
    /// <summary>
    /// ������s�����A(�ǤJ���A��Type)
    /// </summary>
    public void SwitchState(System.Type newStateType)
    {
        SwitchState(stateDic[newStateType]);
    }
}

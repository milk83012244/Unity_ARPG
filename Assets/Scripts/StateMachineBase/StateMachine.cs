using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// 狀態機管理
/// 1.管理與切換持有的狀態類
/// 2.負責狀態更新
/// </summary>
public class StateMachine : SerializedMonoBehaviour
{
    /// <summary>
    /// 當前狀態
    /// </summary>
    [SerializeField] IState currentState;
    /// <summary>
    /// 狀態字典
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
    /// 進入新的狀態
    /// </summary>
    protected void SwitchOn(IState newState)
    {
        currentState = newState;
        newState.Enter();
    }
    /// <summary>
    /// 切換到新的狀態
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
    /// 切換到新的狀態(傳入狀態的Type)
    /// </summary>
    public void SwitchState(System.Type newStateType)
    {
        SwitchState(stateDic[newStateType]);
    }
}

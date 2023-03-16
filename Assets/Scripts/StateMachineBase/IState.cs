using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 狀態介面
/// </summary>
public interface IState 
{
    /// <summary>
    /// 進入狀態
    /// </summary>
    void Enter();
    /// <summary>
    /// 離開狀態
    /// </summary>
    void Exit();
    /// <summary>
    /// 一般用Update
    /// </summary>
    void LogicUpdate();
    /// <summary>
    /// 物理用Update
    /// </summary>
    void PhysicUpdate();
}

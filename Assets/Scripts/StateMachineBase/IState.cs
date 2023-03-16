using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���A����
/// </summary>
public interface IState 
{
    /// <summary>
    /// �i�J���A
    /// </summary>
    void Enter();
    /// <summary>
    /// ���}���A
    /// </summary>
    void Exit();
    /// <summary>
    /// �@���Update
    /// </summary>
    void LogicUpdate();
    /// <summary>
    /// ���z��Update
    /// </summary>
    void PhysicUpdate();
}

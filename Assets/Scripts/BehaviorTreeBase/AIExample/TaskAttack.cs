using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sx.BehaviorTree;

/// <summary>
/// 節點範例 往目標執行攻擊行為
/// </summary>
public class TaskAttack : Node
{
    private float _attackTime = 1f;
    private float _attackCounter = 0f;

    private Animator _animator;
    public TaskAttack(Transform transform)
    {
        _animator = transform.GetComponentInChildren<Animator>();
    }
    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");

        //對目標造成傷害

        //清除目標列表 並返回其他狀態
        ClearData("target");
        _animator.Play("Slime_Blue_SL_Move");

        state = NodeState.RUNNING;
        return state;
    }
}

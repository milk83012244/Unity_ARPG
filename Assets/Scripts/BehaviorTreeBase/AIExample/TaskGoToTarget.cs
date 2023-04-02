using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sx.BehaviorTree;

/// <summary>
/// 節點範例 往目標移動
/// </summary>
public class TaskGoToTarget : Node
{
    private Transform _transform;

    public TaskGoToTarget(Transform transform)
    {
        _transform = transform;
    }
    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        float dis = Vector3.Distance(_transform.position, target.position);
        if (dis > 0.01f && dis <= TestUnit.fovRange)
        {
            _transform.position = Vector3.MoveTowards(_transform.position, target.position, TestUnit.speed * Time.deltaTime);
        }
        else
        {
            ClearData("target");
        }
        state = NodeState.RUNNING;
        return state;
    }
}

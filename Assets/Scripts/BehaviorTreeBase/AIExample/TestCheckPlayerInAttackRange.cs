using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sx.BehaviorTree;

/// <summary>
/// �`�I�d�� �i�J�����d���˴�
/// </summary>
public class TestCheckPlayerInAttackRange : Node
{
    private static int _playerLayerMask = 1 << 7; //0000�줸�첾

    private Transform _transform;
    private Animator _animator;

    public TestCheckPlayerInAttackRange(Transform transform)
    {
        _transform = transform;
        _animator = transform.GetComponentInChildren<Animator>();
    }
    public override NodeState Evaluate()
    {
        object t = GetData("target");
        if (t==null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        Transform target = (Transform)t;
        if (Vector3.Distance(_transform.position,target.position) <= TestUnit.attackRange)
        {
            _animator.Play("Slime_Blue_SL_Attack");
            state = NodeState.SUCCESS;
            return state;
        }
        state = NodeState.FAILURE;
        return state;
    }
}

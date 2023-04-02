using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sx.BehaviorTree;

/// <summary>
/// �`�I�d�� ��L�ؼжi�J�d���˴�
/// </summary>
public class TestCheckPlayerInFOVRange : Node
{
    private static int _playerLayerMask = 1 << 7; //0000�줸�첾

    private Transform _transform;
    private Animator _animator;
    public TestCheckPlayerInFOVRange(Transform transform)
    {
        _transform = transform;
        _animator = transform.GetComponentInChildren<Animator>();
    }
    public override NodeState Evaluate()
    {
        object t = GetData("target");
        if (t == null)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(_transform.position, TestUnit.fovRange, _playerLayerMask);

            if (colliders.Length > 0)
            {
                parent.parent.SetData("target", colliders[0].transform);
                _animator.Play("Slime_Blue_SL_Move");
                state = NodeState.SUCCESS;
                return state;
            }

            state = NodeState.FAILURE;
            return state;
        }

        state = NodeState.SUCCESS;
        return state;
    }
}

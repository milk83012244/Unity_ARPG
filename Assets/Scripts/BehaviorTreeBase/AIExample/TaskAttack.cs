using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sx.BehaviorTree;

/// <summary>
/// �`�I�d�� ���ؼа�������欰
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

        //��ؼгy���ˮ`

        //�M���ؼЦC�� �ê�^��L���A
        ClearData("target");
        _animator.Play("Slime_Blue_SL_Move");

        state = NodeState.RUNNING;
        return state;
    }
}

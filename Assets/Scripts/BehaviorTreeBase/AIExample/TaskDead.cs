using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sx.BehaviorTree;

public class TaskDead : Node
{
    [SerializeField] public int currentDirection; //當前方向

    private Animator _animator;
    private Transform _transform;
    private TestUnit testUnit;

    private CharacterStatsDataMono selfStats;

    public TaskDead(Transform transform)
    {
        _transform = transform;
        _animator = transform.GetComponentInChildren<Animator>();
        selfStats = transform.GetComponentInChildren<CharacterStatsDataMono>();
        testUnit = transform.GetComponentInParent<TestUnit>();
    }
    public override NodeState Evaluate()
    {
        if (selfStats.CurrnetHealth <= 0)
        {
            _animator.Play("Slime_Blue_SL_Die");

            testUnit.DestroySelf();

            state = NodeState.SUCCESS;
            return state;
        }
        else
        {
            state = NodeState.RUNNING;
            return state;
        }
    }
}

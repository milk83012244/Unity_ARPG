using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sx.BehaviorTree;

/// <summary>
/// 節點範例 往目標移動
/// </summary>
public class TaskGoToTarget : Node
{
    private Animator _animator;
    private Transform _transform;
    private CharacterStatsDataMono selfStats;

    private int currentDirection; //當前方向

    public TaskGoToTarget(Transform transform)
    {
        _transform = transform;
        _animator = transform.GetComponentInChildren<Animator>();
        selfStats = transform.GetComponentInChildren<CharacterStatsDataMono>();
    }
    public override NodeState Evaluate()
    {
        if (selfStats.CurrnetHealth <= 0)
        {
            state = NodeState.FAILURE;
            return state;
        }

        Transform target = (Transform)GetData("target");
        float dis = Vector3.Distance(_transform.position, target.position);
        TestUnit.StartMove();
        if (dis > 0.01f && dis <= TestUnit.fovRange)
        {
            _transform.position = Vector3.MoveTowards(_transform.position, target.position, TestUnit.speed * Time.deltaTime);
            currentDirection = DirectionCheck(_transform.position, target.position);
            AnimationDirCheck(currentDirection, "Move");
        }
        else
        {
            ClearData("target");
        }
        state = NodeState.RUNNING;
        return state;
    }
    public int DirectionCheck(Vector3 self, Vector3 target)
    {
        Vector2 direction = target - self;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (-angle >= -45 && -angle <= 45) //右
        {
            return 1;
        }
        else if (-angle <= 135 && -angle > 45)//上
        {
            return 2;
        }
        else if (-angle > 135 && -angle <= 180 || -angle <= -135 && -angle >= -180)//左
        {
            return 3;
        }
        else if (-angle > -135 && -angle < -45)//下
        {
            return 4;
        }
        else
        {
            return currentDirection;
        }
    }
    public void AnimationDirCheck(int currentDirection, string animationType)
    {
        if (currentDirection == 0)
            return;

        if (animationType == "idle")
        {
            switch (currentDirection)
            {
                case 1:
                    _animator.Play("Slime_Blue_SR_idle");
                    break;
                case 2:
                case 3:
                case 4:
                    _animator.Play("Slime_Blue_SL_idle");
                    break;
            }
        }
        else if (animationType == "Move")
        {
            switch (currentDirection)
            {
                case 1:
                case 4:
                    _animator.Play("Slime_Blue_SR_Move");
                    break;
                case 2:
                case 3:
                    _animator.Play("Slime_Blue_SL_Move");
                    break;
            }
        }
    }
}

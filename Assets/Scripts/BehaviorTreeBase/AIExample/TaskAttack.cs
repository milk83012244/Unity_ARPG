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
    private int currentDirection; //當前方向

    private Transform _transform;

    private Animator _animator;
    private OtherCharacterStats selfStats;

    public TaskAttack(Transform transform)
    {
        _transform = transform;
        _animator = transform.GetComponentInChildren<Animator>();
        selfStats = transform.GetComponentInChildren<OtherCharacterStats>();
    }
    public override NodeState Evaluate()
    {
        if (selfStats.CurrnetHealth <= 0)
        {
            state = NodeState.FAILURE;
            return state;
        }

        Transform target = (Transform)GetData("target");
        //playerStats = target.GetComponent<CharacterStatsDataMono>();

        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        currentDirection = DirectionCheck(_transform.position, target.position);

        _attackCounter += Time.deltaTime;
        if (_attackCounter >= _attackTime)
        {
            TestUnit.StopMove();
            AnimationDirCheck(currentDirection, "Attack");
            // _animator.Play("Slime_Blue_SL_Attack");
            //造成傷害在別的腳本檢測

            _attackCounter = 0f;
        }
        if ( stateInfo.normalizedTime < 0.95f)
        {
            state = NodeState.RUNNING;
            return state;
        }
        if ((stateInfo.IsName("Slime_Blue_SL_Attack")|| stateInfo.IsName("Slime_Blue_SR_Attack"))  && stateInfo.normalizedTime >= 0.95f)
        {
            TestUnit.StartMove();
            AnimationDirCheck(currentDirection, "Move");
        }

        //清除目標列表 並返回其他狀態
        //ClearData("target");
        //_animator.Play("Slime_Blue_SL_Move");

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
        else if (animationType == "Attack")
        {
            switch (currentDirection)
            {
                case 1:
                case 4:
                    _animator.Play("Slime_Blue_SR_Attack");
                    break;
                case 2:
                case 3:
                    _animator.Play("Slime_Blue_SL_Attack");
                    break;
            }
        }
    }
}

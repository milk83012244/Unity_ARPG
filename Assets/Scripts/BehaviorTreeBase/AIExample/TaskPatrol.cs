using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sx.BehaviorTree;

/// <summary>
/// �`�I�d�� ����
/// </summary>
public class TaskPatrol : Node
{
    private Animator _animator;

    private Transform _transform; //���ު��D����
    private Transform[] _waypoints; //�����I

    private CharacterStatsDataMono selfStats;

    [SerializeField] public int currentDirection; //��e��V
    private int _currentWaypointIndex = 0;

    private float _waitTime = 1f;
    private float _waitCounter = 0f;
    private bool _waiting = false;
    public TaskPatrol(Transform transform,Transform[] waypoints)
    {
        _transform = transform;
        _waypoints = waypoints;
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
        if (_waiting)
        {
            _waitCounter += Time.deltaTime;
            if (_waitCounter >= _waitTime)
            {
                _waiting = false;
                _animator.Play("Slime_Blue_SL_Move");
            }
        }
        else
        {
            Transform wp = _waypoints[_currentWaypointIndex];
            if (Vector3.Distance(_transform.position, wp.position) < 0.01f)
            {
                _transform.position = wp.position;
                _waitCounter = 0f;
                _waiting = true;

                _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length; //��F��m
                //currentDirection = DirectionCheck(_transform.position, wp.position);
                //AnimationDirCheck(currentDirection,"idle");
                _animator.Play("Slime_Blue_SL_idle");
            }
            else
            {
                _transform.position = Vector3.MoveTowards(_transform.position, wp.position, TestUnit.speed * Time.deltaTime);
                currentDirection = DirectionCheck(_transform.position, wp.position);
                AnimationDirCheck(currentDirection,"Move");
            }
        }
        state = NodeState.RUNNING;
        return state;
    }
    /// <summary>
    ///  �˴���V
    /// </summary>
    public int DirectionCheck(Vector3 self , Vector3 target)
    {
        Vector2 direction = target - self;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (-angle >= -45 && -angle <= 45) //�k
        {
            return 1;
        }
        else if (-angle <= 135 && -angle > 45)//�W
        {
            return 2;
        }
        else if (-angle > 135 && -angle <= 180 || -angle <= -135 && -angle >= -180)//��
        {
            return 3;
        }
        else if (-angle > -135 && -angle < -45)//�U
        {
            return 4;
        }
        else
        {
            return currentDirection;
        }
    }
    public void AnimationDirCheck(int currentDirection,string animationType)
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

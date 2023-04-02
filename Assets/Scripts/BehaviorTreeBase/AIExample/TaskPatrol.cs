using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sx.BehaviorTree;

/// <summary>
/// 節點範例 巡邏
/// </summary>
public class TaskPatrol : Node
{
    private Animator _animator;

    private Transform _transform; //巡邏的主物件
    private Transform[] _waypoints; //巡邏點

    private int _currentWaypointIndex = 0;

    private float _waitTime = 1f;
    private float _waitCounter = 0f;
    private bool _waiting = false;
    public TaskPatrol(Transform transform,Transform[] waypoints)
    {
        _transform = transform;
        _waypoints = waypoints;
        _animator = transform.GetComponentInChildren<Animator>();
    }
    public override NodeState Evaluate()
    {
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

                _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length; //到達位置
                _animator.Play("Slime_Blue_SL_idle");
            }
            else
            {
                _transform.position = Vector3.MoveTowards(_transform.position, wp.position, TestUnit.speed * Time.deltaTime);
                //_transform.LookAt(wp.position);
            }
        }
        state = NodeState.RUNNING;
        return state;
    }
}

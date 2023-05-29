using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sx.EnemyAI
{
    public class Patrol : EnemyAction
    {
        private Transform[] waypoints; //�����I

        private int playerLayerMask = 1 << 7; //0000�줸�첾
        private int _currentWaypointIndex = 0;

        private float waitTime = 1f;
        private float waitCounter = 0f;
        private bool waiting = false;

        public override void OnStart()
        {
            waypoints = enemyUnits.waypoints;
        }

        public override TaskStatus OnUpdate()
        {
            if (selfStats.CurrnetHealth <= 0)
            {
                state = TaskStatus.Failure;
                return state;
            }

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, enemyUnits.fovRange, playerLayerMask);

            if (colliders.Length > 0)
            {
                state = TaskStatus.Success;
                return state;
            }

            if (waiting)
            {
                waitCounter += Time.deltaTime;
                if (waitCounter >= waitTime)
                {
                    waiting = false;
                    animator.Play("Slime_Blue_SL_Move");
                }
            }
            else
            {
                Transform wp = waypoints[_currentWaypointIndex];
                if (Vector3.Distance(transform.position, wp.position) < 0.01f)
                {
                    transform.position = wp.position;
                    waitCounter = 0f;
                    waiting = true;

                    _currentWaypointIndex = (_currentWaypointIndex + 1) % waypoints.Length; //��F��m
                                                                                             //currentDirection = DirectionCheck(_transform.position, wp.position);
                                                                                             //AnimationDirCheck(currentDirection,"idle");
                    animator.Play("Slime_Blue_SL_idle");
                    return state;
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, wp.position, enemyUnits.speed * Time.deltaTime);
                    currentDirection = facePlayer.DirectionCheck(transform.position, wp.position);
                    facePlayer.AnimationDirCheck(currentDirection, "Move",animator);
                }
            }
            state = TaskStatus.Running;
            return state;
        }
    }
}


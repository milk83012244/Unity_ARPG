using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sx.EnemyAI
{
    public class EnemyType1Patrol : EnemyType1Action
    {
        private List<Transform> waypoints; //巡邏點

        private int playerLayerMask = 1 << 7; //0000位元位移
        private int _currentWaypointIndex = 0;

        private float waitTime = 1f;
        private float waitCounter = 0f;
        private bool waiting = false;

        public override void OnStart()
        {
            waypoints = enemyUnitType1.waypoints;
        }

        public override TaskStatus OnUpdate()
        {
            if (enemyUnitType1.currentState == EnemyCurrentState.Stunning || enemyUnitType1.currentState == EnemyCurrentState.Stop ) //無法行動狀態
            {
                state = TaskStatus.Failure;
                return state;
            }
            if (selfStats.CurrnetHealth <= 0|| enemyUnitType1.isAttackState)
            {
                state = TaskStatus.Failure;
                return state;
            }

            if (waiting)
            {
                waitCounter += Time.deltaTime;
                if (waitCounter >= waitTime)
                {
                    waiting = false;
                    enemyUnitType1.currentState = EnemyCurrentState.Patrol;
                    currentDirection = facePlayer.DirectionCheck(transform.position, player.transform.position);
                    facePlayer.AnimationDirCheck(currentDirection, "Move", animator);            
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

                    _currentWaypointIndex = (_currentWaypointIndex + 1) % waypoints.Count; //到達位置
                                                                                            //currentDirection = DirectionCheck(_transform.position, wp.position);
                                                                                            //AnimationDirCheck(currentDirection,"idle");
                    enemyUnitType1.currentState = EnemyCurrentState.Idle;
                    currentDirection = facePlayer.DirectionCheck(transform.position, player.transform.position);
                    facePlayer.AnimationDirCheck(currentDirection, "Idle", animator);
                    //animator.Play(name + "_SL_idle");
                    //return state;
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, wp.position, enemyUnitType1.moveSpeed * Time.deltaTime);
                    currentDirection = facePlayer.DirectionCheck(transform.position, wp.position);
                    facePlayer.AnimationDirCheck(currentDirection, "Move",animator);
                }
            }
            state = TaskStatus.Running;
            return state;
        }
    }
}


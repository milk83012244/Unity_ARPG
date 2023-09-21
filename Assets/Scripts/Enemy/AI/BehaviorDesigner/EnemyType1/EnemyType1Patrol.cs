using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Pathfinding;

namespace Sx.EnemyAI
{
    public class EnemyType1Patrol : EnemyType1Action
    {
        private List<Transform> waypoints; //�����I

        public override void OnStart()
        {
            waypoints = enemyUnitType1.waypoints;
            patrolAI.targets = waypoints.ToArray();
        }

        public override TaskStatus OnUpdate()
        {
            if (enemyUnitType1.currentState == EnemyCurrentState.Stunning || enemyUnitType1.currentState == EnemyCurrentState.Stop ) //�L�k��ʪ��A
            {
                StopAIPath();
                state = TaskStatus.Failure;
                return state;
            }
            if (selfStats.CurrnetHealth <= 0)
            {
                StopAIPath();
                state = TaskStatus.Failure;
                return state;
            }
            //AStar������
            if (patrolAI.reachPath) //�ݾ�
            {
                StopAIPath();

                enemyUnitType1.currentState = EnemyCurrentState.Idle;
                currentDirection = facePlayer.DirectionCheck(transform.position, patrolAI.targets[patrolAI.index].position);
                facePlayer.AnimationDirCheck(currentDirection, "Idle", animator);
            }
            else //����
            {
                StartAIPath();

                enemyUnitType1.currentState = EnemyCurrentState.Patrol;
                //���ʥ浹�M������
                currentDirection = facePlayer.DirectionCheck(transform.position, patrolAI.targets[patrolAI.index].position);
                facePlayer.AnimationDirCheck(currentDirection, "Move", animator);
            }

            //�쨵��
            //if (waiting)
            //{
            //    waitCounter += Time.deltaTime;
            //    if (waitCounter >= waitTime)
            //    {
            //        waiting = false;
            //        enemyUnitType1.currentState = EnemyCurrentState.Patrol;
            //        currentDirection = facePlayer.DirectionCheck(transform.position, player.transform.position);
            //        facePlayer.AnimationDirCheck(currentDirection, "Move", animator);            
            //    }
            //}
            //else
            //{
            //    Transform wp = waypoints[_currentWaypointIndex];
            //    if (Vector3.Distance(transform.position, wp.position) < 0.01f)
            //    {
            //        transform.position = wp.position;
            //        waitCounter = 0f;
            //        waiting = true;

            //        _currentWaypointIndex = (_currentWaypointIndex + 1) % waypoints.Count; //��F��m
            //                                                                                //currentDirection = DirectionCheck(_transform.position, wp.position);
            //                                                                                //AnimationDirCheck(currentDirection,"idle");
            //        enemyUnitType1.currentState = EnemyCurrentState.Idle;
            //        currentDirection = facePlayer.DirectionCheck(transform.position, player.transform.position);
            //        facePlayer.AnimationDirCheck(currentDirection, "Idle", animator);
            //        //animator.Play(name + "_SL_idle");
            //        //return state;
            //    }
            //    else
            //    {
            //        transform.position = Vector3.MoveTowards(transform.position, wp.position, enemyUnitType1.moveSpeed * Time.deltaTime);
            //        currentDirection = facePlayer.DirectionCheck(transform.position, wp.position);
            //        facePlayer.AnimationDirCheck(currentDirection, "Move",animator);
            //    }
            //}
            state = TaskStatus.Running;
            return state;
        }
    }
}


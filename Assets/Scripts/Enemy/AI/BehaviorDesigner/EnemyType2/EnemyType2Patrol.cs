using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Pathfinding;

namespace Sx.EnemyAI
{
    public class EnemyType2Patrol : EnemyType2Action
    {
        private List<Transform> waypoints; //�����I

        public override void OnStart()
        {
            waypoints = enemyUnitType2.waypoints;
            patrolAI.targets = waypoints.ToArray();
        }

        public override TaskStatus OnUpdate()
        {
            if (enemyUnitType2.currentState == EnemyCurrentState.Stunning || enemyUnitType2.currentState == EnemyCurrentState.Stop) //�L�k��ʪ��A
            {
                StopAIPath();
                state = TaskStatus.Failure;
                return state;
            }
            if (selfStats.CurrnetHealth <= 0 )
            {
                StopAIPath();
                state = TaskStatus.Failure;
                return state;
            }

            if (enemyUnitType2.currentState == EnemyCurrentState.Chase || enemyUnitType2.currentState == EnemyCurrentState.Attack)
            {
                state = TaskStatus.Failure;
                return state;
            }

            //AStar������
            if (patrolAI.reachPath) //�ݾ�
            {
                StopAIPath();

                enemyUnitType2.currentState = EnemyCurrentState.Idle;
                currentDirection = facePlayer.DirectionCheck(transform.position, patrolAI.targets[patrolAI.index].position);
                facePlayer.AnimationDirCheck(currentDirection, "Idle", animator);
            }
            else //����
            {
                StartAIPath();

                enemyUnitType2.currentState = EnemyCurrentState.Patrol;
                //���ʥ浹�M������
                currentDirection = facePlayer.DirectionCheck(transform.position, patrolAI.targets[patrolAI.index].position);
                facePlayer.AnimationDirCheck(currentDirection, "Move", animator);
            }

            if (Vector3.Distance(transform.position, player.transform.position) <= enemyUnitType2.maxFovRange)
            {
                state = TaskStatus.Failure;
                return state;
            }

            state = TaskStatus.Running;
            return state;
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sx.EnemyAI
{
    /// <summary>
    /// �W�L���Ľd�򩹪��a����
    /// </summary>
    public class Boss1NormalMove : Boss1Action
    {
        private Transform targetPosition; //��l��m
        private float moveDistance = 1f; //�@�����ʶZ��
        private float distanceMoved = 0f; //��e���ʶZ��
        private Vector2 startPosition;
        private Vector2 lastPosition;
        public override void OnStart()
        {
            targetPosition = PlayerController.GetInstance().transform;
            startPosition = transform.position;
        }
        public override TaskStatus OnUpdate()
        {
            if (enemyBoss1Unit.currentState == EnemyCurrentState.Stunning || enemyBoss1Unit.currentState == EnemyCurrentState.Stop || enemyBoss1Unit.currentState == EnemyCurrentState.Dead) //�L�k��ʪ��A
     
            {
                state = TaskStatus.Failure;
                return state;
            }
            aiDestinationSetter.target = player.transform;

            float dis = Vector3.Distance(transform.position, player.transform.position);
            //Vector2 moveDirection = (targetPosition.position - transform.position).normalized;
            //float distanceToTarget = Vector2.Distance(transform.position, targetPosition.position); //�P�ؼЪ��Z��
            lastPosition = transform.position;
            float frameDistance = Vector2.Distance(startPosition, lastPosition);
            distanceMoved = frameDistance; //�w���ʪ��Z��

            if (distanceMoved >= moveDistance)
            {
                StopAIPath();
                currentDirection = facePlayer.DirectionCheck(transform.position, player.transform.position);
                facePlayer.Boss1AnimationDirCheck(currentDirection, "Idle", animator);
                state = TaskStatus.Success;
                return state;
            }

            if (dis > enemyBoss1Unit.maxFovRange)
            {
                StartAIPath();
                currentDirection = facePlayer.DirectionCheck(transform.position, player.transform.position);
                facePlayer.Boss1AnimationDirCheck(currentDirection, "Walk", animator);
                state = TaskStatus.Running;
                return state;
            }
            else
            {
                StopAIPath();
                currentDirection = facePlayer.DirectionCheck(transform.position, player.transform.position);
                facePlayer.Boss1AnimationDirCheck(currentDirection, "Idle", animator);
                state = TaskStatus.Failure;
                return state;
            }
        }
    }
}


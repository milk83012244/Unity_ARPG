using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sx.EnemyAI
{
    /// <summary>
    /// 超過索敵範圍往玩家移動
    /// </summary>
    public class Boss1NormalMove : Boss1Action
    {
        private Transform targetPosition; //初始位置
        private float moveDistance = 1f; //一次移動距離
        private float distanceMoved = 0f; //當前移動距離
        private Vector2 startPosition;
        private Vector2 lastPosition;
        public override void OnStart()
        {
            targetPosition = PlayerController.GetInstance().transform;
            startPosition = transform.position;
        }
        public override TaskStatus OnUpdate()
        {
            if (enemyBoss1Unit.currentState == EnemyCurrentState.Stunning || enemyBoss1Unit.currentState == EnemyCurrentState.Stop || enemyBoss1Unit.currentState == EnemyCurrentState.Dead) //無法行動狀態
     
            {
                state = TaskStatus.Failure;
                return state;
            }
            aiDestinationSetter.target = player.transform;

            float dis = Vector3.Distance(transform.position, player.transform.position);
            //Vector2 moveDirection = (targetPosition.position - transform.position).normalized;
            //float distanceToTarget = Vector2.Distance(transform.position, targetPosition.position); //與目標的距離
            lastPosition = transform.position;
            float frameDistance = Vector2.Distance(startPosition, lastPosition);
            distanceMoved = frameDistance; //已移動的距離

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


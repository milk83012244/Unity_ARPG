using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sx.EnemyAI
{
    public class Boss1KeepAttackBehavior : Boss1Action
    {
        private Vector2 initialPosition; //初始位置
        private Vector2 targetPosition; //目標位置
        private Vector2 lastPosition;

        float distanceToTarget;
        float stoppingDistance = 0.4f;
        float walkDistance = 0.2f; //一次移動距離
        float nearDistance = 1f;
        float distanceMoved = 0f; //當前移動距離
        bool isMovingEnd;

        private Coroutine coroutine;

        public override void OnStart()
        {
            initialPosition = transform.position;
            distanceMoved = 0;

            switch (enemyBoss1Unit.currentAttackBehavior)
            {
                case EnemyBoss1Unit.Boss1AttackBehavior.WalkL:
                case EnemyBoss1Unit.Boss1AttackBehavior.WalkR:
                    targetPosition = initialPosition + new Vector2(walkDistance, 0f);
                    break;
                case EnemyBoss1Unit.Boss1AttackBehavior.Near:
                    targetPosition = initialPosition + new Vector2(nearDistance, 0f);
                    break;
            }
        }
        public override TaskStatus OnUpdate()
        {
            if (enemyBoss1Unit.currentState == EnemyCurrentState.Stunning || enemyBoss1Unit.currentState == EnemyCurrentState.Stop) //無法行動狀態
            {
                state = TaskStatus.Failure;
                return state;
            }
            if (selfStats.CurrnetHealth <= 0)
            {
                state = TaskStatus.Failure;
                return state;
            }
            distanceToTarget = Vector2.Distance(transform.position, player.transform.position);

            //執行攻擊指令
            if (coroutine == null)
            {
                coroutine = StartCoroutine(StartAttackBehaior());
            }
            if (isMovingEnd)
            {
                state = TaskStatus.Success;
                return state;
            }

            if (facePlayer.CurrentStateTime > 0 && facePlayer.CurrentStateTime < 0.95f && enemyBoss1Unit.currentState == EnemyCurrentState.Stunning) //攻擊中進入硬直
            {
                state = TaskStatus.Success;
                return state;
            }

            state = TaskStatus.Running;
            return state;
        }
        private IEnumerator StartAttackBehaior()
        {
            StopAIMove();
            Vector2 moveDirection;
            moveDirection = (player.transform.position - transform.position).normalized;
            switch (enemyBoss1Unit.currentAttackBehavior)
            {
                case EnemyBoss1Unit.Boss1AttackBehavior.RangeAttack:
                    yield break;
                    break;
                case EnemyBoss1Unit.Boss1AttackBehavior.WalkL:
                    currentDirection = facePlayer.DirectionCheck(transform.position, player.transform.position);
                    facePlayer.Boss1AnimationDirCheck(3, "Walk", animator);
                    while (distanceMoved <= walkDistance)
                    {
                        lastPosition = transform.position;
                        float frameDistance = Vector2.Distance(initialPosition, lastPosition);
                        distanceMoved = frameDistance; //已移動的距離
                        body.velocity = Vector2.left * 0.7f;
                        yield return null;
                    }
                    yield return Yielders.GetWaitForSeconds(0.5f);
                    break;
                case EnemyBoss1Unit.Boss1AttackBehavior.WalkR:
                    currentDirection = facePlayer.DirectionCheck(transform.position, player.transform.position);
                    facePlayer.Boss1AnimationDirCheck(1, "Walk", animator);
                    while (distanceMoved <= walkDistance)
                    {
                        lastPosition = transform.position;
                        float frameDistance = Vector2.Distance(initialPosition, lastPosition);
                        distanceMoved = frameDistance; //已移動的距離
                        body.velocity = Vector2.right * 0.7f;
                        yield return null;
                    }
                    yield return Yielders.GetWaitForSeconds(0.5f);
                    break;
                case EnemyBoss1Unit.Boss1AttackBehavior.Near:
                    body.velocity = moveDirection * enemyBoss1Unit.moveSpeed * 3.5f;
                    facePlayer.Boss1AnimationDirCheck(currentDirection, "Flying", animator);
                    yield return Yielders.GetWaitForSeconds(1f);
                    break;
            }
            body.velocity = Vector2.zero;
            currentDirection = facePlayer.DirectionCheck(transform.position, player.transform.position);
            facePlayer.Boss1AnimationDirCheck(currentDirection, "Idle", animator);
            StartAIMove();
            isMovingEnd = true;
            coroutine = null;
        }
    }
}


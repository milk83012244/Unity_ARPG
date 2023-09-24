using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sx.EnemyAI
{
    /// <summary>
    /// 積極行為攻擊指令執行
    /// </summary>
    public class Boss1PositiveAttackBehavior : Boss1Action
    {
        private AnimatorStateInfo stateInfo;

        private Vector2 initialPosition; //初始位置
        private Vector2 targetPosition; //目標位置

        float distanceToTarget;
        float stoppingDistance = 0.2f;
        float walkDistance = 0.5f; //一次移動距離
        float backOffDistance = 0.7f;
        float nearDistance = 0.7f;
        bool isMovingEnd;

        private Coroutine coroutine;

        public override void OnStart()
        {
            initialPosition = transform.position;
            switch (enemyBoss1Unit.currentAttackBehavior)
            {
                case EnemyBoss1Unit.Boss1AttackBehavior.WalkL:
                case EnemyBoss1Unit.Boss1AttackBehavior.WalkR:
                    targetPosition = initialPosition + new Vector2(walkDistance, 0f);
                    break;
                case EnemyBoss1Unit.Boss1AttackBehavior.Near:
                    targetPosition = initialPosition + new Vector2(nearDistance, 0f);
                    break;
                case EnemyBoss1Unit.Boss1AttackBehavior.BackOff:
                    targetPosition = initialPosition + new Vector2(backOffDistance, 0f);
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
        /// <summary>
        /// 初始接收的攻擊指令
        /// </summary>
        private IEnumerator StartAttackBehaior()
        {
            StopAIMove();

            Vector2 moveDirection;
            moveDirection = (player.transform.position - transform.position).normalized;
            switch (enemyBoss1Unit.currentAttackBehavior)
            {
                case EnemyBoss1Unit.Boss1AttackBehavior.NormalAttack1:
                    //計算與玩家距離 如果大於攻擊範圍就拉近後再攻擊
                    if (Vector3.Distance(transform.position, player.transform.position) > enemyBoss1Unit.attackRange)
                    {
                         //moveDirection = (player.transform.position - transform.position).normalized;
                        if (distanceToTarget > stoppingDistance)
                        {
                            body.velocity = moveDirection * enemyBoss1Unit.moveSpeed * 3;
                            facePlayer.Boss1AnimationDirCheck(currentDirection, "Flying", animator);
                        }
                    }
                    while (distanceToTarget < stoppingDistance)
                    {
                        yield return null;
                    }
                    yield return Yielders.GetWaitForSeconds(0.5f);
                    enemyBoss1Unit.currentState = EnemyCurrentState.Attack; //執行攻擊動作
                    currentDirection = facePlayer.DirectionCheck(transform.position, player.transform.position);
                    facePlayer.Boss1AnimationDirCheck(currentDirection, "Attack", animator, 1);
                    //if (Vector3.Distance(transform.position, player.transform.position) <= enemyBoss1Unit.attackRange)
                    //{

                    //}
                    yield return Yielders.GetWaitForSeconds(1f);
                    break;
                case EnemyBoss1Unit.Boss1AttackBehavior.WalkL:
                    body.velocity = Vector2.left * enemyBoss1Unit.moveSpeed * 0.7f;
                    currentDirection = facePlayer.DirectionCheck(transform.position, player.transform.position);
                    facePlayer.Boss1AnimationDirCheck(3, "Walk", animator);
                    yield return Yielders.GetWaitForSeconds(0.7f);
                    break;
                case EnemyBoss1Unit.Boss1AttackBehavior.WalkR:
                    body.velocity = Vector2.right * enemyBoss1Unit.moveSpeed * 0.7f;
                    currentDirection = facePlayer.DirectionCheck(transform.position, player.transform.position);
                    facePlayer.Boss1AnimationDirCheck(1, "Walk", animator);
                    yield return Yielders.GetWaitForSeconds(0.7f);
                    break;
                case EnemyBoss1Unit.Boss1AttackBehavior.BackOff:
                    body.velocity = -moveDirection * enemyBoss1Unit.moveSpeed * 2;
                    facePlayer.Boss1AnimationDirCheck(currentDirection, "Flying", animator);
                    yield return Yielders.GetWaitForSeconds(0.5f);
                    break;
                case EnemyBoss1Unit.Boss1AttackBehavior.Near:
                    body.velocity = moveDirection * enemyBoss1Unit.moveSpeed * 2;
                    facePlayer.Boss1AnimationDirCheck(currentDirection, "Flying", animator);
                    yield return Yielders.GetWaitForSeconds(0.5f);
                    break;
            }
            currentDirection = facePlayer.DirectionCheck(transform.position, player.transform.position);
            facePlayer.Boss1AnimationDirCheck(currentDirection, "Idle", animator);
            StartAIMove();
            isMovingEnd = true;
            coroutine = null;
        }
    }
}

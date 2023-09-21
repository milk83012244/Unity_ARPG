using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sx.EnemyAI
{
    /// <summary>
    /// �n���欰�������O����
    /// </summary>
    public class Boss1PositiveAttackBehavior : Boss1Action
    {
        private AnimatorStateInfo stateInfo;

        private Vector2 initialPosition; //��l��m
        private Vector2 targetPosition; //�ؼЦ�m

        float distanceToTarget;
        float stoppingDistance = 0.2f;
        float walkDistance = 0.7f;
        float backOffDistance = 1f;
        float nearDistance = 1f;
        bool isMoving;

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
            if (enemyBoss1Unit.currentState == EnemyCurrentState.Stunning || enemyBoss1Unit.currentState == EnemyCurrentState.Stop) //�L�k��ʪ��A
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

            //����������O
            StartAttackBehaior();
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (facePlayer.CurrentStateTime > 0.95f) //�ʵe���񵲧�
            {
                facePlayer.Boss1AnimationDirCheck(currentDirection, "Idle", animator);
                state = TaskStatus.Success;
                return state;
            }
            if (facePlayer.CurrentStateTime > 0 && facePlayer.CurrentStateTime < 0.95f && enemyBoss1Unit.currentState == EnemyCurrentState.Stunning) //�������i�J�w��
            {
                state = TaskStatus.Success;
                return state;
            }

            state = TaskStatus.Running;
            return state;
        }
        /// <summary>
        /// ��l�������������O
        /// </summary>
        private IEnumerable StartAttackBehaior()
        {
            isMoving = true;

            Vector2 moveDirection;
            moveDirection = (player.transform.position - transform.position).normalized;
            switch (enemyBoss1Unit.currentAttackBehavior)
            {
                case EnemyBoss1Unit.Boss1AttackBehavior.NormalAttack1:
                    //�p��P���a�Z�� �p�G�j������d��N�Ԫ��A����
                    if (Vector3.Distance(transform.position, player.transform.position) > enemyBoss1Unit.attackRange)
                    {
                         //moveDirection = (player.transform.position - transform.position).normalized;
                        if (distanceToTarget > stoppingDistance)
                        {
                            body.velocity = moveDirection * enemyBoss1Unit.moveSpeed * 3;
                        }
                        else
                        {
                            body.velocity = Vector2.zero;
                        }
                    }
                    yield return new WaitWhile(() => distanceToTarget < stoppingDistance);

                    if (Vector3.Distance(transform.position, player.transform.position) <= enemyBoss1Unit.attackRange)
                    {
                        enemyBoss1Unit.currentState = EnemyCurrentState.Attack; //��������ʧ@
                        currentDirection = facePlayer.DirectionCheck(transform.position, player.transform.position);
                        facePlayer.Boss1AnimationDirCheck(currentDirection, "Attack", animator, 1);
                    }

                    break;
                case EnemyBoss1Unit.Boss1AttackBehavior.WalkL:
                    if (distanceToTarget > 0)
                        body.velocity = Vector2.left * enemyBoss1Unit.moveSpeed;
                    currentDirection = facePlayer.DirectionCheck(transform.position, player.transform.position);
                    facePlayer.Boss1AnimationDirCheck(currentDirection, "Walk", animator);
                    break;
                case EnemyBoss1Unit.Boss1AttackBehavior.WalkR:
                    if (distanceToTarget > 0)
                        body.velocity = Vector2.right * enemyBoss1Unit.moveSpeed;

                    facePlayer.Boss1AnimationDirCheck(currentDirection, "Walk", animator);
                    break;
                case EnemyBoss1Unit.Boss1AttackBehavior.BackOff:
                    if (distanceToTarget > 0)
                        body.velocity = -moveDirection * enemyBoss1Unit.moveSpeed *2;
                    facePlayer.Boss1AnimationDirCheck(currentDirection, "Flying", animator);
                    break;
                case EnemyBoss1Unit.Boss1AttackBehavior.Near:
                    if (distanceToTarget > 0)
                        body.velocity = moveDirection * enemyBoss1Unit.moveSpeed * 2;
                    facePlayer.Boss1AnimationDirCheck(currentDirection, "Flying", animator);
                    break;
            }
            isMoving = false;
        }
    }
}


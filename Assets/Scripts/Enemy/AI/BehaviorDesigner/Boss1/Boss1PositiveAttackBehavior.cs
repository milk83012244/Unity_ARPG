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
        private Vector2 initialPosition; //��l��m
        private Vector2 targetPosition; //�ؼЦ�m
        private Vector2 lastPosition;

        int attackRandomValue = 0;

        float distanceToTarget;
        float stoppingDistance = 0.2f;
        float walkDistance = 0.3f; //�@�����ʶZ��
        float backOffDistance = 0.7f;
        float nearDistance = 0.7f;
        float distanceMoved = 0f; //��e���ʶZ��
        bool isMovingEnd;

        private Coroutine coroutine;

        public override void OnStart()
        {
            initialPosition = transform.position;
            distanceMoved = 0;
            attackRandomValue = 0;
            switch (enemyBoss1Unit.currentAttackBehavior)
            {
                case EnemyBoss1Unit.Boss1AttackBehavior.WalkL:
                case EnemyBoss1Unit.Boss1AttackBehavior.WalkR:
                    targetPosition = initialPosition + new Vector2(walkDistance, 0f);
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
            if (coroutine == null)
            {
                coroutine = StartCoroutine(StartAttackBehaior());
            }
            if (isMovingEnd)
            {
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
        private IEnumerator StartAttackBehaior()
        {
            StopAIMove();

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
                            facePlayer.Boss1AnimationDirCheck(currentDirection, "Flying", animator);
                        }
                        while (distanceToTarget < stoppingDistance)
                        {
                            yield return null;
                        }
                        yield return Yielders.GetWaitForSeconds(0.5f);
                    }
                    enemyBoss1Unit.currentState = EnemyCurrentState.Attack; //��������ʧ@
                    currentDirection = facePlayer.DirectionCheck(transform.position, player.transform.position);
                    facePlayer.Boss1AnimationDirCheck(currentDirection, "Attack", animator, 1);
                    attackRandomValue = SetRandom();
                    if (attackRandomValue < 60)
                        yield return Yielders.GetWaitForSeconds(1f);
                    else
                        yield return Yielders.GetWaitForSeconds(0.7f);
                    if (attackRandomValue >60) //�G�q����
                    {
                        enemyBoss1Unit.SetAttackBehavior(EnemyBoss1Unit.Boss1AttackBehavior.NormalAttack2);
                        attackRandomValue = SetRandom();
                        currentDirection = facePlayer.DirectionCheck(transform.position, player.transform.position);
                        facePlayer.Boss1AnimationDirCheck(currentDirection, "Attack", animator, 2);
                        if (attackRandomValue < 80)
                            yield return Yielders.GetWaitForSeconds(1f);
                        else
                            yield return Yielders.GetWaitForSeconds(0.7f);
                        if (attackRandomValue > 80) //�T�q����
                        {
                            enemyBoss1Unit.SetAttackBehavior(EnemyBoss1Unit.Boss1AttackBehavior.NormalAttack3);
                            currentDirection = facePlayer.DirectionCheck(transform.position, player.transform.position);
                            facePlayer.Boss1AnimationDirCheck(currentDirection, "Attack", animator, 3);
                            yield return Yielders.GetWaitForSeconds(1f);
                        }
                    }
                    break;
                case EnemyBoss1Unit.Boss1AttackBehavior.WalkL:
                    currentDirection = facePlayer.DirectionCheck(transform.position, player.transform.position);
                    facePlayer.Boss1AnimationDirCheck(3, "Walk", animator);
                    while (distanceMoved <= walkDistance)
                    {
                        lastPosition = transform.position;
                        float frameDistance = Vector2.Distance(initialPosition, lastPosition);
                        distanceMoved = frameDistance; //�w���ʪ��Z��
                        body.velocity = Vector2.left * 0.7f;
                        yield return null;
                    }
                    yield return Yielders.GetWaitForSeconds(0.7f);
                    break;
                case EnemyBoss1Unit.Boss1AttackBehavior.WalkR:
                    currentDirection = facePlayer.DirectionCheck(transform.position, player.transform.position);
                    facePlayer.Boss1AnimationDirCheck(1, "Walk", animator);
                    while (distanceMoved <= walkDistance)
                    {
                        lastPosition = transform.position;
                        float frameDistance = Vector2.Distance(initialPosition, lastPosition);
                        distanceMoved = frameDistance; //�w���ʪ��Z��
                        body.velocity = Vector2.right * 0.7f;
                        yield return null;
                    }
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

        private int SetRandom()
        {
            return UnityEngine.Random.Range(0, 101);
        }
    }
}

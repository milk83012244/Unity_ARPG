using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sx.EnemyAI
{
    /// <summary>
    /// �����ʧ@
    /// </summary>
    public class EnemyType1Attack : EnemyType1Action
    {
        private AnimatorStateInfo stateInfo;

        public override TaskStatus OnUpdate()
        {
            if (enemyUnitType1.currentState == EnemyCurrentState.Stunning || enemyUnitType1.currentState == EnemyCurrentState.Stop) //�L�k��ʪ��A
            {
                state = TaskStatus.Failure;
                return state;
            }
            if (selfStats.CurrnetHealth <= 0|| !enemyUnitType1.isAttackState)
            {
                state = TaskStatus.Failure;
                return state;
            }
            if (Vector3.Distance(transform.position, player.transform.position) > enemyUnitType1.attackRange)
            {
                enemyUnitType1.currentState = EnemyCurrentState.Idle;
                currentDirection = facePlayer.DirectionCheck(transform.position, player.transform.position);
                facePlayer.AnimationDirCheck(currentDirection, "Idle", animator);
                state = TaskStatus.Failure;
                return state;
            }

            enemyUnitType1.currentState = EnemyCurrentState.Attack;
            currentDirection = facePlayer.DirectionCheck(transform.position, player.transform.position);
            facePlayer.AnimationDirCheck(currentDirection, "Attack", animator);
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            if (facePlayer.CurrentStateTime > 0.95f) //�����ʵe���񵲧�
            {
                facePlayer.AnimationDirCheck(currentDirection, "Idle", animator);
                state = TaskStatus.Success;
                return state;
            }
            if (facePlayer.CurrentStateTime> 0 && facePlayer.CurrentStateTime <0.95f && enemyUnitType1.currentState == EnemyCurrentState.Stunning) //�������i�J�w��
            {
                state = TaskStatus.Success;
                return state;
            }

            //enemyUnitType1.StartMove();
            //enemyUnitType1.currentState = CurrentState.Idle;
            //�y���ˮ`�b�O���}���˴�

            state = TaskStatus.Running;
            return state;

        }
    }
}


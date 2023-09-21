using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sx.EnemyAI
{
    /// <summary>
    /// 攻擊動作
    /// </summary>
    public class EnemyType1Attack : EnemyType1Action
    {
        private AnimatorStateInfo stateInfo;

        public override TaskStatus OnUpdate()
        {
            if (enemyUnitType1.currentState == EnemyCurrentState.Stunning || enemyUnitType1.currentState == EnemyCurrentState.Stop) //無法行動狀態
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

            if (facePlayer.CurrentStateTime > 0.95f) //攻擊動畫播放結束
            {
                facePlayer.AnimationDirCheck(currentDirection, "Idle", animator);
                state = TaskStatus.Success;
                return state;
            }
            if (facePlayer.CurrentStateTime> 0 && facePlayer.CurrentStateTime <0.95f && enemyUnitType1.currentState == EnemyCurrentState.Stunning) //攻擊中進入硬直
            {
                state = TaskStatus.Success;
                return state;
            }

            //enemyUnitType1.StartMove();
            //enemyUnitType1.currentState = CurrentState.Idle;
            //造成傷害在別的腳本檢測

            state = TaskStatus.Running;
            return state;

        }
    }
}


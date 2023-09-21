using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sx.EnemyAI
{
    public class EnemyType2Attack : EnemyType2Action
    {
        private AnimatorStateInfo stateInfo;

        public override TaskStatus OnUpdate()
        {
            if (enemyUnitType2.currentState == EnemyCurrentState.Stunning || enemyUnitType2.currentState == EnemyCurrentState.Stop|| enemyUnitType2.currentState == EnemyCurrentState.Dead) //無法行動狀態
            {
                state = TaskStatus.Failure;
                return state;
            }
            if (selfStats.CurrnetHealth <= 0)
            {
                state = TaskStatus.Failure;
                return state;
            }
            if (Vector3.Distance(transform.position, player.transform.position) > enemyUnitType2.attackRange)
            {
                enemyUnitType2.currentState = EnemyCurrentState.Idle;
                currentDirection = facePlayer.DirectionCheck(transform.position, player.transform.position);
                facePlayer.AnimationDirCheck(currentDirection, "Idle", animator);
                state = TaskStatus.Failure;
                return state;
            }

            enemyUnitType2.currentState = EnemyCurrentState.Attack;
            currentDirection = facePlayer.DirectionCheck(transform.position, player.transform.position);
            facePlayer.AnimationDirCheck(currentDirection, "Attack", animator);
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (facePlayer.CurrentStateTime > 0.95f)
            {
                facePlayer.AnimationDirCheck(currentDirection, "Idle", animator);
                state = TaskStatus.Success;
                return state;
            }

            //造成傷害在別的腳本檢測

            state = TaskStatus.Running;
            return state;
        }
    }
}


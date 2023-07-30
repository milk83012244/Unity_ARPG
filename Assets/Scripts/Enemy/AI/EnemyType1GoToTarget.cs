using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sx.EnemyAI
{
    /// <summary>
    /// 追逐狀態
    /// </summary>
    public class EnemyType1GoToTarget : EnemyType1Action
    {
        private int playerLayerMask = 1 << 7; //0000位元位移
        public override TaskStatus OnUpdate()
        {
            if (enemyUnitType1.currentState == EnemyCurrentState.Stunning|| enemyUnitType1.currentState == EnemyCurrentState.Stop) //無法行動狀態
            {
                state = TaskStatus.Failure;
                return state;
            }
            if (selfStats.CurrnetHealth <= 0 || !enemyUnitType1.isAttackState)
            {
                state = TaskStatus.Failure;
                return state;
            }

            float dis = Vector3.Distance(transform.position, player.transform.position);
            enemyUnitType1.StartMove();
            if (dis > enemyUnitType1.minFovRange && dis <= enemyUnitType1.maxFovRange)
            {
                enemyUnitType1.currentState = EnemyCurrentState.Chase;
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, enemyUnitType1.moveSpeed * Time.deltaTime);
                currentDirection = facePlayer.DirectionCheck(transform.position, player.transform.position);
                facePlayer.AnimationDirCheck(currentDirection, "Move",animator);
                if (Vector3.Distance(transform.position, player.transform.position) <= enemyUnitType1.attackRange)
                {
                    state = TaskStatus.Success;
                    return state;
                }
                else
                {
                    state = TaskStatus.Running;
                    return state;
                }
            }
            else if (dis > enemyUnitType1.maxFovRange)
            {
                enemyUnitType1.currentState = EnemyCurrentState.Idle;
                enemyUnitType1.isAttackState = false;
                state = TaskStatus.Failure;
                return state;
            }

            state = TaskStatus.Running;
            return state;
        }
    }
}


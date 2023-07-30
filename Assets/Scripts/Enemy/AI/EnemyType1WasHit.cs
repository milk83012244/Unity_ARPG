using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sx.EnemyAI
{
    /// <summary>
    /// 被攻擊狀態 負責狀態與演出管理 傷害計算在攻擊方
    /// </summary>
    public class EnemyType1WasHit : EnemyType1Action
    {
        public override TaskStatus OnUpdate()
        {
            if (enemyUnitType1.currentState == EnemyCurrentState.Stunning || enemyUnitType1.currentState == EnemyCurrentState.Stop || enemyUnitType1.currentState == EnemyCurrentState.Attack) 
            {
                //無法行動狀態
                //緩速效果
                state = TaskStatus.Success;
                return state;
            }
            else
            {
                state = TaskStatus.Failure;
                return state;
            }
        }
    }
}


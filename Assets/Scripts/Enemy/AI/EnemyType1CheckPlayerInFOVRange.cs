using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sx.EnemyAI
{
    /// <summary>
    /// 索敵範圍
    /// </summary>
    public class EnemyType1CheckPlayerInFOVRange : EnemyType1Action
    {
        private int playerLayerMask = 1 << 7; //0000位元位移
        public override TaskStatus OnUpdate()
        {
            if (enemyUnitType1.currentState == EnemyCurrentState.Stunning || enemyUnitType1.currentState == EnemyCurrentState.Stop) //無法行動狀態
            {
                state = TaskStatus.Failure;
                return state;
            }

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, enemyUnitType1.maxFovRange, playerLayerMask);

            if (colliders.Length > 0)
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
    }
}


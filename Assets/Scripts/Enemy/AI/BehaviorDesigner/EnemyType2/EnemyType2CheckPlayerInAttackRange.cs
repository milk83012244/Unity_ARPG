using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sx.EnemyAI
{
    /// <summary>
    /// 檢查目標是否在攻擊範圍
    /// </summary>
    public class EnemyType2CheckPlayerInAttackRange : EnemyType2Action
    {
        public override TaskStatus OnUpdate()
        {
            if (selfStats.CurrnetHealth <= 0)
            {
                state = TaskStatus.Failure;
                return state;
            }

            if (player == null)
            {
                state = TaskStatus.Failure;
                return state;
            }

            if (Vector3.Distance(transform.position, player.transform.position) <= enemyUnitType2.attackRange)
            {
                enemyUnitType2.inAttackRange = true;
                state = TaskStatus.Success;
                return state;
            }
            else
            {
                enemyUnitType2.inAttackRange = false;
            }

            state = TaskStatus.Running;
            return state;
        }
    }
}


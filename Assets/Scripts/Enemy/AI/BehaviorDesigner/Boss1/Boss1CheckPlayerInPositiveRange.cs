using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sx.EnemyAI
{
    /// <summary>
    /// Boss1積極範圍檢測(Boss房全範圍索敵)
    /// 積極行為:主動攻擊機率提升,會主動靠近玩家進攻,近身攻擊機率提高
    /// </summary>
    public class Boss1CheckPlayerInPositiveRange : Boss1Action
    {
        public override TaskStatus OnUpdate()
        {
            if (enemyBoss1Unit.currentState == EnemyCurrentState.Stunning || enemyBoss1Unit.currentState == EnemyCurrentState.Stop || enemyBoss1Unit.currentState == EnemyCurrentState.Dead) //無法行動狀態
            {
                state = TaskStatus.Failure;
                return state;
            }
            float dis = Vector3.Distance(transform.position, player.transform.position);

            if (dis < enemyBoss1Unit.minFovRange)
            {
                isPositiveRange = true;
                state = TaskStatus.Success; //進入積極範圍行為
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


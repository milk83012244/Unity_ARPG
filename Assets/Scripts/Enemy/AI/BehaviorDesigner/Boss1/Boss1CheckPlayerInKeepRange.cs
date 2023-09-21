using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sx.EnemyAI
{
    /// <summary>
    /// Boss1保守範圍檢測(Boss房全範圍索敵)
    /// 保守行為:主動近身攻擊機率下降,低機率主動靠近玩家進攻,遠程攻擊機率上升
    /// </summary>
    public class Boss1CheckPlayerInKeepRange : Boss1Action
    {
        public override TaskStatus OnUpdate()
        {
            if (enemyBoss1Unit.currentState == EnemyCurrentState.Stunning || enemyBoss1Unit.currentState == EnemyCurrentState.Stop || enemyBoss1Unit.currentState == EnemyCurrentState.Dead) //無法行動狀態
            {
                state = TaskStatus.Failure;
                return state;
            }

            float dis = Vector3.Distance(transform.position, player.transform.position);

            if (dis > enemyBoss1Unit.minFovRange && dis < enemyBoss1Unit.maxFovRange)
            {
                isPositiveRange = false;
                state = TaskStatus.Success; //進入保守範圍行為
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


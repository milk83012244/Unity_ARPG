using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;


namespace Sx.EnemyAI
{
    /// <summary>
    /// 超過索敵範圍的距離檢測
    /// </summary>
    public class Boss1NormalMoveFOVCheck : Boss1Action
    {
        public override TaskStatus OnUpdate()
        {
            if (enemyBoss1Unit.currentState == EnemyCurrentState.Stunning || enemyBoss1Unit.currentState == EnemyCurrentState.Stop || enemyBoss1Unit.currentState == EnemyCurrentState.Dead) //無法行動狀態
            {
                state = TaskStatus.Failure;
                return state;
            }

            float dis = Vector3.Distance(transform.position, player.transform.position);
            StopAIPath();

            if (dis > enemyBoss1Unit.maxFovRange)
            {
                state = TaskStatus.Success; 
                return state;
            }
            else if (dis < enemyBoss1Unit.maxFovRange)
            {
                state = TaskStatus.Failure;
                return state;
            }
            state = TaskStatus.Running;
            return state;
        }
    }
}


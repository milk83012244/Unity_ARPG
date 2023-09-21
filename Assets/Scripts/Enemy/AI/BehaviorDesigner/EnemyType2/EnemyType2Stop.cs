using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

namespace Sx.EnemyAI
{
    /// <summary>
    /// 停止行動狀態
    /// </summary>
    public class EnemyType2Stop : EnemyType2Action
    {
        public override TaskStatus OnUpdate()
        {
            if (selfStats.CurrnetHealth <= 0)
            {
                state = TaskStatus.Failure;
                return state;
            }

            if (enemyUnitType2.currentState == EnemyCurrentState.Stunning)//硬直狀態
            {
                StopAIPath(); //停止尋路功能移動
                animator.Play(name + "_F_Stun");
                state = TaskStatus.Success;
                return state;
            }
            else if (enemyUnitType2.currentState == EnemyCurrentState.Stop) //其他停止狀態
            {
                if (enemyUnitType2.status2ActiveDic[ElementType.Ice])//凍結狀態
                {
                    StopAIPath();
                    animator.Play(name + "_F_Stun");
                    state = TaskStatus.Success;
                    return state;
                }
                else
                {
                    StartAIPath();
                    state = TaskStatus.Failure;
                    return state;
                }
            }
            else
            {
                StartAIPath();
                state = TaskStatus.Failure;
                return state;
            }
        }
    }
}


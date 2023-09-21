using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

namespace Sx.EnemyAI
{
    public class Boss1Stop : Boss1Action
    {
        public override TaskStatus OnUpdate()
        {
            if (selfStats.CurrnetHealth <= 0)
            {
                state = TaskStatus.Failure;
                return state;
            }
            if (enemyBoss1Unit.currentState == EnemyCurrentState.Stunning) //硬直狀態
            {
                StopAIPath(); //停止尋路功能移動
                animator.Play(name + "_SL_Hit");
                state = TaskStatus.Success;
                return state;
            }
            //else if (enemyBoss1Unit.currentState == EnemyCurrentState.Stop) //其他停止狀態
            //{

            //}
            else
            {
                StartAIPath();
                state = TaskStatus.Failure;
                return state;
            }
        }
    }
}


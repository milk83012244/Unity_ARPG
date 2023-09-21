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
            if (enemyBoss1Unit.currentState == EnemyCurrentState.Stunning) //�w�����A
            {
                StopAIPath(); //����M���\�ಾ��
                animator.Play(name + "_SL_Hit");
                state = TaskStatus.Success;
                return state;
            }
            //else if (enemyBoss1Unit.currentState == EnemyCurrentState.Stop) //��L����A
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


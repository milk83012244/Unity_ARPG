using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

namespace Sx.EnemyAI
{
    /// <summary>
    /// �����ʪ��A
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

            if (enemyUnitType2.currentState == EnemyCurrentState.Stunning)//�w�����A
            {
                StopAIPath(); //����M���\�ಾ��
                animator.Play(name + "_F_Stun");
                state = TaskStatus.Success;
                return state;
            }
            else if (enemyUnitType2.currentState == EnemyCurrentState.Stop) //��L����A
            {
                if (enemyUnitType2.status2ActiveDic[ElementType.Ice])//�ᵲ���A
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


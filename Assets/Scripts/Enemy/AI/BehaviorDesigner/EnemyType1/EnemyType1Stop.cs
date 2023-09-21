using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sx.EnemyAI
{
    /// <summary>
    /// �����ʪ��A
    /// </summary>
    public class EnemyType1Stop : EnemyType1Action
    {
        public override TaskStatus OnUpdate()
        {
            if (selfStats.CurrnetHealth <= 0)
            {
                state = TaskStatus.Failure;
                return state;
            }

            if (enemyUnitType1.currentState == EnemyCurrentState.Stunning)
            {
                //enemyUnitType1.StopMove();
                StopAIPath(); //����M���\�ಾ��
                animator.Play(name + "_F_Stun");
                state = TaskStatus.Success;
                return state;
            }
            else if (enemyUnitType1.currentState == EnemyCurrentState.Stop)
            {
                if (enemyUnitType1.status2ActiveDic[ElementType.Ice])//�ᵲ���A
                {
                    //enemyUnitType1.StopMove();
                    StopAIPath();
                    animator.Play(name + "_F_Stun");
                    state = TaskStatus.Success;
                    return state;
                }
                else
                {
                    //enemyUnitType1.StartMove();
                    StartAIPath();
                    state = TaskStatus.Failure;
                    return state;
                }
            }
            else
            {
                //enemyUnitType1.StartMove();
                StartAIPath();
                state = TaskStatus.Failure;
                return state;
            }

        }
    }
}


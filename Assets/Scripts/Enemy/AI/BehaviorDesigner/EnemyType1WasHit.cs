using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sx.EnemyAI
{
    /// <summary>
    /// �Q�������A �t�d���A�P�t�X�޲z �ˮ`�p��b������
    /// </summary>
    public class EnemyType1WasHit : EnemyType1Action
    {
        public override TaskStatus OnUpdate()
        {
            if (enemyUnitType1.currentState == EnemyCurrentState.Stunning || enemyUnitType1.currentState == EnemyCurrentState.Stop || enemyUnitType1.currentState == EnemyCurrentState.Attack) 
            {
                //�L�k��ʪ��A
                //�w�t�ĪG
                state = TaskStatus.Success;
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


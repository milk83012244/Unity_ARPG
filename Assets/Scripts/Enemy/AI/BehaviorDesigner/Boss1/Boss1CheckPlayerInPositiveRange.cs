using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sx.EnemyAI
{
    /// <summary>
    /// Boss1�n���d���˴�(Boss�Х��d�����)
    /// �n���欰:�D�ʧ������v����,�|�D�ʾa�񪱮a�i��,�񨭧������v����
    /// </summary>
    public class Boss1CheckPlayerInPositiveRange : Boss1Action
    {
        public override TaskStatus OnUpdate()
        {
            if (enemyBoss1Unit.currentState == EnemyCurrentState.Stunning || enemyBoss1Unit.currentState == EnemyCurrentState.Stop || enemyBoss1Unit.currentState == EnemyCurrentState.Dead) //�L�k��ʪ��A
            {
                state = TaskStatus.Failure;
                return state;
            }
            float dis = Vector3.Distance(transform.position, player.transform.position);

            if (dis < enemyBoss1Unit.minFovRange)
            {
                isPositiveRange = true;
                state = TaskStatus.Success; //�i�J�n���d��欰
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


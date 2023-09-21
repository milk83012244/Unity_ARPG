using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sx.EnemyAI
{
    /// <summary>
    /// Boss1�O�u�d���˴�(Boss�Х��d�����)
    /// �O�u�欰:�D�ʪ񨭧������v�U��,�C���v�D�ʾa�񪱮a�i��,���{�������v�W��
    /// </summary>
    public class Boss1CheckPlayerInKeepRange : Boss1Action
    {
        public override TaskStatus OnUpdate()
        {
            if (enemyBoss1Unit.currentState == EnemyCurrentState.Stunning || enemyBoss1Unit.currentState == EnemyCurrentState.Stop || enemyBoss1Unit.currentState == EnemyCurrentState.Dead) //�L�k��ʪ��A
            {
                state = TaskStatus.Failure;
                return state;
            }

            float dis = Vector3.Distance(transform.position, player.transform.position);

            if (dis > enemyBoss1Unit.minFovRange && dis < enemyBoss1Unit.maxFovRange)
            {
                isPositiveRange = false;
                state = TaskStatus.Success; //�i�J�O�u�d��欰
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


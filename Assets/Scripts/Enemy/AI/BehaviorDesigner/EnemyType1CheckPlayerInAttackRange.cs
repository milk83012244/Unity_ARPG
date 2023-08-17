using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sx.EnemyAI
{
    /// <summary>
    /// �ˬd�ؼЬO�_�b�����d��
    /// </summary>
    public class EnemyType1CheckPlayerInAttackRange : EnemyType1Action
    {
        public override TaskStatus OnUpdate()
        {
            if (selfStats.CurrnetHealth <= 0||!enemyUnitType1.isAttackState)
            {
                state = TaskStatus.Failure;
                return state;
            }

            if (player == null)
            {
                state = TaskStatus.Failure;
                return state;
            }

            if (Vector3.Distance(transform.position, player.transform.position) <= enemyUnitType1.attackRange)
            {
                state = TaskStatus.Success;
                return state;
            }
            state = TaskStatus.Running;
            return state;
        }
    }



}

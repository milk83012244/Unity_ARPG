using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sx.EnemyAI
{
    /// <summary>
    /// 檢查目標是否在攻擊範圍
    /// </summary>
    public class CheckPlayerInAttackRange : EnemyAction
    {
        public override void OnStart()
        {
            base.OnStart();
        }
        public override TaskStatus OnUpdate()
        {
            if (selfStats.CurrnetHealth <= 0)
            {
                state = TaskStatus.Failure;
                return state;
            }

            if (player == null)
            {
                state = TaskStatus.Failure;
                return state;
            }

            if (Vector3.Distance(transform.position, player.transform.position) <= enemyUnits.attackRange)
            {
                // animator.Play("Slime_Blue_SL_Attack");
                state = TaskStatus.Success;
                return state;
            }
            state = TaskStatus.Failure;
            return state;
        }
        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
        }
        public override void OnEnd()
        {
            base.OnEnd();
        }
    }



}

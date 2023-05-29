using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sx.EnemyAI
{
    public class Dead : EnemyAction
    {
        public override TaskStatus OnUpdate()
        {
            if (selfStats.CurrnetHealth <= 0)
            {
                animator.Play("Slime_Blue_SL_Die");

                enemyUnits.DestroySelf();

                state = TaskStatus.Success;
                return state;
            }
            else
            {
                state = TaskStatus.Running;
                return state;
            }
        }
    }
}


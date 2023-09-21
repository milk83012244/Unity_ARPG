using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

namespace Sx.EnemyAI
{
    public class Boss1Dead : Boss1Action
    {
        public override TaskStatus OnUpdate()
        {
            if (selfStats.CurrnetHealth <= 0)
            {
                //animator.Play(name + "_SL_Die");

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


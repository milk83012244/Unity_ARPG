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
                if (!enemyBoss1Unit.isDown)
                {
                    animator.Play(name + "_F_Down");
                    enemyBoss1Unit.StartDeadCor();
                }
                state = TaskStatus.Running;
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


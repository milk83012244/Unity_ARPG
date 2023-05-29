using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sx.EnemyAI
{
    public class GoToTarget : EnemyAction
    {
        public override TaskStatus OnUpdate()
        {
            if (selfStats.CurrnetHealth <= 0)
            {
                state = TaskStatus.Failure;
                return state;
            }

            float dis = Vector3.Distance(transform.position, player.transform.position);
            enemyUnits.StartMove();
            if (dis > 0.01f && dis <= enemyUnits.fovRange)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, enemyUnits.speed * Time.deltaTime);
                currentDirection = facePlayer.DirectionCheck(transform.position, player.transform.position);
                facePlayer.AnimationDirCheck(currentDirection, "Move",animator);
            }
            else if (dis > enemyUnits.fovRange)
            {
                state = TaskStatus.Failure;
                return state;
            }

            state = TaskStatus.Running;
            return state;
        }
    }
}


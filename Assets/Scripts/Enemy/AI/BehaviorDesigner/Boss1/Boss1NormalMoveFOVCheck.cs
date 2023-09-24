using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;


namespace Sx.EnemyAI
{
    /// <summary>
    /// �W�L���Ľd�򪺶Z���˴�
    /// </summary>
    public class Boss1NormalMoveFOVCheck : Boss1Action
    {
        public override TaskStatus OnUpdate()
        {
            if (enemyBoss1Unit.currentState == EnemyCurrentState.Stunning || enemyBoss1Unit.currentState == EnemyCurrentState.Stop || enemyBoss1Unit.currentState == EnemyCurrentState.Dead) //�L�k��ʪ��A
            {
                state = TaskStatus.Failure;
                return state;
            }

            float dis = Vector3.Distance(transform.position, player.transform.position);
            StopAIPath();

            if (dis > enemyBoss1Unit.maxFovRange)
            {
                state = TaskStatus.Success; 
                return state;
            }
            else if (dis < enemyBoss1Unit.maxFovRange)
            {
                state = TaskStatus.Failure;
                return state;
            }
            state = TaskStatus.Running;
            return state;
        }
    }
}


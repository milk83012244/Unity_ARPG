using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sx.EnemyAI
{
    public class EnemyType2CheckPlayerInFOVRange : EnemyType2Action
    {
        private int playerLayerMask = 1 << 7; //0000位元位移

        public override TaskStatus OnUpdate()
        {
            if (enemyUnitType2.currentState == EnemyCurrentState.Stunning || enemyUnitType2.currentState == EnemyCurrentState.Stop|| enemyUnitType2.currentState == EnemyCurrentState.Dead) //無法行動狀態
            {
                state = TaskStatus.Failure;
                return state;
            }

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, enemyUnitType2.maxFovRange, playerLayerMask);

            if (colliders.Length > 0)
            {
                if (!enemyUnitType2.inAttackRange)
                {
                    enemyUnitType2.EncounterPlayer();
                }

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


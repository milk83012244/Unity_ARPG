using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sx.EnemyAI
{
    public class CheckPlayerInFOVRange : EnemyAction
    {
        private  int playerLayerMask = 1 << 7; //0000¦ì¤¸¦ì²¾

        public override TaskStatus OnUpdate()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, enemyUnits.fovRange, playerLayerMask);

            if (colliders.Length > 0)
            {
                state = TaskStatus.Success;
                return state;
            }

            state = TaskStatus.Failure;
            return state;

            //state = TaskStatus.Success;
            //return state;
        }
    }
}


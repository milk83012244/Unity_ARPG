using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sx.EnemyAI
{
    public class Attack : EnemyAction
    {
        private float attackTime = 1f;
        private float attackCounter = 0f;

        public override TaskStatus OnUpdate()
        {
            if (selfStats.CurrnetHealth <= 0)
            {
                state = TaskStatus.Failure;
                return state;
            }

            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            currentDirection = facePlayer.DirectionCheck(transform.position, player.transform.position);

            attackCounter += Time.deltaTime;
            if (attackCounter >= attackTime)
            {
                TestUnit.StopMove();
                facePlayer.AnimationDirCheck(currentDirection, "Attack",animator);
                animator.Play("Slime_Blue_SL_Attack");
                //�y���ˮ`�b�O���}���˴�

                attackCounter = 0f;
            }
            if (stateInfo.normalizedTime < 0.95f)
            {
                state = TaskStatus.Running;
                return state;
            }
            if ((stateInfo.IsName("Slime_Blue_SL_Attack") || stateInfo.IsName("Slime_Blue_SR_Attack")) && stateInfo.normalizedTime >= 0.95f)
            {
                enemyUnits.StartMove();
                facePlayer.AnimationDirCheck(currentDirection, "Move",animator);
            }

            //�M���ؼЦC�� �ê�^��L���A
            //ClearData("target");
            //_animator.Play("Slime_Blue_SL_Move");

            state = TaskStatus.Running;
            return state;
        }
    }
}


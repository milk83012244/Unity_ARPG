using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sx.EnemyAI
{
    public class EnemyType2GoToTarget : EnemyType2Action
    {
        /// <summary>
        /// �l�v���A
        /// </summary>
        public override TaskStatus OnUpdate()
        {
            if (enemyUnitType2.currentState == EnemyCurrentState.Stunning || enemyUnitType2.currentState == EnemyCurrentState.Stop|| enemyUnitType2.currentState == EnemyCurrentState.Dead) //�L�k��ʪ��A
            {
                StopAIPath();
                state = TaskStatus.Failure;
                return state;
            }
            if (selfStats.CurrnetHealth <= 0)
            {
                StopAIPath();
                state = TaskStatus.Failure;
                return state;
            }
            //�M������]�w�l�v�ؼ�
            aiDestinationSetter.target = player.transform;
            float dis = Vector3.Distance(transform.position, player.transform.position);

            //�Ӿa��Զ}�d��
            if (dis < enemyUnitType2.minFovRange)
            {
                StartAIPath();
                Vector3 moveDirection = transform.position - player.transform.position;
                moveDirection.Normalize();
                transform.Translate(moveDirection * enemyUnitType2.moveSpeed * Time.deltaTime);
            }
            //�˴��l�v�d��
            if (dis > enemyUnitType2.minFovRange && dis <= enemyUnitType2.maxFovRange)
            {
                enemyUnitType2.currentState = EnemyCurrentState.Chase;
                //StartAIPath();
                //���ʥ浹�M������
                //currentDirection = facePlayer.DirectionCheck(transform.position, player.transform.position);
                //facePlayer.AnimationDirCheck(currentDirection, "Move", animator);

                //�˴��i�J�����d��
                if (Vector3.Distance(transform.position, player.transform.position) <= enemyUnitType2.attackRange)
                {
                    //enemyUnitType2.inAttackRange = true;
                    //aIPath.maxSpeed = 0; //�����
                    StopAIPath();
                    state = TaskStatus.Success;
                    return state;
                }
                else
                {
                    StartAIPath();
                    //aIPath.maxSpeed = enemyUnitType2.tempSpeed;
                    currentDirection = facePlayer.DirectionCheck(transform.position, player.transform.position);
                    facePlayer.AnimationDirCheck(currentDirection, "Move", animator);
                    state = TaskStatus.Running;
                    return state;
                }
            }
            else if (dis > enemyUnitType2.maxFovRange) //�W�X�d�򰱤�l�v
            {
                aiDestinationSetter.target = null;
                StopAIPath();
                enemyUnitType2.currentState = EnemyCurrentState.Idle;
                state = TaskStatus.Failure;
                return state;
            }
            state = TaskStatus.Running;
            return state;
        }
    }
}


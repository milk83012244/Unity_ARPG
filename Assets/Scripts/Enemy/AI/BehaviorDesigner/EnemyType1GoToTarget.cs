using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sx.EnemyAI
{
    /// <summary>
    /// �l�v���A
    /// </summary>
    public class EnemyType1GoToTarget : EnemyType1Action
    {
        //private int playerLayerMask = 1 << 7; //0000�줸�첾
        public override TaskStatus OnUpdate()
        {
            if (!enemyUnitType1.isAttackState)
            {
                StopAIPath();
                state = TaskStatus.Failure;
                return state;
            }
            if (enemyUnitType1.currentState == EnemyCurrentState.Stunning|| enemyUnitType1.currentState == EnemyCurrentState.Stop) //�L�k��ʪ��A
            {
                StopAIPath();
                state = TaskStatus.Failure;
                return state;
            }
            if (selfStats.CurrnetHealth <= 0 || !enemyUnitType1.isAttackState)
            {
                StopAIPath();
                state = TaskStatus.Failure;
                return state;
            }
            //�M������]�w�l�v�ؼ�
            aiDestinationSetter.target = player.transform;
            float dis = Vector3.Distance(transform.position, player.transform.position);

            //�Ӿa��Զ}�d��
            if (dis < enemyUnitType1.minFovRange)
            {
                StartAIPath();
                Vector3 moveDirection = transform.position - player.transform.position;
                moveDirection.Normalize();
                transform.Translate(moveDirection * enemyUnitType1.moveSpeed * Time.deltaTime);
                //transform.position = Vector3.MoveTowards(transform.position, -player.transform.position, enemyUnitType1.moveSpeed * Time.deltaTime);
            }
            //�˴��l�v�d��
            if (dis > enemyUnitType1.minFovRange && dis <= enemyUnitType1.maxFovRange)
            {
                enemyUnitType1.currentState = EnemyCurrentState.Chase;
                StartAIPath();
                //���ʥ浹�M������
                currentDirection = facePlayer.DirectionCheck(transform.position, player.transform.position);
                facePlayer.AnimationDirCheck(currentDirection, "Move", animator);

                //�˴��i�J�����d��
                if (Vector3.Distance(transform.position, player.transform.position) <= enemyUnitType1.attackRange)
                {
                    aIPath.maxSpeed = 0; //�����
                    state = TaskStatus.Success;
                    return state;
                }
                else
                {
                    aIPath.maxSpeed = enemyUnitType1.tempSpeed;
                    state = TaskStatus.Running;
                    return state;
                }
            }
            else if (dis > enemyUnitType1.maxFovRange) //�W�X�d�򰱤�l�v
            {
                aiDestinationSetter.target = null;
                StopAIPath();
                enemyUnitType1.currentState = EnemyCurrentState.Idle;
                enemyUnitType1.isAttackState = false;
                state = TaskStatus.Failure;
                return state;
            }


            //�쥻�l�v�ؼФ�k
            //float dis = Vector3.Distance(transform.position, player.transform.position);
            //enemyUnitType1.StartMove();
            //if (dis > enemyUnitType1.minFovRange && dis <= enemyUnitType1.maxFovRange)
            //{
            //    enemyUnitType1.currentState = EnemyCurrentState.Chase;
            //    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, enemyUnitType1.moveSpeed * Time.deltaTime);
            //    currentDirection = facePlayer.DirectionCheck(transform.position, player.transform.position);
            //    facePlayer.AnimationDirCheck(currentDirection, "Move",animator);
            //    if (Vector3.Distance(transform.position, player.transform.position) <= enemyUnitType1.attackRange)
            //    {
            //        state = TaskStatus.Success;
            //        return state;
            //    }
            //    else
            //    {
            //        state = TaskStatus.Running;
            //        return state;
            //    }
            //}
            //else if (dis > enemyUnitType1.maxFovRange)
            //{
            //    enemyUnitType1.currentState = EnemyCurrentState.Idle;
            //    enemyUnitType1.isAttackState = false;
            //    state = TaskStatus.Failure;
            //    return state;
            //}

            state = TaskStatus.Running;
            return state;
        }
    }
}


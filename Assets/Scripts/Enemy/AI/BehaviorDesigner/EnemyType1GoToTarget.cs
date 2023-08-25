using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sx.EnemyAI
{
    /// <summary>
    /// 追逐狀態
    /// </summary>
    public class EnemyType1GoToTarget : EnemyType1Action
    {
        //private int playerLayerMask = 1 << 7; //0000位元位移
        public override TaskStatus OnUpdate()
        {
            if (!enemyUnitType1.isAttackState)
            {
                StopAIPath();
                state = TaskStatus.Failure;
                return state;
            }
            if (enemyUnitType1.currentState == EnemyCurrentState.Stunning|| enemyUnitType1.currentState == EnemyCurrentState.Stop) //無法行動狀態
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
            //尋路插件設定追逐目標
            aiDestinationSetter.target = player.transform;
            float dis = Vector3.Distance(transform.position, player.transform.position);

            //太靠近拉開範圍
            if (dis < enemyUnitType1.minFovRange)
            {
                StartAIPath();
                Vector3 moveDirection = transform.position - player.transform.position;
                moveDirection.Normalize();
                transform.Translate(moveDirection * enemyUnitType1.moveSpeed * Time.deltaTime);
                //transform.position = Vector3.MoveTowards(transform.position, -player.transform.position, enemyUnitType1.moveSpeed * Time.deltaTime);
            }
            //檢測追逐範圍
            if (dis > enemyUnitType1.minFovRange && dis <= enemyUnitType1.maxFovRange)
            {
                enemyUnitType1.currentState = EnemyCurrentState.Chase;
                StartAIPath();
                //移動交給尋路插件
                currentDirection = facePlayer.DirectionCheck(transform.position, player.transform.position);
                facePlayer.AnimationDirCheck(currentDirection, "Move", animator);

                //檢測進入攻擊範圍
                if (Vector3.Distance(transform.position, player.transform.position) <= enemyUnitType1.attackRange)
                {
                    aIPath.maxSpeed = 0; //停止移動
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
            else if (dis > enemyUnitType1.maxFovRange) //超出範圍停止追逐
            {
                aiDestinationSetter.target = null;
                StopAIPath();
                enemyUnitType1.currentState = EnemyCurrentState.Idle;
                enemyUnitType1.isAttackState = false;
                state = TaskStatus.Failure;
                return state;
            }


            //原本追逐目標方法
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


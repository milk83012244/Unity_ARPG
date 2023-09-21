using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sx.EnemyAI
{
    public class EnemyType2GoToTarget : EnemyType2Action
    {
        /// <summary>
        /// 追逐狀態
        /// </summary>
        public override TaskStatus OnUpdate()
        {
            if (enemyUnitType2.currentState == EnemyCurrentState.Stunning || enemyUnitType2.currentState == EnemyCurrentState.Stop|| enemyUnitType2.currentState == EnemyCurrentState.Dead) //無法行動狀態
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
            //尋路插件設定追逐目標
            aiDestinationSetter.target = player.transform;
            float dis = Vector3.Distance(transform.position, player.transform.position);

            //太靠近拉開範圍
            if (dis < enemyUnitType2.minFovRange)
            {
                StartAIPath();
                Vector3 moveDirection = transform.position - player.transform.position;
                moveDirection.Normalize();
                transform.Translate(moveDirection * enemyUnitType2.moveSpeed * Time.deltaTime);
            }
            //檢測追逐範圍
            if (dis > enemyUnitType2.minFovRange && dis <= enemyUnitType2.maxFovRange)
            {
                enemyUnitType2.currentState = EnemyCurrentState.Chase;
                //StartAIPath();
                //移動交給尋路插件
                //currentDirection = facePlayer.DirectionCheck(transform.position, player.transform.position);
                //facePlayer.AnimationDirCheck(currentDirection, "Move", animator);

                //檢測進入攻擊範圍
                if (Vector3.Distance(transform.position, player.transform.position) <= enemyUnitType2.attackRange)
                {
                    //enemyUnitType2.inAttackRange = true;
                    //aIPath.maxSpeed = 0; //停止移動
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
            else if (dis > enemyUnitType2.maxFovRange) //超出範圍停止追逐
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


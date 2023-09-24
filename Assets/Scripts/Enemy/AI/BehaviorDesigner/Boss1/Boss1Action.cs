using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Pathfinding;

namespace Sx.EnemyAI
{
    /// <summary>
    /// Boss1:Boss房為固定空間 全範圍內都會主動攻擊,但是離玩家超過一定距離會進入保守模式
    /// 行為:進入攻擊範圍時,會隨機避開或是發動攻擊,保守模式時發動攻擊的頻率會比較低
    /// </summary>
    public class Boss1Action : EnemyPublicAction
    {
        protected EnemyBoss1Unit enemyBoss1Unit;

        public override void OnAwake()
        {
            player = PlayerController.GetInstance();
            body = GetComponent<Rigidbody2D>();
            enemyBoss1Unit = GetComponent<EnemyBoss1Unit>();
            facePlayer = GetComponent<FacePlayer>();
            selfStats = GetComponent<OtherCharacterStats>();
            animator = this.gameObject.GetComponentInChildren<Animator>();

            aiDestinationSetter = GetComponent<AIDestinationSetter>();
            //patrolAI = GetComponent<EnemyType1PartolAI>();
            aIPath = GetComponent<AIPath>();

            attackType = selfStats.enemyBattleData.attackType;

            name = selfStats.enemyBattleData.characterName;
        }

        protected virtual void StopAIMove()
        {
            aIPath.canMove = false;
        }
        protected virtual void StartAIMove()
        {
            aIPath.canMove = true;
        }
    }
}


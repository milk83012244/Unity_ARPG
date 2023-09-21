using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Pathfinding;

namespace Sx.EnemyAI
{
    /// <summary>
    /// 敵人行為樹事件節點用的資料 給事件節點繼承 並初始化敵人的組件 
    /// 類型2:玩家在索敵範圍內主動攻擊 超過追擊範圍就會停止攻擊回到預設位置
    /// </summary>
    public class EnemyType2Action : EnemyPublicAction
    {
        protected EnemyUnitType2 enemyUnitType2;

        public override void OnAwake()
        {
            player = PlayerController.GetInstance();

            body = GetComponent<Rigidbody2D>();
            enemyUnitType2 = GetComponent<EnemyUnitType2>();
            facePlayer = GetComponent<FacePlayer>();
            selfStats = GetComponent<OtherCharacterStats>();
            animator = this.gameObject.GetComponentInChildren<Animator>();

            aiDestinationSetter = GetComponent<AIDestinationSetter>();
            patrolAI = GetComponent<EnemyType1PartolAI>();
            aIPath = GetComponent<AIPath>();

            attackType = selfStats.enemyBattleData.attackType;

            name = selfStats.enemyBattleData.characterName;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sx.EnemyAI
{
    /// <summary>
    /// 敵人事件節點用的資料 給事件節點繼承 並初始化敵人的組件 
    /// 類型1:不主動攻擊 受到攻擊才開始攻擊 超過追擊範圍就會停止攻擊回到預設位置
    /// </summary>
    public class EnemyType1Action : Action
    {
        protected PlayerController player;

        protected Rigidbody2D body;
        protected Animator animator;
        protected EnemyUnitType1 enemyUnitType1;
        protected FacePlayer facePlayer;
        protected OtherCharacterStats selfStats;
        protected string name;
        protected TaskStatus state;

        protected int currentDirection=0;

        public override void OnAwake()
        {
            player = PlayerController.GetInstance();

            body = GetComponent<Rigidbody2D>();
            enemyUnitType1 = GetComponent<EnemyUnitType1>();
            facePlayer = GetComponent<FacePlayer>();
            selfStats = GetComponent<OtherCharacterStats>();
            animator = this.gameObject.GetComponentInChildren<Animator>();

            name = selfStats.enemyBattleData.characterName;
        }
    }
}


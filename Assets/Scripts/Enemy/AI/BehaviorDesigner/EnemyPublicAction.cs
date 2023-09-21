using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Pathfinding;


namespace Sx.EnemyAI
{
    /// <summary>
    /// 敵人行為樹事件共用資料
    /// </summary>
    public class EnemyPublicAction : Action
    {
        //尋路插件
        protected EnemyType1PartolAI patrolAI; //巡邏
        protected AIDestinationSetter aiDestinationSetter; //指定目標
        protected AIPath aIPath; //路線移動

        protected PlayerController player;
        protected Rigidbody2D body;
        protected Animator animator;
        protected FacePlayer facePlayer;
        protected OtherCharacterStats selfStats;
        protected string name;
        protected TaskStatus state;
        protected AttackType attackType; //攻擊類型

        protected int currentDirection = 0;

        /// <summary>
        /// 開啟AI尋路移動功能並開始搜尋路徑
        /// </summary>
        public virtual void StartAIPath()
        {
            aIPath.isStopped = false;
            aIPath.canSearch = true;
        }
        /// <summary>
        /// 停止AI尋路移動功能且停止搜尋路徑
        /// </summary>
        public virtual void StopAIPath()
        {
            aIPath.isStopped = true;
            aIPath.canSearch = false;
        }
    }
}



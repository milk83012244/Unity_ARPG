using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Pathfinding;

namespace Sx.EnemyAI
{

}
/// <summary>
/// 敵人事件節點用的資料 給事件節點繼承 並初始化敵人的組件 
/// 類型2:玩家在索敵範圍內主動攻擊 超過追擊範圍就會停止攻擊回到預設位置
/// </summary>
public class EnemyType2Action : Action
{
    protected EnemyType1PartolAI patrolAI; //巡邏
    protected AIDestinationSetter aiDestinationSetter; //指定目標
    protected AIPath aIPath; //路線移動

    protected PlayerController player;
    protected Rigidbody2D body;
    protected Animator animator;
    protected EnemyUnitType2 enemyUnitType2;
    protected FacePlayer facePlayer;
    protected OtherCharacterStats selfStats;
    protected string name;
    protected TaskStatus state;

    protected int currentDirection = 0;

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

        name = selfStats.enemyBattleData.characterName;
    }

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

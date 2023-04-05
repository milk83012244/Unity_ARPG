using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 角色攻擊控制管理
/// </summary>
public class MoAttackController : MonoBehaviour
{
    private int enemiesLayer = 1 << 8;
    private CharacterStatsDataMono characterStats;
    private void Awake()
    {
        characterStats = GetComponentInParent<CharacterStatsDataMono>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        characterStats.isCritical = Random.value < characterStats.testAttackData.criticalChance; //爆擊判斷
        CharacterStatsDataMono defander = collision.GetComponentInParent<CharacterStatsDataMono>();
        //攻擊目標
        characterStats.TakeDamage(characterStats, defander);
        defander.GetComponentInParent<TestUnit>().SpawnDamageText(characterStats.currentDamage);
    }
}

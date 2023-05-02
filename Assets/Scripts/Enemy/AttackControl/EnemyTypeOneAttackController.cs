using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTypeOneAttackController : MonoBehaviour
{
    private int enemiesLayer = 1 << 8;
    private CharacterStatsDataMono characterStats;
    private void Awake()
    {
        characterStats = GetComponentInParent<CharacterStatsDataMono>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        characterStats.isCritical = Random.value < characterStats.attackData.criticalChance; //ÃzÀ»§PÂ_
        //§ðÀ»¥Ø¼Ð
        characterStats.TakeDamage(characterStats, collision.GetComponentInParent<CharacterStatsDataMutiMono>());
    }
}

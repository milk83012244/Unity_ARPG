using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ¨¤¦â§ðÀ»±±¨îºÞ²z
/// </summary>
public class MoAttackController : MonoBehaviour
{
    private int enemiesLayer = 1 << 8;
    private CharacterStatsDataMutiMono characterStats;
    private void Awake()
    {
        characterStats = GetComponentInParent<CharacterStatsDataMutiMono>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        characterStats.isCritical = Random.value < characterStats.attackData[characterStats.currentCharacterID].criticalChance; //ÃzÀ»§PÂ_
        CharacterStatsDataMono defander = collision.GetComponentInParent<CharacterStatsDataMono>(); //¼Ä¤è¨¾¿m­È
        //§ðÀ»¥Ø¼Ð
        characterStats.TakeDamage(characterStats, defander);
        defander.GetComponentInParent<TestUnit>().SpawnDamageText(characterStats.currentDamage);
    }
}

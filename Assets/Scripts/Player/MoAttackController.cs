using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����������޲z
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
        characterStats.isCritical = Random.value < characterStats.testAttackData.criticalChance; //�z���P�_
        CharacterStatsDataMono defander = collision.GetComponentInParent<CharacterStatsDataMono>();
        //�����ؼ�
        characterStats.TakeDamage(characterStats, defander);
        defander.GetComponentInParent<TestUnit>().SpawnDamageText(characterStats.currentDamage);
    }
}

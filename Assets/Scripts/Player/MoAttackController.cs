using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����������޲z
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
        characterStats.isCritical = Random.value < characterStats.attackData[characterStats.currentCharacterID].criticalChance; //�z���P�_
        CharacterStatsDataMono defander = collision.GetComponentInParent<CharacterStatsDataMono>(); //�Ĥ訾�m��
        //�����ؼ�
        characterStats.TakeDamage(characterStats, defander);
        defander.GetComponentInParent<TestUnit>().SpawnDamageText(characterStats.currentDamage);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTypeOneAttackController : MonoBehaviour
{
    private int enemiesLayer = 1 << 8;
    private OtherCharacterStats characterStats;
    private void Awake()
    {
        characterStats = GetComponentInParent<OtherCharacterStats>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        characterStats.isCritical = Random.value < characterStats.attackData.criticalChance; //�z���P�_
        //�����ؼ�
        characterStats.TakeDamage(characterStats, collision.GetComponentInParent<PlayerCharacterStats>());
    }
}

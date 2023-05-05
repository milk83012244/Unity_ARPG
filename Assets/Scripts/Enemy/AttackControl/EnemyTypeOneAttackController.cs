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
        characterStats.isCritical = Random.value < characterStats.attackData.criticalChance; //ÃzÀ»§PÂ_
        //§ðÀ»¥Ø¼Ð
        characterStats.TakeDamage(characterStats, collision.GetComponentInParent<PlayerCharacterStats>());
    }
}

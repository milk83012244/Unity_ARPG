using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin_BNormalAttack : MonoBehaviour
{
    [HideInInspector] public OtherCharacterStats characterStats;
    [HideInInspector] public EnemyUnitType2 enemyUnit;
    private EnemySpawner enemySpawner;
    private PlayerEffectSpawner playerEffectSpawner;

    public Transform FirePosition;
    [HideInInspector] public Transform PlayerPosition;

    private void Awake()
    {
        characterStats = GetComponentInParent<OtherCharacterStats>();
        enemyUnit = GetComponentInParent<EnemyUnitType2>();
        playerEffectSpawner = GetComponentInParent<PlayerEffectSpawner>();

        PlayerPosition = PlayerController.GetInstance().transform;
    }
    private void Start()
    {
        enemySpawner = enemyUnit.enemySpawner;
    }

    /// <summary>
    /// µo®g¤l¼u
    /// </summary>
    public void FireProjectile()
    {
        //Quaternion rotation = Quaternion.Euler(0f, 0f, bulletAngle);
        Goblin_B_Projectile projectile = enemySpawner.goblin_BprojectileEffectPool.Spawn(transform.position , enemySpawner.EnemyProjectilepoolParent);
        //bullet.transform.localPosition = this.transform.position;
        projectile.SetTargetPosition(PlayerPosition.position, this.transform.position);
        projectile.GetCharacterStats(characterStats);
    }
}

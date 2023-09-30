using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoCounterAttack : MonoBehaviour
{
    private PlayerCharacterStats characterStats;
    private PlayerEffectSpawner playerEffectSpawner;
    [HideInInspector]public PlayerInput playerInput;

    private bool activeMarkAttack;

    //是否有賦予屬性
    [HideInInspector] public bool fireElement;
    [HideInInspector] public bool iceElement;
    [HideInInspector] public bool windElement;
    [HideInInspector] public bool thunderElement;
    [HideInInspector] public bool lightElement;
    [HideInInspector] public bool darkElement;

    private void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        characterStats = GetComponentInParent<PlayerCharacterStats>();
        playerEffectSpawner = GetComponentInParent<PlayerEffectSpawner>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        characterStats.isCritical = Random.value < characterStats.attackData[characterStats.currentCharacterID].criticalChance;
        IDamageable damageable = collision.GetComponent<IDamageable>();

        if (damageable != null)
        {
            OtherCharacterStats defander = collision.GetComponent<OtherCharacterStats>();
            Enemy enemyUnit = defander.GetComponent<Enemy>();
            EnemyUnitType1 enemyUnitType1;
            EnemyUnitType2 enemyUnitType2;
            EnemyBoss1Unit enemyBoss1Unit;

            #region 可傷害敵人共通
            if (enemyUnit.isMarked)
            {
                enemyUnit.ClearMark();
                activeMarkAttack = true;
            }
            else
            {
                enemyUnit.SetMark(MarkType.Mo);
                activeMarkAttack = false;
            }
            if (activeMarkAttack)
            {
                characterStats.TakeMarkDamage(characterStats, defander, characterStats.isCritical);
                enemyUnit.SpawnMarkDamageText(characterStats.currentDamage, characterStats.isCritical);
                characterStats.TakeDamage(characterStats, defander, characterStats.isCritical, isCounter: true);
            }
            else
            {
                characterStats.TakeDamage(characterStats, defander, characterStats.isCritical, isCounter: true);
            }
            enemyUnit.SpawnDamageText(characterStats.currentDamage, characterStats.isCritical);

            CinemachineShake.GetInstance().ShakeCamera(0.35f, 0.15f);//攝影機震動

            SlashHitEffect slashHitEffect = playerEffectSpawner.SlashHitEffectPool.Spawn(collision.transform.position, playerEffectSpawner.effectParent);
            if (playerInput.currentDirection == 3)
            {
                slashHitEffect.transform.localScale = new Vector3(-1, 1, 1);
            }
            #endregion
            switch (enemyUnit.typeID)
            {
                case 1:
                    enemyUnitType1 = enemyUnit as EnemyUnitType1;
                    //觸發敵人受擊狀態
                    enemyUnitType1.DamageByPlayer();
                    //敵人閃爍效果
                    enemyUnitType1.StartFlash();
                    //賦予敵人硬直值
                    if (enemyUnitType1.canStun)
                        characterStats.TakeStunValue(characterStats, defander, freeMul: 1.5f);
                    break;
                case 2:
                    enemyUnitType2 = enemyUnit as EnemyUnitType2;
                    //觸發敵人受擊狀態
                    enemyUnitType2.DamageByPlayer();
                    //敵人閃爍效果
                    enemyUnitType2.StartFlash();
                    //賦予敵人硬直值
                    if (enemyUnitType2.canStun)
                        characterStats.TakeStunValue(characterStats, defander, freeMul: 1.5f);
                    break;
                case 1001:
                    enemyBoss1Unit = enemyUnit as EnemyBoss1Unit;
                    //觸發敵人受擊狀態
                    enemyBoss1Unit.DamageByPlayer();
                    //賦予敵人硬直值
                    if (enemyBoss1Unit.canStun)
                        characterStats.TakeStunValue(characterStats, defander, freeMul: 1.5f);
                    break;
            }
            //造成敵人擊退
            Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
            float knockbackValue = characterStats.attackData[characterStats.currentCharacterID].knockbackValue;
            switch (enemyUnit.typeID)
            {
                case 1:
                    enemyUnitType1 = enemyUnit as EnemyUnitType1;
                    enemyUnitType1.StartKnockback(knockbackDirection, knockbackValue *= 1.5f);
                    break;
                case 2:
                    enemyUnitType2 = enemyUnit as EnemyUnitType2;
                    enemyUnitType2.StartKnockback(knockbackDirection, knockbackValue *= 1.5f);
                    break;
                case 1001:
                    enemyBoss1Unit = enemyUnit as EnemyBoss1Unit;
                    enemyBoss1Unit.StartKnockback(knockbackDirection, knockbackValue *= 1.5f);
                    break;
            }
        }
    }
}

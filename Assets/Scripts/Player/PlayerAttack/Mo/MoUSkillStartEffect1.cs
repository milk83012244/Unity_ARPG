using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoUSkillStartEffect1 : MonoBehaviour
{
    private MoUSkillEffectSpawner moUSkillEffectSpawner;
    private PlayerCharacterStats characterStats;
    public void Init(PlayerCharacterStats characterStats,MoUSkillEffectSpawner moUSkillEffectSpawner)
    {
        this.characterStats = characterStats;
        this.moUSkillEffectSpawner = moUSkillEffectSpawner;
    }
    public void SpawnStartEffect2AnimateEvent()
    {
        if (moUSkillEffectSpawner != null)
            moUSkillEffectSpawner.SpawnStartEffect2();
    }
    public void AnimateEndEvent()
    {
        this.gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (characterStats == null)
        {
            return;
        }

        IDamageable damageable = collision.GetComponent<IDamageable>();

        if (damageable == null)
        {
            return;
        }
        else
        {
            characterStats.isCritical = UnityEngine.Random.value < characterStats.attackData[characterStats.currentCharacterID].criticalChance;

            OtherCharacterStats defander = collision.GetComponent<OtherCharacterStats>();

            if (defander != null)
            {
                Enemy enemyUnit = defander.GetComponent<Enemy>();
                EnemyUnitType1 enemyUnitType1;
                EnemyUnitType2 enemyUnitType2;
                EnemyBoss1Unit enemyBoss1Unit;
                #region 可傷害敵人共通
                characterStats.TakeDamage(characterStats, defander, characterStats.isCritical);
                enemyUnit.SpawnDamageText(characterStats.currentDamage, characterStats.isCritical);
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
                            characterStats.TakeStunValue(characterStats, defander);
                        break;
                    case 2:
                        enemyUnitType2 = enemyUnit as EnemyUnitType2;
                        //觸發敵人受擊狀態
                        enemyUnitType2.DamageByPlayer();
                        //敵人閃爍效果
                        enemyUnitType2.StartFlash();
                        //賦予敵人硬直值
                        if (enemyUnitType2.canStun)
                            characterStats.TakeStunValue(characterStats, defander);
                        break;
                    case 1001:
                        enemyBoss1Unit = enemyUnit as EnemyBoss1Unit;
                        //觸發敵人受擊狀態
                        enemyBoss1Unit.DamageByPlayer();
                        //敵人閃爍效果
                       // enemyBoss1Unit.StartFlash();
                        //賦予敵人硬直值
                        if (enemyBoss1Unit.canStun)
                            characterStats.TakeStunValue(characterStats, defander);
                        break;
                }
                //造成敵人擊退
                Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
                float knockbackValue = characterStats.attackData[characterStats.currentCharacterID].knockbackValue;
                switch (enemyUnit.typeID)
                {
                    case 1:
                        enemyUnitType1 = enemyUnit as EnemyUnitType1;
                            enemyUnitType1.StartKnockback(knockbackDirection, knockbackValue *5);
                        break;
                    case 2:
                        enemyUnitType2 = enemyUnit as EnemyUnitType2;
                            enemyUnitType2.StartKnockback(knockbackDirection, knockbackValue *5);
                        break;
                    case 1001: // boss_1
                        enemyBoss1Unit = enemyUnit as EnemyBoss1Unit;
                            enemyBoss1Unit.StartKnockback(knockbackDirection, knockbackValue *5);
                        break;
                }
            }
        }
    }
}

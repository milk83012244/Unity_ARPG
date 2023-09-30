using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiaSkill2_Damage_Wind : MonoBehaviour
{
    public LiaSkill2Effect liaSkill2Effect;

    private void DealDamage(IDamageable damageable, ElementType element)
    {
        if (element != ElementType.Wind || damageable == null)
        {
            return;
        }
        liaSkill2Effect.characterStats.isCritical = Random.value < liaSkill2Effect.characterStats.attackData[liaSkill2Effect.characterStats.currentCharacterID].criticalChance;
        if (damageable != null)
        {
            Enemy enemyUnit = damageable as Enemy;
            EnemyUnitType1 enemyUnitType1;
            EnemyUnitType2 enemyUnitType2;
            EnemyBoss1Unit enemyBoss1Unit;
            if (enemyUnit == null)
            {
                return;
            }
            OtherCharacterStats defander = enemyUnit.GetComponent<OtherCharacterStats>();

            #region 可傷害敵人共通
            liaSkill2Effect.characterStats.TakeDamage(liaSkill2Effect.characterStats, defander, ElementType.Wind, liaSkill2Effect.characterStats.isCritical, isSkill2: true);
            enemyUnit.SpawnDamageText(liaSkill2Effect.characterStats.currentDamage, ElementType.Wind, liaSkill2Effect.characterStats.isCritical);
            defander.characterElementCounter.AddElementCount(liaSkill2Effect.element, 2, liaSkill2Effect.characterStats);

            if (enemyUnit.isMarked)
            {
                liaSkill2Effect.characterStats.TakeMarkDamage(liaSkill2Effect.characterStats, defander, liaSkill2Effect.characterStats.isCritical);
                enemyUnit.SpawnMarkDamageText(liaSkill2Effect.characterStats.currentDamage, liaSkill2Effect.characterStats.isCritical);
                liaSkill2Effect.characterStats.TakeDamage(liaSkill2Effect.characterStats, defander, liaSkill2Effect.characterStats.isCritical);
                enemyUnit.SpawnDamageText(liaSkill2Effect.characterStats.currentDamage, liaSkill2Effect.characterStats.isCritical);

                enemyUnit.ClearMark();
            }
            CinemachineShake.GetInstance().ShakeCamera(0.4f, 0.2f);//攝影機震動
            #endregion

            switch (enemyUnit.typeID)
            {
                case 1:
                    enemyUnitType1 = enemyUnit as EnemyUnitType1;
                    //觸發敵人受擊狀態
                    enemyUnitType1.DamageByPlayer();
                    //敵人閃爍效果
                    enemyUnitType1.StartFlash();
                    if (enemyUnitType1.canStun)
                    {
                        //造成敵人硬直
                        liaSkill2Effect.characterStats.TakeStunValue(liaSkill2Effect.characterStats, defander, isSkill2: true);
                    }
                    break;
                case 2:
                    enemyUnitType2 = enemyUnit as EnemyUnitType2;
                    //觸發敵人受擊狀態
                    enemyUnitType2.DamageByPlayer();
                    //敵人閃爍效果
                    enemyUnitType2.StartFlash();
                    if (enemyUnitType2.canStun)
                    {
                        //造成敵人硬直
                        liaSkill2Effect.characterStats.TakeStunValue(liaSkill2Effect.characterStats, defander, isSkill2: true);
                    }
                    break;
                case 1001:
                    enemyBoss1Unit = enemyUnit as EnemyBoss1Unit;
                    //觸發敵人受擊狀態
                    enemyBoss1Unit.DamageByPlayer();
                    //敵人閃爍效果
                    //enemyBoss1Unit.StartFlash();
                    if (enemyBoss1Unit.canStun)
                    {
                        //造成敵人硬直
                        liaSkill2Effect.characterStats.TakeStunValue(liaSkill2Effect.characterStats, defander, isSkill2: true);
                    }
                    break;
            }
            Vector2 knockbackDirection;
            float knockbackValue;
            switch (enemyUnit.typeID)
            {
                case 1:
                    enemyUnitType1 = enemyUnit as EnemyUnitType1;
                    //造成敵人擊退
                    knockbackDirection = (enemyUnitType1.transform.position - transform.position).normalized;
                    knockbackValue = liaSkill2Effect.characterStats.attackData[liaSkill2Effect.characterStats.currentCharacterID].knockbackValue;
                    enemyUnitType1.StartKnockback(knockbackDirection, knockbackValue *= liaSkill2Effect.characterStats.attackData[liaSkill2Effect.characterStats.currentCharacterID].skill2knockbackValueMultplier);
                    break;
                case 2:
                    enemyUnitType2 = enemyUnit as EnemyUnitType2;
                    //造成敵人擊退
                    knockbackDirection = (enemyUnitType2.transform.position - transform.position).normalized;
                    knockbackValue = liaSkill2Effect.characterStats.attackData[liaSkill2Effect.characterStats.currentCharacterID].knockbackValue;
                    enemyUnitType2.StartKnockback(knockbackDirection, knockbackValue *= liaSkill2Effect.characterStats.attackData[liaSkill2Effect.characterStats.currentCharacterID].skill2knockbackValueMultplier);
                    break;
                case 1001:
                    enemyBoss1Unit = enemyUnit as EnemyBoss1Unit;
                    //造成敵人擊退
                    knockbackDirection = (enemyBoss1Unit.transform.position - transform.position).normalized;
                    knockbackValue = liaSkill2Effect.characterStats.attackData[liaSkill2Effect.characterStats.currentCharacterID].knockbackValue;
                    enemyBoss1Unit.StartKnockback(knockbackDirection, knockbackValue *= liaSkill2Effect.characterStats.attackData[liaSkill2Effect.characterStats.currentCharacterID].skill2knockbackValueMultplier);
                    break;
            }

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            IDamageable damageable = collision.GetComponent<IDamageable>();
            DealDamage(damageable, liaSkill2Effect.element);
        }
    }
}

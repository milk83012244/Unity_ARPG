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
            OtherCharacterStats defander = enemyUnit.GetComponent<OtherCharacterStats>();

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
            EnemyUnitType1 enemyUnitType1 = enemyUnit as EnemyUnitType1;
            //觸發敵人受擊狀態
            enemyUnitType1.DamageByPlayer();
            //敵人閃爍效果
            enemyUnitType1.StartFlash();
            //造成敵人硬直
            liaSkill2Effect.characterStats.TakeStunValue(liaSkill2Effect.characterStats, defander, isSkill2: true);
            //造成敵人擊退
            Vector2 knockbackDirection = (enemyUnitType1.transform.position - transform.position).normalized;
            float knockbackValue = liaSkill2Effect.characterStats.attackData[liaSkill2Effect.characterStats.currentCharacterID].knockbackValue;
            enemyUnitType1.StartKnockback(knockbackDirection, knockbackValue *= liaSkill2Effect.characterStats.attackData[liaSkill2Effect.characterStats.currentCharacterID].skill2knockbackValueMultplier);
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

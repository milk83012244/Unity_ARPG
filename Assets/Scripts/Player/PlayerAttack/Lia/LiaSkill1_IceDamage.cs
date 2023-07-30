using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiaSkill1_IceDamage : MonoBehaviour
{
    public LiaSkill1Effect liaSkill1Effect;

    private void DealDamage(IDamageable damageable, ElementType element)//�y���@���ˮ`
    {
        if (element != ElementType.Ice || damageable== null)
        {
            return;
        }

        liaSkill1Effect.characterStats.isCritical = Random.value < liaSkill1Effect.characterStats.attackData[liaSkill1Effect.characterStats.currentCharacterID].criticalChance;
        if (damageable != null)
        {
            Enemy enemyUnit = damageable as Enemy;
            OtherCharacterStats defander = enemyUnit.GetComponent<OtherCharacterStats>();

            if (element == ElementType.Ice)
            {
                liaSkill1Effect.characterStats.TakeDamage(liaSkill1Effect.characterStats, defander, ElementType.Ice, liaSkill1Effect.characterStats.isCritical, isSkill1: true);
                enemyUnit.SpawnDamageText(liaSkill1Effect.characterStats.currentDamage, ElementType.Ice, liaSkill1Effect.characterStats.isCritical);
                defander.characterElementCounter.AddElementCount(liaSkill1Effect.element, 2);

                if (enemyUnit.isMarked)
                {
                    liaSkill1Effect.characterStats.TakeMarkDamage(liaSkill1Effect.characterStats, defander, liaSkill1Effect.characterStats.isCritical);
                    enemyUnit.SpawnMarkDamageText(liaSkill1Effect.characterStats.currentDamage, liaSkill1Effect.characterStats.isCritical);
                    liaSkill1Effect.characterStats.TakeDamage(liaSkill1Effect.characterStats, defander, liaSkill1Effect.characterStats.isCritical);
                    enemyUnit.SpawnDamageText(liaSkill1Effect.characterStats.currentDamage, liaSkill1Effect.characterStats.isCritical);

                    enemyUnit.ClearMark();
                }
            }
            EnemyUnitType1 enemyUnitType1 = enemyUnit as EnemyUnitType1;
            //Ĳ�o�ĤH�������A
            enemyUnitType1.isAttackState = true;
            //�ĤH�{�{�ĪG
            enemyUnitType1.StartFlash();
            //�y���ĤH�w��
            liaSkill1Effect.characterStats.TakeStunValue(liaSkill1Effect.characterStats, defander, isSkill1: true);
            //�y���ĤH���h
            Vector2 knockbackDirection = (enemyUnitType1.transform.position - transform.position).normalized;
            float knockbackValue = liaSkill1Effect.characterStats.attackData[liaSkill1Effect.characterStats.currentCharacterID].knockbackValue;
            enemyUnitType1.StartKnockback(knockbackDirection, knockbackValue *= liaSkill1Effect.characterStats.attackData[liaSkill1Effect.characterStats.currentCharacterID].skill1knockbackValueMultplier);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            IDamageable damageable = collision.GetComponent<IDamageable>();
            DealDamage(damageable, liaSkill1Effect.element);
        }
    }
}

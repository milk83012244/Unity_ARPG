using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiaSkill1_WindDamage : MonoBehaviour
{
    private Dictionary<Collider2D, bool> targetDic = new Dictionary<Collider2D, bool>(); //�wĲ�o���ؼЦC��

    public LiaSkill1Effect liaSkill1Effect;

    private void OnDisable()
    {
        ClearTargetDic();
    }
    private void DealDamage(IDamageable damageable, ElementType element)//�y���@���ˮ`
    {
        if (element != ElementType.Wind || damageable == null)
        {
            return;
        }

        liaSkill1Effect.characterStats.isCritical = Random.value < liaSkill1Effect.characterStats.attackData[liaSkill1Effect.characterStats.currentCharacterID].criticalChance;
        if (damageable != null)
        {
            Enemy enemyUnit = damageable as Enemy;
            OtherCharacterStats defander = enemyUnit.GetComponent<OtherCharacterStats>();

            if (element == ElementType.Wind)
            {
                liaSkill1Effect.characterStats.TakeDamage(liaSkill1Effect.characterStats, defander, ElementType.Wind, liaSkill1Effect.characterStats.isCritical, isSkill1: true);
                enemyUnit.SpawnDamageText(liaSkill1Effect.characterStats.currentDamage, ElementType.Wind, liaSkill1Effect.characterStats.isCritical);
                
                defander.characterElementCounter.AddElementCount(liaSkill1Effect.element, 2, liaSkill1Effect.characterStats);

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
            enemyUnitType1.DamageByPlayer();
            //�ĤH�{�{�ĪG
            enemyUnitType1.StartFlash();
            //�y���ĤH�w��
            liaSkill1Effect.characterStats.TakeStunValue(liaSkill1Effect.characterStats, defander, isSkill1: true);

            //�y���ĤH���h�ĪG
            //Vector2 knockbackDirection = (enemyUnitType1.transform.position - transform.position).normalized;
            //float knockbackValue = liaSkill1Effect.characterStats.attackData[liaSkill1Effect.characterStats.currentCharacterID].knockbackValue;
            //enemyUnitType1.StartKnockback(knockbackDirection, knockbackValue *= liaSkill1Effect.characterStats.attackData[liaSkill1Effect.characterStats.currentCharacterID].skill1knockbackValueMultplier);
        }
    }
    /// <summary>
    /// �M��Ĳ�o�ؼЪ��e��
    /// </summary>
    public void ClearTargetDic()
    {
        targetDic.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            if (!targetDic.ContainsKey(collision))
            {
                targetDic.Add(collision, false);
                IDamageable damageable = collision.GetComponent<IDamageable>();
                DealDamage(damageable, liaSkill1Effect.element);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiaSkill2_Damage_Ice : MonoBehaviour
{
    public LiaSkill2Effect liaSkill2Effect;
    private Dictionary<Collider2D, bool> targetDic = new Dictionary<Collider2D, bool>(); //�wĲ�o���ؼЦC��

    private void OnDisable()
    {
        ClearTargetDic();
    }

    private void DealDamage(IDamageable damageable, ElementType element)
    {
        if (element != ElementType. Ice || damageable == null)
        {
            return;
        }
        liaSkill2Effect.characterStats.isCritical = Random.value < liaSkill2Effect.characterStats.attackData[liaSkill2Effect.characterStats.currentCharacterID].criticalChance;
        if (damageable != null)
        {
            Enemy enemyUnit = damageable as Enemy;
            OtherCharacterStats defander = enemyUnit.GetComponent<OtherCharacterStats>();

            switch (element)
            {
                case ElementType.Fire:
                    break;
                case ElementType.Ice:
                    liaSkill2Effect.characterStats.TakeDamage(liaSkill2Effect.characterStats, defander, ElementType.Ice, liaSkill2Effect.characterStats.isCritical,isSkill2:true);
                    enemyUnit.SpawnDamageText(liaSkill2Effect.characterStats.currentDamage, ElementType.Ice, liaSkill2Effect.characterStats.isCritical);
                    defander.characterElementCounter.AddElementCount(liaSkill2Effect.element, 2);
                    break;
                case ElementType.Wind:
                    break;
                case ElementType.Thunder:
                    break;
            }
            if (enemyUnit.isMarked)
            {
                liaSkill2Effect.characterStats.TakeMarkDamage(liaSkill2Effect.characterStats, defander, liaSkill2Effect.characterStats.isCritical);
                enemyUnit.SpawnMarkDamageText(liaSkill2Effect.characterStats.currentDamage, liaSkill2Effect.characterStats.isCritical);
                liaSkill2Effect.characterStats.TakeDamage(liaSkill2Effect.characterStats, defander, liaSkill2Effect.characterStats.isCritical);
                enemyUnit.SpawnDamageText(liaSkill2Effect.characterStats.currentDamage, liaSkill2Effect.characterStats.isCritical);

                enemyUnit.ClearMark();
            }
            EnemyUnitType1 enemyUnitType1 = enemyUnit as EnemyUnitType1;
            //Ĳ�o�ĤH�������A
            enemyUnitType1.isAttackState = true;
            //�ĤH�{�{�ĪG
            enemyUnitType1.StartFlash();
            //�y���ĤH�w��
            liaSkill2Effect.characterStats.TakeStunValue(liaSkill2Effect.characterStats, defander,isSkill2:true);
            //�y���ĤH���h
            Vector2 knockbackDirection = (enemyUnitType1.transform.position - transform.position).normalized;
            float knockbackValue = liaSkill2Effect.characterStats.attackData[liaSkill2Effect.characterStats.currentCharacterID].knockbackValue;
            enemyUnitType1.StartKnockback(knockbackDirection, knockbackValue *= liaSkill2Effect.characterStats.attackData[liaSkill2Effect.characterStats.currentCharacterID].skill2knockbackValueMultplier);
        }
    }
    public void ClearTargetDic()
    {
        targetDic.Clear();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!targetDic.ContainsKey(collision))
        {
            targetDic.Add(collision, false);
            IDamageable damageable = collision.GetComponent<IDamageable>();
            DealDamage(damageable, liaSkill2Effect.element);
        }
    }
}

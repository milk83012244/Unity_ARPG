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
            EnemyUnitType1 enemyUnitType1;
            EnemyUnitType2 enemyUnitType2;
            EnemyBoss1Unit enemyBoss1Unit;

            if (enemyUnit == null)
            {
                return;
            }

            OtherCharacterStats defander = enemyUnit.GetComponent<OtherCharacterStats>();

            #region �i�ˮ`�ĤH�@�q
            liaSkill2Effect.characterStats.TakeDamage(liaSkill2Effect.characterStats, defander, ElementType.Ice, liaSkill2Effect.characterStats.isCritical, isSkill2: true);
            enemyUnit.SpawnDamageText(liaSkill2Effect.characterStats.currentDamage, ElementType.Ice, liaSkill2Effect.characterStats.isCritical);
            defander.characterElementCounter.AddElementCount(liaSkill2Effect.element, 2);

            if (enemyUnit.isMarked)
            {
                liaSkill2Effect.characterStats.TakeMarkDamage(liaSkill2Effect.characterStats, defander, liaSkill2Effect.characterStats.isCritical);
                enemyUnit.SpawnMarkDamageText(liaSkill2Effect.characterStats.currentDamage, liaSkill2Effect.characterStats.isCritical);
                liaSkill2Effect.characterStats.TakeDamage(liaSkill2Effect.characterStats, defander, liaSkill2Effect.characterStats.isCritical);
                enemyUnit.SpawnDamageText(liaSkill2Effect.characterStats.currentDamage, liaSkill2Effect.characterStats.isCritical);

                enemyUnit.ClearMark();
            }
            CinemachineShake.GetInstance().ShakeCamera(0.4f, 0.2f);//��v���_��
            #endregion

            switch (enemyUnit.typeID)
            {
                case 1:
                    enemyUnitType1 = enemyUnit as EnemyUnitType1;
                    //Ĳ�o�ĤH�������A
                    enemyUnitType1.DamageByPlayer();
                    //�ĤH�{�{�ĪG
                    enemyUnitType1.StartFlash();
                    if (enemyUnitType1.canStun)
                    {
                        //�y���ĤH�w��
                        liaSkill2Effect.characterStats.TakeStunValue(liaSkill2Effect.characterStats, defander, isSkill2: true);
                    }
                    break;
                case 2:
                    enemyUnitType2 = enemyUnit as EnemyUnitType2;
                    //Ĳ�o�ĤH�������A
                    enemyUnitType2.DamageByPlayer();
                    //�ĤH�{�{�ĪG
                    enemyUnitType2.StartFlash();
                    if (enemyUnitType2.canStun)
                    {
                        //�y���ĤH�w��
                        liaSkill2Effect.characterStats.TakeStunValue(liaSkill2Effect.characterStats, defander, isSkill2: true);
                    }
                    break;
                case 1001:
                    enemyBoss1Unit = enemyUnit as EnemyBoss1Unit;
                    //Ĳ�o�ĤH�������A
                    enemyBoss1Unit.DamageByPlayer();
                    //�ĤH�{�{�ĪG
                    //enemyBoss1Unit.StartFlash();
                    if (enemyBoss1Unit.canStun)
                    {
                        //�y���ĤH�w��
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
                    //�y���ĤH���h
                    knockbackDirection = (enemyUnitType1.transform.position - transform.position).normalized;
                    knockbackValue = liaSkill2Effect.characterStats.attackData[liaSkill2Effect.characterStats.currentCharacterID].knockbackValue;
                    enemyUnitType1.StartKnockback(knockbackDirection, knockbackValue *= liaSkill2Effect.characterStats.attackData[liaSkill2Effect.characterStats.currentCharacterID].skill2knockbackValueMultplier);
                    break;
                case 2:
                    enemyUnitType2 = enemyUnit as EnemyUnitType2;
                    //�y���ĤH���h
                    knockbackDirection = (enemyUnitType2.transform.position - transform.position).normalized;
                    knockbackValue = liaSkill2Effect.characterStats.attackData[liaSkill2Effect.characterStats.currentCharacterID].knockbackValue;
                    enemyUnitType2.StartKnockback(knockbackDirection, knockbackValue *= liaSkill2Effect.characterStats.attackData[liaSkill2Effect.characterStats.currentCharacterID].skill2knockbackValueMultplier);
                    break;
                case 1001:
                    enemyBoss1Unit = enemyUnit as EnemyBoss1Unit;
                    knockbackDirection = (enemyBoss1Unit.transform.position - transform.position).normalized;
                    knockbackValue = liaSkill2Effect.characterStats.attackData[liaSkill2Effect.characterStats.currentCharacterID].knockbackValue;
                    enemyBoss1Unit.StartKnockback(knockbackDirection, knockbackValue *= liaSkill2Effect.characterStats.attackData[liaSkill2Effect.characterStats.currentCharacterID].skill2knockbackValueMultplier);
                    break;
            }
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

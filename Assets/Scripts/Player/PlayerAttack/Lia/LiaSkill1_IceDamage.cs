using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiaSkill1_IceDamage : MonoBehaviour
{
    private Dictionary<Collider2D, bool> targetDic = new Dictionary<Collider2D, bool>(); //�wĲ�o���ؼЦC��

    public LiaSkill1Effect liaSkill1Effect;

    private void OnDisable()
    {
        ClearTargetDic();
    }

    private void DealDamage(IDamageable damageable, ElementType element)//�y���@���ˮ`
    {
        if (element != ElementType.Ice || damageable == null)
        {
            return;
        }

        liaSkill1Effect.characterStats.isCritical = Random.value < liaSkill1Effect.characterStats.attackData[liaSkill1Effect.characterStats.currentCharacterID].criticalChance;
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
                CinemachineShake.GetInstance().ShakeCamera(0.4f, 0.2f);//��v���_��
            }
            #endregion

            switch (enemyUnit.typeID)
            {
                case 1:
                    enemyUnitType1 = enemyUnit as EnemyUnitType1;
                    //Ĳ�o�ĤH�������A
                    enemyUnitType1.DamageByPlayer();
                    //�ĤH�{�{�ĪG
                    enemyUnitType1.StartFlash();
                    //�y���ĤH�w��
                    if (enemyUnitType1.canStun)
                        liaSkill1Effect.characterStats.TakeStunValue(liaSkill1Effect.characterStats, defander, isSkill1: true);
                    break;
                case 2:
                    enemyUnitType2 = enemyUnit as EnemyUnitType2;
                    //Ĳ�o�ĤH�������A
                    enemyUnitType2.DamageByPlayer();
                    //�ĤH�{�{�ĪG
                    enemyUnitType2.StartFlash();
                    //�y���ĤH�w��
                    if (enemyUnitType2.canStun)
                        liaSkill1Effect.characterStats.TakeStunValue(liaSkill1Effect.characterStats, defander, isSkill1: true);
                    break;
                case 1001:
                    enemyBoss1Unit = enemyUnit as EnemyBoss1Unit;
                    //Ĳ�o�ĤH�������A
                    enemyBoss1Unit.DamageByPlayer();
                    //�ĤH�{�{�ĪG
                    // enemyBoss1Unit.StartFlash();
                    //�y���ĤH�w��
                    if (enemyBoss1Unit.canStun)
                        liaSkill1Effect.characterStats.TakeStunValue(liaSkill1Effect.characterStats, defander, isSkill1: true);
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
                    knockbackValue = liaSkill1Effect.characterStats.attackData[liaSkill1Effect.characterStats.currentCharacterID].knockbackValue;
                    enemyUnitType1.StartKnockback(knockbackDirection, knockbackValue *= liaSkill1Effect.characterStats.attackData[liaSkill1Effect.characterStats.currentCharacterID].skill1knockbackValueMultplier);
                    break;
                case 2:
                    enemyUnitType2 = enemyUnit as EnemyUnitType2;
                    //�y���ĤH���h
                    knockbackDirection = (enemyUnitType2.transform.position - transform.position).normalized;
                    knockbackValue = liaSkill1Effect.characterStats.attackData[liaSkill1Effect.characterStats.currentCharacterID].knockbackValue;
                    enemyUnitType2.StartKnockback(knockbackDirection, knockbackValue *= liaSkill1Effect.characterStats.attackData[liaSkill1Effect.characterStats.currentCharacterID].skill1knockbackValueMultplier);
                    break;
                case 1001:
                    enemyBoss1Unit = enemyUnit as EnemyBoss1Unit;
                    //�y���ĤH���h
                    knockbackDirection = (enemyBoss1Unit.transform.position - transform.position).normalized;
                    knockbackValue = liaSkill1Effect.characterStats.attackData[liaSkill1Effect.characterStats.currentCharacterID].knockbackValue;
                    enemyBoss1Unit.StartKnockback(knockbackDirection, knockbackValue *= liaSkill1Effect.characterStats.attackData[liaSkill1Effect.characterStats.currentCharacterID].skill1knockbackValueMultplier);
                    break;
            }

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

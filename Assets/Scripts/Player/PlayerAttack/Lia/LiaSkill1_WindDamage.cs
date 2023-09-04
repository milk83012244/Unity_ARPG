using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiaSkill1_WindDamage : MonoBehaviour
{
    private Dictionary<Collider2D, bool> targetDic = new Dictionary<Collider2D, bool>(); //已觸發的目標列表

    public LiaSkill1Effect liaSkill1Effect;

    private void OnDisable()
    {
        ClearTargetDic();
    }
    private void DealDamage(IDamageable damageable, ElementType element)//造成一次傷害
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
            //觸發敵人受擊狀態
            enemyUnitType1.DamageByPlayer();
            //敵人閃爍效果
            enemyUnitType1.StartFlash();
            //造成敵人硬直
            liaSkill1Effect.characterStats.TakeStunValue(liaSkill1Effect.characterStats, defander, isSkill1: true);

            //造成敵人擊退效果
            //Vector2 knockbackDirection = (enemyUnitType1.transform.position - transform.position).normalized;
            //float knockbackValue = liaSkill1Effect.characterStats.attackData[liaSkill1Effect.characterStats.currentCharacterID].knockbackValue;
            //enemyUnitType1.StartKnockback(knockbackDirection, knockbackValue *= liaSkill1Effect.characterStats.attackData[liaSkill1Effect.characterStats.currentCharacterID].skill1knockbackValueMultplier);
        }
    }
    /// <summary>
    /// 清除觸發目標的容器
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

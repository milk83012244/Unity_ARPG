using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiaSkill1_WindEffectDamage : MonoBehaviour
{
    public LiaSkill1Effect liaSkill1Effect;

    public float damageInterval = 0.5f;
    public float damageDuration = 3f;

    private bool isDamaging = false;

    private Dictionary<IDamageable, Coroutine> damageCoroutines = new Dictionary<IDamageable, Coroutine>();

    private void OnEnable()
    {
        StartDamaging();
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private void StartDamaging()
    {
        isDamaging = true;
    }
    private IEnumerator StopDamaging()
    {
        yield return Yielders.GetWaitForSeconds(damageDuration);
    }
    private IEnumerator DealDamage(IDamageable damageable, ElementType element)
    {
        if (element != ElementType.Wind || damageable == null)
        {
            yield break;
        }

        while (isDamaging)
        {
            liaSkill1Effect.characterStats.isCritical = Random.value < liaSkill1Effect.characterStats.attackData[liaSkill1Effect.characterStats.currentCharacterID].criticalChance;
            if (damageable != null)
            {
                Enemy enemyUnit = damageable as Enemy;
                OtherCharacterStats defander = enemyUnit.GetComponent<OtherCharacterStats>();

                if (element == ElementType.Wind)
                {
                    liaSkill1Effect.characterStats.TakeSubDamage(liaSkill1Effect.characterStats, defander, ElementType.Wind);
                    enemyUnit.SpawnDamageText(liaSkill1Effect.characterStats.currentDamage, ElementType.Wind, isSub: true);

                    ///添加賦予屬性值
                    defander.characterElementCounter.AddElementCount(liaSkill1Effect.element, 1);

                    if (enemyUnit.isMarked)
                    {
                        liaSkill1Effect.characterStats.TakeMarkDamage(liaSkill1Effect.characterStats, defander, liaSkill1Effect.characterStats.isCritical);
                        enemyUnit.SpawnMarkDamageText(liaSkill1Effect.characterStats.currentDamage, liaSkill1Effect.characterStats.isCritical);
                        liaSkill1Effect.characterStats.TakeDamage(liaSkill1Effect.characterStats, defander, liaSkill1Effect.characterStats.isCritical);
                        enemyUnit.SpawnDamageText(liaSkill1Effect.characterStats.currentDamage, liaSkill1Effect.characterStats.isCritical);

                        enemyUnit.ClearMark();
                    }
                    EnemyUnitType1 enemyUnitType1 = enemyUnit as EnemyUnitType1;
                    //觸發敵人受擊狀態
                    enemyUnitType1.isAttackState = true;
                    //敵人閃爍效果
                    enemyUnitType1.StartFlash();
                }
            }
            yield return Yielders.GetWaitForSeconds(damageInterval);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            IDamageable damageable = collision.GetComponent<IDamageable>();
            //StartCoroutine(DealDamage(damageable, liaSkill1Effect.element));
            if (damageable != null && !damageCoroutines.ContainsKey(damageable))
            {
                Coroutine coroutine = StartCoroutine(DealDamage(damageable, liaSkill1Effect.element));
                damageCoroutines.Add(damageable, coroutine);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            IDamageable damageable = collision.GetComponent<IDamageable>();
            if (damageable != null && damageCoroutines.ContainsKey(damageable))
            {
                Coroutine coroutine = damageCoroutines[damageable];
                StopCoroutine(coroutine);
                damageCoroutines.Remove(damageable);
            }
        }
    }
}

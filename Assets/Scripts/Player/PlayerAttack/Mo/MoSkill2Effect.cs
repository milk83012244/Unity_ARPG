using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoSkill2Effect : MonoBehaviour
{
    public MoSkill2Attack moSkill2Attack;

    public float damageInterval = 0.5f;
    public float damageDuration = 2f;

    private bool isDamaging = false;

    private Dictionary<IDamageable, Coroutine> damageCoroutines = new Dictionary<IDamageable, Coroutine>();
    private void OnEnable()
    {
        StartDamaging();
    }


    private void StartDamaging()
    {
        isDamaging = true;
        StartCoroutine(StopDamaging());
    }
    private IEnumerator StopDamaging()
    {
        yield return new WaitForSeconds(damageDuration);
        isDamaging = false;
        Recycle();
        //StartCoroutine(Recycle());
    }
    private IEnumerator DealDamage(IDamageable damageable)
    {
        while (isDamaging)
        {
            moSkill2Attack.characterStats.isCritical = Random.value < moSkill2Attack.characterStats.attackData[moSkill2Attack.characterStats.currentCharacterID].criticalChance;
            if (damageable != null)
            {
                Enemy enemyUnit = damageable as Enemy;

                if (enemyUnit != null)
                {
                    OtherCharacterStats defander = enemyUnit.GetComponent<OtherCharacterStats>();

                    moSkill2Attack.characterStats.TakeSubDamage(moSkill2Attack.characterStats, defander, moSkill2Attack.characterStats.isCritical);
                    enemyUnit.SpawnDamageText(moSkill2Attack.characterStats.currentDamage, moSkill2Attack.characterStats.isCritical, isSub: true);

                    EnemyUnitType1 enemyUnitType1 = enemyUnit as EnemyUnitType1;
                    //觸發敵人受擊狀態
                    enemyUnitType1.DamageByPlayer();
                    //敵人閃爍效果
                    enemyUnitType1.StartFlash();
                }
            }

            yield return new WaitForSeconds(damageInterval);
        }
    }

    public void Recycle() //特效類自己回收
    {
       // yield return new WaitForSeconds(0);
        ObjectPool<MoSkill2Effect>.Instance.Recycle(this);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        MoSkill2Effect dashAttackEffect = collision.GetComponent<MoSkill2Effect>();
        if (dashAttackEffect != null)
        {
            return;
        }

        if (collision.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            IDamageable damageable = collision.GetComponent<IDamageable>();
            //StartCoroutine(DealDamage(damageable, liaSkill1Effect.element));
            if (damageable != null && !damageCoroutines.ContainsKey(damageable))
            {
                Coroutine coroutine = StartCoroutine(DealDamage(damageable));
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

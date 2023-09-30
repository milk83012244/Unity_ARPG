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
    private void OnDisable()
    {
        StopAllCoroutines();
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
            if (damageable == null)
            {
                yield break;
            }

            moSkill2Attack.characterStats.isCritical = Random.value < moSkill2Attack.characterStats.attackData[moSkill2Attack.characterStats.currentCharacterID].criticalChance;
 
            if (damageable != null)
            {
                Enemy enemyUnit = damageable as Enemy;
                EnemyUnitType1 enemyUnitType1;
                EnemyUnitType2 enemyUnitType2;
                EnemyBoss1Unit enemyBoss1Unit;

                if (enemyUnit ==null)
                {
                    yield break;
                }

                if (enemyUnit != null)
                {
                    OtherCharacterStats defander = enemyUnit.GetComponent<OtherCharacterStats>();

                    #region �i�ˮ`�ĤH�@�q
                    moSkill2Attack.characterStats.TakeSubDamage(moSkill2Attack.characterStats, defander, moSkill2Attack.characterStats.isCritical);
                    enemyUnit.SpawnDamageText(moSkill2Attack.characterStats.currentDamage, moSkill2Attack.characterStats.isCritical, isSub: true);
                    #endregion

                    switch (enemyUnit.typeID)
                    {
                        case 1:
                            enemyUnitType1 = enemyUnit as EnemyUnitType1;
                            //Ĳ�o�ĤH�������A
                            enemyUnitType1.DamageByPlayer();
                            //�ĤH�{�{�ĪG
                            enemyUnitType1.StartFlash();
                            break;
                        case 2:
                            enemyUnitType2 = enemyUnit as EnemyUnitType2;
                            //Ĳ�o�ĤH�������A
                            enemyUnitType2.DamageByPlayer();
                            //�ĤH�{�{�ĪG
                            enemyUnitType2.StartFlash();
                            break;
                        case 1001:
                            enemyBoss1Unit = enemyUnit as EnemyBoss1Unit;
                            //Ĳ�o�ĤH�������A
                            enemyBoss1Unit.DamageByPlayer();
                            //�ĤH�{�{�ĪG
                            //enemyBoss1Unit.StartFlash();
                            break;
                    }
                }
            }

            yield return new WaitForSeconds(damageInterval);
        }
    }

    public void Recycle() //�S�����ۤv�^��
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

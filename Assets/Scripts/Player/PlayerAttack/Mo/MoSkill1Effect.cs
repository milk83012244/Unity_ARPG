using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoSkill1Effect : MonoBehaviour
{
    public MoSkill1Attack moSkill1Attack;

    public float damageInterval = 0.5f;
    public float damageDuration = 3f;

    private bool isDamaging = false;

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
        yield return Yielders.GetWaitForSeconds(damageDuration);
        StartCoroutine(Recycle());
    }

    private IEnumerator DealDamage(IDamageable damageable)
    {
        while (isDamaging)
        {
            moSkill1Attack.characterStats.isCritical = Random.value < moSkill1Attack.characterStats.attackData[moSkill1Attack.characterStats.currentCharacterID].criticalChance;
            if (damageable != null)
            {
                Enemy enemyUnit = damageable as Enemy;

                if (enemyUnit != null)
                {
                    OtherCharacterStats defander = enemyUnit.GetComponent<OtherCharacterStats>();

                    moSkill1Attack.characterStats.TakeSubDamage(moSkill1Attack.characterStats, defander, moSkill1Attack.characterStats.isCritical);
                    enemyUnit.SpawnDamageText(moSkill1Attack.characterStats.currentDamage, moSkill1Attack.characterStats.isCritical);

                    EnemyUnitType1 enemyUnitType1 = enemyUnit as EnemyUnitType1;
                    //Ĳ�o�ĤH�������A
                    enemyUnitType1.isAttackState = true;
                    //�ĤH�{�{�ĪG
                    enemyUnitType1.StartFlash();
                }
            }
            yield return Yielders.GetWaitForSeconds(damageInterval);
        }
    }

    /// <summary>
    /// �^������
    /// </summary>
    public IEnumerator Recycle()
    {
        isDamaging = false;
        yield return new WaitForSeconds(0.1f);
        ObjectPool<MoSkill1Effect>.Instance.Recycle(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponent<IDamageable>();
        StartCoroutine(DealDamage(damageable));
    }
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    StopDamaging();
    //}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoSkill2Effect : MonoBehaviour
{
    public MoSkill2Attack moSkill2Attack;

    public float damageInterval = 0.5f;
    public float damageDuration = 2f;

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
                EnemyUnits enemyUnit = damageable as EnemyUnits;
                OtherCharacterStats defander = enemyUnit.GetComponent<OtherCharacterStats>();

                moSkill2Attack.characterStats.TakeSubDamage(moSkill2Attack.characterStats, defander, moSkill2Attack.characterStats.isCritical);
                enemyUnit.SpawnDamageText(moSkill2Attack.characterStats.currentDamage, moSkill2Attack.characterStats.isCritical);
            }

            yield return new WaitForSeconds(damageInterval);
        }
    }

    public void Recycle() //特效類自己回收
    {
       // yield return new WaitForSeconds(0);
        ObjectPool<MoSkill2Effect>.GetInstance().Recycle(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MoSkill2Effect dashAttackEffect = collision.GetComponent<MoSkill2Effect>();
        if (dashAttackEffect != null)
        {
            return;
        }

        IDamageable damageable = collision.GetComponent<IDamageable>();

        if (damageable != null)
        {
            StartCoroutine(DealDamage(damageable));
        }
    }
}

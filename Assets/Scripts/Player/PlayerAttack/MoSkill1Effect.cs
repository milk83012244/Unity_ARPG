using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoSkill1Effect : MonoBehaviour
{
    public MoSkill1Attack moSkill1Attack;

    public float damageInterval = 0.5f;
    public float damageDuration = 3f;

    private bool isDamaging = false;

    private void StartDamaging()
    {
        if (!isDamaging)
        {
            isDamaging = true;
            StartCoroutine(DealDamage());
            StartCoroutine(StopDamaging());
        }
    }
    private IEnumerator StopDamaging()
    {
        if (isDamaging)
        {
            yield return Yielders.GetWaitForSeconds(damageDuration);
            isDamaging = false;
            StopCoroutine(DealDamage());
            Destroy(this.gameObject);
        }
    }

    private IEnumerator DealDamage()
    {
        while (isDamaging)
        {
            moSkill1Attack.characterStats.isCritical = Random.value < moSkill1Attack.characterStats.attackData[moSkill1Attack.characterStats.currentCharacterID].criticalChance;
            GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
            OtherCharacterStats defander = enemy.GetComponent<OtherCharacterStats>();
            TestUnit enemyUnit = defander.GetComponent<TestUnit>();

            if (enemy != null)
            {
                moSkill1Attack.characterStats.TakeSubDamage(moSkill1Attack.characterStats, defander, moSkill1Attack.characterStats.isCritical);
                enemyUnit.SpawnDamageText(moSkill1Attack.characterStats.currentDamage, moSkill1Attack.characterStats.isCritical);
            }
            yield return new WaitForSeconds(damageInterval);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            StartDamaging();
        }
        //StartDamaging();
    }
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    StopDamaging();
    //}
}

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
    private void OnDisable()
    {
        ObjectPool<MoSkill1Effect>.Instance.Recycle(this);
        StopAllCoroutines();
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
            if (damageable == null)
            {
                yield break;
            }

            moSkill1Attack.characterStats.isCritical = Random.value < moSkill1Attack.characterStats.attackData[moSkill1Attack.characterStats.currentCharacterID].criticalChance;
 
            if (damageable != null)
            {
                Enemy enemyUnit = damageable as Enemy;
                EnemyUnitType1 enemyUnitType1;
                EnemyUnitType2 enemyUnitType2;
                EnemyBoss1Unit enemyBoss1Unit;

                if (enemyUnit == null)
                {
                    yield break;
                }

                OtherCharacterStats defander = enemyUnit.GetComponent<OtherCharacterStats>();
                #region 可傷害敵人共通
                moSkill1Attack.characterStats.TakeSubDamage(moSkill1Attack.characterStats, defander, moSkill1Attack.characterStats.isCritical);
                enemyUnit.SpawnDamageText(moSkill1Attack.characterStats.currentDamage, moSkill1Attack.characterStats.isCritical);
                #endregion

                switch (enemyUnit.typeID)
                {
                    case 1:
                        enemyUnitType1 = enemyUnit as EnemyUnitType1;
                        //觸發敵人受擊狀態
                        enemyUnitType1.DamageByPlayer();
                        //敵人閃爍效果
                        enemyUnitType1.StartFlash();
                        break;
                    case 2:
                        enemyUnitType2 = enemyUnit as EnemyUnitType2;
                        //觸發敵人受擊狀態
                        enemyUnitType2.DamageByPlayer();
                        //敵人閃爍效果
                        enemyUnitType2.StartFlash();
                        break;
                    case 1001:
                        enemyBoss1Unit = enemyUnit as EnemyBoss1Unit;
                        //觸發敵人受擊狀態
                        enemyBoss1Unit.DamageByPlayer();
                        //敵人閃爍效果
                        //enemyBoss1Unit.StartFlash();
                        break;
                }
            }
            yield return Yielders.GetWaitForSeconds(damageInterval);
        }
    }

    /// <summary>
    /// 回收物件
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1NormalAttackController : MonoBehaviour
{
    private OtherCharacterStats characterStats;
    private EnemyBoss1Unit enemyBoss1Unit;

    private void Awake()
    {
        characterStats = GetComponentInParent<OtherCharacterStats>();
        enemyBoss1Unit = GetComponentInParent<EnemyBoss1Unit>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        characterStats.isCritical = Random.value < characterStats.enemyAttackData.criticalChance; //�z���P�_
        PlayerCharacterStats playerCharacterStats = collision.GetComponent<PlayerCharacterStats>();

        if (playerCharacterStats == null)
        {
            return;
        }

        //characterStats.TakeDamage(characterStats, playerCharacterStats);
        if (MoCounterCheck.guardCheckActive || MoCounterCheck.isGuardHit)
        {
            if (MoCounterCheck.guardCheckActive)
                Debug.Log(string.Format("<color=#ff0000>{0}</color>", "Ĳ�o" + collision.name + "������,�ˮ`�L��"));
            else if (MoCounterCheck.isGuardHit)
                Debug.Log(string.Format("<color=#ff0000>{0}</color>", collision.name + "�����ʧ@��,�ˮ`�L��"));
        }
        else
        {
            if (playerCharacterStats != null)
            {
                //����q��
                switch (enemyBoss1Unit.currentAttackBehavior)
                {
                    case EnemyBoss1Unit.Boss1AttackBehavior.NormalAttack1:
                        characterStats.TakeDamage(characterStats, playerCharacterStats, ElementType.Wind);
                        PlayerController.GetInstance().uIDisplay.SpawnDamageText(characterStats.currentDamage, ElementType.Wind);
                        break;
                    case EnemyBoss1Unit.Boss1AttackBehavior.NormalAttack2:
                        characterStats.TakeDamage(characterStats, playerCharacterStats, ElementType.Wind,freeMul:1.15f);
                        PlayerController.GetInstance().uIDisplay.SpawnDamageText(characterStats.currentDamage, ElementType.Wind);
                        break;
                    case EnemyBoss1Unit.Boss1AttackBehavior.NormalAttack3:
                        characterStats.TakeDamage(characterStats, playerCharacterStats, ElementType.Wind, freeMul: 1.3f);
                        PlayerController.GetInstance().uIDisplay.SpawnDamageText(characterStats.currentDamage, ElementType.Wind);
                        break;
                }
                //if (isMarkAttack && activeMarkAttack)
                //{
                //    characterStats.TakeMarkDamage(characterStats, defander, characterStats.isCritical);
                //    PlayerController.GetInstance().uIDisplay.SpawnMarkDamageText(characterStats.currentDamage, characterStats.isCritical);
                //    characterStats.TakeDamage(characterStats, defander, characterStats.isCritical);
                //    PlayerController.GetInstance().uIDisplay.SpawnDamageText(characterStats.currentDamage, characterStats.isCritical);
                //}
                //characterStats.TakeDamage(characterStats, playerCharacterStats, characterStats.isCritical);
                //PlayerController.GetInstance().uIDisplay.SpawnDamageText(characterStats.currentDamage, characterStats.isCritical);
                //�ᤩ�w����
                if (playerCharacterStats.GetPlayerCanStun())
                    characterStats.TakeStunValue(characterStats, playerCharacterStats);
            }
        }
    }
}

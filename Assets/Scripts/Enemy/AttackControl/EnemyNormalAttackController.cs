using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �@��ĤH���q�����ˮ`�p��
/// </summary>
public class EnemyNormalAttackController : MonoBehaviour
{
    private OtherCharacterStats characterStats;

    //�O�_���ᤩ�ݩ�
    [HideInInspector] public bool fireElement;
    [HideInInspector] public bool iceElement;
    [HideInInspector] public bool windElement;
    [HideInInspector] public bool thunderElement;
    [HideInInspector] public bool lightElement;
    [HideInInspector] public bool darkElement;

    private void Awake()
    {
        characterStats = GetComponentInParent<OtherCharacterStats>();
    }
    private void OnEnable()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        characterStats.isCritical = Random.value < characterStats.enemyAttackData.criticalChance; //�z���P�_
        //�����ؼ�
        PlayerCharacterStats playerCharacterStats = collision.GetComponent<PlayerCharacterStats>();
        //characterStats.TakeDamage(characterStats, playerCharacterStats);
        if (MoCounterCheck.guardCheckActive || MoCounterCheck.isGuardHit)
        {
            if (MoCounterCheck.guardCheckActive)
                Debug.Log(string.Format("<color=#ff0000>{0}</color>", "Ĳ�o" + collision.name + "������,�ˮ`�L��"));
            else if(MoCounterCheck.isGuardHit)
                Debug.Log(string.Format("<color=#ff0000>{0}</color>", collision.name + "�����ʧ@��,�ˮ`�L��"));
        }
        else
        {
            if (playerCharacterStats != null)
            {
                if (fireElement)
                {
                    characterStats.TakeDamage(characterStats, playerCharacterStats, ElementType.Fire);
                    PlayerController.GetInstance().uIDisplay.SpawnDamageText(characterStats.currentDamage, ElementType.Fire);
                }
                else if (iceElement)
                {
                    characterStats.TakeDamage(characterStats, playerCharacterStats, ElementType.Ice);
                    PlayerController.GetInstance().uIDisplay.SpawnDamageText(characterStats.currentDamage, ElementType.Ice);
                }
                else if (windElement)
                {
                    characterStats.TakeDamage(characterStats, playerCharacterStats, ElementType.Wind);
                    PlayerController.GetInstance().uIDisplay.SpawnDamageText(characterStats.currentDamage, ElementType.Wind);
                }
                else if (thunderElement)
                {
                    characterStats.TakeDamage(characterStats, playerCharacterStats, ElementType.Thunder);
                    PlayerController.GetInstance().uIDisplay.SpawnDamageText(characterStats.currentDamage, ElementType.Thunder);
                }
                else if (lightElement)
                {
                    characterStats.TakeDamage(characterStats, playerCharacterStats, ElementType.Light);
                    PlayerController.GetInstance().uIDisplay.SpawnDamageText(characterStats.currentDamage, ElementType.Light);
                }
                else if (darkElement)
                {
                    characterStats.TakeDamage(characterStats, playerCharacterStats, ElementType.Dark);
                    PlayerController.GetInstance().uIDisplay.SpawnDamageText(characterStats.currentDamage, ElementType.Dark);
                }
                //if (isMarkAttack && activeMarkAttack)
                //{
                //    characterStats.TakeMarkDamage(characterStats, defander, characterStats.isCritical);
                //    PlayerController.GetInstance().uIDisplay.SpawnMarkDamageText(characterStats.currentDamage, characterStats.isCritical);
                //    characterStats.TakeDamage(characterStats, defander, characterStats.isCritical);
                //    PlayerController.GetInstance().uIDisplay.SpawnDamageText(characterStats.currentDamage, characterStats.isCritical);
                //}
                characterStats.TakeDamage(characterStats, playerCharacterStats, characterStats.isCritical);
                PlayerController.GetInstance().uIDisplay.SpawnDamageText(characterStats.currentDamage, characterStats.isCritical);
                //�ᤩ�w����
                if (playerCharacterStats.GetPlayerCanStun())
                    characterStats.TakeStunValue(characterStats, playerCharacterStats);
            }
        }
    }
}

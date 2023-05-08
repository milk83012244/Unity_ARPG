using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 角色攻擊控制管理
/// </summary>
public class MoNormalAttack : MonoBehaviour
{
    private int enemiesLayer = 1 << 8;
    private PlayerCharacterStats characterStats;

    public bool isMarkAttack;
    bool activeMarkAttack;

    public int attackCount = 0;

    public bool fireElement;
    public bool iceElement;
    public bool windElement;
    public bool thunderElement;
    public bool lightElement;
    public bool darkElement;
    private void OnEnable()
    {
        isMarkAttack = false;
    }
    private void Awake()
    {
        characterStats = GetComponentInParent<PlayerCharacterStats>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        characterStats.isCritical = Random.value < characterStats.attackData[characterStats.currentCharacterID].criticalChance;
        OtherCharacterStats defander = collision.GetComponent<OtherCharacterStats>();
        TestUnit enemyUnit = defander.GetComponent<TestUnit>();

        if (PlayerState_Attack3.isAttack3)
        {
            isMarkAttack = true;
        }
        if (isMarkAttack)
        {
            if (enemyUnit.isMarked)
            {
                enemyUnit.ClearMark();
                activeMarkAttack = true;
            }
            else
            {
                enemyUnit.SetMark(MarkType.Mo);
                activeMarkAttack = false;
            }
        }
        //屬性測試
        if (fireElement)
        {
            characterStats.TakeDamage(characterStats, defander, ElementType.Fire);
            enemyUnit.SpawnDamageText(characterStats.currentDamage, ElementType.Fire);
        }
        else if (iceElement)
        {
            characterStats.TakeDamage(characterStats, defander, ElementType.Ice);
            enemyUnit.SpawnDamageText(characterStats.currentDamage, ElementType.Ice);
        }
        else if (windElement)
        {
            characterStats.TakeDamage(characterStats, defander, ElementType.Wind);
            enemyUnit.SpawnDamageText(characterStats.currentDamage, ElementType.Wind);
        }
        else if (thunderElement)
        {
            characterStats.TakeDamage(characterStats, defander, ElementType.Thunder);
            enemyUnit.SpawnDamageText(characterStats.currentDamage, ElementType.Thunder);
        }
        else if (lightElement)
        {
            characterStats.TakeDamage(characterStats, defander, ElementType.Light);
            enemyUnit.SpawnDamageText(characterStats.currentDamage, ElementType.Light);
        }
        else if (darkElement)
        {
            characterStats.TakeDamage(characterStats, defander, ElementType.Dark);
            enemyUnit.SpawnDamageText(characterStats.currentDamage, ElementType.Dark);
        }
        else
        {
            if (activeMarkAttack)
            {
                characterStats.TakeMarkDamage(characterStats, defander, characterStats.isCritical);
                enemyUnit.SpawnMarkDamageText(characterStats.currentDamage, characterStats.isCritical);
                characterStats.TakeDamage(characterStats, defander, characterStats.isCritical);
                enemyUnit.SpawnDamageText(characterStats.currentDamage, characterStats.isCritical);
            }
            else
            {
                characterStats.TakeDamage(characterStats, defander, characterStats.isCritical);
                enemyUnit.SpawnDamageText(characterStats.currentDamage, characterStats.isCritical);
            }
        }
    }
}

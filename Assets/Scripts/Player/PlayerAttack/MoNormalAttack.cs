using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 角色攻擊控制管理
/// </summary>
public class MoNormalAttack : MonoBehaviour,IAttack3Observer
{
    private int enemiesLayer = 1 << 8;
    private PlayerCharacterStats characterStats;

    private bool isAttack3;

    public int attackCount = 0;

    public bool fireElement;
    public bool iceElement;
    public bool windElement;
    public bool thunderElement;
    public bool lightElement;
    public bool darkElement;
    private void Awake()
    {
        isAttack3 = false;
        characterStats = GetComponentInParent<PlayerCharacterStats>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        characterStats.isCritical = Random.value < characterStats.attackData[characterStats.currentCharacterID].criticalChance;
        OtherCharacterStats defander = collision.GetComponent<OtherCharacterStats>();

        //屬性測試
        if (fireElement)
        {
            characterStats.TakeDamage(characterStats, defander, ElementType.Fire);
            defander.GetComponent<TestUnit>().SpawnDamageText(characterStats.currentDamage, ElementType.Fire);
        }
        else if (iceElement)
        {
            characterStats.TakeDamage(characterStats, defander, ElementType.Ice);
            defander.GetComponent<TestUnit>().SpawnDamageText(characterStats.currentDamage, ElementType.Ice);
        }
        else if (windElement)
        {
            characterStats.TakeDamage(characterStats, defander, ElementType.Wind);
            defander.GetComponent<TestUnit>().SpawnDamageText(characterStats.currentDamage, ElementType.Wind);
        }
        else if (thunderElement)
        {
            characterStats.TakeDamage(characterStats, defander, ElementType.Thunder);
            defander.GetComponent<TestUnit>().SpawnDamageText(characterStats.currentDamage, ElementType.Thunder);
        }
        else if (lightElement)
        {
            characterStats.TakeDamage(characterStats, defander, ElementType.Light);
            defander.GetComponent<TestUnit>().SpawnDamageText(characterStats.currentDamage, ElementType.Light);
        }
        else if (darkElement)
        {
            characterStats.TakeDamage(characterStats, defander, ElementType.Dark);
            defander.GetComponent<TestUnit>().SpawnDamageText(characterStats.currentDamage, ElementType.Dark);
        }
        else
        {
            characterStats.TakeDamage(characterStats, defander);
            defander.GetComponent<TestUnit>().SpawnDamageText(characterStats.currentDamage, characterStats.isCritical);
        }
    }

    public void Notify(bool isAttack3)
    {
        this.isAttack3 = isAttack3;
    }
}

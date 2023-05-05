using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 獲取角色資料並計算 
/// </summary>
public class OtherCharacterStats : MonoBehaviour
{
    public CharacterBattleDataSO characterData;
    public CharacterAttackDataSO attackData;

    public List<float> ElementDefences = new List<float>();

    [HideInInspector] public int currentDamage;
    [HideInInspector] public bool isCritical;

    #region 從ChatacterDataSO讀取值
    public int MaxHealth
    {
        get
        {
            if (characterData != null)
                return characterData.mexHealth;
            else
                return 0;
        }
        set
        {
            characterData.mexHealth = value;
        }
    }
    public int CurrnetHealth
    {
        get
        {
            if (characterData != null)
                return characterData.currentHealth;
            else
                return 0;
        }
        set
        {
            characterData.currentHealth = value;
        }
    }
    public float BaseDefence
    {
        get
        {
            if (characterData != null)
                return characterData.baseDefence;
            else
                return 0;
        }
        set
        {
            characterData.baseDefence = value;
        }
    }
    public float CurrentDefence
    {
        get
        {
            if (characterData != null)
                return characterData.currentDefence;
            else
                return 0;
        }
        set
        {
            characterData.currentDefence = value;
        }
    }
    #endregion

    private void Start()
    {
        ElementDefences.Add(characterData.fireElementDefence);
        ElementDefences.Add(characterData.iceElementDefence);
        ElementDefences.Add(characterData.windElementDefence);
        ElementDefences.Add(characterData.thunderElementDefence);
        ElementDefences.Add(characterData.lightElementDefence);
        ElementDefences.Add(characterData.darkElementDefence);
    }

    #region 傷害計算
    /// <summary>
    /// 造成傷害(獲得敵方腳本扣生命值)
    /// </summary>
    public void TakeDamage(OtherCharacterStats attacker, PlayerCharacterStats defender, bool isCritical = false)//敵人只會攻擊到玩家
    {
        DamageCalculator damageCalculator = new DamageCalculator();

        float damagefloat = Mathf.Max(damageCalculator.CalculateDamage(attacker) - (defender.CurrentDefence), 0);

        int damage = (int)Mathf.Round(damagefloat);

        defender.CurrnetHealth = defender.CurrnetHealth - damage;

        //UI更新
        //經驗提升等
    }
    /// <summary>
    /// 造成屬性傷害
    /// </summary>
    public void TakeDamage(PlayerCharacterStats attacker, OtherCharacterStats defender, ElementType elementType, bool isCritical = false)
    {
        DamageCalculator damageCalculator = new DamageCalculator();
        float currentDefence = defender.CurrentDefence;

        float damagefloat = Mathf.Max(damageCalculator.CalculateDamage(attacker, elementType) -
            (defender.CurrentDefence + (currentDefence *= defender.GetElementDefence(elementType))), 0);

        int damage = (int)Mathf.Round(damagefloat);

        defender.CurrnetHealth = defender.CurrnetHealth - damage;

        //UI更新
        //經驗提升等
    }
    #endregion
    public float GetElementDefence(ElementType elementType)
    {
        return ElementDefences[(int)elementType];
    }
}

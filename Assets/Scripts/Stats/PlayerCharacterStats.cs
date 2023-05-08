using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 獲取角色資料 可造成傷害(可切換角色)
/// </summary>
public class PlayerCharacterStats : MonoBehaviour
{
    public int currentCharacterID;

    //之後要改成隊伍有對應角色重新載入成對應角色的資料(用字典等鍵值對儲存)
    public List<CharacterBattleDataSO>  characterData = new List<CharacterBattleDataSO>();
    public List<CharacterAttackDataSO>  attackData = new List<CharacterAttackDataSO>();

    public List<float> ElementDefences = new List<float>();

    [HideInInspector] public int currentDamage;
    [HideInInspector] public bool isCritical;

    #region 從ChatacterDataSO讀取值
    public int MaxHealth
    {
        get
        {
            if (characterData != null)
                return characterData[currentCharacterID].mexHealth;
            else
                return 0;
        }
        set
        {
            characterData[currentCharacterID].mexHealth = value;
        }
    }
    public int CurrnetHealth
    {
        get
        {
            if (characterData != null)
                return characterData[currentCharacterID].currentHealth;
            else
                return 0;
        }
        set
        {
            characterData[currentCharacterID].currentHealth = value;
        }
    }
    public float BaseDefence
    {
        get
        {
            if (characterData != null)
                return characterData[currentCharacterID].baseDefence;
            else
                return 0;
        }
        set
        {
            characterData[currentCharacterID].baseDefence = value;
        }
    }
    public float CurrentDefence
    {
        get
        {
            if (characterData != null)
                return characterData[currentCharacterID].currentDefence;
            else
                return 0;
        }
        set
        {
            characterData[currentCharacterID].currentDefence = value;
        }
    }
    #endregion

    private void Start()
    {
        ElementDefences.Add(characterData[currentCharacterID].fireElementDefence);
        ElementDefences.Add(characterData[currentCharacterID].iceElementDefence);
        ElementDefences.Add(characterData[currentCharacterID].windElementDefence);
        ElementDefences.Add(characterData[currentCharacterID].thunderElementDefence);
        ElementDefences.Add(characterData[currentCharacterID].lightElementDefence);
        ElementDefences.Add(characterData[currentCharacterID].darkElementDefence);
    }

    #region 傷害計算
    /// <summary>
    /// 造成傷害(獲得敵方腳本扣生命值)
    /// </summary>
    public void TakeDamage(PlayerCharacterStats attacker, OtherCharacterStats defender, bool isCritical = false,bool isSkill=false)
    {
        DamageCalculator damageCalculator = new DamageCalculator();

        float damagefloat = Mathf.Max(damageCalculator.CalculateDamage(attacker, isCritical,isSkill) - (defender.CurrentDefence), 0);

        currentDamage = (int)Mathf.Round(damagefloat);

        defender.CurrnetHealth = defender.CurrnetHealth - currentDamage;

        //UI更新
        //經驗提升等
    }
    /// <summary>
    /// 標記傷害
    /// </summary>
    public void TakeMarkDamage(PlayerCharacterStats attacker, OtherCharacterStats defender, bool isCritical = false)
    {
        DamageCalculator damageCalculator = new DamageCalculator();

        float damagefloat = Mathf.Max(damageCalculator.CalculateMarkDamage(attacker, isCritical) - (defender.CurrentDefence), 0);

        currentDamage = (int)Mathf.Round(damagefloat);

        defender.CurrnetHealth = defender.CurrnetHealth - currentDamage;

        //UI更新
        //經驗提升等
    }
    /// <summary>
    /// 副傷害
    /// </summary>
    public void TakeSubDamage(PlayerCharacterStats attacker, OtherCharacterStats defender, bool isCritical = false, bool isSkill = false)
    {
        DamageCalculator damageCalculator = new DamageCalculator();

        float damagefloat = Mathf.Max(damageCalculator.CalculateSubDamage(attacker, isCritical,0.5f) - (defender.CurrentDefence), 0);

        currentDamage = (int)Mathf.Round(damagefloat);

        defender.CurrnetHealth = defender.CurrnetHealth - currentDamage;

        //UI更新
        //經驗提升等
    }
    /// <summary>
    /// 造成屬性傷害(獲得敵方腳本扣生命值)
    /// </summary>
    public void TakeDamage(PlayerCharacterStats attacker, OtherCharacterStats defender, ElementType elementType, bool isCritical = false)
    {
        //獲得計算器
        DamageCalculator damageCalculator = new DamageCalculator();
        float currentDefence = defender.CurrentDefence;

        //扣除防禦力
        float damagefloat = Mathf.Max(damageCalculator.CalculateDamage(attacker, elementType) -
            (defender.CurrentDefence + (currentDefence *= defender.GetElementDefence(elementType))), 0);

        currentDamage = (int)Mathf.Round(damagefloat);

        //造成傷害
        defender.CurrnetHealth = defender.CurrnetHealth - currentDamage;

        //UI更新
        //經驗提升等
    }
    #endregion
    public void ResetData()
    {
        if (ElementDefences.Count != 0)
        {
            ElementDefences.Clear();

            ElementDefences.Add(characterData[currentCharacterID].fireElementDefence);
            ElementDefences.Add(characterData[currentCharacterID].iceElementDefence);
            ElementDefences.Add(characterData[currentCharacterID].windElementDefence);
            ElementDefences.Add(characterData[currentCharacterID].thunderElementDefence);
            ElementDefences.Add(characterData[currentCharacterID].lightElementDefence);
            ElementDefences.Add(characterData[currentCharacterID].darkElementDefence);
        }
    }
    public float GetElementDefence(ElementType elementType)
    {
        return ElementDefences[(int)elementType];
    }
}

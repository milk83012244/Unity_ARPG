using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 獲取角色資料並計算(可切換角色)
/// </summary>
public class CharacterStatsDataMutiMono : MonoBehaviour
{
    public int currentCharacterID;

    public List<CharacterBattleDataSO>  characterData = new List<CharacterBattleDataSO>();
    public List<CharacterAttackDataSO>  attackData = new List<CharacterAttackDataSO>();

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
    public int BaseDefence
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
    public int CurrentDefence
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

    #region 傷害計算
    /// <summary>
    /// 造成傷害(獲得敵方腳本扣生命值)
    /// </summary>
    public void TakeDamage(CharacterStatsDataMutiMono attacker, CharacterStatsDataMono defender)
    {
        int damage = Mathf.Max(attacker.CurrentDamage() - defender.CurrentDefence, 0); //Max最小只會是0不會變負值
        defender.CurrnetHealth = Mathf.Max(defender.CurrnetHealth - damage, 0);

        //UI更新
        //經驗提升等
    }

    /// <summary>
    /// 計算傷害
    /// </summary>
    public int CurrentDamage()
    {
        float coreDamage = UnityEngine.Random.Range(attackData[currentCharacterID].minDamage, attackData[currentCharacterID].maxDamage);

        //計算附加傷害

        if (isCritical)
        {
            coreDamage *= attackData[currentCharacterID].criticalMultplier;
            Debug.Log("爆擊" + coreDamage);
        }
        currentDamage = (int)coreDamage;
        return (int)coreDamage;
    }
    #endregion
}

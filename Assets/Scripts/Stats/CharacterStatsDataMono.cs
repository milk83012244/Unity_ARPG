using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 獲取角色資料並計算 
/// </summary>
public class CharacterStatsDataMono : MonoBehaviour
{
    public CharacterBattleDataSO characterData;
    public CharacterAttackDataSO attackData;

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
    public int BaseDefence
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
    public int CurrentDefence
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

    #region 傷害計算
    /// <summary>
    /// 造成傷害(獲得敵方腳本扣生命值)
    /// </summary>
    public void TakeDamage(CharacterStatsDataMono attacker, CharacterStatsDataMutiMono defender)//敵人只會攻擊到玩家
    {
        int damage = Mathf.Max(attacker.CurrentDamage() - defender.CurrentDefence,0); //Max最小只會是0不會變負值
        defender.CurrnetHealth = Mathf.Max(defender.CurrnetHealth - damage, 0);

        //UI更新
        //經驗提升等
    }

    /// <summary>
    /// 計算傷害
    /// </summary>
    public int CurrentDamage()
    {
        float coreDamage = UnityEngine.Random.Range(attackData.minDamage, attackData.maxDamage);

        //計算附加傷害

        if (isCritical)
        {
            coreDamage *= attackData.criticalMultplier;
            Debug.Log("爆擊" + coreDamage);
        }
        currentDamage = (int)coreDamage;
        return (int)coreDamage;
    }
    #endregion
}

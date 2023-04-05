using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 獲取角色資料並計算 
/// </summary>
public class CharacterStatsDataMono : MonoBehaviour
{
    public TestCharacterDataSO testCharacterData;
    public TestAttackDataSO testAttackData;

    [HideInInspector] public int currentDamage;
    [HideInInspector] public bool isCritical;

    #region 從ChatacterDataSO讀取值
    public int MaxHealth
    {
        get
        {
            if (testCharacterData != null)
                return testCharacterData.mexHealth;
            else
                return 0;
        }
        set
        {
            testCharacterData.mexHealth = value;
        }
    }
    public int CurrnetHealth
    {
        get
        {
            if (testCharacterData != null)
                return testCharacterData.currentHealth;
            else
                return 0;
        }
        set
        {
            testCharacterData.currentHealth = value;
        }
    }
    public int BaseDefence
    {
        get
        {
            if (testCharacterData != null)
                return testCharacterData.baseDefence;
            else
                return 0;
        }
        set
        {
            testCharacterData.baseDefence = value;
        }
    }
    public int CurrentDefence
    {
        get
        {
            if (testCharacterData != null)
                return testCharacterData.currentDefence;
            else
                return 0;
        }
        set
        {
            testCharacterData.currentDefence = value;
        }
    }
    #endregion

    #region 傷害計算
    /// <summary>
    /// 造成傷害(獲得敵方腳本扣生命值)
    /// </summary>
    public void TakeDamage(CharacterStatsDataMono attacker, CharacterStatsDataMono defender)
    {
        int damage = Mathf.Max(attacker.CurrentDamage() - defender.CurrentDefence,0); //Max最小只會是0不會變負值
        defender.CurrnetHealth = Mathf.Max(defender.CurrnetHealth - damage, 0);

        //UI更新
        //經驗提升等
    }

    /// <summary>
    /// 計算傷害
    /// </summary>
    private int CurrentDamage()
    {
        float coreDamage = UnityEngine.Random.Range(testAttackData.minDamage, testAttackData.maxDamage);

        if (isCritical)
        {
            coreDamage *= testAttackData.criticalMultplier;
            Debug.Log("爆擊" + coreDamage);
        }
        currentDamage = (int)coreDamage;
        return (int)coreDamage;
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCalculator
{
    [HideInInspector] public int currentDamage;
    [HideInInspector] public bool isCritical;

    /// <summary>
    /// 玩家攻擊計算
    /// </summary>
    public int CalculateDamage(PlayerCharacterStats attacker, bool isCritical = false,bool isSkill =false)
    {
        float coreDamage = UnityEngine.Random.Range(attacker.attackData[attacker.currentCharacterID].minDamage, attacker.attackData[attacker.currentCharacterID].maxDamage);
        Debug.Log("基礎傷害: " + coreDamage);
        //附加效果

        //技能傷害
        if (isSkill)
        {
            Debug.Log("技能傷害: " + coreDamage + " x " + attacker.attackData[attacker.currentCharacterID].skill1Multplier);
            coreDamage *= attacker.attackData[attacker.currentCharacterID].skill1Multplier;
        }

        if (isCritical)
        {
            Debug.Log("爆擊傷害: " + coreDamage + " x " + attacker.attackData[attacker.currentCharacterID].criticalMultplier);
            coreDamage *= attacker.attackData[attacker.currentCharacterID].criticalMultplier;
        }
        currentDamage = (int)Mathf.Round(coreDamage);
        Debug.Log(" 總傷害為: " + currentDamage);
        return currentDamage;
    }
    /// <summary>
    /// 玩家標記傷害計算
    /// </summary>
    public int CalculateMarkDamage(PlayerCharacterStats attacker, bool isCritical = false)
    {
        float coreDamage = UnityEngine.Random.Range(attacker.attackData[attacker.currentCharacterID].minDamage, attacker.attackData[attacker.currentCharacterID].maxDamage);
        Debug.Log("基礎傷害: " + coreDamage);
        //附加效果

        Debug.Log("標記傷害: " + coreDamage + " x " + attacker.attackData[attacker.currentCharacterID].markMultplier);
        coreDamage *= attacker.attackData[attacker.currentCharacterID].markMultplier;

        if (isCritical)
        {
            Debug.Log("爆擊傷害: " + coreDamage + " x " + attacker.attackData[attacker.currentCharacterID].criticalMultplier);
            coreDamage *= attacker.attackData[attacker.currentCharacterID].criticalMultplier;
        }
        currentDamage = (int)Mathf.Round(coreDamage);
        Debug.Log(" 總傷害為: " + currentDamage);
        return currentDamage;
    }
    /// <summary>
    /// 玩家副攻擊計算
    /// </summary>
    public int CalculateSubDamage(PlayerCharacterStats attacker, float subMultplier, bool isCritical = false)
    {
        float coreDamage = UnityEngine.Random.Range(attacker.attackData[attacker.currentCharacterID].minDamage, attacker.attackData[attacker.currentCharacterID].maxDamage);
        coreDamage *= subMultplier;
        Debug.Log("基礎傷害: " + coreDamage);
        //附加效果

        if (isCritical)
        {
            Debug.Log("爆擊傷害: " + coreDamage + " x " + attacker.attackData[attacker.currentCharacterID].criticalMultplier);
            coreDamage *= attacker.attackData[attacker.currentCharacterID].criticalMultplier;
        }
        currentDamage = (int)Mathf.Round(coreDamage);
        Debug.Log(" 總傷害為: " + currentDamage);
        return currentDamage;
    }
    /// <summary>
    /// 玩家屬性攻擊計算
    /// </summary>
    public float CalculateDamage(PlayerCharacterStats attacker, ElementType elementType)
    {
        float coreDamage = UnityEngine.Random.Range(attacker.attackData[attacker.currentCharacterID].minDamage, attacker.attackData[attacker.currentCharacterID].maxDamage);

        //計算附加傷害

        //計算屬性加成

        if (isCritical)
        {
            coreDamage *= attacker.attackData[attacker.currentCharacterID].criticalMultplier;
            Debug.Log("爆擊" + coreDamage);
        }
        //currentDamage = (int)coreDamage;
        return (int)coreDamage;
    }
    /// <summary>
    /// 非玩家控制角色傷害計算
    /// </summary>
    public float CalculateDamage(OtherCharacterStats attacker)
    {
        float coreDamage = UnityEngine.Random.Range(attacker.attackData.minDamage, attacker.attackData.maxDamage);

        //計算附加傷害

        if (isCritical)
        {
            coreDamage *= attacker.attackData.criticalMultplier;
            Debug.Log("爆擊" + coreDamage);
        }
        //currentDamage = (int)coreDamage;
        return (int)coreDamage;
    }
    public float CalculateDamage(OtherCharacterStats attacker, ElementType elementType)
    {
        float coreDamage = UnityEngine.Random.Range(attacker.attackData.minDamage, attacker.attackData.maxDamage);

        //計算附加傷害

        //計算屬性加成

        if (isCritical)
        {
            coreDamage *= attacker.attackData.criticalMultplier;
            Debug.Log("爆擊" + coreDamage);
        }
        //currentDamage = (int)coreDamage;
        return (int)coreDamage;
    }
}

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
    public int CalculateDamage(PlayerCharacterStats attacker)
    {
        float coreDamage = UnityEngine.Random.Range(attacker.attackData[attacker.currentCharacterID].minDamage, attacker.attackData[attacker.currentCharacterID].maxDamage);

        //附加效果

        if (isCritical)
        {
            coreDamage *= attacker.attackData[attacker.currentCharacterID].criticalMultplier;
            Debug.Log("爆擊" + coreDamage);
        }
        currentDamage = (int)coreDamage;
        return (int)coreDamage;
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
    /// 其他角色傷害計算
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

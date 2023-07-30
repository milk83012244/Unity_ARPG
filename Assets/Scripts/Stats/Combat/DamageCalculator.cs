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
    public int CalculateDamage(PlayerCharacterStats attacker, bool isCritical = false, bool isSkill1 =false, bool isSkill2 = false,bool isCounter=false, float freeMul = 0)
    {
        float coreDamage = UnityEngine.Random.Range(attacker.attackData[attacker.currentCharacterID].minDamage, attacker.attackData[attacker.currentCharacterID].maxDamage);
        Debug.Log(attacker.characterData[attacker.currentCharacterID].characterName + "基礎傷害: " + coreDamage);
        //附加效果

        //反擊傷害
        if (isCounter)
        {
            Debug.Log(attacker.characterData[attacker.currentCharacterID].characterName + "反擊傷害: " + coreDamage);
            coreDamage *= 1.3f;
        }
        //技能傷害
        if (isSkill1)
        {
            Debug.Log(attacker.characterData[attacker.currentCharacterID].characterName + "技能1傷害: " + coreDamage + " x " + attacker.attackData[attacker.currentCharacterID].skill1Multplier);
            coreDamage *= attacker.attackData[attacker.currentCharacterID].skill1Multplier;
        }
        if (isSkill2)
        {
            Debug.Log(attacker.characterData[attacker.currentCharacterID].characterName + "技能2傷害: " + coreDamage + " x " + attacker.attackData[attacker.currentCharacterID].skill2Multplier);
            coreDamage *= attacker.attackData[attacker.currentCharacterID].skill2Multplier;
        }
        if (freeMul != 0)
        {
            Debug.Log(attacker.characterData[attacker.currentCharacterID].characterName + "額外倍率傷害: " + coreDamage + " x " + freeMul);
            coreDamage *= freeMul;
        }

        if (isCritical)
        {
            Debug.Log(attacker.characterData[attacker.currentCharacterID].characterName + "爆擊傷害: " + coreDamage + " x " + attacker.attackData[attacker.currentCharacterID].criticalMultplier);
            coreDamage *= attacker.attackData[attacker.currentCharacterID].criticalMultplier;
        }
        currentDamage = (int)Mathf.Round(coreDamage);
        Debug.Log(attacker.characterData[attacker.currentCharacterID].characterName + " 總傷害為: " + currentDamage);
        return currentDamage;
    }
    /// <summary>
    /// 玩家標記傷害計算
    /// </summary>
    public int CalculateMarkDamage(PlayerCharacterStats attacker, bool isCritical = false, float freeMul = 0)
    {
        float coreDamage = UnityEngine.Random.Range(attacker.attackData[attacker.currentCharacterID].minDamage, attacker.attackData[attacker.currentCharacterID].maxDamage);
        Debug.Log(attacker.characterData[attacker.currentCharacterID].characterName + "基礎傷害: " + coreDamage);
        //附加效果

        Debug.Log("標記傷害: " + coreDamage + " x " + attacker.attackData[attacker.currentCharacterID].markMultplier);
        coreDamage *= attacker.attackData[attacker.currentCharacterID].markMultplier;

        if (isCritical)
        {
            Debug.Log(attacker.characterData[attacker.currentCharacterID].characterName + "爆擊傷害: " + coreDamage + " x " + attacker.attackData[attacker.currentCharacterID].criticalMultplier);
            coreDamage *= attacker.attackData[attacker.currentCharacterID].criticalMultplier;
        }
        if (freeMul != 0)
        {
            Debug.Log(attacker.characterData[attacker.currentCharacterID].characterName + "額外倍率傷害: " + coreDamage + " x " + freeMul);
            coreDamage *= freeMul;
        }
        currentDamage = (int)Mathf.Round(coreDamage);
        Debug.Log(attacker.characterData[attacker.currentCharacterID].characterName + " 總傷害為: " + currentDamage);
        return currentDamage;
    }

    /// <summary>
    /// 玩家屬性攻擊計算
    /// </summary>
    public float CalculateDamage(PlayerCharacterStats attacker, ElementType elementType, bool isCritical = false, bool isSkill1 = false, bool isSkill2 = false, float freeMul = 0)
    {
        float coreDamage = UnityEngine.Random.Range(attacker.attackData[attacker.currentCharacterID].minDamage, attacker.attackData[attacker.currentCharacterID].maxDamage);

        //計算屬性加成
        coreDamage *= (1 + (attacker.attackData[attacker.currentCharacterID].elementDamages[elementType] /= 100));

        //技能傷害
        if (isSkill1)
        {
            Debug.Log(attacker.characterData[attacker.currentCharacterID].characterName + "技能1傷害: " + coreDamage + " x " + attacker.attackData[attacker.currentCharacterID].skill1Multplier);
            coreDamage *= attacker.attackData[attacker.currentCharacterID].skill1Multplier;
        }
        if (isSkill2)
        {
            Debug.Log(attacker.characterData[attacker.currentCharacterID].characterName + "技能2傷害: " + coreDamage + " x " + attacker.attackData[attacker.currentCharacterID].skill2Multplier);
            coreDamage *= attacker.attackData[attacker.currentCharacterID].skill2Multplier;
        }
        if (freeMul != 0)
        {
            Debug.Log(attacker.characterData[attacker.currentCharacterID].characterName + "額外倍率傷害: " + coreDamage + " x " + freeMul);
            coreDamage *= freeMul;
        }

        if (isCritical)
        {
            coreDamage *= attacker.attackData[attacker.currentCharacterID].criticalMultplier;
            Debug.Log(attacker.characterData[attacker.currentCharacterID].characterName +"造成爆擊" + coreDamage);
        }
        //currentDamage = (int)coreDamage;
        return (int)coreDamage;
    }

    /// <summary>
    /// 玩家副攻擊計算
    /// </summary>
    public int CalculateSubDamage(PlayerCharacterStats attacker, float subMultplier, bool isCritical = false, float freeMul = 0)
    {
        float coreDamage = UnityEngine.Random.Range(attacker.attackData[attacker.currentCharacterID].minDamage, attacker.attackData[attacker.currentCharacterID].maxDamage);
        coreDamage *= subMultplier;
        Debug.Log(attacker.characterData[attacker.currentCharacterID].characterName + "基礎傷害: " + coreDamage);
        //附加效果
        if (freeMul != 0)
        {
            Debug.Log(attacker.characterData[attacker.currentCharacterID].characterName + "額外倍率傷害: " + coreDamage + " x " + freeMul);
            coreDamage *= freeMul;
        }

        if (isCritical)
        {
            Debug.Log(attacker.characterData[attacker.currentCharacterID].characterName + "爆擊傷害: " + coreDamage + " x " + attacker.attackData[attacker.currentCharacterID].criticalMultplier);
            coreDamage *= attacker.attackData[attacker.currentCharacterID].criticalMultplier;
        }
        currentDamage = (int)Mathf.Round(coreDamage);
        Debug.Log(attacker.characterData[attacker.currentCharacterID].characterName + " 總傷害為: " + currentDamage);
        return currentDamage;
    }
    /// <summary>
    /// 玩家屬性副攻擊計算
    /// </summary>
    public int CalculateSubDamage(PlayerCharacterStats attacker, float subMultplier, ElementType elementType, bool isCritical = false, float freeMul = 0)
    {

        float coreDamage = UnityEngine.Random.Range(attacker.attackData[attacker.currentCharacterID].minDamage, attacker.attackData[attacker.currentCharacterID].maxDamage);
        coreDamage *= subMultplier;
        Debug.Log(attacker.characterData[attacker.currentCharacterID].characterName + "基礎傷害: " + coreDamage);

        coreDamage *= (1 + (attacker.attackData[attacker.currentCharacterID].elementDamages[elementType] /= 100));

        if (freeMul != 0)
        {
            Debug.Log(attacker.characterData[attacker.currentCharacterID].characterName + "額外倍率傷害: " + coreDamage + " x " + freeMul);
            coreDamage *= freeMul;
        }

        if (isCritical)
        {
            Debug.Log(attacker.characterData[attacker.currentCharacterID].characterName + "爆擊傷害: " + coreDamage + " x " + attacker.attackData[attacker.currentCharacterID].criticalMultplier);
            coreDamage *= attacker.attackData[attacker.currentCharacterID].criticalMultplier;
        }
        currentDamage = (int)Mathf.Round(coreDamage);
        Debug.Log(attacker.characterData[attacker.currentCharacterID].characterName + " 總傷害為: " + currentDamage);
        return currentDamage;
    }
    /// <summary>
    /// Lia技能2Buff副傷害
    /// </summary>
    public int CalculateLiaSkill2SubDamage(PlayerCharacterStats attacker, float subMultplier, ElementType elementType, bool isCritical = false, float freeMul = 0)
    {
        float coreDamage = UnityEngine.Random.Range(attacker.attackData[attacker.currentCharacterID].minDamage, attacker.attackData[attacker.currentCharacterID].maxDamage);

        //計算屬性加成
        coreDamage *= (1 + (attacker.attackData[2].elementDamages[elementType] /= 100));

        coreDamage *= subMultplier;
        Debug.Log( "Lia技能2BUFF基礎傷害: " + coreDamage);

        if (freeMul != 0)
        {
            Debug.Log(attacker.characterData[attacker.currentCharacterID].characterName + "額外倍率傷害: " + coreDamage + " x " + freeMul);
            coreDamage *= freeMul;
        }

        if (isCritical)
        {
            Debug.Log("Lia技能2BUFF爆擊傷害: " + coreDamage + " x " + attacker.attackData[attacker.currentCharacterID].criticalMultplier);
            coreDamage *= attacker.attackData[attacker.currentCharacterID].criticalMultplier;
        }
        currentDamage = (int)Mathf.Round(coreDamage);
        Debug.Log( "Lia技能2BUFF總傷害為: " + currentDamage);
        return currentDamage;
    }

    // 非玩家控制角色傷害計算
    /// <summary>
    /// 敵人攻擊計算
    /// </summary>
    public int CalculateDamage(OtherCharacterStats attacker, bool isCritical = false, bool isSkill = false, float freeMul = 0)
    {
        float coreDamage = UnityEngine.Random.Range(attacker.enemyAttackData.minDamage, attacker.enemyAttackData.maxDamage);
        Debug.Log(attacker.enemyBattleData.characterName + "基礎傷害: " + coreDamage);
        //附加效果

        //技能傷害
        if (isSkill)
        {
            Debug.Log(attacker.enemyBattleData.characterName + "技能傷害: " + coreDamage + " x " + attacker.enemyAttackData.skill1Multplier);
            coreDamage *= attacker.enemyAttackData.skill1Multplier;
        }

        if (freeMul != 0)
        {
            Debug.Log(attacker.enemyBattleData.characterName + "額外倍率傷害: " + coreDamage + " x " + freeMul);
            coreDamage *= freeMul;
        }

        if (isCritical)
        {
            Debug.Log(attacker.enemyBattleData.characterName + "爆擊傷害: " + coreDamage + " x " + attacker.enemyAttackData.criticalMultplier);
            coreDamage *= attacker.enemyAttackData.criticalMultplier;
        }
        currentDamage = (int)Mathf.Round(coreDamage);
        Debug.Log(attacker.enemyBattleData.characterName + " 總傷害為: " + currentDamage);
        return currentDamage;
    }
    /// <summary>
    /// 敵人屬性攻擊計算
    /// </summary>
    public float CalculateDamage(OtherCharacterStats attacker, ElementType elementType, bool isSkill = false, float freeMul = 0)
    {
        float coreDamage = UnityEngine.Random.Range(attacker.enemyAttackData.minDamage, attacker.enemyAttackData.maxDamage);

        coreDamage *= (1 + (attacker.enemyAttackData.elementDamages[elementType] /= 100));

        if (isSkill)
        {
            Debug.Log(attacker.enemyBattleData.characterName + "技能傷害: " + coreDamage + " x " + attacker.enemyAttackData.skill1Multplier);
            coreDamage *= attacker.enemyAttackData.skill1Multplier;
        }

        if (freeMul != 0)
        {
            Debug.Log(attacker.enemyBattleData.characterName + "額外倍率傷害: " + coreDamage + " x " + freeMul);
            coreDamage *= freeMul;
        }

        //計算屬性加成
        Debug.Log(attacker.enemyBattleData.characterName + "屬性傷害" + coreDamage);

        if (isCritical)
        {
            coreDamage *= attacker.enemyAttackData.criticalMultplier;
            Debug.Log(attacker.enemyBattleData.characterName + "屬性爆擊傷害" + coreDamage);
        }
        //currentDamage = (int)coreDamage;
        return (int)coreDamage;
    }
}

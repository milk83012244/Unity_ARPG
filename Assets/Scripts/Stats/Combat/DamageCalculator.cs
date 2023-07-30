using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCalculator
{
    [HideInInspector] public int currentDamage;
    [HideInInspector] public bool isCritical;

    /// <summary>
    /// ���a�����p��
    /// </summary>
    public int CalculateDamage(PlayerCharacterStats attacker, bool isCritical = false, bool isSkill1 =false, bool isSkill2 = false,bool isCounter=false, float freeMul = 0)
    {
        float coreDamage = UnityEngine.Random.Range(attacker.attackData[attacker.currentCharacterID].minDamage, attacker.attackData[attacker.currentCharacterID].maxDamage);
        Debug.Log(attacker.characterData[attacker.currentCharacterID].characterName + "��¦�ˮ`: " + coreDamage);
        //���[�ĪG

        //�����ˮ`
        if (isCounter)
        {
            Debug.Log(attacker.characterData[attacker.currentCharacterID].characterName + "�����ˮ`: " + coreDamage);
            coreDamage *= 1.3f;
        }
        //�ޯ�ˮ`
        if (isSkill1)
        {
            Debug.Log(attacker.characterData[attacker.currentCharacterID].characterName + "�ޯ�1�ˮ`: " + coreDamage + " x " + attacker.attackData[attacker.currentCharacterID].skill1Multplier);
            coreDamage *= attacker.attackData[attacker.currentCharacterID].skill1Multplier;
        }
        if (isSkill2)
        {
            Debug.Log(attacker.characterData[attacker.currentCharacterID].characterName + "�ޯ�2�ˮ`: " + coreDamage + " x " + attacker.attackData[attacker.currentCharacterID].skill2Multplier);
            coreDamage *= attacker.attackData[attacker.currentCharacterID].skill2Multplier;
        }
        if (freeMul != 0)
        {
            Debug.Log(attacker.characterData[attacker.currentCharacterID].characterName + "�B�~���v�ˮ`: " + coreDamage + " x " + freeMul);
            coreDamage *= freeMul;
        }

        if (isCritical)
        {
            Debug.Log(attacker.characterData[attacker.currentCharacterID].characterName + "�z���ˮ`: " + coreDamage + " x " + attacker.attackData[attacker.currentCharacterID].criticalMultplier);
            coreDamage *= attacker.attackData[attacker.currentCharacterID].criticalMultplier;
        }
        currentDamage = (int)Mathf.Round(coreDamage);
        Debug.Log(attacker.characterData[attacker.currentCharacterID].characterName + " �`�ˮ`��: " + currentDamage);
        return currentDamage;
    }
    /// <summary>
    /// ���a�аO�ˮ`�p��
    /// </summary>
    public int CalculateMarkDamage(PlayerCharacterStats attacker, bool isCritical = false, float freeMul = 0)
    {
        float coreDamage = UnityEngine.Random.Range(attacker.attackData[attacker.currentCharacterID].minDamage, attacker.attackData[attacker.currentCharacterID].maxDamage);
        Debug.Log(attacker.characterData[attacker.currentCharacterID].characterName + "��¦�ˮ`: " + coreDamage);
        //���[�ĪG

        Debug.Log("�аO�ˮ`: " + coreDamage + " x " + attacker.attackData[attacker.currentCharacterID].markMultplier);
        coreDamage *= attacker.attackData[attacker.currentCharacterID].markMultplier;

        if (isCritical)
        {
            Debug.Log(attacker.characterData[attacker.currentCharacterID].characterName + "�z���ˮ`: " + coreDamage + " x " + attacker.attackData[attacker.currentCharacterID].criticalMultplier);
            coreDamage *= attacker.attackData[attacker.currentCharacterID].criticalMultplier;
        }
        if (freeMul != 0)
        {
            Debug.Log(attacker.characterData[attacker.currentCharacterID].characterName + "�B�~���v�ˮ`: " + coreDamage + " x " + freeMul);
            coreDamage *= freeMul;
        }
        currentDamage = (int)Mathf.Round(coreDamage);
        Debug.Log(attacker.characterData[attacker.currentCharacterID].characterName + " �`�ˮ`��: " + currentDamage);
        return currentDamage;
    }

    /// <summary>
    /// ���a�ݩʧ����p��
    /// </summary>
    public float CalculateDamage(PlayerCharacterStats attacker, ElementType elementType, bool isCritical = false, bool isSkill1 = false, bool isSkill2 = false, float freeMul = 0)
    {
        float coreDamage = UnityEngine.Random.Range(attacker.attackData[attacker.currentCharacterID].minDamage, attacker.attackData[attacker.currentCharacterID].maxDamage);

        //�p���ݩʥ[��
        coreDamage *= (1 + (attacker.attackData[attacker.currentCharacterID].elementDamages[elementType] /= 100));

        //�ޯ�ˮ`
        if (isSkill1)
        {
            Debug.Log(attacker.characterData[attacker.currentCharacterID].characterName + "�ޯ�1�ˮ`: " + coreDamage + " x " + attacker.attackData[attacker.currentCharacterID].skill1Multplier);
            coreDamage *= attacker.attackData[attacker.currentCharacterID].skill1Multplier;
        }
        if (isSkill2)
        {
            Debug.Log(attacker.characterData[attacker.currentCharacterID].characterName + "�ޯ�2�ˮ`: " + coreDamage + " x " + attacker.attackData[attacker.currentCharacterID].skill2Multplier);
            coreDamage *= attacker.attackData[attacker.currentCharacterID].skill2Multplier;
        }
        if (freeMul != 0)
        {
            Debug.Log(attacker.characterData[attacker.currentCharacterID].characterName + "�B�~���v�ˮ`: " + coreDamage + " x " + freeMul);
            coreDamage *= freeMul;
        }

        if (isCritical)
        {
            coreDamage *= attacker.attackData[attacker.currentCharacterID].criticalMultplier;
            Debug.Log(attacker.characterData[attacker.currentCharacterID].characterName +"�y���z��" + coreDamage);
        }
        //currentDamage = (int)coreDamage;
        return (int)coreDamage;
    }

    /// <summary>
    /// ���a�Ƨ����p��
    /// </summary>
    public int CalculateSubDamage(PlayerCharacterStats attacker, float subMultplier, bool isCritical = false, float freeMul = 0)
    {
        float coreDamage = UnityEngine.Random.Range(attacker.attackData[attacker.currentCharacterID].minDamage, attacker.attackData[attacker.currentCharacterID].maxDamage);
        coreDamage *= subMultplier;
        Debug.Log(attacker.characterData[attacker.currentCharacterID].characterName + "��¦�ˮ`: " + coreDamage);
        //���[�ĪG
        if (freeMul != 0)
        {
            Debug.Log(attacker.characterData[attacker.currentCharacterID].characterName + "�B�~���v�ˮ`: " + coreDamage + " x " + freeMul);
            coreDamage *= freeMul;
        }

        if (isCritical)
        {
            Debug.Log(attacker.characterData[attacker.currentCharacterID].characterName + "�z���ˮ`: " + coreDamage + " x " + attacker.attackData[attacker.currentCharacterID].criticalMultplier);
            coreDamage *= attacker.attackData[attacker.currentCharacterID].criticalMultplier;
        }
        currentDamage = (int)Mathf.Round(coreDamage);
        Debug.Log(attacker.characterData[attacker.currentCharacterID].characterName + " �`�ˮ`��: " + currentDamage);
        return currentDamage;
    }
    /// <summary>
    /// ���a�ݩʰƧ����p��
    /// </summary>
    public int CalculateSubDamage(PlayerCharacterStats attacker, float subMultplier, ElementType elementType, bool isCritical = false, float freeMul = 0)
    {

        float coreDamage = UnityEngine.Random.Range(attacker.attackData[attacker.currentCharacterID].minDamage, attacker.attackData[attacker.currentCharacterID].maxDamage);
        coreDamage *= subMultplier;
        Debug.Log(attacker.characterData[attacker.currentCharacterID].characterName + "��¦�ˮ`: " + coreDamage);

        coreDamage *= (1 + (attacker.attackData[attacker.currentCharacterID].elementDamages[elementType] /= 100));

        if (freeMul != 0)
        {
            Debug.Log(attacker.characterData[attacker.currentCharacterID].characterName + "�B�~���v�ˮ`: " + coreDamage + " x " + freeMul);
            coreDamage *= freeMul;
        }

        if (isCritical)
        {
            Debug.Log(attacker.characterData[attacker.currentCharacterID].characterName + "�z���ˮ`: " + coreDamage + " x " + attacker.attackData[attacker.currentCharacterID].criticalMultplier);
            coreDamage *= attacker.attackData[attacker.currentCharacterID].criticalMultplier;
        }
        currentDamage = (int)Mathf.Round(coreDamage);
        Debug.Log(attacker.characterData[attacker.currentCharacterID].characterName + " �`�ˮ`��: " + currentDamage);
        return currentDamage;
    }
    /// <summary>
    /// Lia�ޯ�2Buff�ƶˮ`
    /// </summary>
    public int CalculateLiaSkill2SubDamage(PlayerCharacterStats attacker, float subMultplier, ElementType elementType, bool isCritical = false, float freeMul = 0)
    {
        float coreDamage = UnityEngine.Random.Range(attacker.attackData[attacker.currentCharacterID].minDamage, attacker.attackData[attacker.currentCharacterID].maxDamage);

        //�p���ݩʥ[��
        coreDamage *= (1 + (attacker.attackData[2].elementDamages[elementType] /= 100));

        coreDamage *= subMultplier;
        Debug.Log( "Lia�ޯ�2BUFF��¦�ˮ`: " + coreDamage);

        if (freeMul != 0)
        {
            Debug.Log(attacker.characterData[attacker.currentCharacterID].characterName + "�B�~���v�ˮ`: " + coreDamage + " x " + freeMul);
            coreDamage *= freeMul;
        }

        if (isCritical)
        {
            Debug.Log("Lia�ޯ�2BUFF�z���ˮ`: " + coreDamage + " x " + attacker.attackData[attacker.currentCharacterID].criticalMultplier);
            coreDamage *= attacker.attackData[attacker.currentCharacterID].criticalMultplier;
        }
        currentDamage = (int)Mathf.Round(coreDamage);
        Debug.Log( "Lia�ޯ�2BUFF�`�ˮ`��: " + currentDamage);
        return currentDamage;
    }

    // �D���a�����ˮ`�p��
    /// <summary>
    /// �ĤH�����p��
    /// </summary>
    public int CalculateDamage(OtherCharacterStats attacker, bool isCritical = false, bool isSkill = false, float freeMul = 0)
    {
        float coreDamage = UnityEngine.Random.Range(attacker.enemyAttackData.minDamage, attacker.enemyAttackData.maxDamage);
        Debug.Log(attacker.enemyBattleData.characterName + "��¦�ˮ`: " + coreDamage);
        //���[�ĪG

        //�ޯ�ˮ`
        if (isSkill)
        {
            Debug.Log(attacker.enemyBattleData.characterName + "�ޯ�ˮ`: " + coreDamage + " x " + attacker.enemyAttackData.skill1Multplier);
            coreDamage *= attacker.enemyAttackData.skill1Multplier;
        }

        if (freeMul != 0)
        {
            Debug.Log(attacker.enemyBattleData.characterName + "�B�~���v�ˮ`: " + coreDamage + " x " + freeMul);
            coreDamage *= freeMul;
        }

        if (isCritical)
        {
            Debug.Log(attacker.enemyBattleData.characterName + "�z���ˮ`: " + coreDamage + " x " + attacker.enemyAttackData.criticalMultplier);
            coreDamage *= attacker.enemyAttackData.criticalMultplier;
        }
        currentDamage = (int)Mathf.Round(coreDamage);
        Debug.Log(attacker.enemyBattleData.characterName + " �`�ˮ`��: " + currentDamage);
        return currentDamage;
    }
    /// <summary>
    /// �ĤH�ݩʧ����p��
    /// </summary>
    public float CalculateDamage(OtherCharacterStats attacker, ElementType elementType, bool isSkill = false, float freeMul = 0)
    {
        float coreDamage = UnityEngine.Random.Range(attacker.enemyAttackData.minDamage, attacker.enemyAttackData.maxDamage);

        coreDamage *= (1 + (attacker.enemyAttackData.elementDamages[elementType] /= 100));

        if (isSkill)
        {
            Debug.Log(attacker.enemyBattleData.characterName + "�ޯ�ˮ`: " + coreDamage + " x " + attacker.enemyAttackData.skill1Multplier);
            coreDamage *= attacker.enemyAttackData.skill1Multplier;
        }

        if (freeMul != 0)
        {
            Debug.Log(attacker.enemyBattleData.characterName + "�B�~���v�ˮ`: " + coreDamage + " x " + freeMul);
            coreDamage *= freeMul;
        }

        //�p���ݩʥ[��
        Debug.Log(attacker.enemyBattleData.characterName + "�ݩʶˮ`" + coreDamage);

        if (isCritical)
        {
            coreDamage *= attacker.enemyAttackData.criticalMultplier;
            Debug.Log(attacker.enemyBattleData.characterName + "�ݩ��z���ˮ`" + coreDamage);
        }
        //currentDamage = (int)coreDamage;
        return (int)coreDamage;
    }
}

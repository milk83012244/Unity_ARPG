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
    public int CalculateDamage(PlayerCharacterStats attacker, bool isCritical = false,bool isSkill =false)
    {
        float coreDamage = UnityEngine.Random.Range(attacker.attackData[attacker.currentCharacterID].minDamage, attacker.attackData[attacker.currentCharacterID].maxDamage);
        Debug.Log("��¦�ˮ`: " + coreDamage);
        //���[�ĪG

        //�ޯ�ˮ`
        if (isSkill)
        {
            Debug.Log("�ޯ�ˮ`: " + coreDamage + " x " + attacker.attackData[attacker.currentCharacterID].skill1Multplier);
            coreDamage *= attacker.attackData[attacker.currentCharacterID].skill1Multplier;
        }

        if (isCritical)
        {
            Debug.Log("�z���ˮ`: " + coreDamage + " x " + attacker.attackData[attacker.currentCharacterID].criticalMultplier);
            coreDamage *= attacker.attackData[attacker.currentCharacterID].criticalMultplier;
        }
        currentDamage = (int)Mathf.Round(coreDamage);
        Debug.Log(" �`�ˮ`��: " + currentDamage);
        return currentDamage;
    }
    /// <summary>
    /// ���a�аO�ˮ`�p��
    /// </summary>
    public int CalculateMarkDamage(PlayerCharacterStats attacker, bool isCritical = false)
    {
        float coreDamage = UnityEngine.Random.Range(attacker.attackData[attacker.currentCharacterID].minDamage, attacker.attackData[attacker.currentCharacterID].maxDamage);
        Debug.Log("��¦�ˮ`: " + coreDamage);
        //���[�ĪG

        Debug.Log("�аO�ˮ`: " + coreDamage + " x " + attacker.attackData[attacker.currentCharacterID].markMultplier);
        coreDamage *= attacker.attackData[attacker.currentCharacterID].markMultplier;

        if (isCritical)
        {
            Debug.Log("�z���ˮ`: " + coreDamage + " x " + attacker.attackData[attacker.currentCharacterID].criticalMultplier);
            coreDamage *= attacker.attackData[attacker.currentCharacterID].criticalMultplier;
        }
        currentDamage = (int)Mathf.Round(coreDamage);
        Debug.Log(" �`�ˮ`��: " + currentDamage);
        return currentDamage;
    }
    /// <summary>
    /// ���a�Ƨ����p��
    /// </summary>
    public int CalculateSubDamage(PlayerCharacterStats attacker, float subMultplier, bool isCritical = false)
    {
        float coreDamage = UnityEngine.Random.Range(attacker.attackData[attacker.currentCharacterID].minDamage, attacker.attackData[attacker.currentCharacterID].maxDamage);
        coreDamage *= subMultplier;
        Debug.Log("��¦�ˮ`: " + coreDamage);
        //���[�ĪG

        if (isCritical)
        {
            Debug.Log("�z���ˮ`: " + coreDamage + " x " + attacker.attackData[attacker.currentCharacterID].criticalMultplier);
            coreDamage *= attacker.attackData[attacker.currentCharacterID].criticalMultplier;
        }
        currentDamage = (int)Mathf.Round(coreDamage);
        Debug.Log(" �`�ˮ`��: " + currentDamage);
        return currentDamage;
    }
    /// <summary>
    /// ���a�ݩʧ����p��
    /// </summary>
    public float CalculateDamage(PlayerCharacterStats attacker, ElementType elementType)
    {
        float coreDamage = UnityEngine.Random.Range(attacker.attackData[attacker.currentCharacterID].minDamage, attacker.attackData[attacker.currentCharacterID].maxDamage);

        //�p����[�ˮ`

        //�p���ݩʥ[��

        if (isCritical)
        {
            coreDamage *= attacker.attackData[attacker.currentCharacterID].criticalMultplier;
            Debug.Log("�z��" + coreDamage);
        }
        //currentDamage = (int)coreDamage;
        return (int)coreDamage;
    }
    /// <summary>
    /// �D���a�����ˮ`�p��
    /// </summary>
    public float CalculateDamage(OtherCharacterStats attacker)
    {
        float coreDamage = UnityEngine.Random.Range(attacker.attackData.minDamage, attacker.attackData.maxDamage);

        //�p����[�ˮ`

        if (isCritical)
        {
            coreDamage *= attacker.attackData.criticalMultplier;
            Debug.Log("�z��" + coreDamage);
        }
        //currentDamage = (int)coreDamage;
        return (int)coreDamage;
    }
    public float CalculateDamage(OtherCharacterStats attacker, ElementType elementType)
    {
        float coreDamage = UnityEngine.Random.Range(attacker.attackData.minDamage, attacker.attackData.maxDamage);

        //�p����[�ˮ`

        //�p���ݩʥ[��

        if (isCritical)
        {
            coreDamage *= attacker.attackData.criticalMultplier;
            Debug.Log("�z��" + coreDamage);
        }
        //currentDamage = (int)coreDamage;
        return (int)coreDamage;
    }
}

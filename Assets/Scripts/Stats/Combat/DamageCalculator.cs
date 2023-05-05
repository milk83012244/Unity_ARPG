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
    public int CalculateDamage(PlayerCharacterStats attacker)
    {
        float coreDamage = UnityEngine.Random.Range(attacker.attackData[attacker.currentCharacterID].minDamage, attacker.attackData[attacker.currentCharacterID].maxDamage);

        //���[�ĪG

        if (isCritical)
        {
            coreDamage *= attacker.attackData[attacker.currentCharacterID].criticalMultplier;
            Debug.Log("�z��" + coreDamage);
        }
        currentDamage = (int)coreDamage;
        return (int)coreDamage;
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
    /// ��L����ˮ`�p��
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

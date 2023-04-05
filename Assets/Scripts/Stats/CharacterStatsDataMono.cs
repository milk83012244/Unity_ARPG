using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��������ƨíp�� 
/// </summary>
public class CharacterStatsDataMono : MonoBehaviour
{
    public TestCharacterDataSO testCharacterData;
    public TestAttackDataSO testAttackData;

    [HideInInspector] public int currentDamage;
    [HideInInspector] public bool isCritical;

    #region �qChatacterDataSOŪ����
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

    #region �ˮ`�p��
    /// <summary>
    /// �y���ˮ`(��o�Ĥ�}�����ͩR��)
    /// </summary>
    public void TakeDamage(CharacterStatsDataMono attacker, CharacterStatsDataMono defender)
    {
        int damage = Mathf.Max(attacker.CurrentDamage() - defender.CurrentDefence,0); //Max�̤p�u�|�O0���|�ܭt��
        defender.CurrnetHealth = Mathf.Max(defender.CurrnetHealth - damage, 0);

        //UI��s
        //�g�紣�ɵ�
    }

    /// <summary>
    /// �p��ˮ`
    /// </summary>
    private int CurrentDamage()
    {
        float coreDamage = UnityEngine.Random.Range(testAttackData.minDamage, testAttackData.maxDamage);

        if (isCritical)
        {
            coreDamage *= testAttackData.criticalMultplier;
            Debug.Log("�z��" + coreDamage);
        }
        currentDamage = (int)coreDamage;
        return (int)coreDamage;
    }
    #endregion
}

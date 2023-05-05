using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��������ƨíp�� 
/// </summary>
public class OtherCharacterStats : MonoBehaviour
{
    public CharacterBattleDataSO characterData;
    public CharacterAttackDataSO attackData;

    public List<float> ElementDefences = new List<float>();

    [HideInInspector] public int currentDamage;
    [HideInInspector] public bool isCritical;

    #region �qChatacterDataSOŪ����
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
    public float BaseDefence
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
    public float CurrentDefence
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

    private void Start()
    {
        ElementDefences.Add(characterData.fireElementDefence);
        ElementDefences.Add(characterData.iceElementDefence);
        ElementDefences.Add(characterData.windElementDefence);
        ElementDefences.Add(characterData.thunderElementDefence);
        ElementDefences.Add(characterData.lightElementDefence);
        ElementDefences.Add(characterData.darkElementDefence);
    }

    #region �ˮ`�p��
    /// <summary>
    /// �y���ˮ`(��o�Ĥ�}�����ͩR��)
    /// </summary>
    public void TakeDamage(OtherCharacterStats attacker, PlayerCharacterStats defender, bool isCritical = false)//�ĤH�u�|�����쪱�a
    {
        DamageCalculator damageCalculator = new DamageCalculator();

        float damagefloat = Mathf.Max(damageCalculator.CalculateDamage(attacker) - (defender.CurrentDefence), 0);

        int damage = (int)Mathf.Round(damagefloat);

        defender.CurrnetHealth = defender.CurrnetHealth - damage;

        //UI��s
        //�g�紣�ɵ�
    }
    /// <summary>
    /// �y���ݩʶˮ`
    /// </summary>
    public void TakeDamage(PlayerCharacterStats attacker, OtherCharacterStats defender, ElementType elementType, bool isCritical = false)
    {
        DamageCalculator damageCalculator = new DamageCalculator();
        float currentDefence = defender.CurrentDefence;

        float damagefloat = Mathf.Max(damageCalculator.CalculateDamage(attacker, elementType) -
            (defender.CurrentDefence + (currentDefence *= defender.GetElementDefence(elementType))), 0);

        int damage = (int)Mathf.Round(damagefloat);

        defender.CurrnetHealth = defender.CurrnetHealth - damage;

        //UI��s
        //�g�紣�ɵ�
    }
    #endregion
    public float GetElementDefence(ElementType elementType)
    {
        return ElementDefences[(int)elementType];
    }
}

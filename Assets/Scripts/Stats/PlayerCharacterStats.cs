using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��������� �i�y���ˮ`(�i��������)
/// </summary>
public class PlayerCharacterStats : MonoBehaviour
{
    public int currentCharacterID;

    //����n�令����������⭫�s���J���������⪺���(�Φr�嵥��ȹ��x�s)
    public List<CharacterBattleDataSO>  characterData = new List<CharacterBattleDataSO>();
    public List<CharacterAttackDataSO>  attackData = new List<CharacterAttackDataSO>();

    public List<float> ElementDefences = new List<float>();

    [HideInInspector] public int currentDamage;
    [HideInInspector] public bool isCritical;

    #region �qChatacterDataSOŪ����
    public int MaxHealth
    {
        get
        {
            if (characterData != null)
                return characterData[currentCharacterID].mexHealth;
            else
                return 0;
        }
        set
        {
            characterData[currentCharacterID].mexHealth = value;
        }
    }
    public int CurrnetHealth
    {
        get
        {
            if (characterData != null)
                return characterData[currentCharacterID].currentHealth;
            else
                return 0;
        }
        set
        {
            characterData[currentCharacterID].currentHealth = value;
        }
    }
    public float BaseDefence
    {
        get
        {
            if (characterData != null)
                return characterData[currentCharacterID].baseDefence;
            else
                return 0;
        }
        set
        {
            characterData[currentCharacterID].baseDefence = value;
        }
    }
    public float CurrentDefence
    {
        get
        {
            if (characterData != null)
                return characterData[currentCharacterID].currentDefence;
            else
                return 0;
        }
        set
        {
            characterData[currentCharacterID].currentDefence = value;
        }
    }
    #endregion

    private void Start()
    {
        ElementDefences.Add(characterData[currentCharacterID].fireElementDefence);
        ElementDefences.Add(characterData[currentCharacterID].iceElementDefence);
        ElementDefences.Add(characterData[currentCharacterID].windElementDefence);
        ElementDefences.Add(characterData[currentCharacterID].thunderElementDefence);
        ElementDefences.Add(characterData[currentCharacterID].lightElementDefence);
        ElementDefences.Add(characterData[currentCharacterID].darkElementDefence);
    }

    #region �ˮ`�p��
    /// <summary>
    /// �y���ˮ`(��o�Ĥ�}�����ͩR��)
    /// </summary>
    public void TakeDamage(PlayerCharacterStats attacker, OtherCharacterStats defender, bool isCritical = false,bool isSkill=false)
    {
        DamageCalculator damageCalculator = new DamageCalculator();

        float damagefloat = Mathf.Max(damageCalculator.CalculateDamage(attacker, isCritical,isSkill) - (defender.CurrentDefence), 0);

        currentDamage = (int)Mathf.Round(damagefloat);

        defender.CurrnetHealth = defender.CurrnetHealth - currentDamage;

        //UI��s
        //�g�紣�ɵ�
    }
    /// <summary>
    /// �аO�ˮ`
    /// </summary>
    public void TakeMarkDamage(PlayerCharacterStats attacker, OtherCharacterStats defender, bool isCritical = false)
    {
        DamageCalculator damageCalculator = new DamageCalculator();

        float damagefloat = Mathf.Max(damageCalculator.CalculateMarkDamage(attacker, isCritical) - (defender.CurrentDefence), 0);

        currentDamage = (int)Mathf.Round(damagefloat);

        defender.CurrnetHealth = defender.CurrnetHealth - currentDamage;

        //UI��s
        //�g�紣�ɵ�
    }
    /// <summary>
    /// �ƶˮ`
    /// </summary>
    public void TakeSubDamage(PlayerCharacterStats attacker, OtherCharacterStats defender, bool isCritical = false, bool isSkill = false)
    {
        DamageCalculator damageCalculator = new DamageCalculator();

        float damagefloat = Mathf.Max(damageCalculator.CalculateSubDamage(attacker, isCritical,0.5f) - (defender.CurrentDefence), 0);

        currentDamage = (int)Mathf.Round(damagefloat);

        defender.CurrnetHealth = defender.CurrnetHealth - currentDamage;

        //UI��s
        //�g�紣�ɵ�
    }
    /// <summary>
    /// �y���ݩʶˮ`(��o�Ĥ�}�����ͩR��)
    /// </summary>
    public void TakeDamage(PlayerCharacterStats attacker, OtherCharacterStats defender, ElementType elementType, bool isCritical = false)
    {
        //��o�p�⾹
        DamageCalculator damageCalculator = new DamageCalculator();
        float currentDefence = defender.CurrentDefence;

        //�������m�O
        float damagefloat = Mathf.Max(damageCalculator.CalculateDamage(attacker, elementType) -
            (defender.CurrentDefence + (currentDefence *= defender.GetElementDefence(elementType))), 0);

        currentDamage = (int)Mathf.Round(damagefloat);

        //�y���ˮ`
        defender.CurrnetHealth = defender.CurrnetHealth - currentDamage;

        //UI��s
        //�g�紣�ɵ�
    }
    #endregion
    public void ResetData()
    {
        if (ElementDefences.Count != 0)
        {
            ElementDefences.Clear();

            ElementDefences.Add(characterData[currentCharacterID].fireElementDefence);
            ElementDefences.Add(characterData[currentCharacterID].iceElementDefence);
            ElementDefences.Add(characterData[currentCharacterID].windElementDefence);
            ElementDefences.Add(characterData[currentCharacterID].thunderElementDefence);
            ElementDefences.Add(characterData[currentCharacterID].lightElementDefence);
            ElementDefences.Add(characterData[currentCharacterID].darkElementDefence);
        }
    }
    public float GetElementDefence(ElementType elementType)
    {
        return ElementDefences[(int)elementType];
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

/// <summary>
/// ����ƭȭp��
/// </summary>
public class CharacterStats
{
    /// <summary>
    /// ��¦��
    /// </summary>
    public float BaseValue;

    /// <summary>
    /// �̲׭p���
    /// </summary>
    public float Value { 
        get 
        {
            if (isDirty || BaseValue != lastBaseValue)
            {
                lastBaseValue = BaseValue;
                _value = CalculateFinalValue();
                isDirty = false;
            }
            return _value;
        } 
    }

    /// <summary>
    /// ��e�p���
    /// </summary>
    private float _value;
    /// <summary>
    /// �̤p��¦��
    /// </summary>
    private float lastBaseValue = float.MinValue;

    /// <summary>
    /// �O�_�ק�L
    /// </summary>
    private bool isDirty = true;

    /// <summary>
    /// �ƭȭקﾹ��
    /// </summary>
    private readonly List<StatModifier> statModifiers;
    /// <summary>
    /// ���ѵ��~�����ƭȭקﾹ��(�uŪ)
    /// </summary>
    public readonly ReadOnlyCollection<StatModifier> StatModifiers;

    /// <summary>
    /// ��l��
    /// </summary>
    public CharacterStats(float baseValue)
    {
        BaseValue = baseValue;
        statModifiers = new List<StatModifier>();
        StatModifiers = statModifiers.AsReadOnly();
    }
    /// <summary>
    /// �K�[�ƭȭקﾹ
    /// </summary>
    public void AddModifier(StatModifier mod)
    {
        isDirty = true;
        statModifiers.Add(mod);
        statModifiers.Sort(CompareModifierOrder); //���u���űƦC
    }
    /// <summary>
    /// ����ƭȭקﾹ�u����
    /// </summary>
    private int CompareModifierOrder(StatModifier a, StatModifier b)
    {
        if (a.Order < b.Order)
            return -1;
        else if (a.Order > b.Order)
            return 1;
        return 0;
    }
    /// <summary>
    /// �����ƭȭקﾹ
    /// </summary>
    public bool RemoveModifier(StatModifier mod)
    {     
        if (statModifiers.Remove(mod))
        {
            isDirty = true;
            return true;
        }
        return false;
    }
    /// <summary>
    /// �R���ӷ����Ҧ��קﾹ
    /// </summary>
    public bool RemoveAllModifierFromSource(object source)
    {
        bool didRemove = false;
        for (int i = statModifiers.Count - 1; i <= 0 ; i--) //�q�̫�@�Ӷ}�l�R��
        {
            if (statModifiers[i].Source == source)
            {
                isDirty = true;
                didRemove = true;
                statModifiers.RemoveAt(i);
            }
        }
        return didRemove;
    }
    /// <summary>
    /// �p��̲׭Ȩê�^
    /// </summary>
    private float CalculateFinalValue()
    {
        float finalValue = BaseValue;
        float sumPercentAdd = 0;

        for (int i = 0; i < statModifiers.Count; i++)
        {
            StatModifier mod = statModifiers[i];

            if (mod.Type == StatModType.Flat) 
            {
                finalValue += mod.Value;
            }
            else if (mod.Type == StatModType.PercentAdd) //�ʤ����|�[
            {
                sumPercentAdd += mod.Value;
                //�O�̫�@�Ӽƭȭקﾹ �Ϊ̤��O�ʤ����|�[����
                if (i +1 >= statModifiers.Count || statModifiers[i+1].Type != StatModType.PercentAdd) 
                {
                    finalValue *= 1 + sumPercentAdd;
                    sumPercentAdd = 0;
                }
            }
            else if (mod.Type == StatModType.PercentMult) //�ʤ��񭿲v�W�[
            {
                finalValue *= 1 + mod.Value;
            }
        }

        return (float)Math.Round(finalValue, 4);//�|�ˤ��J�p���I
    }
}

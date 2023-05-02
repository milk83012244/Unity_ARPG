using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

/// <summary>
/// 角色數值計算
/// </summary>
public class CharacterStats
{
    /// <summary>
    /// 基礎值
    /// </summary>
    public float BaseValue;

    /// <summary>
    /// 最終計算值
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
    /// 當前計算值
    /// </summary>
    private float _value;
    /// <summary>
    /// 最小基礎值
    /// </summary>
    private float lastBaseValue = float.MinValue;

    /// <summary>
    /// 是否修改過
    /// </summary>
    private bool isDirty = true;

    /// <summary>
    /// 數值修改器組
    /// </summary>
    private readonly List<StatModifier> statModifiers;
    /// <summary>
    /// 提供給外部的數值修改器組(只讀)
    /// </summary>
    public readonly ReadOnlyCollection<StatModifier> StatModifiers;

    /// <summary>
    /// 初始化
    /// </summary>
    public CharacterStats(float baseValue)
    {
        BaseValue = baseValue;
        statModifiers = new List<StatModifier>();
        StatModifiers = statModifiers.AsReadOnly();
    }
    /// <summary>
    /// 添加數值修改器
    /// </summary>
    public void AddModifier(StatModifier mod)
    {
        isDirty = true;
        statModifiers.Add(mod);
        statModifiers.Sort(CompareModifierOrder); //照優先級排列
    }
    /// <summary>
    /// 比較數值修改器優先級
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
    /// 移除數值修改器
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
    /// 刪除來源的所有修改器
    /// </summary>
    public bool RemoveAllModifierFromSource(object source)
    {
        bool didRemove = false;
        for (int i = statModifiers.Count - 1; i <= 0 ; i--) //從最後一個開始刪除
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
    /// 計算最終值並返回
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
            else if (mod.Type == StatModType.PercentAdd) //百分比疊加
            {
                sumPercentAdd += mod.Value;
                //是最後一個數值修改器 或者不是百分比疊加類型
                if (i +1 >= statModifiers.Count || statModifiers[i+1].Type != StatModType.PercentAdd) 
                {
                    finalValue *= 1 + sumPercentAdd;
                    sumPercentAdd = 0;
                }
            }
            else if (mod.Type == StatModType.PercentMult) //百分比倍率增加
            {
                finalValue *= 1 + mod.Value;
            }
        }

        return (float)Math.Round(finalValue, 4);//四捨五入小數點
    }
}

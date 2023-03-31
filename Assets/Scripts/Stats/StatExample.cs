using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    public CharacterStats Strength;
}
/// <summary>
/// 範例
/// </summary>
public class StatExample 
{
    StatModifier mod1, mod2;
    /// <summary>
    /// 裝備
    /// </summary>
    public void Equip(Character c)
    {
        mod1 = new StatModifier(10, StatModType.Flat,this); //創建一個新的數值修改器
        mod2 = new StatModifier(0.1f, StatModType.PercentMult, this);
        c.Strength.AddModifier(mod1);
        c.Strength.AddModifier(mod2);
    }
    /// <summary>
    /// 卸下裝備
    /// </summary>
    public void Unequip(Character c)
    {
        c.Strength.RemoveAllModifierFromSource(this);
    }
}

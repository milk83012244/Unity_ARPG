using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    public CharacterStats Strength;
}
/// <summary>
/// �d��
/// </summary>
public class StatExample 
{
    StatModifier mod1, mod2;
    /// <summary>
    /// �˳�
    /// </summary>
    public void Equip(Character c)
    {
        mod1 = new StatModifier(10, StatModType.Flat,this); //�Ыؤ@�ӷs���ƭȭקﾹ
        mod2 = new StatModifier(0.1f, StatModType.PercentMult, this);
        c.Strength.AddModifier(mod1);
        c.Strength.AddModifier(mod2);
    }
    /// <summary>
    /// ���U�˳�
    /// </summary>
    public void Unequip(Character c)
    {
        c.Strength.RemoveAllModifierFromSource(this);
    }
}

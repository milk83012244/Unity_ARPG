using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 掛在技能1的生成物件上 造成傷害
/// </summary>
public class Mo_Skill1 : MonoBehaviour
{
    public CharacterAttackDataSO attackData;

    [HideInInspector] public int currentDamage;
    [HideInInspector] public bool isCritical;

}

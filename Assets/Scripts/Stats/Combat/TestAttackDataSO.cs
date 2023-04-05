using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "TestAttackData")]
public class TestAttackDataSO : ScriptableObject
{
    public float attackRange;
    public float skillRange;
    public float coolDown;
    public int minDamage;
    public int maxDamage;

    public float criticalMultplier;//√z¿ª≠ø≤v
    public float criticalChance;//√z¿ª≤v
}

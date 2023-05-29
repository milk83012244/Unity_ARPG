using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����԰���O���
/// </summary>
[CreateAssetMenu(menuName = "Data/CharacterData/CharacterBattleData", fileName = "CharacterBattleData")]
public class CharacterBattleDataSO : ScriptableObject
{
    public ElementType elementType;
    public AttackType attackType;
    public int PlayerNormalAttackCount; //���ⴶ�𦸼�

    [Header("Stats Info")]
    public int mexHealth;
    public int currentHealth;
    public float baseDefence;
    public float currentDefence;

    //�ݩʴ�K(���m)
    public float fireElementDefence;
    public float iceElementDefence;
    public float windElementDefence;
    public float thunderElementDefence;
    public float darkElementDefence;
    public float lightElementDefence;

    [Header("Level")]
    public int currentLevel;
    public int maxLevel;
    public int baseExp;
    public int currentExp;
}

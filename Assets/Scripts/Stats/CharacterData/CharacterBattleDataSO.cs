using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// ����԰���O���
/// </summary>
[CreateAssetMenu(menuName = "Data/CharacterData/CharacterBattleData", fileName = "CharacterBattleData")]
public class CharacterBattleDataSO : SerializedScriptableObject
{
    [Header("�����T")]
    public string characterName = "";
    public int characterID;//����ߤ@ID
    public CharacterDamageType damageType; //��������
    public AttackType attackType;
    public int PlayerNormalAttackCount; //���ⴶ�𦸼�

    [Header("��¦��O��")]
    public int mexHealth;
    public int currentHealth;
    public float baseDefence;
    public float currentDefence;
    public float currentUSkillValue;

    [Header("���h �w���ƭ�")]
    public float knockbackResistance; //���h��ܭ�
    public float maxStunValue;
    public float currentStunValue; //�w���� ���F�|Ĳ�o�w��
    public float stunResistance;//�w����ܭ�
    public float stunRecovorTime;//�w�����A�^�_�ɶ�
    public float stunCooldownTime;//�w�����A�N�o�ɶ�
    public float stunValueTime;//�w���ȧ��������`�ɪ�

    [Header("�ݩʴ�K")]
    public Dictionary<ElementType, float> elementDefense = new Dictionary<ElementType, float>();

    [Header("���żƭ�")]
    public readonly List<int> experiencePerLevel = new List<int>();
    public int currentLevel;
    public int maxLevel;
    public int baseExp;
    public int currentExp;
}

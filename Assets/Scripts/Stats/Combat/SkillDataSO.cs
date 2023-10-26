using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/CharacterData/SkillData", fileName = "SkillData")]
public class SkillDataSO : ScriptableObject
{
    public bool hasFiring;//�O�_���˷ǥ\��
    public float skillFiringRange;//�Ǥߪ����ʽd��

    public int skillLevel;
    public int skillUseCount;
    public float skillDuration; //���ī��ޯ����ɶ�
    public float skillCoolDown;

}

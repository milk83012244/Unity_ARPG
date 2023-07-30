using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Data/CharacterData/CharacterRewardData", fileName = "CharacterRewardData")]
public class CharacterRewardDataSO : SerializedScriptableObject
{
    public int experiencePoints;
    public int moneyReward;
    public List<GameObject> itemRewards = new List<GameObject>();
}

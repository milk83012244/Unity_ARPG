using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Lia�W�ۦ������
/// </summary>
[CreateAssetMenu(menuName = "Data/UnlockData/Lia", fileName = "LiaUnlockData")]
public class LiaUnlockDataSO : SerializedScriptableObject
{
    //���ꪺ�ݩ�
    public Dictionary<ElementType, bool> elementUnlockDic = new Dictionary<ElementType, bool>();
}

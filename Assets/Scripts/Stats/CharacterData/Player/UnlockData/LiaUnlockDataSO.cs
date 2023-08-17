using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Lia獨自有的資料
/// </summary>
[CreateAssetMenu(menuName = "Data/UnlockData/Lia", fileName = "LiaUnlockData")]
public class LiaUnlockDataSO : SerializedScriptableObject
{
    //解鎖的屬性
    public Dictionary<ElementType, bool> elementUnlockDic = new Dictionary<ElementType, bool>();
}

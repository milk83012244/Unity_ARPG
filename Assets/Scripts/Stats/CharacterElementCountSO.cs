using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using UnityEngine.Events;

/// <summary>
/// 賦予屬性疊加資料
/// </summary>
[CreateAssetMenu(menuName = "Data/CharacterData/CharacterElementCount", fileName = "CharacterElementCount")]
public class CharacterElementCountSO : SerializedScriptableObject
{
    public Dictionary<ElementType, int> elementCountDic = new Dictionary<ElementType, int>();

    public Dictionary<ElementType, Coroutine> elementCountDownDic = new Dictionary<ElementType, Coroutine>();//1階持續協程
    public Dictionary<ElementType, Coroutine> elementCoolDownDic = new Dictionary<ElementType, Coroutine>();//觸發2階後的冷卻時間
    public Dictionary<ElementType, bool> canUseElement = new Dictionary<ElementType, bool>(); //是否可觸發
    public float elementCountDownCoolDown;//1階持續時間
    public Dictionary<ElementType, float> elementCoolDown = new Dictionary<ElementType, float>();

    public List<ElementType> currentState2 = new List<ElementType>();

    [HideInInspector] public UnityEvent addElementCountEvent = new UnityEvent();//添加屬性階層事件
    [HideInInspector] public UnityEvent removeElementCountEvent = new UnityEvent(); //清除屬性階層事件

    [HideInInspector] public UnityEvent<ElementType> State2EffectTriggerEvent = new UnityEvent<ElementType>();//觸發2階事件

    [HideInInspector] public UnityEvent<ElementType> State2MixEffectTriggerEvent = new UnityEvent<ElementType>();//觸發2階組合事件

    /// <summary>
    /// 重置資料
    /// </summary>
    public void ResetAllData()
    {
        for (int i = 0; i < elementCountDic.Keys.Count; i++)
        {
            elementCountDic[(ElementType)i] = 0;
        }
        for (int i = 0; i < canUseElement.Keys.Count; i++)
        {
            canUseElement[(ElementType)i] = true;
        }
    }

}

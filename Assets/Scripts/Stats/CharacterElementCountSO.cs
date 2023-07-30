using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using UnityEngine.Events;

/// <summary>
/// �ᤩ�ݩ��|�[���
/// </summary>
[CreateAssetMenu(menuName = "Data/CharacterData/CharacterElementCount", fileName = "CharacterElementCount")]
public class CharacterElementCountSO : SerializedScriptableObject
{
    public Dictionary<ElementType, int> elementCountDic = new Dictionary<ElementType, int>();

    public Dictionary<ElementType, Coroutine> elementCountDownDic = new Dictionary<ElementType, Coroutine>();
    public Dictionary<ElementType, Coroutine> elementCoolDownDic = new Dictionary<ElementType, Coroutine>();//Ĳ�o2���᪺�N�o�ɶ�
    public Dictionary<ElementType, bool> canUseElement = new Dictionary<ElementType, bool>();
    public float elementCountDownCoolDown;
    public Dictionary<ElementType, float> elementCoolDown = new Dictionary<ElementType, float>();

    [HideInInspector] public UnityEvent addElementCountEvent = new UnityEvent();
    [HideInInspector] public UnityEvent removeElementCountEvent = new UnityEvent();

    /// <summary>
    /// ���m���
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

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

    public Dictionary<ElementType, Coroutine> elementCountDownDic = new Dictionary<ElementType, Coroutine>();//1�������{
    public Dictionary<ElementType, Coroutine> elementCoolDownDic = new Dictionary<ElementType, Coroutine>();//Ĳ�o2���᪺�N�o�ɶ�
    public Dictionary<ElementType, bool> canUseElement = new Dictionary<ElementType, bool>(); //�O�_�iĲ�o
    public float elementCountDownCoolDown;//1������ɶ�
    public Dictionary<ElementType, float> elementCoolDown = new Dictionary<ElementType, float>();

    public List<ElementType> currentState2 = new List<ElementType>();

    [HideInInspector] public UnityEvent addElementCountEvent = new UnityEvent();//�K�[�ݩʶ��h�ƥ�
    [HideInInspector] public UnityEvent removeElementCountEvent = new UnityEvent(); //�M���ݩʶ��h�ƥ�

    [HideInInspector] public UnityEvent<ElementType> State2EffectTriggerEvent = new UnityEvent<ElementType>();//Ĳ�o2���ƥ�

    [HideInInspector] public UnityEvent<ElementType> State2MixEffectTriggerEvent = new UnityEvent<ElementType>();//Ĳ�o2���զX�ƥ�

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

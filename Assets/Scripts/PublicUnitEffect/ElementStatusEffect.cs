using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

/// <summary>
/// 接收屬性階層資料 管理2階物件的顯示隱藏
/// </summary>
public class ElementStatusEffect : SerializedMonoBehaviour
{
    public CharacterElementCountSO currentElementCountSO;

    public Dictionary<ElementType,GameObject> ElementEffectObject = new Dictionary<ElementType, GameObject>();
    public Dictionary<ElementType, GameObject> ElementEffectMixObject = new Dictionary<ElementType, GameObject>();

    private void OnDestroy()
    {
        currentElementCountSO.State2MixEffectTriggerEvent.RemoveListener(MixElementStatusEffect);
    }
    private void Start()
    {
        SetAllElementDeactivate();

        currentElementCountSO.State2MixEffectTriggerEvent.AddListener(MixElementStatusEffect);
    }
    public void SetCharacterElementCountSO(CharacterElementCountSO characterElementCountSO)
    {
        currentElementCountSO = characterElementCountSO;
    }
    /// <summary>
    /// 關閉除了光暗所有2階物件
    /// </summary>
    public void ElementEffectObjectDeactivate()
    {
        for (int i = 0; i < ElementEffectObject.Keys.Count; i++)
        {
            if ((ElementType)i != ElementType.Light || (ElementType)i != ElementType.Dark)
            {
                if (ElementEffectObject[(ElementType)i] != null && ElementEffectObject[(ElementType)i].activeSelf)
                {
                    ElementEffectObject[(ElementType)i].SetActive(false);
                }
            }
        }
    }
    /// <summary>
    /// 2階演出物件開關
    /// </summary>
    public void SetElementActive(ElementType elementType,bool isActive)
    {
        switch (elementType)
        {
            case ElementType.Fire:
                ElementEffectObject[ElementType.Fire].SetActive(isActive);
                break;
            case ElementType.Ice:
                ElementEffectObject[ElementType.Ice].SetActive(isActive);
                break;
            case ElementType.Wind:
                ElementEffectObject[ElementType.Wind].SetActive(isActive);
                break;
            case ElementType.Thunder:
                ElementEffectObject[ElementType.Thunder].SetActive(isActive);
                break;
            case ElementType.Light:
                break;
            case ElementType.Dark:
                break;
        }
    }
    public void SetAllElementDeactivate()
    {
        for (int i = 0; i < ElementEffectObject.Keys.Count; i++)
        {
            if (ElementEffectObject[(ElementType)i]!= null)
            {
                ElementEffectObject[ElementType.Wind].SetActive(false);
            }
        }
    }

    /// <summary>
    /// 2階屬性組合觸發效果
    /// </summary>
    public void MixElementStatusEffect(ElementType elementType)
    {
        switch (elementType)
        {
            case ElementType.Fire:
                Debug.Log("觸發" + elementType + "2階組合的爆炸效果");
                break;
            case ElementType.Ice:
                Debug.Log("觸發" + elementType + "2階組合的碎冰效果");
                break;
            case ElementType.Wind:
                Debug.Log("觸發" + elementType + "2階組合的風爆效果");
                break;
            case ElementType.Thunder:
                Debug.Log("觸發" + elementType + "2階組合的落雷效果");
                break;
        }
        ElementEffectMixObject[elementType].SetActive(true);
    }
}

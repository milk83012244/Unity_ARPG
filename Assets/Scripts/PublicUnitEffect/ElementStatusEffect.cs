using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

/// <summary>
/// �����ݩʶ��h��� �޲z2�������������
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
    /// �������F���t�Ҧ�2������
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
    /// 2���t�X����}��
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
    /// 2���ݩʲզXĲ�o�ĪG
    /// </summary>
    public void MixElementStatusEffect(ElementType elementType)
    {
        switch (elementType)
        {
            case ElementType.Fire:
                Debug.Log("Ĳ�o" + elementType + "2���զX���z���ĪG");
                break;
            case ElementType.Ice:
                Debug.Log("Ĳ�o" + elementType + "2���զX���H�B�ĪG");
                break;
            case ElementType.Wind:
                Debug.Log("Ĳ�o" + elementType + "2���զX�����z�ĪG");
                break;
            case ElementType.Thunder:
                Debug.Log("Ĳ�o" + elementType + "2���զX�����p�ĪG");
                break;
        }
        ElementEffectMixObject[elementType].SetActive(true);
    }
}

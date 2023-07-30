using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ElementStatusEffect : SerializedMonoBehaviour
{
    public Dictionary<ElementType,GameObject> ElementEffectObject = new Dictionary<ElementType, GameObject>();

    private void Start()
    {
        foreach (KeyValuePair<ElementType, GameObject> item in ElementEffectObject)
        {
            item.Value.SetActive(false);
        }
    }

    public void SetElementActive(ElementType elementType,bool isActive)
    {
        switch (elementType)
        {
            case ElementType.Fire:
                break;
            case ElementType.Ice:
                ElementEffectObject[ElementType.Ice].SetActive(isActive);
                break;
            case ElementType.Wind:
                break;
            case ElementType.Thunder:
                break;
            case ElementType.Light:
                break;
            case ElementType.Dark:
                break;
        }
    }
}

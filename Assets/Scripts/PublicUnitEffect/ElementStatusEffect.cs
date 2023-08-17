using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ElementStatusEffect : SerializedMonoBehaviour
{
    public CharacterElementCountSO currentElementCount;

    public Dictionary<ElementType,GameObject> ElementEffectObject = new Dictionary<ElementType, GameObject>();
    public Dictionary<ElementType, GameObject> ElementEffectMixObject = new Dictionary<ElementType, GameObject>();

    public float searchRadius;
    public LayerMask enemiesLayer;

    private void Start()
    {
        foreach (KeyValuePair<ElementType, GameObject> item in ElementEffectObject)
        {
            item.Value.SetActive(false);
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
    /// <summary>
    /// 混合屬性觸發
    /// </summary>
    public void ElementStatus2MixTrigger(ElementType elementType)
    {
        switch (elementType)
        {
            case ElementType.Fire:
                break;
            case ElementType.Ice:
                break;
            case ElementType.Wind:
                break;
            case ElementType.Thunder:
                break;
        }
    }
    private IEnumerator ElementStatus2MixIce()
    {
        ElementEffectObject[ElementType.Ice].SetActive(false);
        ElementEffectMixObject[ElementType.Ice].SetActive(true);

        yield return Yielders.GetWaitForSeconds(0.1f);
        while (true)
        {
            Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, searchRadius, enemiesLayer);
            foreach (Collider2D target in targets)
            {
                OtherCharacterStats otherCharacter = target.GetComponent<OtherCharacterStats>();
                //篩選賦予一階冰的單位
                if (otherCharacter.enemyElementCountData.elementCountDic[ElementType.Ice] == 1)
                {

                }
            }
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = UnityEngine.Color.green;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }
}

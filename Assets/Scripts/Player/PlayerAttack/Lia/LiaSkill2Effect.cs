using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiaSkill2Effect : MonoBehaviour
{
    [HideInInspector] public PlayerCharacterStats characterStats;

    private Animator currentAnimator;
    public ElementType element;

    public List<GameObject> elementTypes;

    //是否有賦予屬性
    [HideInInspector] public bool fireElement;
    [HideInInspector] public bool iceElement;
    [HideInInspector] public bool windElement;
    [HideInInspector] public bool thunderElement;
    [HideInInspector] public bool lightElement;
    [HideInInspector] public bool darkElement;

    public void SetTargetPosition(Vector3 target)
    {

    }

    /// <summary>
    /// 設定元素物件的組件
    /// </summary>
    private void SetElementComponent(ElementType element)
    {
        currentAnimator = elementTypes[(int)element].GetComponent<Animator>();
    }

    public void GetCharacterStats(PlayerCharacterStats characterStats)
    {
        this.characterStats = characterStats;
        SetElementType();
        StartCoroutine(SetAnimation());
    }
    private void SetElementType()
    {
        for (int i = 0; i < elementTypes.Count; i++)
        {
            if (elementTypes[i] != null && elementTypes[i].activeSelf)
            {
                elementTypes[i].SetActive(false);
            }
        }

        element = characterStats.attackData[characterStats.currentCharacterID].elementType;
        switch (element)
        {
            case ElementType.Fire:
                fireElement = true;
                iceElement = false;
                windElement = false;
                thunderElement = false;
                lightElement = false;
                darkElement = false;
                elementTypes[(int)element].SetActive(true);
                break;
            case ElementType.Ice:
                fireElement = false;
                iceElement = true;
                windElement = false;
                thunderElement = false;
                lightElement = false;
                darkElement = false;
                elementTypes[(int)element].SetActive(true);
                SetElementComponent(element);
                break;
            case ElementType.Wind:
                fireElement = false;
                iceElement = false;
                windElement = true;
                thunderElement = false;
                lightElement = false;
                darkElement = false;
                elementTypes[(int)element].SetActive(true);
                SetElementComponent(element);
                break;
            case ElementType.Thunder:
                fireElement = false;
                iceElement = false;
                windElement = false;
                thunderElement = true;
                lightElement = false;
                darkElement = false;
                elementTypes[(int)element].SetActive(true);
                break;
            case ElementType.Light:
                fireElement = false;
                iceElement = false;
                windElement = false;
                thunderElement = false;
                lightElement = true;
                darkElement = false;
                elementTypes[(int)element].SetActive(true);
                break;
            case ElementType.Dark:
                fireElement = false;
                iceElement = false;
                windElement = false;
                thunderElement = false;
                lightElement = false;
                darkElement = true;
                elementTypes[(int)element].SetActive(true);
                break;
            case ElementType.None:
                fireElement = false;
                iceElement = false;
                windElement = false;
                thunderElement = false;
                lightElement = false;
                darkElement = false;
                elementTypes[(int)element].SetActive(true);
                break;
        }
    }
    private IEnumerator SetAnimation()
    {
        switch (element)
        {
            case ElementType.Fire:
                break;
            case ElementType.Ice:
                elementTypes[(int)element].SetActive(true);
                yield return Yielders.GetWaitForSeconds(0.7f);
                Recycle();
                break;
            case ElementType.Wind:
                elementTypes[(int)element].SetActive(true);
                yield return Yielders.GetWaitForSeconds(0.7f);
                Recycle();
                break;
            case ElementType.Thunder:
                break;
        }
    }
    public void Recycle() //自己回收
    {
        switch (element)
        {
            case ElementType.Fire:
                break;
            case ElementType.Ice:
                elementTypes[(int)element].SetActive(false);
                ObjectPool<LiaSkill2Effect>.Instance.Recycle(this);
                break;
            case ElementType.Wind:
                elementTypes[(int)element].SetActive(false);
                ObjectPool<LiaSkill2Effect>.Instance.Recycle(this);
                break;
            case ElementType.Thunder:
                break;
        }
        //ObjectPool<LiaSkill1Effect>.GetInstance().Recycle(this);
    }
}

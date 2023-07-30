using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 賦予屬性疊加計算處理
/// </summary>
public class CharacterElementCounter : MonoBehaviour
{
    public OtherCharacterStats otherCharacterStats;
    public PlayerCharacterStats playerCharacterStats;
    [HideInInspector] public CharacterElementCountSO characterElementCountSO;

    private void Awake()
    {
        if (otherCharacterStats !=null)
        {
            characterElementCountSO = otherCharacterStats.enemyElementCountData;
        }
        else if (playerCharacterStats != null)
        {
            characterElementCountSO = playerCharacterStats.elementCountData[playerCharacterStats.currentCharacterID];
        }
    }
    public void TriggerAddEvent()
    {
        characterElementCountSO.addElementCountEvent.Invoke();
    }
    public void TriggerRemoveEvent()
    {
        characterElementCountSO.removeElementCountEvent.Invoke();
    }

    /// <summary>
    /// 添加賦予屬性數
    /// </summary>
    public void AddElementCount(ElementType elementType, int type)
    {
        if (characterElementCountSO.canUseElement[elementType] == false)
        {
            Debug.Log("未賦予屬性 還在冷卻中");
            return;
        }

        //if (elementCoolDownDic[elementType] != null)//當前屬性已觸發過在冷卻
        //{
        //    return;
        //}

        if (type == 1)
        {
            if (characterElementCountSO.elementCountDic[elementType] < 1)
            {
                characterElementCountSO.elementCountDic[elementType] = type;

                //開始計時
                Coroutine coroutine = StartCoroutine(ElementRemoveCountDown(elementType));
                if (characterElementCountSO.elementCountDownDic[elementType] != null)
                    StopCoroutine(characterElementCountSO.elementCountDownDic[elementType]);

                characterElementCountSO.elementCountDownDic[elementType] = coroutine;

                TriggerAddEvent();
            }
            else
            {
                Debug.Log("已有相同" + elementType + "屬性1層");
            }
        }
        else if (type == 2)
        {
            if (characterElementCountSO.elementCountDic[elementType] == 1)
            {
                characterElementCountSO.elementCountDic[elementType] = type;
                StopCoroutine(characterElementCountSO.elementCountDownDic[elementType]);
                Coroutine coolDownCoroutine = StartCoroutine(Element2TriggerCoolDown(elementType));
                characterElementCountSO.elementCoolDownDic[elementType] = coolDownCoroutine;

                TriggerAddEvent();
            }
            else
                Debug.Log(elementType + " 屬性未達1層");
        }
    }
    /// <summary>
    /// 移除賦予屬性數
    /// </summary>
    public void RemoveElementCount(ElementType elementType)
    {
        if (characterElementCountSO.elementCountDic[elementType] > 0)
        {
            characterElementCountSO.elementCountDic[elementType] = 0;
            TriggerRemoveEvent();
        }
        else
            Debug.Log(elementType + " 屬性已到最小層數");
    }
    /// <summary>
    /// 沒有觸發時計時屬性附加狀態消失時間
    /// </summary>
    public IEnumerator ElementRemoveCountDown(ElementType elementType)
    {
        yield return Yielders.GetWaitForSeconds(characterElementCountSO.elementCountDownCoolDown);
        RemoveElementCount(elementType);
        RemoveElementTriggerCoolDownCor(elementType);
    }
    /// <summary>
    /// 觸發2階後冷卻(持續)時間(如果被提早觸發組合效果 總冷卻時間不變)
    /// </summary>
    public IEnumerator Element2TriggerCoolDown(ElementType elementType)
    {
        characterElementCountSO.canUseElement[elementType] = false;
        yield return Yielders.GetWaitForSeconds(characterElementCountSO.elementCoolDown[elementType]);
        characterElementCountSO.elementCountDic[elementType] = 0;
        characterElementCountSO.canUseElement[elementType] = true;
    }
    private void RemoveElementTriggerCoolDownCor(ElementType elementType)
    {
        if (characterElementCountSO.elementCoolDownDic[elementType] != null)
        {
            StopCoroutine(characterElementCountSO.elementCoolDownDic[elementType]);
            characterElementCountSO.elementCoolDownDic[elementType] = null;
        }
    }
}

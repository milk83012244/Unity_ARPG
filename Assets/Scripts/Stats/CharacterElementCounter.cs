using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 賦予屬性疊加計算處理
/// 負責:賦予屬性層數的計算與計時
/// </summary>
public class CharacterElementCounter : MonoBehaviour
{
    //使用者
    public OtherCharacterStats otherCharacterStats; 
    public PlayerCharacterStats playerCharacterStats;
    //賦予屬性狀態者
    [HideInInspector] public OtherCharacterStats giverOtherCharacterStats;
    [HideInInspector] public PlayerCharacterStats giverPlayerCharacterStats;

    [HideInInspector] public CharacterElementCountSO characterElementCountSO;

    float elementDefence;
    private bool isOtherCharacterStats;
    private bool isMixElementActive;

    private void Awake()
    {
        if (otherCharacterStats !=null)
        {
            characterElementCountSO = otherCharacterStats.enemyElementCountData;

            isOtherCharacterStats = true;
        }
        else if (playerCharacterStats != null)
        {
            characterElementCountSO = playerCharacterStats.elementCountData[playerCharacterStats.currentCharacterID];

            isOtherCharacterStats = false;
        }
    }
    /// <summary>
    /// 觸發屬性2階事件
    /// </summary>
    public void Element2EventTrigger(ElementType elementType)
    {
        characterElementCountSO.State2EffectTriggerEvent?.Invoke(elementType);
    }
    public void TriggerAddUIEvent()
    {
        characterElementCountSO.addElementCountEvent.Invoke();
    }
    public void TriggerRemoveUIEvent()
    {
        characterElementCountSO.removeElementCountEvent.Invoke();
    }

    /// <summary>
    /// 添加賦予屬性數
    /// </summary>
    public void AddElementCount(ElementType elementType, int type ,PlayerCharacterStats giverPlayer = null,OtherCharacterStats giverOther =null)
    {
        if (characterElementCountSO.canUseElement[elementType] == false)
        {
            Debug.Log("未賦予" + elementType + "屬性 還在冷卻中");
            return;
        }

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

                TriggerAddUIEvent();
            }
            else
            {
                Debug.Log("已有相同" + elementType + "屬性1層");
            }
        }
        else if (type == 2)
        {
            //接收賦予狀態者的能力值 只有2階會觸發
            if (giverPlayer != null)
            {
                giverPlayerCharacterStats = giverPlayer;
            }
            else if (giverOther != null)
            {
                giverOtherCharacterStats = giverOther;
            }

            if (characterElementCountSO.elementCountDic[elementType] == 1)
            {
                characterElementCountSO.elementCountDic[elementType] = type;
                StopCoroutine(characterElementCountSO.elementCountDownDic[elementType]);
                Coroutine coolDownCoroutine = StartCoroutine(Element2TriggerCoolDown(elementType));
                characterElementCountSO.elementCoolDownDic[elementType] = coolDownCoroutine;

                //計算抗性決定要不要觸發
                if (isOtherCharacterStats)
                {
                    elementDefence = otherCharacterStats.GetElementDefence(elementType);
                }
                else if (!isOtherCharacterStats)
                {
                    elementDefence = playerCharacterStats.GetElementDefence(elementType);
                }
                int triggerRate = (int)(100 - elementDefence);

                int randomRate = Random.Range(0, 101);
                if (triggerRate >= randomRate)
                {
                    AddCurrentElement(elementType);

                    if (isMixElementActive ==false)
                    {
                        TriggerAddUIEvent();
                        Element2EventTrigger(elementType);
                    }
                }
                else
                {
                    Debug.Log(elementType + " 屬性抗性 不觸發");
                }
                //TriggerAddEvent();
                //AddCurrentElement(elementType);
            }
            else
                Debug.Log(elementType + " 屬性未達1層");
        }
    }
    /// <summary>
    /// 儲存組合用的層數
    /// </summary>
    /// <param name="elementType"></param>
    public void AddCurrentElement(ElementType elementType)
    {
        isMixElementActive = false;

        if (characterElementCountSO.currentState2.Count == 0)
        {
            characterElementCountSO.currentState2.Add(elementType);
        }
        else if (characterElementCountSO.currentState2.Count == 1)
        {
            if (characterElementCountSO.currentState2[0] != elementType)
            {
                characterElementCountSO.currentState2.Add(elementType);
                //4屬會觸發組合效果
                if ((characterElementCountSO.currentState2[0] == ElementType.Fire || characterElementCountSO.currentState2[0] == ElementType.Ice || characterElementCountSO.currentState2[0] == ElementType.Wind || characterElementCountSO.currentState2[0] == ElementType.Thunder))
                {
                    if ((characterElementCountSO.currentState2[1] == ElementType.Fire || characterElementCountSO.currentState2[1] == ElementType.Ice || characterElementCountSO.currentState2[1] == ElementType.Wind || characterElementCountSO.currentState2[1] == ElementType.Thunder))
                    {
                        Debug.Log("觸發2階" + characterElementCountSO.currentState2[0] + "組合效果");
                        isMixElementActive = true;
                        characterElementCountSO.State2MixEffectTriggerEvent?.Invoke(characterElementCountSO.currentState2[0]);
                    }
                }
                //光暗會互相覆蓋效果
                else if (characterElementCountSO.currentState2[0] == ElementType.Light || characterElementCountSO.currentState2[0] == ElementType.Dark)
                {
                    Debug.Log("覆蓋" + elementType + "效果");
                }
            }
        }
    }
    public void ClearCurrentElement()
    {
        characterElementCountSO.currentState2.Clear();
    }
    /// <summary>
    /// 移除賦予屬性數
    /// </summary>
    public void RemoveElementCount(ElementType elementType)
    {
        if (characterElementCountSO.elementCountDic[elementType] > 0)
        {
            characterElementCountSO.elementCountDic[elementType] = 0;
            ClearCurrentElement();
            TriggerRemoveUIEvent();
        }
        else
            Debug.Log(elementType + " 屬性已到最小層數");
    }
    //設定當前屬性

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

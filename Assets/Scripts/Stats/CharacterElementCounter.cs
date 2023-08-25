using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ᤩ�ݩ��|�[�p��B�z
/// �t�d:�ᤩ�ݩʼh�ƪ��p��P�p��
/// </summary>
public class CharacterElementCounter : MonoBehaviour
{
    //�ϥΪ�
    public OtherCharacterStats otherCharacterStats; 
    public PlayerCharacterStats playerCharacterStats;
    //�ᤩ�ݩʪ��A��
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
    /// Ĳ�o�ݩ�2���ƥ�
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
    /// �K�[�ᤩ�ݩʼ�
    /// </summary>
    public void AddElementCount(ElementType elementType, int type ,PlayerCharacterStats giverPlayer = null,OtherCharacterStats giverOther =null)
    {
        if (characterElementCountSO.canUseElement[elementType] == false)
        {
            Debug.Log("���ᤩ" + elementType + "�ݩ� �٦b�N�o��");
            return;
        }

        if (type == 1)
        {
            if (characterElementCountSO.elementCountDic[elementType] < 1)
            {
                characterElementCountSO.elementCountDic[elementType] = type;

                //�}�l�p��
                Coroutine coroutine = StartCoroutine(ElementRemoveCountDown(elementType));
                if (characterElementCountSO.elementCountDownDic[elementType] != null)
                    StopCoroutine(characterElementCountSO.elementCountDownDic[elementType]);

                characterElementCountSO.elementCountDownDic[elementType] = coroutine;

                TriggerAddUIEvent();
            }
            else
            {
                Debug.Log("�w���ۦP" + elementType + "�ݩ�1�h");
            }
        }
        else if (type == 2)
        {
            //�����ᤩ���A�̪���O�� �u��2���|Ĳ�o
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

                //�p��ܩʨM�w�n���nĲ�o
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
                    Debug.Log(elementType + " �ݩʧܩ� ��Ĳ�o");
                }
                //TriggerAddEvent();
                //AddCurrentElement(elementType);
            }
            else
                Debug.Log(elementType + " �ݩʥ��F1�h");
        }
    }
    /// <summary>
    /// �x�s�զX�Ϊ��h��
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
                //4�ݷ|Ĳ�o�զX�ĪG
                if ((characterElementCountSO.currentState2[0] == ElementType.Fire || characterElementCountSO.currentState2[0] == ElementType.Ice || characterElementCountSO.currentState2[0] == ElementType.Wind || characterElementCountSO.currentState2[0] == ElementType.Thunder))
                {
                    if ((characterElementCountSO.currentState2[1] == ElementType.Fire || characterElementCountSO.currentState2[1] == ElementType.Ice || characterElementCountSO.currentState2[1] == ElementType.Wind || characterElementCountSO.currentState2[1] == ElementType.Thunder))
                    {
                        Debug.Log("Ĳ�o2��" + characterElementCountSO.currentState2[0] + "�զX�ĪG");
                        isMixElementActive = true;
                        characterElementCountSO.State2MixEffectTriggerEvent?.Invoke(characterElementCountSO.currentState2[0]);
                    }
                }
                //���t�|�����л\�ĪG
                else if (characterElementCountSO.currentState2[0] == ElementType.Light || characterElementCountSO.currentState2[0] == ElementType.Dark)
                {
                    Debug.Log("�л\" + elementType + "�ĪG");
                }
            }
        }
    }
    public void ClearCurrentElement()
    {
        characterElementCountSO.currentState2.Clear();
    }
    /// <summary>
    /// �����ᤩ�ݩʼ�
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
            Debug.Log(elementType + " �ݩʤw��̤p�h��");
    }
    //�]�w��e�ݩ�

    /// <summary>
    /// �S��Ĳ�o�ɭp���ݩʪ��[���A�����ɶ�
    /// </summary>
    public IEnumerator ElementRemoveCountDown(ElementType elementType)
    {
        yield return Yielders.GetWaitForSeconds(characterElementCountSO.elementCountDownCoolDown);
        RemoveElementCount(elementType);
        RemoveElementTriggerCoolDownCor(elementType);
    }
    /// <summary>
    /// Ĳ�o2����N�o(����)�ɶ�(�p�G�Q����Ĳ�o�զX�ĪG �`�N�o�ɶ�����)
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

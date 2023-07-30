using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ᤩ�ݩ��|�[�p��B�z
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
    /// �K�[�ᤩ�ݩʼ�
    /// </summary>
    public void AddElementCount(ElementType elementType, int type)
    {
        if (characterElementCountSO.canUseElement[elementType] == false)
        {
            Debug.Log("���ᤩ�ݩ� �٦b�N�o��");
            return;
        }

        //if (elementCoolDownDic[elementType] != null)//��e�ݩʤwĲ�o�L�b�N�o
        //{
        //    return;
        //}

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

                TriggerAddEvent();
            }
            else
            {
                Debug.Log("�w���ۦP" + elementType + "�ݩ�1�h");
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
                Debug.Log(elementType + " �ݩʥ��F1�h");
        }
    }
    /// <summary>
    /// �����ᤩ�ݩʼ�
    /// </summary>
    public void RemoveElementCount(ElementType elementType)
    {
        if (characterElementCountSO.elementCountDic[elementType] > 0)
        {
            characterElementCountSO.elementCountDic[elementType] = 0;
            TriggerRemoveEvent();
        }
        else
            Debug.Log(elementType + " �ݩʤw��̤p�h��");
    }
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

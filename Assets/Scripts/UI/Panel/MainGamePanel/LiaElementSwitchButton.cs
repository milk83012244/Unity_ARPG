using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Lia�������ݩʫ��s
/// �t�d:���������Ĳ�o,�\����s�Ϥ��ഫ,�ޯ�Ϥ��ഫ
/// </summary>
public class LiaElementSwitchButton : MonoBehaviour
{
    public Image switchButtonMask;

    public GameObject arrow;
    public Vector2[] arrowPos = new Vector2[4];
    public Quaternion[] arrowRotate = new Quaternion[4];
    public List<Sprite> elementMasks = new List<Sprite>();

    /// <summary>
    /// �]�w���s
    /// </summary>
    public void SetElementIcon(ElementType elementType)
    {
        //�����B�n�Ϥ�
        switchButtonMask.sprite = elementMasks[(int)elementType];
        //���ʫ��w��m
        arrow.transform.localPosition = arrowPos[(int)elementType];
        arrow.transform.localRotation = arrowRotate[(int)elementType];
        //�t�X�S�ĵ�
    }
}

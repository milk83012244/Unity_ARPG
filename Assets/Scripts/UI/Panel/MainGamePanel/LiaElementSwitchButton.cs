using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Lia的切換屬性按鈕
/// 負責:接收按鍵並觸發,功能按鈕圖片轉換,技能圖片轉換
/// </summary>
public class LiaElementSwitchButton : MonoBehaviour
{
    public Image switchButtonMask;

    public GameObject arrow;
    public Vector2[] arrowPos = new Vector2[4];
    public Quaternion[] arrowRotate = new Quaternion[4];
    public List<Sprite> elementMasks = new List<Sprite>();

    /// <summary>
    /// 設定按鈕
    /// </summary>
    public void SetElementIcon(ElementType elementType)
    {
        //切換遮罩圖片
        switchButtonMask.sprite = elementMasks[(int)elementType];
        //移動指針位置
        arrow.transform.localPosition = arrowPos[(int)elementType];
        arrow.transform.localRotation = arrowRotate[(int)elementType];
        //演出特效等
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 全局物件生成器 可以直接把要生成的物件寫在這裡在任何地方都可以輕易呼叫
/// </summary>
public class GameAssets : MonoBehaviour
{
    private static GameAssets _i;

    public static GameAssets i
    {
        get
        {
            if (_i ==null)
            {
                _i = (Instantiate(Resources.Load("GameAssets"))as GameObject).GetComponent<GameAssets>(); //在Resources資料夾中建一個GameAssets 把此腳本掛上去 
            }
            return _i;
        }
    }
    //就可以呼叫掛在這裡的物件
    [Header("UI相關")]
    public Transform dialogueBubbleCanvas; //氣泡對話框
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel
{
    public UIType uIType;
    /// <summary>
    /// 當前Panel在場景中對應的物件
    /// </summary>
    public GameObject activeObj;

    public BasePanel(UIType uIType)
    {
       this. uIType = uIType;
    }

    public virtual void OnStart()
    {
        Debug.Log($" {uIType.Name} 啟用 ");
        if (activeObj.GetComponent<CanvasGroup>() == null)
        {
            activeObj.AddComponent<CanvasGroup>();
        }
    }
    public virtual void OnEnable()
    {
        UIUtility.GetInstance().GetOrAddComponent<CanvasGroup>(activeObj).interactable = true; //開啟畫布的交互功能
    }
    public virtual void OnDisable()
    {
        UIUtility.GetInstance().GetOrAddComponent<CanvasGroup>(activeObj).interactable = false; //關閉畫布的交互功能
    }
    public virtual void OnDestory()
    {
        UIUtility.GetInstance().GetOrAddComponent<CanvasGroup>(activeObj).interactable = false; //關閉畫布的交互功能
    }
}

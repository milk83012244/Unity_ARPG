using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel
{
    public UIType uIType;
    /// <summary>
    /// ��ePanel�b����������������
    /// </summary>
    public GameObject activeObj;

    public BasePanel(UIType uIType)
    {
       this. uIType = uIType;
    }

    public virtual void OnStart()
    {
        Debug.Log($" {uIType.Name} �ҥ� ");
        if (activeObj.GetComponent<CanvasGroup>() == null)
        {
            activeObj.AddComponent<CanvasGroup>();
        }
    }
    public virtual void OnEnable()
    {
        UIUtility.GetInstance().GetOrAddComponent<CanvasGroup>(activeObj).interactable = true; //�}�ҵe�����椬�\��
    }
    public virtual void OnDisable()
    {
        UIUtility.GetInstance().GetOrAddComponent<CanvasGroup>(activeObj).interactable = false; //�����e�����椬�\��
    }
    public virtual void OnDestory()
    {
        UIUtility.GetInstance().GetOrAddComponent<CanvasGroup>(activeObj).interactable = false; //�����e�����椬�\��
    }
}

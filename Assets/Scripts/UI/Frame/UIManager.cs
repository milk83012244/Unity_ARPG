using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    /// <summary>
    /// 儲存UI的棧
    /// </summary>
    public Stack<BasePanel> uiStack;
    /// <summary>
    /// 儲存Panel的字典
    /// </summary>
    public Dictionary<string, GameObject> uiPanelDic;
    /// <summary>
    /// 當前場景UI畫布物件
    /// </summary>
    public GameObject canvsObj;

    private static UIManager instance;

    /// <summary>
    /// 實例化單例
    /// </summary>
    /// <returns></returns>
    public static UIManager GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("沒有UIManager實例");
            return instance;
        }
        else
        {
            return instance;
        }
    }

    public UIManager()
    {
        instance = this;
        uiStack = new Stack<BasePanel>();
        uiPanelDic = new Dictionary<string, GameObject>();
    }
    /// <summary>
    /// 獲得UI物件並返回
    /// </summary>
    /// <param name="uIType"></param>
    /// <returns></returns>
    public GameObject GetSingleObject(UIType uIType)
    {
        if (uiPanelDic.ContainsKey(uIType.Name))
        {
            return uiPanelDic[uIType.Name];
        }
        if (canvsObj == null)
        {
            canvsObj = UIUtility.GetInstance().FindCanvas();
        }
        //從本地路徑實例化物件
        GameObject gameObject = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(uIType.Path),canvsObj.transform);
        return gameObject;
    }

    /// <summary>
    /// 將Panel放進棧並開啟
    /// </summary>
    /// <param name="basePanel">繼承基類的Panel</param>
    public void Push(BasePanel basePanel)
    {
        Debug.Log($"{basePanel.uIType.Name} Push進Stack");

        if (uiStack.Count > 0)
        {
            //獲取元素並禁用
            uiStack.Peek().OnDisable();
        }

        GameObject uiObject = GetSingleObject(basePanel.uIType);
        uiPanelDic.Add(basePanel.uIType.Name, uiObject);
        basePanel.activeObj = uiObject;

        if (uiStack.Count == 0)
        {
            uiStack.Push(basePanel);//Stack的Push方法
        }
        else
        {
            if (uiStack.Peek().uIType.Name == basePanel.uIType.Name)
            {
                uiStack.Push(basePanel);
            }
        }

        basePanel.OnStart();
    }
    /// <summary>
    /// 將Panel彈出棧並關閉
    /// </summary>
    /// <param name="isLoad">是否關閉所有Panel</param>
    public void Pop(bool isLoad)
    {
        if (isLoad == true)
        {
            if (uiStack.Count > 0)
            {
                uiStack.Peek().OnDisable();
                uiStack.Peek().OnDestory();
                GameObject.Destroy(uiPanelDic[uiStack.Peek().uIType.Name]);
                uiPanelDic.Remove(uiStack.Peek().uIType.Name);
                //做完前置作業再彈出棧
                uiStack.Pop();
                //遞歸直到所有Panel都彈出
                Pop(true);
            }
        }
        else
        {
            if (uiStack.Count > 0)
            {
                uiStack.Peek().OnDisable();
                uiStack.Peek().OnDestory();
                GameObject.Destroy(uiPanelDic[uiStack.Peek().uIType.Name]);
                uiPanelDic.Remove(uiStack.Peek().uIType.Name);
                //做完前置作業再彈出棧
                uiStack.Pop();
                //如果前面還有Panel就顯示
                if (uiStack.Count > 0)
                {
                    uiStack.Peek().OnEnable();
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    /// <summary>
    /// �x�sUI����
    /// </summary>
    public Stack<BasePanel> uiStack;
    /// <summary>
    /// �x�sPanel���r��
    /// </summary>
    public Dictionary<string, GameObject> uiPanelDic;
    /// <summary>
    /// ��e����UI�e������
    /// </summary>
    public GameObject canvsObj;

    private static UIManager instance;

    /// <summary>
    /// ��ҤƳ��
    /// </summary>
    /// <returns></returns>
    public static UIManager GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("�S��UIManager���");
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
    /// ��oUI����ê�^
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
        //�q���a���|��Ҥƪ���
        GameObject gameObject = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(uIType.Path),canvsObj.transform);
        return gameObject;
    }

    /// <summary>
    /// �NPanel��i�̨ö}��
    /// </summary>
    /// <param name="basePanel">�~�Ӱ�����Panel</param>
    public void Push(BasePanel basePanel)
    {
        Debug.Log($"{basePanel.uIType.Name} Push�iStack");

        if (uiStack.Count > 0)
        {
            //��������øT��
            uiStack.Peek().OnDisable();
        }

        GameObject uiObject = GetSingleObject(basePanel.uIType);
        uiPanelDic.Add(basePanel.uIType.Name, uiObject);
        basePanel.activeObj = uiObject;

        if (uiStack.Count == 0)
        {
            uiStack.Push(basePanel);//Stack��Push��k
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
    /// �NPanel�u�X�̨�����
    /// </summary>
    /// <param name="isLoad">�O�_�����Ҧ�Panel</param>
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
                //�����e�m�@�~�A�u�X��
                uiStack.Pop();
                //���k����Ҧ�Panel���u�X
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
                //�����e�m�@�~�A�u�X��
                uiStack.Pop();
                //�p�G�e���٦�Panel�N���
                if (uiStack.Count > 0)
                {
                    uiStack.Peek().OnEnable();
                }
            }
        }
    }
}

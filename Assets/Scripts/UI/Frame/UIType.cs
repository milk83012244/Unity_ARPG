using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIType
{
    private string path; //UI路徑
    public string Path { get => path; }
    private string name; //UI名稱
    public string Name { get => name; }
    /// <summary>
    /// //獲得UI路徑和名稱(初始化)
    /// </summary>
    /// <param name="uiPath">Panel路徑</param>
    /// <param name="uiName">Panel名稱</param>
    public UIType(string uiPath,string uiName) 
    {
        path = uiPath;
        name = uiName;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIType
{
    private string path; //UI���|
    public string Path { get => path; }
    private string name; //UI�W��
    public string Name { get => name; }
    /// <summary>
    /// //��oUI���|�M�W��(��l��)
    /// </summary>
    /// <param name="uiPath">Panel���|</param>
    /// <param name="uiName">Panel�W��</param>
    public UIType(string uiPath,string uiName) 
    {
        path = uiPath;
        name = uiName;
    }
}

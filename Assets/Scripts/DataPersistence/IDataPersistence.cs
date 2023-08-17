using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 數據持久化介面 需要使用數據持久化的資料繼承這個介面
/// </summary>
public interface IDataPersistence 
{
    /// <summary>
    /// 從GameData讀取資料
    /// </summary>
    void LoadData(GameData gameData);
    /// <summary>
    /// 保存資料 複寫GameData
    /// </summary>
    void SaveData(GameData gameData);
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 保存與讀取用的資料
/// </summary>
[System.Serializable] //可序列化的標籤
public class GameData
{
    [Header("NormalData")] //要儲存的資料
    public long lastUpdated; //最後遊玩的
    public string saveTime;
    public float playTime;

    [Header("ScriptableObjectData")]
    public SerializableDictionary<int, string> partyData; //當前隊伍資料
    public Vector3 playerPosition; //玩家當前位置

    public GameData() //新遊戲資料初始化
    {
        saveTime = "";
        playTime = 0;
        partyData = new SerializableDictionary<int, string>();
        partyData.Add(1, "Mo");
        partyData.Add(2, "Lia");
        partyData.Add(3, "");
        playerPosition = new Vector2(0, 0);
    }

    public string GetSaveTime()
    {
        return saveTime;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �O�s�PŪ���Ϊ����
/// </summary>
[System.Serializable] //�i�ǦC�ƪ�����
public class GameData
{
    [Header("NormalData")] //�n�x�s�����
    public long lastUpdated; //�̫�C����
    public string saveTime;
    public float playTime;

    [Header("ScriptableObjectData")]
    public SerializableDictionary<int, string> partyData; //��e������
    public Vector3 playerPosition; //���a��e��m

    public GameData() //�s�C����ƪ�l��
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �}�s�C�����w�]�s�ɤ��e
/// </summary>
public class StartNewGameSave : MonoBehaviour,IDataPersistence
{
    public DateTime saveTime;

    public void LoadData(GameData gameData)
    {
        
    }

    public void SaveData(GameData gameData)
    {
        this.saveTime = new DateTime();
        this.saveTime = DateTime.Now;
        gameData.saveTime = this.saveTime.ToString();
    }
}

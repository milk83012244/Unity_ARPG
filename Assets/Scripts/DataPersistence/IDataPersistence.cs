using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ƾګ��[�Ƥ��� �ݭn�ϥμƾګ��[�ƪ�����~�ӳo�Ӥ���
/// </summary>
public interface IDataPersistence 
{
    /// <summary>
    /// �qGameDataŪ�����
    /// </summary>
    void LoadData(GameData gameData);
    /// <summary>
    /// �O�s��� �ƼgGameData
    /// </summary>
    void SaveData(GameData gameData);
}

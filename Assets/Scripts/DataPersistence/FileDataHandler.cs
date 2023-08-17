using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;
using System.IO;

/// <summary>
/// 數據持久化 檔案處理器
/// </summary>
public class FileDataHandler
{
    private string dataDirPath = ""; //檔案路徑
    private string dataFileName = ""; //檔案名稱

    private bool useEncryption = false; //使用加密

    private readonly string encryptionCodeWord = "word";

    private readonly string backupExtension = ".bak"; //備份副檔名

    public FileDataHandler(string dateDirPath, string dataFileName,bool useEncryption)//初始化路徑與名稱
    {
        this.dataDirPath = dateDirPath;
        this.dataFileName = dataFileName;
        this.useEncryption = useEncryption;
    }
    /// <summary>
    /// 讀取檔案
    /// </summary>
    public GameData Load(string profileId, bool allowRestoreFromBackup =true)
    {
        if (profileId == null)
        {
            return null;
        }
        //寫入檔案路徑 OS共通
        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
        //讀取的遊戲資料
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                //打開路徑檔案讀取
                using(FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                if (useEncryption) //啟用解密
                {
                    dataToLoad = EncrypeDecrypt(dataToLoad);
                }

                //反序列化Json檔轉回C#物件
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);

            }
            catch (Exception e)
            {
                if (allowRestoreFromBackup)
                {
                    Debug.LogWarning("從路徑讀取檔案發生錯誤,嘗試從備份還原" + "\n" + e);
                    bool rollbackSuccess = AttemptRollback(fullPath);

                    if (rollbackSuccess) //如果讀取成功就不再讀取備份
                    {
                        loadedData = Load(profileId, false); //不從備份復原
                    }
                }
                else
                {
                    Debug.LogWarning("從路徑讀取檔案發生錯誤" + fullPath + "沒有啟動備份" + "\n" + e);
                }
            }
        }
        return loadedData;
    }
    /// <summary>
    /// 保存檔案
    /// </summary>
    public void Save(GameData gameData, string profileId)
    {
        if (profileId == null)
        {
            return;
        }

        //寫入檔案路徑 OS共通
        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
        //備份路徑
        string backupFilePath = fullPath + backupExtension;
        try
        {
            //電腦上不存在檔案時在路徑創建新的目錄
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            //序列化C#資料轉為json
            string dataToStore = JsonUtility.ToJson(gameData, true);

            if (useEncryption) //啟用加密
            {
                dataToStore = EncrypeDecrypt(dataToStore);
            }

            //string dataToStore = JsonMapper.ToJson(gameData);
            //寫入序列化資料到檔案
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }

            //備份檔案
            GameData verifiedGameData = Load(profileId);//驗證存檔資料
            if (verifiedGameData != null)
            {
                File.Copy(fullPath, backupFilePath, true);
            }
            else
            {
                throw new Exception("無法驗證檔案,無法備份");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("從路徑保存檔案發生錯誤" + fullPath + "\n" + e);
        }
    }
    /// <summary>
    /// 刪除存檔
    /// </summary>
    public void Delete(string profileID)
    {
        Debug.Log("啟動刪除檔案,目錄ID: " + profileID);

        if (profileID == null)
        {
            return;
        }
        string fullPath = Path.Combine(dataDirPath,profileID,dataFileName);
        try
        {
            if (File.Exists(fullPath))
            {
                Directory.Delete(Path.GetDirectoryName(fullPath), true);
            }
            else
            {
                Debug.LogWarning("找不到要刪除的路徑" + fullPath);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("刪除存檔目錄失敗" + profileID + "路徑 :" + fullPath + "\n" + e);
        }
    }

    /// <summary>
    /// 讀取所有存檔文件
    /// </summary>
    public Dictionary<string, GameData> LoadAllProfiles()
    {
        Dictionary<string, GameData> profileDictionary = new Dictionary<string, GameData>();

        //尋找目錄下的保存檔案
        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(dataDirPath).EnumerateDirectories();
        foreach (DirectoryInfo dirInfo in dirInfos)
        {
            string profileID = dirInfo.Name;

            //安全性檢查跳過不包含保存檔案的目錄
            string fullPath = Path.Combine(dataDirPath, profileID, dataFileName);
            if (!File.Exists(fullPath))
            {
                Debug.LogWarning("跳過不包含保存檔案的目錄" + profileID);
                continue;
            }

            //把有保存檔案的目錄ID放到字典
            GameData profileData = Load(profileID);

            //安全性檢查 檢查目錄中是否有保存資料
            if (profileData != null)
            {
                profileDictionary.Add(profileID, profileData);
            }
            else
            {
                Debug.LogError("讀取目錄文件時發生問題" + profileID);
            }
        }

        return profileDictionary;
    }
    /// <summary>
    /// 獲得最新的存檔目錄
    /// </summary>
    public string GetMostRecentlyUpdatedProfileID()
    {
        string mostRecentProfileID = null;

        Dictionary<string, GameData> profilesGameData = LoadAllProfiles();
        //遍歷所有存檔目錄
        foreach (KeyValuePair<string,GameData> pair in profilesGameData)
        {
            string profileID = pair.Key;
            GameData gameData = pair.Value;

            if (gameData == null)
            {
                continue;
            }
            //第一個找到的就是最新的
            if (mostRecentProfileID == null)
            {
                mostRecentProfileID = profileID;
            }
            else //否則尋找最新的
            {
                DateTime mostRecentDateTime = DateTime.FromBinary(profilesGameData[mostRecentProfileID].lastUpdated);
                DateTime newDateTime = DateTime.FromBinary(gameData.lastUpdated);
                if (newDateTime > mostRecentDateTime)
                {
                    mostRecentProfileID = profileID;
                }
            }
        }
        return mostRecentProfileID;
    }

    /// <summary>
    /// 加密文字
    /// </summary>
    private string EncrypeDecrypt(string data)
    {
        string modifiedData = "";
        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }
        return modifiedData;
    }
    //嘗試還原備份檔案
    private bool AttemptRollback(string fullPath)
    {
        bool success = false;
        string backupFilePath = fullPath + backupExtension;
        try //還原完成
        {
            if (File.Exists(backupFilePath))
            {
                File.Copy(backupFilePath, fullPath, true);
                success = true;
                Debug.LogWarning("已從備份檔案還原" + backupFilePath);
            }
            else
            {
                throw new Exception("嘗試備份但是沒有備份檔案");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("嘗試還原備份檔案錯誤" + backupFilePath + "\n");
        }
        return success;
    }
}

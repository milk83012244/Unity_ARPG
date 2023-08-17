using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;
using System.IO;

/// <summary>
/// �ƾګ��[�� �ɮ׳B�z��
/// </summary>
public class FileDataHandler
{
    private string dataDirPath = ""; //�ɮ׸��|
    private string dataFileName = ""; //�ɮצW��

    private bool useEncryption = false; //�ϥΥ[�K

    private readonly string encryptionCodeWord = "word";

    private readonly string backupExtension = ".bak"; //�ƥ����ɦW

    public FileDataHandler(string dateDirPath, string dataFileName,bool useEncryption)//��l�Ƹ��|�P�W��
    {
        this.dataDirPath = dateDirPath;
        this.dataFileName = dataFileName;
        this.useEncryption = useEncryption;
    }
    /// <summary>
    /// Ū���ɮ�
    /// </summary>
    public GameData Load(string profileId, bool allowRestoreFromBackup =true)
    {
        if (profileId == null)
        {
            return null;
        }
        //�g�J�ɮ׸��| OS�@�q
        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
        //Ū�����C�����
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                //���}���|�ɮ�Ū��
                using(FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                if (useEncryption) //�ҥθѱK
                {
                    dataToLoad = EncrypeDecrypt(dataToLoad);
                }

                //�ϧǦC��Json����^C#����
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);

            }
            catch (Exception e)
            {
                if (allowRestoreFromBackup)
                {
                    Debug.LogWarning("�q���|Ū���ɮ׵o�Ϳ��~,���ձq�ƥ��٭�" + "\n" + e);
                    bool rollbackSuccess = AttemptRollback(fullPath);

                    if (rollbackSuccess) //�p�GŪ�����\�N���AŪ���ƥ�
                    {
                        loadedData = Load(profileId, false); //���q�ƥ��_��
                    }
                }
                else
                {
                    Debug.LogWarning("�q���|Ū���ɮ׵o�Ϳ��~" + fullPath + "�S���Ұʳƥ�" + "\n" + e);
                }
            }
        }
        return loadedData;
    }
    /// <summary>
    /// �O�s�ɮ�
    /// </summary>
    public void Save(GameData gameData, string profileId)
    {
        if (profileId == null)
        {
            return;
        }

        //�g�J�ɮ׸��| OS�@�q
        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
        //�ƥ����|
        string backupFilePath = fullPath + backupExtension;
        try
        {
            //�q���W���s�b�ɮ׮ɦb���|�Ыطs���ؿ�
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            //�ǦC��C#����ରjson
            string dataToStore = JsonUtility.ToJson(gameData, true);

            if (useEncryption) //�ҥΥ[�K
            {
                dataToStore = EncrypeDecrypt(dataToStore);
            }

            //string dataToStore = JsonMapper.ToJson(gameData);
            //�g�J�ǦC�Ƹ�ƨ��ɮ�
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }

            //�ƥ��ɮ�
            GameData verifiedGameData = Load(profileId);//���Ҧs�ɸ��
            if (verifiedGameData != null)
            {
                File.Copy(fullPath, backupFilePath, true);
            }
            else
            {
                throw new Exception("�L�k�����ɮ�,�L�k�ƥ�");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("�q���|�O�s�ɮ׵o�Ϳ��~" + fullPath + "\n" + e);
        }
    }
    /// <summary>
    /// �R���s��
    /// </summary>
    public void Delete(string profileID)
    {
        Debug.Log("�ҰʧR���ɮ�,�ؿ�ID: " + profileID);

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
                Debug.LogWarning("�䤣��n�R�������|" + fullPath);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("�R���s�ɥؿ�����" + profileID + "���| :" + fullPath + "\n" + e);
        }
    }

    /// <summary>
    /// Ū���Ҧ��s�ɤ��
    /// </summary>
    public Dictionary<string, GameData> LoadAllProfiles()
    {
        Dictionary<string, GameData> profileDictionary = new Dictionary<string, GameData>();

        //�M��ؿ��U���O�s�ɮ�
        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(dataDirPath).EnumerateDirectories();
        foreach (DirectoryInfo dirInfo in dirInfos)
        {
            string profileID = dirInfo.Name;

            //�w�����ˬd���L���]�t�O�s�ɮת��ؿ�
            string fullPath = Path.Combine(dataDirPath, profileID, dataFileName);
            if (!File.Exists(fullPath))
            {
                Debug.LogWarning("���L���]�t�O�s�ɮת��ؿ�" + profileID);
                continue;
            }

            //�⦳�O�s�ɮת��ؿ�ID���r��
            GameData profileData = Load(profileID);

            //�w�����ˬd �ˬd�ؿ����O�_���O�s���
            if (profileData != null)
            {
                profileDictionary.Add(profileID, profileData);
            }
            else
            {
                Debug.LogError("Ū���ؿ����ɵo�Ͱ��D" + profileID);
            }
        }

        return profileDictionary;
    }
    /// <summary>
    /// ��o�̷s���s�ɥؿ�
    /// </summary>
    public string GetMostRecentlyUpdatedProfileID()
    {
        string mostRecentProfileID = null;

        Dictionary<string, GameData> profilesGameData = LoadAllProfiles();
        //�M���Ҧ��s�ɥؿ�
        foreach (KeyValuePair<string,GameData> pair in profilesGameData)
        {
            string profileID = pair.Key;
            GameData gameData = pair.Value;

            if (gameData == null)
            {
                continue;
            }
            //�Ĥ@�ӧ�쪺�N�O�̷s��
            if (mostRecentProfileID == null)
            {
                mostRecentProfileID = profileID;
            }
            else //�_�h�M��̷s��
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
    /// �[�K��r
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
    //�����٭�ƥ��ɮ�
    private bool AttemptRollback(string fullPath)
    {
        bool success = false;
        string backupFilePath = fullPath + backupExtension;
        try //�٭짹��
        {
            if (File.Exists(backupFilePath))
            {
                File.Copy(backupFilePath, fullPath, true);
                success = true;
                Debug.LogWarning("�w�q�ƥ��ɮ��٭�" + backupFilePath);
            }
            else
            {
                throw new Exception("���ճƥ����O�S���ƥ��ɮ�");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("�����٭�ƥ��ɮ׿��~" + backupFilePath + "\n");
        }
        return success;
    }
}

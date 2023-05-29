using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPartyManager : MonoBehaviour
{
    //private static PlayerPartyManager instance;

    //public static PlayerPartyManager GetInstance()
    //{
    //    if (instance == null)
    //    {
    //        Debug.LogError("沒有PlayerPartyManager實例");
    //        return instance;
    //    }
    //    else
    //    {
    //        return instance;
    //    }
    //}
    /// <summary>
    /// 隊伍
    /// </summary>
    [SerializeField] public Dictionary<int, string> partys = new Dictionary<int, string>();

    private void Awake()
    {
        DebugPartys();
        //if (instance == null)
        //{
        //    instance = this;
        //}
        //else
        //{
        //    Destroy(this.gameObject);
        //}
    }
    /// <summary>
    /// 添加隊伍成員
    /// </summary>
    public void AddPartyMember(int partyId,string name)
    {
        if (partys.Count < 3)
        {
            partys.Add(partyId, name);
            //更新UI介面等
        }
        else
        {
            Debug.Log("隊伍人數已上限");
        }
    }
    /// <summary>
    /// 移除隊伍成員
    /// </summary>
    public void RemovePartyMember(int partyId)
    {
        if (partys.ContainsKey(partyId))
        {
            partys.Remove(partyId);
        }
        else
        {
            Debug.Log("無此隊員");
        }
    }
    /// <summary>
    /// 更換隊伍成員
    /// </summary>
    public void ReplacePartyMember(int currentPartyId,string newName)
    {
        if (partys.ContainsKey(currentPartyId))
        {
            partys[currentPartyId] = newName;
        }
        else
        {
            Debug.Log("找不到隊伍編號 + " + currentPartyId);
        }
    }

    /// <summary>
    /// 測試用 之後組隊會從組隊UI介面
    /// </summary>
    private void DebugPartys()
    {
        AddPartyMember(1, "Mo");
        AddPartyMember(2, "Lia");
    }
}

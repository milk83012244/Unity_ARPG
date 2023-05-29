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
    //        Debug.LogError("�S��PlayerPartyManager���");
    //        return instance;
    //    }
    //    else
    //    {
    //        return instance;
    //    }
    //}
    /// <summary>
    /// ����
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
    /// �K�[�����
    /// </summary>
    public void AddPartyMember(int partyId,string name)
    {
        if (partys.Count < 3)
        {
            partys.Add(partyId, name);
            //��sUI������
        }
        else
        {
            Debug.Log("����H�Ƥw�W��");
        }
    }
    /// <summary>
    /// ���������
    /// </summary>
    public void RemovePartyMember(int partyId)
    {
        if (partys.ContainsKey(partyId))
        {
            partys.Remove(partyId);
        }
        else
        {
            Debug.Log("�L������");
        }
    }
    /// <summary>
    /// �󴫶����
    /// </summary>
    public void ReplacePartyMember(int currentPartyId,string newName)
    {
        if (partys.ContainsKey(currentPartyId))
        {
            partys[currentPartyId] = newName;
        }
        else
        {
            Debug.Log("�䤣�춤��s�� + " + currentPartyId);
        }
    }

    /// <summary>
    /// ���ե� ����ն��|�q�ն�UI����
    /// </summary>
    private void DebugPartys()
    {
        AddPartyMember(1, "Mo");
        AddPartyMember(2, "Lia");
    }
}

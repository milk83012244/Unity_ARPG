using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Data/CharacterData/PartyData", fileName = "PartyData")]
public class PartyDataSO : SerializedScriptableObject
{
    public Dictionary<int, string> currentParty = new Dictionary<int, string>();

    private void Awake()
    {
        ResetPartyData();
    }
    public void ResetPartyData()
    {
        for (int i = 1; i < currentParty.Keys.Count +1; i++)
        {
            currentParty[i] = "";
        }
    }
}

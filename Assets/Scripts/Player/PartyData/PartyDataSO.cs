using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Data/CharacterData/PartyData", fileName = "PartyData")]
public class PartyDataSO : SerializedScriptableObject
{
    public Dictionary<int, string> currentParty = new Dictionary<int, string>();

    public void ResetPartyData()
    {
        for (int i = 0; i < currentParty.Keys.Count; i++)
        {
            currentParty[i] = "";
        }
    }
}

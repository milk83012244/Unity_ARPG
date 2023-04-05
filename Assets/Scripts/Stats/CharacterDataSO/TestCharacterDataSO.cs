using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Data",menuName = "TestCharacterData")]
public class TestCharacterDataSO : ScriptableObject
{
    [Header("Stats Info")]
    public int mexHealth;
    public int currentHealth;
    public int baseDefence;
    public int currentDefence;
}

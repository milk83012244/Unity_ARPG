using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 可序列化字典類
/// </summary>
[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<TKey> keys = new List<TKey>();
    [SerializeField] private List<TValue> values = new List<TValue>();

    public void OnBeforeSerialize() //序列化之前將字典轉換
    {
        keys.Clear();
        values.Clear();
        foreach (KeyValuePair<TKey,TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }
    public void OnAfterDeserialize() //序列化之後轉換回字典
    {
        this.Clear();

        if (keys.Count != values.Count)
        {
            Debug.LogError("key的數量(" + keys.Count + ")不匹配value值(" + values.Count + ")的數量");
        }

        for (int i = 0; i < keys.Count; i++)
        {
            this.Add(keys[i], values[i]);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// てㄥ摸
/// </summary>
[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<TKey> keys = new List<TKey>();
    [SerializeField] private List<TValue> values = new List<TValue>();

    public void OnBeforeSerialize() //てぇ玡盢ㄥ锣传
    {
        keys.Clear();
        values.Clear();
        foreach (KeyValuePair<TKey,TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }
    public void OnAfterDeserialize() //てぇ锣传ㄥ
    {
        this.Clear();

        if (keys.Count != values.Count)
        {
            Debug.LogError("key计秖(" + keys.Count + ")ぃで皌value(" + values.Count + ")计秖");
        }

        for (int i = 0; i < keys.Count; i++)
        {
            this.Add(keys[i], values[i]);
        }
    }
}

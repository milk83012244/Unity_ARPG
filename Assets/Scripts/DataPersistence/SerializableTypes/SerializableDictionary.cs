using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �i�ǦC�Ʀr����
/// </summary>
[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<TKey> keys = new List<TKey>();
    [SerializeField] private List<TValue> values = new List<TValue>();

    public void OnBeforeSerialize() //�ǦC�Ƥ��e�N�r���ഫ
    {
        keys.Clear();
        values.Clear();
        foreach (KeyValuePair<TKey,TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }
    public void OnAfterDeserialize() //�ǦC�Ƥ����ഫ�^�r��
    {
        this.Clear();

        if (keys.Count != values.Count)
        {
            Debug.LogError("key���ƶq(" + keys.Count + ")���ǰtvalue��(" + values.Count + ")���ƶq");
        }

        for (int i = 0; i < keys.Count; i++)
        {
            this.Add(keys[i], values[i]);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���������
/// </summary>
public class ObjectPool<T> where T : MonoBehaviour
{
    private static ObjectPool<T> instance;
    public static ObjectPool<T> Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ObjectPool<T>();
            }
            return instance;
        }
    }

    private Queue<T> objectQueue;
    private GameObject prefab;

    public int queueCount { get { return objectQueue.Count; } }

    /// <summary>
    /// ��ҤƳ��
    /// </summary>
    /// <returns></returns>
    //public static ObjectPool<T> GetInstance()
    //{
    //    if (instance == null)
    //    {
    //        Debug.LogError("�S��ObjectPool���");
    //        return instance;
    //    }
    //    else
    //    {
    //        return instance;
    //    }
    //}

    public void InitPool(GameObject prefab, int warmUpCount = 0, Transform parent = null)
    {
        this.prefab = prefab;
        this.objectQueue = new Queue<T>();

        List<T> warmUpList = new List<T>();
        for (int i = 0; i < warmUpCount; i++) //�w���Ыت��������
        {
            T t = instance.Spawn(Vector3.zero, parent);
            warmUpList.Add(t);
        }
        for (int i = 0; i < warmUpList.Count; i++)
        {
            instance.Recycle(warmUpList[i]);
        }
    }

    /// <summary>
    /// �q��������ͦ�����
    /// </summary>
    public T Spawn(Vector3 position, Transform parent = null)
    {
        if (prefab == null)
        {
            Debug.LogError(typeof(T).ToString() + "�S���]�w����");
            return default(T);
        }
        if (queueCount <= 0) //��������S���N�ͦ��s��
        {
            GameObject g = Object.Instantiate(prefab, position, Quaternion.identity, parent);
            T t = g.GetComponent<T>();
            if (t == null)
            {
                Debug.LogError(typeof(T).ToString() + "�S��prefab");
                return default(T);
            }
            objectQueue.Enqueue(t);
        }
        T obj = objectQueue.Dequeue();
        obj.gameObject.transform.position = position;
        //obj.gameObject.transform.rotation = quaternion; //��V

        #region ��ͦ����󰵪��ʧ@
        obj.gameObject.SetActive(true);
        #endregion

        return obj;
    }
    /// <summary>
    /// �N����^���쪫����� �άO�R��
    /// </summary>
    public void Recycle(T obj,bool isDestory=false)
    {
        objectQueue.Enqueue(obj);

        #region ��n�^�������󰵪��ʧ@
        if (isDestory)
        {
            MonoBehaviour.Destroy(obj.gameObject);
        }
        else
        {
            obj.gameObject.SetActive(false);
        }

        #endregion
    }
}

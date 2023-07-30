using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 物件池基類
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
    /// 實例化單例
    /// </summary>
    /// <returns></returns>
    //public static ObjectPool<T> GetInstance()
    //{
    //    if (instance == null)
    //    {
    //        Debug.LogError("沒有ObjectPool實例");
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
        for (int i = 0; i < warmUpCount; i++) //預先創建物件池物件
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
    /// 從物件池中生成物件
    /// </summary>
    public T Spawn(Vector3 position, Transform parent = null)
    {
        if (prefab == null)
        {
            Debug.LogError(typeof(T).ToString() + "沒有設定物件");
            return default(T);
        }
        if (queueCount <= 0) //物件池中沒有就生成新的
        {
            GameObject g = Object.Instantiate(prefab, position, Quaternion.identity, parent);
            T t = g.GetComponent<T>();
            if (t == null)
            {
                Debug.LogError(typeof(T).ToString() + "沒有prefab");
                return default(T);
            }
            objectQueue.Enqueue(t);
        }
        T obj = objectQueue.Dequeue();
        obj.gameObject.transform.position = position;
        //obj.gameObject.transform.rotation = quaternion; //轉向

        #region 對生成物件做的動作
        obj.gameObject.SetActive(true);
        #endregion

        return obj;
    }
    /// <summary>
    /// 將物件回收到物件池中 或是刪除
    /// </summary>
    public void Recycle(T obj,bool isDestory=false)
    {
        objectQueue.Enqueue(obj);

        #region 對要回收的物件做的動作
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

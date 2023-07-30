using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ObjectShaker : MonoBehaviour
{
    private static ObjectShaker instance;

    public static ObjectShaker Instance
    {
        get
        {
            return instance;
        }
    }
    //public static ObjectShaker GetInstance()
    //{
    //    if (instance == null)
    //    {
    //        Debug.LogError("沒有ObjectShaker實例");
    //        return instance;
    //    }
    //    else
    //    {
    //        return instance;
    //    }
    //}

    //public float duration = 1f;      // 震動的持續時間
    //public float strength = 1f;      // 震動的強度
    //public int vibrato = 10;        // 震動的次數
    //public float randomness = 90f;  // 震動的隨機度

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void ObjectShake(Transform transform,float duration,float strengthX,float strengthY, int vibrato, float randomness)
    {
        // 建立一個動畫序列
        Sequence sequence = DOTween.Sequence();

        // 在X軸上進行震動
        sequence.Append(transform.DOShakePosition(duration, new Vector3(strengthX, strengthY, 0f), vibrato, randomness));

        // 在Y軸上進行震動
        //sequence.Append(transform.DOShakePosition(duration, new Vector3(0f, strength, 0f), vibrato, randomness));

        // 啟動動畫序列
        sequence.Play();
    }
}

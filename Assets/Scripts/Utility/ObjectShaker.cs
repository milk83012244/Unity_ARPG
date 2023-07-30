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
    //        Debug.LogError("�S��ObjectShaker���");
    //        return instance;
    //    }
    //    else
    //    {
    //        return instance;
    //    }
    //}

    //public float duration = 1f;      // �_�ʪ�����ɶ�
    //public float strength = 1f;      // �_�ʪ��j��
    //public int vibrato = 10;        // �_�ʪ�����
    //public float randomness = 90f;  // �_�ʪ��H����

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
        // �إߤ@�Ӱʵe�ǦC
        Sequence sequence = DOTween.Sequence();

        // �bX�b�W�i��_��
        sequence.Append(transform.DOShakePosition(duration, new Vector3(strengthX, strengthY, 0f), vibrato, randomness));

        // �bY�b�W�i��_��
        //sequence.Append(transform.DOShakePosition(duration, new Vector3(0f, strength, 0f), vibrato, randomness));

        // �Ұʰʵe�ǦC
        sequence.Play();
    }
}

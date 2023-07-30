using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashHitEffect : MonoBehaviour
{
    public List<GameObject> SlashHitEffectObjects;
    
    private void Start()
    {
        RandomSelectEffect();
        StartCoroutine(Recycle());
    }
    public IEnumerator Recycle() //�ۤv�^��
    {
        yield return new WaitForSeconds(1f);
        ObjectPool<SlashHitEffect>.Instance.Recycle(this);
    }
    private void RandomSelectEffect()
    {
        int index = Random.Range(0, 3);
        SlashHitEffectObjects[index].SetActive(true);
    }
}

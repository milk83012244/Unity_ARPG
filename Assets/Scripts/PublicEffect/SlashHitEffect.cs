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
    public IEnumerator Recycle() //自己回收
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

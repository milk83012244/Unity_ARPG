using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallHitEffect : MonoBehaviour
{
    public List<GameObject> BallHitEffectObjects;

    private void OnDisable()
    {
        StopAllCoroutines(); 
    }
    private void Start()
    {
        RandomSelectEffect();
        StartCoroutine(Recycle());
    }
    public IEnumerator Recycle() //自己回收
    {
        yield return Yielders.GetWaitForSeconds(1f);
        BallHitEffectObjects[0].SetActive(false);
        ObjectPool<BallHitEffect>.Instance.Recycle(this);
    }
    private void RandomSelectEffect()
    {
        //int index = Random.Range(0, 3);
        BallHitEffectObjects[0].SetActive(true);
    }
}

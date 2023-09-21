using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NoticeIconMove : MonoBehaviour
{
    [SerializeField] private float upHigh; //上升的最高高度
    [SerializeField] private float endHigh; //到定點的高度
    [SerializeField] private float stayTime;

    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
    private void OnEnable()
    {
        StartActiveMove();
    }
    public void StartActiveMove()
    {
        this.gameObject.SetActive(true);
        StartCoroutine(ActiveMove());
    }
    private IEnumerator ActiveMove()
    {
        this.transform.DOLocalMoveY(upHigh, 0.15f);
        yield return Yielders.GetWaitForSeconds(0.15f);
        this.transform.DOLocalMoveY(endHigh, 0.05f);
        yield return Yielders.GetWaitForSeconds(0.05f);
        yield return Yielders.GetWaitForSeconds(stayTime);
        this.gameObject.SetActive(false);
    }
}

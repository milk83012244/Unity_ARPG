using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class LeftHintTip : MonoBehaviour
{
    public GameObject tipsTextPrefab;
    public Transform tipsParent;
    public int poolSize = 5;
    public float textUpSpeed = 1f;
    public float fadeDuration = 1.0f; // 文字淡出動畫時間
    public float slideDuration = 1.0f; // 文字滑動動畫時間
    public float showDuration = 2.0f; // 文字顯示持續時間

    private Queue<GameObject> tipsQueue = new Queue<GameObject>();
    private List<GameObject> tipsPool = new List<GameObject>();

    private void Start()
    {
        // 初始化物件池，生成多個提示UI並隱藏它們
        for (int i = 0; i < poolSize; i++)
        {
            GameObject tipsInstance = Instantiate(tipsTextPrefab, tipsParent);
            tipsInstance.SetActive(false);
            tipsPool.Add(tipsInstance);
        }
    }

    /// <summary>
    /// 顯示提示UI
    /// </summary>
    public void ShowTips(string message)
    {
        GameObject tipsInstance = GetPooledTips();
        if (tipsInstance == null)
            return;
        //tipsInstance.SetActive(true);
        TextMeshProUGUI tipsText = tipsInstance.GetComponentInChildren<TextMeshProUGUI>();
        Image background = tipsInstance.GetComponent<Image>();
        tipsText.text = message;

        tipsQueue.Enqueue(tipsInstance);

        Color initialColor = tipsText.color;
        initialColor.a = 0;
        tipsText.color = initialColor;
        background.color = initialColor;

        tipsInstance.SetActive(true);
        tipsInstance.transform.SetAsLastSibling();

        tipsText.DOFade(1f, fadeDuration/2);
        background.DOFade(1, fadeDuration).OnComplete(() =>
        {
            StartCoroutine(HideTipsAfterDelay(tipsInstance));
        });


        // 動畫：文字向上滑動
        //Vector3 originalPosition = tipsInstance.transform.position;
        //Vector3 targetPosition = originalPosition + Vector3.up * textUpSpeed;
        //tipsInstance.transform.DOMove(targetPosition, slideDuration).OnComplete(() =>
        //{
        //    StartCoroutine(HideTipsAfterDelay(tipsInstance));
        //});
    }
    /// <summary>
    /// 從物件池中獲取可用的提示UI
    /// </summary>
    private GameObject GetPooledTips()
    {
        if (tipsPool.Count > 0)
        {
            GameObject tipsInstance = tipsPool[0];
            tipsPool.RemoveAt(0);
            tipsQueue.Enqueue(tipsInstance);
            return tipsInstance;
        }
        return null;
    }
    /// <summary>
    /// 將提示UI返回到物件池
    /// </summary>
    private void ReturnToPool(GameObject tipsInstance)
    {
        tipsInstance.SetActive(false);
        tipsPool.Add(tipsInstance);
        tipsQueue.Dequeue();
    }
    /// <summary>
    /// 延遲指定時間後，隱藏提示UI
    /// </summary>
    private IEnumerator HideTipsAfterDelay(GameObject tipsInstance)
    {
        yield return Yielders.GetWaitForSeconds(showDuration);

        TextMeshProUGUI tipsText = tipsInstance.GetComponentInChildren<TextMeshProUGUI>();
        Image background = tipsInstance.GetComponent<Image>();
        background.DOFade(0f, fadeDuration);
        tipsText.DOFade(0f, fadeDuration);
        yield return Yielders.GetWaitForSeconds(fadeDuration);
        yield return Yielders.GetWaitForSeconds(0.3f);
        ReturnToPool(tipsInstance);
    }
}

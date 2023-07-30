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
    public float fadeDuration = 1.0f; // ��r�H�X�ʵe�ɶ�
    public float slideDuration = 1.0f; // ��r�ưʰʵe�ɶ�
    public float showDuration = 2.0f; // ��r��ܫ���ɶ�

    private Queue<GameObject> tipsQueue = new Queue<GameObject>();
    private List<GameObject> tipsPool = new List<GameObject>();

    private void Start()
    {
        // ��l�ƪ�����A�ͦ��h�Ӵ���UI�����å���
        for (int i = 0; i < poolSize; i++)
        {
            GameObject tipsInstance = Instantiate(tipsTextPrefab, tipsParent);
            tipsInstance.SetActive(false);
            tipsPool.Add(tipsInstance);
        }
    }

    /// <summary>
    /// ��ܴ���UI
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


        // �ʵe�G��r�V�W�ư�
        //Vector3 originalPosition = tipsInstance.transform.position;
        //Vector3 targetPosition = originalPosition + Vector3.up * textUpSpeed;
        //tipsInstance.transform.DOMove(targetPosition, slideDuration).OnComplete(() =>
        //{
        //    StartCoroutine(HideTipsAfterDelay(tipsInstance));
        //});
    }
    /// <summary>
    /// �q�����������i�Ϊ�����UI
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
    /// �N����UI��^�쪫���
    /// </summary>
    private void ReturnToPool(GameObject tipsInstance)
    {
        tipsInstance.SetActive(false);
        tipsPool.Add(tipsInstance);
        tipsQueue.Dequeue();
    }
    /// <summary>
    /// ������w�ɶ���A���ô���UI
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

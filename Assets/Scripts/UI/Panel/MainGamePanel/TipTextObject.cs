using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class TipTextObject : MonoBehaviour
{
    private bool isShowing = false;

    public TextMeshProUGUI tipText;
    public Image backCover;

    private void Start()
    {
        tipText.DOFade(0f, 0f);
        backCover.DOFade(0f, 0f);
    }
    public void startShowTextCor(string str)
    {
        if (isShowing)
        {
            return;
        }

        StartCoroutine(ShowText(str));
    }
    private IEnumerator ShowText(string str)
    {
        isShowing = true;

        tipText.text = str;
        backCover.enabled = true;
        tipText.enabled = true;
        backCover.DOFade(0.5f, 0.3f);
        tipText.DOFade(1f, 0.5f);
        yield return Yielders.GetWaitForSeconds(2f);
        backCover.DOFade(0f, 1f);
        tipText.DOFade(0f, 1f);
        yield return Yielders.GetWaitForSeconds(1f);
        tipText.enabled = false;
        backCover.enabled = false;

        isShowing = false;
    }
}

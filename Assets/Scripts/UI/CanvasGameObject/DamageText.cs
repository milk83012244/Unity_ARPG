using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    public enum DamageType
    {
        Normal,
    }

    public TMP_Text damageText;

    private void OnEnable()
    {
        StartCoroutine(Recycle());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    /// <summary>
    /// �]�w��ܭ�
    /// </summary>
    public void SetDamageText(int damage)
    {
        damageText.text = damage.ToString();
    }
    /// <summary>
    /// �^������
    /// </summary>
    public IEnumerator Recycle()
    {
        yield return new WaitForSeconds(0.6f);
        ObjectPool<DamageText>.GetInstance().Recycle(this);
    }
}

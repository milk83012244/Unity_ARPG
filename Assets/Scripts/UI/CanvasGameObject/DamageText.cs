using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    public TextMeshProUGUI damageText;
    public Image criticalicon;
    private void OnEnable()
    {
        StartCoroutine(Recycle());
        criticalicon.enabled = false;
    }
    private void OnDisable()
    {
        this.transform.localPosition = new Vector3(1.2f, 1.2f, 1.2f);
        StopAllCoroutines();
    }
    /// <summary>
    /// 設定顯示值
    /// </summary>
    public void SetDamageText(int damage,bool isCritical =false)
    {
        damageText.color = new Color(1f, 1f, 1f, 1f);
        damageText.text = damage.ToString();

        if (isCritical)
        {
            criticalicon.enabled = true;
        }
    }
    public void SetDamageText(int damage,ElementType elementType, bool isCritical = false)
    {
        switch (elementType)
        {
            case ElementType.Fire:
                damageText.color = new Color(1f, 0.42f, 0.0047f, 1f);
                break;
            case ElementType.Ice:
                damageText.color = new Color(0.51f, 0.78f, 0.89f, 1f);
                break;
            case ElementType.Wind:
                damageText.color = new Color(0.3f, 1f, 0.56f, 1f);
                break;
            case ElementType.Thunder:
                damageText.color = new Color(0.21f, 0.29f, 0.86f, 1f);
                break;
            case ElementType.Light:
                damageText.color = new Color(1f, 0.96f, 0.6f, 1f);
                break;
            case ElementType.Dark:
                damageText.color = new Color(0.24f, 0.13f, 0.32f, 1f);
                break;
        }
        damageText.text = damage.ToString();

        if (isCritical)
        {
            criticalicon.enabled = true;
            //this.transform.localPosition = new Vector3(1.2f, 1.2f, 1.2f);
        }
    }
    /// <summary>
    /// 回收物件
    /// </summary>
    public IEnumerator Recycle()
    {
        yield return new WaitForSeconds(0.6f);
        ObjectPool<DamageText>.Instance.Recycle(this);
    }
}

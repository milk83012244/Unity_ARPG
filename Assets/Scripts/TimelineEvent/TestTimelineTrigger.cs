using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DemoTrigger : MonoBehaviour
{
    public Transform TeleportPoint;
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("觸發傳送");
        //獲得Player的碰撞體 有才傳送
        //傳送 淡入淡出效果
        this.gameObject.SetActive(false);
    }
}

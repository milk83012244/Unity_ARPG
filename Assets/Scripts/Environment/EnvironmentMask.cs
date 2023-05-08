using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 場景裝飾物件角色在後方的遮罩(掛在玩家身上)
/// </summary>
public class EnvironmentMask : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //可以指定碰撞的東西再做遮罩計算

        if (transform.position.y > collision.transform.position.y)
        {
            SpriteRenderer spriteRenderer = collision.gameObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null) //設定場景物件透明度
            {
                spriteRenderer.color = new(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.5f);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        SpriteRenderer spriteRenderer = collision.gameObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null) //設定場景物件透明度
        {
            spriteRenderer.color = new(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
        }
    }
}

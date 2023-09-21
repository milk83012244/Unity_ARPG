using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵人與玩家前後層級動態控制
/// </summary>
public class EnemyOrderLayerChange : MonoBehaviour
{
    private Transform playerPos;
    private float distance;
    private int currentDirection; //當前方向
    private float currentDistance;

    public SpriteRenderer spriteRenderer;

    private void Start()
    {
        playerPos = PlayerController.GetInstance().transform;
    }
    private void Update()
    {
        OrderLayerChange();
        //CurrentDistanceCheck();
    }
    private void OrderLayerChange()
    {
        if (this.transform.position.y > playerPos.position.y) //Y軸比玩家高
        {
            spriteRenderer.sortingOrder = -1;
        }
        else if (this.transform.position.y <= playerPos.position.y)
        {
            spriteRenderer.sortingOrder = 1;
        }
    }
    private void CurrentDistanceCheck()
    {
        currentDistance = Vector2.Distance(this.transform.position,playerPos.position);
    }
}

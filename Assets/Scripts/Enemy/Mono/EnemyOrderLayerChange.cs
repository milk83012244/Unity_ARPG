using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ĤH�P���a�e��h�ŰʺA����
/// </summary>
public class EnemyOrderLayerChange : MonoBehaviour
{
    private Transform playerPos;
    private float distance;
    private int currentDirection; //��e��V
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
        if (this.transform.position.y > playerPos.position.y) //Y�b�񪱮a��
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

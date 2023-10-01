using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 場景物件與玩家的層級控制
/// </summary>
public class EnvironmentPartsSortingLayer : MonoBehaviour
{
    public float checkDistance;
    private float currentDistance;
    private PlayerController playerController;
    private SpriteRenderer spriteRenderer;
    private void Start()
    {
        playerController = PlayerController.GetInstance();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    void Update()
    {
        OrderLayerChange();
    }
    private void OrderLayerChange()
    {
        currentDistance = Vector2.Distance(this.transform.position, playerController.transform.position);
        if (this.transform.position.y < playerController.transform.position.y && currentDistance <= checkDistance) //在玩家前面且在自身範圍內
        {
            spriteRenderer.sortingLayerName = "EnvironmentFront";
            spriteRenderer.color = new(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
        }
        else //在玩家後面
        {
            spriteRenderer.sortingLayerName = "Environment";
            spriteRenderer.color = new(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
        }
    }
}

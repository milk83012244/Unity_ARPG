using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 場景物件與玩家的層級控制
/// </summary>
public class EnvironmentPartsSortingLayer : MonoBehaviour
{
    public float checkDistance;
    private int playerColliderLayer = 8;
    private float currentDistance;
    private float subCurrentDistance;
    private PlayerController playerController;
    private SubCharacterController subController;
    private SpriteRenderer spriteRenderer;
    private void Start()
    {
        playerController = PlayerController.GetInstance();
        subController = SubCharacterController.GetInstance();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    void Update()
    {
        OrderLayerChange();
    }
    private void OrderLayerChange()
    {
        currentDistance = Vector2.Distance(this.transform.position, playerController.transform.position);

        subCurrentDistance = Vector2.Distance(this.transform.position, subController.transform.position);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<PlayerController>() != null && collision.gameObject.layer == playerColliderLayer)
        {
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
        if (collision.GetComponent<SubCharacterController>() != null)
        {
            if (subController.gameObject.activeSelf)
            {
                if (this.transform.position.y < subController.transform.position.y && subCurrentDistance <= checkDistance) //輔助角色
                {
                    spriteRenderer.sortingLayerName = "EnvironmentFront";
                    spriteRenderer.color = new(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
                }
                else
                {
                    spriteRenderer.sortingLayerName = "Environment";
                    spriteRenderer.color = new(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
                }
            }
        }
    }
}

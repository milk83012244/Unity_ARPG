using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ��������P���a���h�ű���
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
        if (this.transform.position.y < playerController.transform.position.y && currentDistance <= checkDistance) //�b���a�e���B�b�ۨ��d��
        {
            spriteRenderer.sortingLayerName = "EnvironmentFront";
            spriteRenderer.color = new(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
        }
        else //�b���a�᭱
        {
            spriteRenderer.sortingLayerName = "Environment";
            spriteRenderer.color = new(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
        }
    }
}

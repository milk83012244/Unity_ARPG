using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ͤ訤�Ⲿ�ʱ���
/// </summary>
public class SubCharacterController : MonoBehaviour
{
    private Rigidbody2D rig2D;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private PlayerController playerController;

    [SerializeField] private float distance;
    [SerializeField] public int currentDirection; //��e��V
    public bool Moveing => currentDistance > distance;
    [SerializeField] public float currentDistance;

    private void Awake()
    {
        rig2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    private void Update()
    {
        OrderLayerChange();
        currentDirection = DriectionCheck();
    }
    private void OrderLayerChange()
    {
        if (this.transform.position.y > playerController.transform.position.y)
        {
            spriteRenderer.sortingOrder = 0;
        }
        else if (this.transform.position.y <= playerController.transform.position.y)
        {
            spriteRenderer.sortingOrder = 1;
        }
    }
    /// <summary>
    /// �O�_�Ұʸ��H
    /// </summary>
    public bool FollowingCheck()
    {
        currentDistance = Vector2.Distance(this.transform.position, playerController.transform.position);
        if (currentDistance > distance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// �ͤ訤����H���a
    /// </summary>
    public void Following(float speed)
    {
        currentDistance = Vector2.Distance(this.transform.position, playerController.transform.position);

        if (currentDistance > distance)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, playerController.transform.position, speed *Time.deltaTime);
        }
    }
    /// <summary>
    /// �ͤ訤��P���a��V
    /// </summary>
    public int DriectionCheck()
    {
        Vector3 sub = this.transform.position;
        Vector3 player = playerController.transform.position;
        Vector2 direction = player - sub;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (-angle >= -45 && -angle <= 45) //�k
        {
            return 1;
        }
        else if (-angle <= 135 && -angle > 45)//�W
        {
            return 2;
        }
        else if (-angle > 135 && -angle <= 180 || -angle <= -135 && -angle >= -180)//��
        {
            return 3;
        }
        else if (-angle > -135 && -angle < -45)//�U
        {
            return 4;
        }
        else
        {
            return currentDirection;
        }
    }
}

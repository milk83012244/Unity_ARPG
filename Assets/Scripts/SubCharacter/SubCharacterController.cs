using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 友方角色移動控制
/// </summary>
public class SubCharacterController : MonoBehaviour
{
    private Rigidbody2D rig2D;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private PlayerController playerController;

    [SerializeField] private float distance;
    [SerializeField] public int currentDirection; //當前方向
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
    /// 是否啟動跟隨
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
    /// 友方角色跟隨玩家
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
    /// 友方角色與玩家方向
    /// </summary>
    public int DriectionCheck()
    {
        Vector3 sub = this.transform.position;
        Vector3 player = playerController.transform.position;
        Vector2 direction = player - sub;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (-angle >= -45 && -angle <= 45) //右
        {
            return 1;
        }
        else if (-angle <= 135 && -angle > 45)//上
        {
            return 2;
        }
        else if (-angle > 135 && -angle <= 180 || -angle <= -135 && -angle >= -180)//左
        {
            return 3;
        }
        else if (-angle > -135 && -angle < -45)//下
        {
            return 4;
        }
        else
        {
            return currentDirection;
        }
    }
}

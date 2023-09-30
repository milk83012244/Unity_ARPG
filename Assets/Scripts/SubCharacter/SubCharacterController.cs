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
    private EnemyBoss1Unit enemyBoss1Unit;
    [SerializeField] private PlayerController playerController;

    [SerializeField] private float distance;
    public int currentDirection; //當前方向
    public int currentDirectionLeftRight; //當前方向(只有左右)

    public bool Moveing => currentDistance > distance;

    public float currentDistance;
    public float maskDistance;

    private void OnEnable()
    {
        GameManager.Instance.onNormalGameStateChanged += OnGameStateChanged;
        GameManager.Instance.onBattleGameStateChanged += OnGameStateChanged;
        GameManager.Instance.onPasueGameStateChanged += OnGameStateChanged;
        GameManager.Instance.onGameOverGameStateChanged += OnGameStateChanged;
    }
    private void OnDisable()
    {
        GameManager.Instance.onNormalGameStateChanged -= OnGameStateChanged;
        GameManager.Instance.onBattleGameStateChanged -= OnGameStateChanged;
        GameManager.Instance.onPasueGameStateChanged -= OnGameStateChanged;
        GameManager.Instance.onGameOverGameStateChanged -= OnGameStateChanged;
    }
    private void OnDestroy()
    {
        GameManager.Instance.onNormalGameStateChanged -= OnGameStateChanged;
        GameManager.Instance.onBattleGameStateChanged -= OnGameStateChanged;
        GameManager.Instance.onPasueGameStateChanged -= OnGameStateChanged;
        GameManager.Instance.onGameOverGameStateChanged -= OnGameStateChanged;
    }

    private void Awake()
    {
        rig2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    private void Update()
    {
        OrderLayerChange();
        currentDistanceCheck();
        currentDirection = DriectionCheck();
        currentDirectionLeftRight = DriectionCheckLeftRight();
    }
    /// <summary>
    /// 與主要控制角色的層級控制
    /// </summary>
    private void OrderLayerChange()
    {
        maskDistance = Vector2.Distance(this.transform.position, playerController.transform.position);
        if (this.transform.position.y > playerController.transform.position.y)
        {
            spriteRenderer.sortingOrder = -1;
            spriteRenderer.color = new(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
        }
        else if (this.transform.position.y <= playerController.transform.position.y)
        {
            spriteRenderer.sortingOrder = 1;
            //透明度與圖片前後設定
            if (maskDistance < distance - 0.05f)
            {
                spriteRenderer.color = new(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.6f);
            }
            else if (Mathf.Abs(playerController.transform.position.x - this.transform.position.x) < 0.2f && playerController.transform.position.y - this.transform.position.y < 0.7f)
            {
                spriteRenderer.color = new(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.6f);
            }
            else
            {
                spriteRenderer.color = new(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
            }
        }
    }
    private void currentDistanceCheck()
    {
        currentDistance = Vector2.Distance(this.transform.position, playerController.transform.position);
    }
    /// <summary>
    /// 是否啟動跟隨
    /// </summary>
    public bool FollowingCheck()
    {
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
    /// 啟動走路距離
    /// </summary>
    public bool WalkCheck()
    {
        if (currentDistance > distance && currentDistance < distance * 2.2f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// 啟動跑步距離
    /// </summary>
    public bool RunCheck()
    {
        if (currentDistance > distance && currentDistance >= distance * 2.2f)
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
        //currentDistance = Vector2.Distance(this.transform.position, playerController.transform.position);

        if (currentDistance > distance)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, playerController.transform.position, speed * Time.deltaTime);
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
    /// <summary>
    /// 友方角色與玩家方向(只有左右)
    /// </summary>
    public int DriectionCheckLeftRight()
    {
        Vector3 sub = this.transform.position;
        Vector3 player = playerController.transform.position;
        Vector2 direction = player - sub;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (-angle >= -90 && -angle <= 90) //右
        {
            return 1;
        }
        else//左
        {
            return 3;
        }
    }

    /// <summary>
    /// 在特定遊戲狀態下啟用
    /// </summary>
    private void OnGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.Normal || newGameState == GameState.Battle;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 玩家控制器 負責玩家的移動控制
/// </summary>
public class PlayerController : MonoBehaviour, IDataPersistence
{
    #region 單例
    private PlayerController()
    {
        instance = this;
    }

    private static PlayerController instance; //只限於敵人獲取用

    public static PlayerController GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("沒有PlayerController實例");
            return instance;
        }
        else
        {
            return instance;
        }
    }
    #endregion

    private PlayerInput input;
    private Rigidbody2D rig2D;
    private PlayerStateMachine stateMachine;
    private PlayerCharacterSwitch characterSwitch;
    private PlayerCharacterStats characterStats;
    public PlayerBattleUIDisplay uIDisplay;
    public AttackButtons attackButtons;

    public GameObject rangedAimObject;
    public GameObject meleeAimObject;
    public GameObject SkillCursorObject;
    public GameObject LiaProjectilePos;

    private Coroutine DodgeMoveCor;
    private Coroutine MoSkill2MoveCor;

    //移動控制
    public float MoveSpeedX => Mathf.Abs(rig2D.velocity.x);
    public float MoveSpeedY => Mathf.Abs(rig2D.velocity.y);

    public Vector2 targetPosition;

    [HideInInspector] public bool canSlowDownSpeed = true;
    private float slowDownRate = 1;
    public float SlowDownRate //緩速倍率 最低0.5倍
    {
        get
        {
            //if (canSlowDownSpeed && SlowDownRate>=0.5f) //可被緩速
            //    return SlowDownRate;
            //else if(SlowDownRate <= 0.5f)
            //    return 0.5f;
            //else
                return slowDownRate;
        }
        set
        {
            if (canSlowDownSpeed && slowDownRate >= 0.5f) //可被緩速
                slowDownRate = value;
            else if (slowDownRate <= 0.5f)
                slowDownRate = 0.5f;
            else
                slowDownRate = 1;
        }
    }
    [HideInInspector] public float accelerateRate = 1; //加速倍率

    //遠程準心控制
    public float aimRotationSpeed = 5f;  // 準心旋轉速度
    public float aimDistance = 0.25f;
    public float aimHigh = 0.23f;

    //技能瞄準功能控制
    public float skillFiringMaxRange;

    public bool canUseNormalAttack;
    public bool canUseSkill1;//技能是否可使用開關與技能CD無關
    public bool canUseSkill2;
    public bool canUseUSkill;
    public bool isDamageing;

    private bool isEnable = true;

    private void OnDestroy()
    {
        GameManager.Instance.onNormalGameStateChanged -= OnGameStateChanged;
        GameManager.Instance.onBattleGameStateChanged -= OnGameStateChanged;
        GameManager.Instance.onPasueGameStateChanged -= OnGameStateChanged;
        GameManager.Instance.onGameOverGameStateChanged -= OnGameStateChanged;

        GameManager.Instance.onNonePlayerBehaviourStateChanged -= OnPlayerBehaviourStateChanged;
        GameManager.Instance.onInteractivePlayerBehaviourStateChanged -= OnPlayerBehaviourStateChanged;
        GameManager.Instance.onTalkingPlayerBehaviourStateChanged -= OnPlayerBehaviourStateChanged;
        GameManager.Instance.onInCutScenePlayerBehaviourStateChanged -= OnPlayerBehaviourStateChanged;

        characterStats.hpZeroEvent -= HpZeroEvent;
        characterSwitch.DownSwitchEnd -= DownSwitchEndEvent;
    }
    private void Awake()
    {
        //遊戲狀態事件監聽註冊
        GameManager.Instance.onNormalGameStateChanged += OnGameStateChanged;
        GameManager.Instance.onBattleGameStateChanged += OnGameStateChanged;
        GameManager.Instance.onPasueGameStateChanged += OnGameStateChanged;
        GameManager.Instance.onGameOverGameStateChanged += OnGameStateChanged;

        //玩家行為事件監聽註冊
        GameManager.Instance.onNonePlayerBehaviourStateChanged += OnPlayerBehaviourStateChanged;
        GameManager.Instance.onInteractivePlayerBehaviourStateChanged += OnPlayerBehaviourStateChanged;
        GameManager.Instance.onTalkingPlayerBehaviourStateChanged += OnPlayerBehaviourStateChanged;
        GameManager.Instance.onInCutScenePlayerBehaviourStateChanged += OnPlayerBehaviourStateChanged;

        input = GetComponent<PlayerInput>();
        rig2D = GetComponent<Rigidbody2D>();
        stateMachine = GetComponent<PlayerStateMachine>();
        characterSwitch = GetComponent<PlayerCharacterSwitch>();
        characterStats = GetComponent<PlayerCharacterStats>();
    }
    private void Start()
    {
        //HP歸0事件監聽
        characterStats.hpZeroEvent += HpZeroEvent;
        characterSwitch.DownSwitchEnd += DownSwitchEndEvent;

        SetNormalAttackCanUse(true);
        SetSkill1CanUse(true);
        SetSkill2CanUse(true);
        SetUSkillCanUse(true);

        input.EnableGamePlayInputs();
    }

    /// <summary>
    /// 設定控制角色的數值
    /// </summary>
    public void SetCharacterStats(PlayerCharacterStats characterStats)
    {
        this.characterStats = characterStats;
        SpriteRenderer rangedAimSprite = rangedAimObject.GetComponent<SpriteRenderer>();
        //SpriteRenderer meleeAimSprite = meleeAimObject.GetComponent<SpriteRenderer>();

        if (this.characterStats.characterData[this.characterStats.currentCharacterID].attackType != AttackType.RangedAttack)
        {
            rangedAimSprite.enabled = false;
            //meleeAimSprite.enabled = true;
        }
        else if (this.characterStats.characterData[this.characterStats.currentCharacterID].attackType == AttackType.RangedAttack)
        {
            rangedAimSprite.enabled = true;
            //meleeAimSprite.enabled = false;
        }
    }
    public void SkillCursorObjectSetActive(bool isOpen)
    {
        SkillCursorObject.SetActive(isOpen);
    }
    #region 技能/遠程攻擊控制
    /// <summary>
    /// 遠距離角色準心旋轉
    /// </summary>
    public void RotateAim(Transform aimTransform)
    {
        if (!isEnable)
            return;

        //不是遠距攻擊角色
        if (characterStats.characterData[characterStats.currentCharacterID].attackType != AttackType.RangedAttack)
        {
            return;
        }
        //發動攻擊時停止旋轉準心
        if (PlayerState_Attack.isAttack1 || PlayerState_Attack2.isAttack2)
        {
            return;
        }

        #region 滑鼠用
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = transform.position.z;
        // 計算準心的位置
        Vector3 aimDirection = mousePosition - transform.position;
        aimDirection.z = 0f;  // 將Z軸設置為0，以確保在2D平面上旋轉
        aimDirection.Normalize();  // 正規化向量，使其長度為1
        Vector3 aimPosition = LiaProjectilePos.transform.position + new Vector3(0, aimHigh, 0) + aimDistance * aimDirection;
        aimTransform.position = aimPosition;
        #endregion

        #region 手把用
        //Vector3 aimPosition = transform.position + new Vector3(0, aimHigh, 0) + aimDistance * aimTransform.right;
        //aimTransform.position = aimPosition;

        //Vector3 aimDirection = new Vector3(input.AimAxes.x, input.AimAxes.y);
        #endregion

        // 固定準心在最後的角度
        if (aimDirection.magnitude < 0.1f)
        {
            aimDirection = aimTransform.right;
        }

        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimTransform.rotation = Quaternion.Euler(0f, 0f, aimAngle);
    }
    /// <summary>
    /// 技能準心移動控制
    /// </summary>
    public void MoveSkillCursor(Transform skillCursorTransform)
    {
        if (!isEnable)
            return;

        if (characterSwitch.currentSkillManager.skills[0].hasFiring)
        {
            if (input.PressingSkill1)
            {
                #region 滑鼠用
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0f;
                float distance = Vector3.Distance(mousePosition, this.transform.position); //滑鼠與玩家距離

                if (distance > characterSwitch.currentSkillManager.skills[0].skillFiringRange) //固定位置在最大瞄準範圍
                {
                    Vector3 direction = (mousePosition - this.transform.position).normalized;
                    mousePosition = this.transform.position + direction * characterSwitch.currentSkillManager.skills[0].skillFiringRange;
                }

                skillCursorTransform.position = mousePosition;

                #endregion
                #region 手把用

                #endregion
            }
        }
        else
        {
            return;
        }
    }

    #endregion

    #region 移動控制
    public void Move(float speed)
    {
        if (!isEnable)
            return;

        SetVelocityX(speed * input.AxisX);
        SetVelocityY(speed * input.AxisY);
    }
    public void MoveXY(float speedx, float speedy)
    {
        if (!isEnable)
            return;

        SetVelocityXY(speedx * input.AxisX, speedy * input.AxisY);
    }
    //public void SetVelocity(Vector3 velocity)
    //{
    //    if (!isEnable)
    //        return;

    //    rig2D.velocity = velocity * accelerateRate * SlowDownRate;
    //}
    public void SetVelocityX(float velocityX)
    {
        if (!isEnable)
            return;

        rig2D.velocity = new Vector3(velocityX, rig2D.velocity.y) * accelerateRate * SlowDownRate;
    }
    public void SetVelocityY(float velocityY)
    {
        if (!isEnable)
            return;
        rig2D.velocity = new Vector3(rig2D.velocity.x, velocityY) * accelerateRate * SlowDownRate;
    }
    public void SetVelocityXY(float speedX, float speedY)
    {
        if (!isEnable)
            return;

        float XY = Mathf.Sqrt(speedX * speedX + speedY * speedY);
        rig2D.velocity = new Vector3(speedX * Mathf.Sqrt(0.5f), speedY * Mathf.Sqrt(0.5f)) * accelerateRate * SlowDownRate;
    }
    public void StartDodgeMoveCor(Vector2 dodgeDir, float speed, float moveDuration)
    {
        if (!isEnable)
            return;

        if (DodgeMoveCor != null)
        {
            return;
        }

        DodgeMoveCor = StartCoroutine(DodgeMove(dodgeDir, speed, moveDuration));
    }
    public void StartDodgeMoveXYCor(Vector2 dodgeDir, float speedX, float speedY, float moveDuration)
    {
        if (!isEnable)
            return;

        if (DodgeMoveCor != null)
        {
            return;
        }

        DodgeMoveCor = StartCoroutine(DodgeMoveXY(dodgeDir, speedX, speedY, moveDuration));
    }
    private IEnumerator DodgeMove(Vector2 dodgeDir, float speed, float moveDuration)
    {
        rig2D.velocity = dodgeDir * speed * 1 * accelerateRate * SlowDownRate;
        yield return Yielders.GetWaitForSeconds(moveDuration);
        rig2D.velocity = dodgeDir * 0;
        DodgeMoveCor = null;
    }
    private IEnumerator DodgeMoveXY(Vector2 dodgeDir, float speedX, float speedY, float moveDuration)
    {
        rig2D.velocity = new Vector3(speedX * Mathf.Sqrt(0.5f), speedY * Mathf.Sqrt(0.5f)) * dodgeDir * accelerateRate * SlowDownRate;
        yield return Yielders.GetWaitForSeconds(moveDuration);
        rig2D.velocity = dodgeDir * 0;
        DodgeMoveCor = null;
    }
    public void StartMoSkill2MoveCor(Vector2 Dir, float speed, float moveDuration)
    {
        if (!isEnable)
            return;

        if (MoSkill2MoveCor != null)
        {
            StopCoroutine(MoSkill2MoveCor);
        }
        MoSkill2MoveCor = StartCoroutine(MoSkill2Move(Dir, speed, moveDuration));
    }
    public void StartMoSkill2MoveXYCor(Vector2 Dir, float speedX, float speedY, float moveDuration)
    {
        if (!isEnable)
            return;

        if (MoSkill2MoveCor != null)
        {
            StopCoroutine(MoSkill2MoveCor);
        }
        MoSkill2MoveCor = StartCoroutine(MoSkill2MoveXY(Dir, speedX, speedY, moveDuration));
    }
    private IEnumerator MoSkill2Move(Vector2 Dir, float speed, float moveDuration)
    {
        rig2D.velocity = Dir * speed * 1 * accelerateRate * SlowDownRate;
        yield return Yielders.GetWaitForSeconds(moveDuration);
        rig2D.velocity = Dir * 0;
        MoSkill2MoveCor = null;
    }
    private IEnumerator MoSkill2MoveXY(Vector2 Dir, float speedX, float speedY, float moveDuration)
    {
        rig2D.velocity = new Vector3(speedX * Mathf.Sqrt(0.5f), speedY * Mathf.Sqrt(0.5f)) * Dir * accelerateRate * SlowDownRate;
        yield return Yielders.GetWaitForSeconds(moveDuration);
        rig2D.velocity = Dir * 0;
        MoSkill2MoveCor = null;
    }
    #endregion

    #region 戰鬥控制
    public void SetNormalAttackCanUse(bool canUse)
    {
        canUseNormalAttack = canUse;
        attackButtons.normalAttackCanUseAction?.Invoke(canUse);
    }
    /// <summary>
    /// 技能1是否可以使用控制
    /// </summary>
    public void SetSkill1CanUse(bool canUse)
    {
        canUseSkill1 = canUse;
        attackButtons.skill1CanUseAction?.Invoke(canUse);
    }
    public void SetSkill2CanUse(bool canUse)
    {
        canUseSkill2 = canUse;
        attackButtons.skill2CanUseAction?.Invoke(canUse);
    }
    public void SetUSkillCanUse(bool canUse)
    {
        canUseUSkill = canUse;
        attackButtons.USkillCanUseAction?.Invoke(canUse);
    }
    public void StartDamageState(bool start)
    {
        isDamageing = start;
    }
    #endregion

    private void HpZeroEvent()
    {
        isEnable = false;
    }
    private void DownSwitchEndEvent()
    {
        isEnable = true;
    }

    /// <summary>
    /// 在特定遊戲狀態下啟用
    /// </summary>
    private void OnGameStateChanged(GameState newGameState)
    {
        isEnable = newGameState == GameState.Normal || newGameState == GameState.Battle;
        rig2D.velocity = Vector2.zero;
    }
    /// <summary>
    /// 在特定玩家行為狀態下啟用
    /// </summary>
    private void OnPlayerBehaviourStateChanged(PlayerBehaviourState playerBehaviourState)
    {
        isEnable = playerBehaviourState == PlayerBehaviourState.None;
        rig2D.velocity = Vector2.zero;
    }

    public void LoadData(GameData gameData)
    {
        //讀取玩家位置
        this.transform.position = gameData.playerPosition;
    }

    public void SaveData(GameData gameData)
    {
        //儲存玩家位置
        gameData.playerPosition = this.transform.position;
    }
}

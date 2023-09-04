using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// ���a��� �t�d���a�����ʱ���
/// </summary>
public class PlayerController : MonoBehaviour
{
    #region ���
    private PlayerController()
    {
        instance = this;
    }

    private static PlayerController instance; //�u����ĤH�����

    public static PlayerController GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("�S��PlayerController���");
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

    //���ʱ���
    public float MoveSpeedX => Mathf.Abs(rig2D.velocity.x);
    public float MoveSpeedY => Mathf.Abs(rig2D.velocity.y);

    public Vector2 targetPosition;

    //���{�Ǥ߱���
    public float aimRotationSpeed = 5f;  // �Ǥ߱���t��
    public float aimDistance = 0.25f;
    public float aimHigh = 0.23f;

    //�ޯ�˷ǥ\�౱��
    public float skillFiringMaxRange;

    public bool canUseNormalAttack;
    public bool canUseSkill1;//�ޯ�O�_�i�ϥζ}���P�ޯ�CD�L��
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
    }
    private void Awake()
    {
        //�C�����A�ƥ��ť���U
        GameManager.Instance.onNormalGameStateChanged += OnGameStateChanged;
        GameManager.Instance.onBattleGameStateChanged += OnGameStateChanged;
        GameManager.Instance.onPasueGameStateChanged += OnGameStateChanged;
        GameManager.Instance.onGameOverGameStateChanged += OnGameStateChanged;

        //���a�欰�ƥ��ť���U
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
        SetNormalAttackCanUse(true);
        SetSkill1CanUse(true);
        SetSkill2CanUse(true);
        SetUSkillCanUse(true);

        input.EnableGamePlayInputs();
    }

    /// <summary>
    /// �]�w����⪺�ƭ�
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
    #region �ޯ�/���{��������
    /// <summary>
    /// ���Z������Ǥ߱���
    /// </summary>
    public void RotateAim(Transform aimTransform)
    {
        if (!isEnable)
            return;

        //���O���Z��������
        if (characterStats.characterData[characterStats.currentCharacterID].attackType != AttackType.RangedAttack)
        {
            return;
        }
        //�o�ʧ����ɰ������Ǥ�
        if (PlayerState_Attack.isAttack1 || PlayerState_Attack2.isAttack2)
        {
            return;
        }

        #region �ƹ���
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = transform.position.z;
        // �p��Ǥߪ���m
        Vector3 aimDirection = mousePosition - transform.position;
        aimDirection.z = 0f;  // �NZ�b�]�m��0�A�H�T�O�b2D�����W����
        aimDirection.Normalize();  // ���W�ƦV�q�A�Ϩ���׬�1
        Vector3 aimPosition = LiaProjectilePos.transform.position + new Vector3(0, aimHigh, 0) + aimDistance * aimDirection;
        aimTransform.position = aimPosition;
        #endregion

        #region ����
        //Vector3 aimPosition = transform.position + new Vector3(0, aimHigh, 0) + aimDistance * aimTransform.right;
        //aimTransform.position = aimPosition;

        //Vector3 aimDirection = new Vector3(input.AimAxes.x, input.AimAxes.y);
        #endregion

        // �T�w�Ǥߦb�̫᪺����
        if (aimDirection.magnitude < 0.1f)
        {
            aimDirection = aimTransform.right;
        }

        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimTransform.rotation = Quaternion.Euler(0f, 0f, aimAngle);
    }
    /// <summary>
    /// �ޯ�Ǥ߲��ʱ���
    /// </summary>
    public void MoveSkillCursor(Transform skillCursorTransform)
    {
        if (!isEnable)
            return;

        if (characterSwitch.currentSkillManager.skills[0].hasFiring)
        {
            if (input.PressingSkill1)
            {
                #region �ƹ���
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0f;
                float distance = Vector3.Distance(mousePosition, this.transform.position); //�ƹ��P���a�Z��

                if (distance > characterSwitch.currentSkillManager.skills[0].skillFiringRange) //�T�w��m�b�̤j�˷ǽd��
                {
                    Vector3 direction = (mousePosition - this.transform.position).normalized;
                    mousePosition = this.transform.position + direction * characterSwitch.currentSkillManager.skills[0].skillFiringRange;
                }

                skillCursorTransform.position = mousePosition;

                #endregion
                #region ����

                #endregion
            }
        }
        else
        {
            return;
        }
    }

    #endregion

    #region ���ʱ���
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
    public void SetVelocity(Vector3 velocity)
    {
        if (!isEnable)
            return;

        rig2D.velocity = velocity;
    }
    public void SetVelocityX(float velocityX)
    {
        if (!isEnable)
            return;

        rig2D.velocity = new Vector3(velocityX, rig2D.velocity.y);
    }
    public void SetVelocityY(float velocityY)
    {
        if (!isEnable)
            return;
        rig2D.velocity = new Vector3(rig2D.velocity.x, velocityY);
    }
    public void SetVelocityXY(float speedX, float speedY)
    {
        if (!isEnable)
            return;

        float XY = Mathf.Sqrt(speedX * speedX + speedY * speedY);
        rig2D.velocity = new Vector3(speedX * Mathf.Sqrt(0.5f), speedY * Mathf.Sqrt(0.5f));
    }
    //public void DodgeMove(Vector2 dodgeDir, float speed)
    //{
    //    rig2D.velocity = dodgeDir * speed * 1;
    //}
    //public void DodgeMoveXY(Vector2 dodgeDir, float speedX, float speedY)
    //{
    //    rig2D.velocity = new Vector3(speedX * Mathf.Sqrt(0.5f), speedY * Mathf.Sqrt(0.5f)) * dodgeDir;
    //}
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
        rig2D.velocity = dodgeDir * speed * 1;
        yield return Yielders.GetWaitForSeconds(moveDuration);
        rig2D.velocity = dodgeDir * 0;
        DodgeMoveCor = null;
    }
    private IEnumerator DodgeMoveXY(Vector2 dodgeDir, float speedX, float speedY, float moveDuration)
    {
        rig2D.velocity = new Vector3(speedX * Mathf.Sqrt(0.5f), speedY * Mathf.Sqrt(0.5f)) * dodgeDir;
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
            return;
        }
        MoSkill2MoveCor = StartCoroutine(MoSkill2Move(Dir, speed, moveDuration));
    }
    public void StartMoSkill2MoveXYCor(Vector2 Dir, float speedX, float speedY, float moveDuration)
    {
        if (!isEnable)
            return;

        if (MoSkill2MoveCor != null)
        {
            return;
        }
        MoSkill2MoveCor = StartCoroutine(MoSkill2MoveXY(Dir, speedX, speedY, moveDuration));
    }
    private IEnumerator MoSkill2Move(Vector2 Dir, float speed,float moveDuration)
    {
        rig2D.velocity = Dir * speed * 1;
        yield return Yielders.GetWaitForSeconds(moveDuration);
        rig2D.velocity = Dir * 0;
        MoSkill2MoveCor = null;
    }
    private IEnumerator MoSkill2MoveXY(Vector2 Dir, float speedX, float speedY, float moveDuration)
    {
        rig2D.velocity = new Vector3(speedX * Mathf.Sqrt(0.5f), speedY * Mathf.Sqrt(0.5f)) * Dir;
        yield return Yielders.GetWaitForSeconds(moveDuration);
        rig2D.velocity = Dir * 0;
        MoSkill2MoveCor = null;
    }
    #endregion

    #region �԰�����
    public void SetNormalAttackCanUse(bool canUse)
    {
        canUseNormalAttack = canUse;
        attackButtons.normalAttackCanUseAction?.Invoke(canUse);
    }
    /// <summary>
    /// �ޯ�1�O�_�i�H�ϥα���
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

    /// <summary>
    /// �b�S�w�C�����A�U�ҥ�
    /// </summary>
    private void OnGameStateChanged(GameState newGameState)
    {
        isEnable = newGameState == GameState.Normal || newGameState == GameState.Battle;
    }
    /// <summary>
    /// �b�S�w���a�欰���A�U�ҥ�
    /// </summary>
    private void OnPlayerBehaviourStateChanged(PlayerBehaviourState playerBehaviourState)
    {
        isEnable = playerBehaviourState == PlayerBehaviourState.None;
    }
}

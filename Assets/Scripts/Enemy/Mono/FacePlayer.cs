using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FacePlayer : MonoBehaviour
{
    float stateStartTime;

    private OtherCharacterStats selfStats;
    private string selfName;
    private Animator animator;
    public int currentDirection;

    /// <summary>
    /// �ʵe�����Х�
    /// </summary>
    public bool IsAnimationFinished => StateDuration >= animator.GetCurrentAnimatorStateInfo(0).length;
    /// <summary>
    /// ���A����ɶ�
    /// </summary>
    protected float StateDuration => Time.time - stateStartTime;
    /// <summary>
    /// ��e���A�ɶ�
    /// </summary>
    public float CurrentStateTime => animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

    private void Start()
    {
        selfStats = GetComponent<OtherCharacterStats>();
        animator = this.gameObject.GetComponentInChildren<Animator>();
        selfName = selfStats.enemyBattleData.characterName;
    }
    public int DirectionCheck(Vector3 self, Vector3 target)
    {
        Vector2 direction = target - self;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (-angle >= -45 && -angle <= 45) //�k
        {
            return 1;
        }
        else if (-angle <= 135 && -angle > 45)//�U
        {
            return 2;
        }
        else if (-angle > 135 && -angle <= 180 || -angle <= -135 && -angle >= -180)//��
        {
            return 3;
        }
        else if (-angle > -135 && -angle < -45)//�W
        {
            return 4;
        }
        else
        {
            return currentDirection;
        }
    }
  /// <summary>
  /// ��V�P�ʵe
  /// </summary>
    public void AnimationDirCheck(int currentDirection, string animationType, Animator animator)
    {
        stateStartTime = Time.time;

        if (animationType == "Idle")
        {
            switch (currentDirection)
            {
                case 1:
                    animator.Play(selfName + "_SR_idle");
                    break;
                case 2:
                case 3:
                case 4:
                    animator.Play(selfName + "_SL_idle");
                    break;
            }
        }
        else if (animationType == "Move")
        {
            switch (currentDirection)
            {
                case 1:
                case 4:
                    animator.Play(selfName + "_SR_Move");
                    break;
                case 2:
                case 3:
                    animator.Play(selfName + "_SL_Move");
                    break;
            }
        }
        else if (animationType == "Attack")
        {
            //if (!IsAnimationFinished)
            //    return;

            switch (currentDirection)
            {
                case 1:
                    animator.Play(selfName + "_SR_Attack");
                    break;
                case 2:
                    animator.Play(selfName + "_F_Attack");
                    break;
                case 3:
                    animator.Play(selfName + "_SL_Attack");
                    break;
                case 4:
                    animator.Play(selfName + "_B_Attack");
                    break;
            }
        }
    }

    /// <summary>
    /// Boss1����V�P�ʵe
    /// </summary>
    public void Boss1AnimationDirCheck(int currentDirection, string animationType, Animator animator, int attackType = 1)
    {
        stateStartTime = Time.time;

        if (animationType == "Idle")
        {
            switch (currentDirection)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                    animator.Play(selfName + "_F_BattleIdle");
                    break;
            }
        }
        else if (animationType == "Walk")
        {
            switch (currentDirection)
            {
                case 1:
                case 4:
                    animator.Play(selfName + "_SR_Walk");
                    break;
                case 2:
                case 3:
                    animator.Play(selfName + "_SL_Walk");
                    break;
            }
        }
        else if (animationType == "Flying")
        {
            switch (currentDirection)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                    animator.Play(selfName + "F_Flying");
                    break;
            }
        }
        else if (animationType == "Attack")
        {
            switch (currentDirection)
            {
                case 1:
                    switch (attackType)
                    {
                        case 1:
                            animator.Play(selfName + "_SR_Attack1");
                            break;
                        case 2:
                            animator.Play(selfName + "_SR_Attack2");
                            break;
                        case 3:
                            animator.Play(selfName + "_SR_Attack3");
                            break;
                    }
                    break;
                case 2:
                    switch (attackType)
                    {
                        case 1:
                            animator.Play(selfName + "_F_Attack1");
                            break;
                        case 2:
                            animator.Play(selfName + "_F_Attack2");
                            break;
                        case 3:
                            animator.Play(selfName + "_F_Attack3");
                            break;
                    }
                    break;
                case 3:
                    switch (attackType)
                    {
                        case 1:
                            animator.Play(selfName + "_SL_Attack1");
                            break;
                        case 2:
                            animator.Play(selfName + "_SL_Attack2");
                            break;
                        case 3:
                            animator.Play(selfName + "_SL_Attack3");
                            break;
                    }
                    break;
                case 4:
                    switch (attackType)
                    {
                        case 1:
                            animator.Play(selfName + "_B_Attack1");
                            break;
                        case 2:
                            animator.Play(selfName + "_B_Attack2");
                            break;
                        case 3:
                            animator.Play(selfName + "_B_Attack3");
                            break;
                    }
                    break;
            }
        }
    }
}


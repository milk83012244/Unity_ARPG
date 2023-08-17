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
    protected bool IsAnimationFinished => StateDuration >= animator.GetCurrentAnimatorStateInfo(0).length;
    /// <summary>
    /// ���A����ɶ�
    /// </summary>
    protected float StateDuration => Time.time - stateStartTime;
    /// <summary>
    /// ��e���A�ɶ�
    /// </summary>
    protected float CurrentStateTime => animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

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
    /// �t�d��V�P���񲾰ʫݾ��ʵe
    /// </summary>
    public void AnimationDirCheck(int currentDirection, string animationType, Animator animator)
    {
        if (currentDirection == 0)
            return;

        stateStartTime = Time.time;

        if (animationType == "Idle")
        {
            switch (currentDirection)
            {
                case 1:
                    animator.Play(selfName +"_SR_idle");
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
}


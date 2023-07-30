using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RewardManager : MonoBehaviour
{
    #region ���
    private static RewardManager instance;
    public static RewardManager Instance
    {
        get
        {
            return instance;
        }
    }
    #endregion

    public PlayerLevelSystem levelSystem;
    public LeftHintTip leftHintTip;

    [HideInInspector] public UnityEvent<CharacterRewardDataSO> onEnemyDefeatedEvent;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        onEnemyDefeatedEvent.AddListener(RewardPlayer);
    }
    public void RewardPlayer(CharacterRewardDataSO rewardData)
    {
        levelSystem.GetExperience(rewardData.experiencePoints);
        //leftHintTip.ShowTips("��o 100 ����");
        leftHintTip.ShowTips("��o�g��� : " + rewardData.experiencePoints);
        //leftHintTip.ShowTips("��o�D��G�����_�c");
        //�W�[����
        //�������~
    }
}

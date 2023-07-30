using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RewardManager : MonoBehaviour
{
    #region 單例
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
        //leftHintTip.ShowTips("獲得 100 金錢");
        leftHintTip.ShowTips("獲得經驗值 : " + rewardData.experiencePoints);
        //leftHintTip.ShowTips("獲得道具：神秘寶箱");
        //增加金錢
        //掉落物品
    }
}

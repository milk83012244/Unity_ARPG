using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

/// <summary>
/// 等級系統 管理經驗值相關顯示 與提供等級系統功能
/// </summary>
public class PlayerLevelSystem : MonoBehaviour
{
    private PlayerCharacterStats characterStats;

    public TextMeshProUGUI levelText;
    public Image experienceBarImage;

    [HideInInspector] public LevelSystem levelSystem;
    private LevelSystemAnimated levelSystemAnimated;

    public PlayerLevelSystem()
    {
        levelSystem = new LevelSystem();

        //.OnExperienceChanged += LevelSystem_OnExperienceChanged;
        levelSystem.OnLevelChanged += LevelSystem_OnLevelChanged;
    }
    private void Awake()
    {
        characterStats = GetComponent<PlayerCharacterStats>();

        levelSystem.level = characterStats.characterData[characterStats.currentCharacterID].currentLevel;
        levelSystem.experience = characterStats.characterData[characterStats.currentCharacterID].currentExp;
        levelSystem.experiencePerLevel = characterStats.characterData[characterStats.currentCharacterID].experiencePerLevel;

        levelSystemAnimated = new LevelSystemAnimated(levelSystem);

        this.SetLevelSystem(levelSystem);
        this.SetLevelSystemAnimated(levelSystemAnimated);

        SetLevelSystemData();
    }

    private void Update()
    {
        levelSystemAnimated.Update();
    }
    /// <summary>
    /// 設定經驗值資料
    /// </summary>
    public void SetLevelSystemData()
    {
        levelSystem.level = characterStats.characterData[characterStats.currentCharacterID].currentLevel;
        levelSystem.experience = characterStats.characterData[characterStats.currentCharacterID].currentExp;
        levelSystem.experiencePerLevel = characterStats.characterData[characterStats.currentCharacterID].experiencePerLevel;

        this.SetLevelSystem(levelSystem);
        this.SetLevelSystemAnimated(levelSystemAnimated);

        SetExperienceBarSize(levelSystem.GetExperienceNormalized());
        SetLevelNumber(levelSystem.level - 1);
        GetExperience(0);
    }
    /// <summary>
    /// 獲得經驗值
    /// </summary>
    public void GetExperience(int amount)
    {
        levelSystem.AddExperience(amount);
        characterStats.characterData[characterStats.currentCharacterID].currentExp = levelSystem.experience;
    }
    /// <summary>
    /// 設定經驗條
    /// </summary>
    private void SetExperienceBarSize(float experienceNormalized)
    {
        experienceBarImage.fillAmount = experienceNormalized;
    }
    private void SetLevelNumber(int levelNumber)
    {
        levelText.text = "Lv: " + (levelNumber + 1);
    }
    public void SetLevelSystem(LevelSystem levelSystem)
    {
        this.levelSystem = levelSystem;
    }
    /// <summary>
    /// 升級事件
    /// </summary>
    private void LevelSystem_OnLevelChanged(object sender, EventArgs e)
    {
        Debug.Log("升級至: " + levelSystem.level + "等級");
    }
    /// <summary>
    /// 等級經驗值動畫執行
    /// </summary>
    public void SetLevelSystemAnimated(LevelSystemAnimated levelSystemAnimated)
    {
        this.levelSystemAnimated = levelSystemAnimated;
        SetLevelNumber(levelSystemAnimated.GetLevelNumber());
        SetExperienceBarSize(levelSystemAnimated.GetExperienceNormalized());

        levelSystemAnimated.OnExperienceChanged += LevelSystemAnimated_OnExperienceChanged;
        levelSystemAnimated.OnLevelChanged += LevelSystemAnimated_OnLevelChanged;
    }
    /// <summary>
    /// 動畫改變等級的事件
    /// </summary>
    private void LevelSystemAnimated_OnLevelChanged(object sender, EventArgs e)
    {
        SetLevelNumber(levelSystemAnimated.GetLevelNumber());
    }
    /// <summary>
    /// 動畫改變經驗值的事件
    /// </summary>
    private void LevelSystemAnimated_OnExperienceChanged(object sender, System.EventArgs e)
    {
        SetExperienceBarSize(levelSystemAnimated.GetExperienceNormalized());
    }
}

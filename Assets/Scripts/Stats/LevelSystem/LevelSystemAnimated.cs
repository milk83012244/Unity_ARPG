using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 等級系統相關動畫控制
/// </summary>
public class LevelSystemAnimated
{
    public event EventHandler OnExperienceChanged; //獲得經驗值事件
    public event EventHandler OnLevelChanged; //升級事件

    private LevelSystem levelSystem;

    private bool isAnimating;

    private float updateTimer; //fps刷新秒數
    private float updateTimerMax; //fps最大刷新秒數

    private int level;
    private int experience;
    //private int experienceToNextLevel;

    public LevelSystemAnimated(LevelSystem levelSystem)
    {
        SetLevelSystem(levelSystem);
        updateTimerMax = 0.016f; //60fps的更新秒數

        //Application.targetFrameRate = 10;
    }
    public void SetLevelSystem(LevelSystem levelSystem)
    {
        this.levelSystem = levelSystem;

        level = levelSystem.GetLevelNumber();
        experience = levelSystem.GetExperience();
        //experienceToNextLevel = levelSystem.GetExperienceToNextLevel();

        levelSystem.OnExperienceChanged += LevelSystem_OnExperienceChanged;
        levelSystem.OnLevelChanged += LevelSystem_OnLevelChanged;
    }

    private void LevelSystem_OnLevelChanged(object sender, EventArgs e)
    {
        isAnimating = true;
    }

    private void LevelSystem_OnExperienceChanged(object sender, EventArgs e)
    {
        isAnimating = true;
    }
    public void Update() //增加經驗值
    {
        if (isAnimating)
        {
            updateTimer += Time.deltaTime;
            while (updateTimer > updateTimerMax)
            {
                updateTimer -= updateTimerMax; //刷新動畫計時
                UpdateAddExperience();
            }
        }
        //Debug.Log(level + " " + experience);
    }
    /// <summary>
    /// 更新經驗值
    /// </summary>
    private void UpdateAddExperience()
    {
        if (level < levelSystem.GetLevelNumber())
        {
            AddExperience();
        }
        else
        {
            if (experience < levelSystem.GetExperience())
            {
                AddExperience();
            }
            else
            {
                isAnimating = false;
            }
        }
    }
    /// <summary>
    /// 增加經驗值
    /// </summary>
    private void AddExperience() 
    {
        experience++;
        if (experience>=levelSystem.GetExperienceToNextLevel(level))
        {
            level++;
            experience = 0;
            if (OnLevelChanged != null) OnLevelChanged(this, EventArgs.Empty);
        }

        if (OnExperienceChanged != null) OnExperienceChanged(this, EventArgs.Empty);
    }

    public int GetLevelNumber()
    {
        return level;
    }
    public float GetExperienceNormalized()
    {
        if (levelSystem.IsMaxLevel(level))
            return 1f;
        else
            return (float)experience / levelSystem.GetExperienceToNextLevel(level);
    }
}

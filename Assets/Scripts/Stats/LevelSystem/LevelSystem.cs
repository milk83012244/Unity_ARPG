using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 等級系統
/// </summary>
public class LevelSystem
{
    public event EventHandler OnExperienceChanged; //獲得經驗值事件
    public event EventHandler OnLevelChanged; //升級事件

    public List<int> experiencePerLevel = new List<int>(); //升級經驗表
    public int level; //當前等級
    public int experience; //當前經驗
    //private int experienceToNextLevel; //到下一級所需經驗

    public LevelSystem()
    {
        level = 0;
        experience = 0;
        //experienceToNextLevel = 100;
    }
    /// <summary>
    /// 獲得經驗值
    /// </summary>
    public void AddExperience(int amount)
    {
        if (!IsMaxLevel())
        {
            experience += amount;
            while (!IsMaxLevel() && experience >= GetExperienceToNextLevel(level))
            {
                experience -= GetExperienceToNextLevel(level);
                level++;
                if (OnLevelChanged != null) OnLevelChanged(this, EventArgs.Empty);
            }

            if (OnExperienceChanged != null) OnExperienceChanged(this, EventArgs.Empty);
        }
    }
    /// <summary>
    /// 獲得等級的值
    /// </summary>
    public int GetLevelNumber()
    {
        return level;
    }
    /// <summary>
    /// 獲得經驗值的值
    /// </summary>
    public int GetExperience()
    {
        return experience;
    }
    /// <summary>
    /// 獲得與下一等級經驗值差的值
    /// </summary>
    public int GetExperienceToNextLevel(int level)
    {
        if (level<experiencePerLevel.Count)
        {
            return experiencePerLevel[level];
        }
        else
        {
            Debug.LogError("已達最大等級" + level);
            return 100;
        }
    }
    /// <summary>
    /// 獲得經驗值的百分比
    /// </summary>
    public float GetExperienceNormalized()
    {
        if (IsMaxLevel())
            return 1f;
        else
            return (float)experience / GetExperienceToNextLevel(level);
    }

    public bool IsMaxLevel()
    {
        return IsMaxLevel(level);
    }
    /// <summary>
    /// 是否已達最大等級
    /// </summary>
    public bool IsMaxLevel(int level)
    {
        return level == experiencePerLevel.Count - 1;
    }
}

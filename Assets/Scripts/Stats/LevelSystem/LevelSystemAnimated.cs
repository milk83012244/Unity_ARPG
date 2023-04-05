using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���Ũt�ά����ʵe����
/// </summary>
public class LevelSystemAnimated
{
    public event EventHandler OnExperienceChanged; //��o�g��Ȩƥ�
    public event EventHandler OnLevelChanged; //�ɯŨƥ�

    private LevelSystem levelSystem;

    private bool isAnimating;

    private float updateTimer; //fps��s���
    private float updateTimerMax; //fps�̤j��s���

    private int level;
    private int experience;
    //private int experienceToNextLevel;

    public LevelSystemAnimated(LevelSystem levelSystem)
    {
        SetLevelSystem(levelSystem);
        updateTimerMax = 0.016f; //60fps����s���

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
    public void Update() //�W�[�g���
    {
        if (isAnimating)
        {
            updateTimer += Time.deltaTime;
            while (updateTimer > updateTimerMax)
            {
                updateTimer -= updateTimerMax; //��s�ʵe�p��
                UpdateAddExperience();
            }
        }
        //Debug.Log(level + " " + experience);
    }
    /// <summary>
    /// ��s�g���
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
    /// �W�[�g���
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���Ũt��
/// </summary>
public class LevelSystem
{
    public event EventHandler OnExperienceChanged; //��o�g��Ȩƥ�
    public event EventHandler OnLevelChanged; //�ɯŨƥ�

    private static readonly int[] experiencePerLevel = new[] { 100,120,140,160,180,200,220,250,300,400 }; //�ɯŸg���
    private int level; //���e����
    private int experience; //���e�g��
    //private int experienceToNextLevel; //��U�@�ũһݸg��

    public LevelSystem()
    {
        level = 0;
        experience = 0;
        //experienceToNextLevel = 100;
    }
    /// <summary>
    /// ��o�g���
    /// </summary>
    public void AddExperience(int amount)
    {
        if (!IsMaxLevel())
        {
            experience += amount;
            while (!IsMaxLevel() && experience >= GetExperienceToNextLevel(level))
            {
                //�ɯźt�X

                experience -= GetExperienceToNextLevel(level);
                level++;
                if (OnLevelChanged != null) OnLevelChanged(this, EventArgs.Empty);
            }

            if (OnExperienceChanged != null) OnExperienceChanged(this, EventArgs.Empty);
        }
    }
    /// <summary>
    /// ��o���Ū���
    /// </summary>
    public int GetLevelNumber()
    {
        return level;
    }
    /// <summary>
    /// ��o�g��Ȫ���
    /// </summary>
    public int GetExperience()
    {
        return experience;
    }
    /// <summary>
    /// ��o�P�U�@���Ÿg��Ȯt����
    /// </summary>
    public int GetExperienceToNextLevel(int level)
    {
        if (level<experiencePerLevel.Length)
        {
            return experiencePerLevel[level];
        }
        else
        {
            Debug.LogError("�w�F�̤j����" + level);
            return 100;
        }
    }
    /// <summary>
    /// ��o�g��Ȫ��ʤ���
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
    /// �O�_�w�F�̤j����
    /// </summary>
    public bool IsMaxLevel(int level)
    {
        return level == experiencePerLevel.Length - 1;
    }
}
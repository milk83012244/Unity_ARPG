using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

/// <summary>
/// ���Ũt�� �޲z�g��Ȭ������ �P���ѵ��Ũt�Υ\��
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
    /// �]�w�g��ȸ��
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
    /// ��o�g���
    /// </summary>
    public void GetExperience(int amount)
    {
        levelSystem.AddExperience(amount);
        characterStats.characterData[characterStats.currentCharacterID].currentExp = levelSystem.experience;
    }
    /// <summary>
    /// �]�w�g���
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
    /// �ɯŨƥ�
    /// </summary>
    private void LevelSystem_OnLevelChanged(object sender, EventArgs e)
    {
        Debug.Log("�ɯŦ�: " + levelSystem.level + "����");
    }
    /// <summary>
    /// ���Ÿg��Ȱʵe����
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
    /// �ʵe���ܵ��Ū��ƥ�
    /// </summary>
    private void LevelSystemAnimated_OnLevelChanged(object sender, EventArgs e)
    {
        SetLevelNumber(levelSystemAnimated.GetLevelNumber());
    }
    /// <summary>
    /// �ʵe���ܸg��Ȫ��ƥ�
    /// </summary>
    private void LevelSystemAnimated_OnExperienceChanged(object sender, System.EventArgs e)
    {
        SetExperienceBarSize(levelSystemAnimated.GetExperienceNormalized());
    }
}

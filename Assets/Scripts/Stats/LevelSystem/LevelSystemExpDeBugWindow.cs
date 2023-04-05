using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

/// <summary>
/// �g��Ƚոե�
/// </summary>
public class LevelSystemExpDeBugWindow : MonoBehaviour
{
    public TextMeshProUGUI levelText; //���Ť�r
    //public Text levelText;
    public Image experienceBarImage; //�g���UI

    public Button expriencebtn1;
    public Button expriencebtn2;
    public Button expriencebtn3;
    private LevelSystem levelSystem;
    private LevelSystemAnimated levelSystemAnimated;
    private void Awake()
    {
        expriencebtn1.onClick.AddListener(()=>levelSystem.AddExperience(5));
        expriencebtn2.onClick.AddListener(() => levelSystem.AddExperience(50));
        expriencebtn3.onClick.AddListener(() => levelSystem.AddExperience(500));
    }
    /// <summary>
    /// �i�ױ��P�g��ʤ���ȱ��� �i�H�z�L�ʵe�ʺA�W�[
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
    public void SetLevelSystemAnimated(LevelSystemAnimated levelSystemAnimated)
    {
        this.levelSystemAnimated = levelSystemAnimated;
        SetLevelNumber(levelSystemAnimated.GetLevelNumber());
        SetExperienceBarSize(levelSystemAnimated.GetExperienceNormalized());

        //�q�\�ɯũδ��ɸg��Ȭ����ƥ� ��L�}���ϥνd��
        levelSystemAnimated.OnExperienceChanged += LevelSystemAnimated_OnExperienceChanged;
        levelSystemAnimated.OnLevelChanged += LevelSystemAnimated_OnLevelChanged;
    }

    private void LevelSystemAnimated_OnLevelChanged(object sender, EventArgs e)
    {
        SetLevelNumber(levelSystemAnimated.GetLevelNumber());
    }

    private void LevelSystemAnimated_OnExperienceChanged(object sender,System.EventArgs e)
    {
        SetExperienceBarSize(levelSystemAnimated.GetExperienceNormalized());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

/// <summary>
/// 經驗值調試用
/// </summary>
public class LevelSystemExpDeBugWindow : MonoBehaviour
{
    public TextMeshProUGUI levelText; //等級文字
    //public Text levelText;
    public Image experienceBarImage; //經驗條UI

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
    /// 進度條與經驗百分比值掛勾 可以透過動畫動態增加
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

        //訂閱升級或提升經驗值相關事件 其他腳本使用範例
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

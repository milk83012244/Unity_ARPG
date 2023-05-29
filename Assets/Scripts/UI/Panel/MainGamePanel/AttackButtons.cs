using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;


public class AttackButtons : SerializedMonoBehaviour
{
    public Dictionary<string, SkillDataSO> skill1List = new Dictionary<string, SkillDataSO>();
    public Dictionary<string, SkillDataSO> skill2List = new Dictionary<string, SkillDataSO>();
    //public List<SkillDataSO> skill1List = new List<SkillDataSO>();
    //public List<SkillDataSO> skill2List = new List<SkillDataSO>();

    public List<Sprite> skill1SlotImages = new List<Sprite>();
    public List<Sprite> skill2SlotImages = new List<Sprite>();

    public Image skill1IconMask;
    public Image skill2IconMask;

    public Image skill1CountIcon;
    public Image skill2CountIcon;

    public TextMeshProUGUI skill1CountText;
    public TextMeshProUGUI skill2CountText;

    private bool hasUseCount;

    public void StartSkill1Count(string characterName)
    {
        skill1CountText.enabled = true;
        StartCoroutine(skill1CDCount(characterName));
    }
    private IEnumerator skill1CDCount(string characterName)
    {
        float currentTime = skill1List[characterName].skillCoolDown;

        while (currentTime >= 0)
        {
            currentTime -= Time.deltaTime;
            skill1CountText.text = currentTime.ToString("F1");
            skill1IconMask.fillAmount = currentTime / skill1List[characterName].skillCoolDown;

            yield return null;
        }
        skill1CountText.enabled = false;
    }

    public void StartSkill2Count(string characterName)
    {
        skill2CountText.enabled = true;
        StartCoroutine(skill2CDCount(characterName));
    }
    private IEnumerator skill2CDCount(string characterName)
    {
        float currentTime = skill2List[characterName].skillCoolDown;

        while (currentTime >= 0)
        {
            currentTime -= Time.deltaTime;
            skill2CountText.text = currentTime.ToString("F1");
            skill2IconMask.fillAmount = currentTime / skill2List[characterName].skillCoolDown;

            yield return null;
        }
        skill2CountText.enabled = false;
    }
}

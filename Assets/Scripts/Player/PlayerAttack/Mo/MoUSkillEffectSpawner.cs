using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoUSkillEffectSpawner : MonoBehaviour
{
    private PlayerSkillManager skillManager;

    [HideInInspector] public SkillDataSO skillData;
    [HideInInspector] public PlayerInput playerInput;
    [HideInInspector] public PlayerCharacterStats characterStats;
    public GameObject uskillStartEffect1Prefab;
    public GameObject uskillStartEffect2Prefab;
    public Transform spawnParent1;
    public Transform spawnParent2;

    private GameObject uskillStartEffect1Object; //已生成的物件
    private GameObject uskillStartEffect2Object; //已生成的物件
    private MoUSkillStartEffect1 moUSkillStartEffect1;
    private MoUSkillStartEffect2 moUSkillStartEffect2;
    private void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        skillManager = GetComponentInParent<PlayerSkillManager>();
        characterStats = GetComponentInParent<PlayerCharacterStats>();
    }
    private void Start()
    {
        if (skillManager != null)
        {
            skillData = skillManager.skills[2];
        }
    }
    public void SpawnStartEffect1()
    {
        if (uskillStartEffect1Object == null)
        {
            uskillStartEffect1Object = Instantiate(uskillStartEffect1Prefab, spawnParent1);
            moUSkillStartEffect1 = uskillStartEffect1Object.GetComponent<MoUSkillStartEffect1>();
        }
        else
            uskillStartEffect1Object.SetActive(true);

        if (moUSkillStartEffect1 != null)
        {
            moUSkillStartEffect1.Init(characterStats,this);
        }
    }
    public void SpawnStartEffect2()
    {
        if (uskillStartEffect2Object == null)
        {
            uskillStartEffect2Object = Instantiate(uskillStartEffect2Prefab, spawnParent2);
            moUSkillStartEffect2 = uskillStartEffect2Object.GetComponent<MoUSkillStartEffect2>();
        }
        else
            uskillStartEffect2Object.SetActive(true);

        if (moUSkillStartEffect2 != null)
        {
            moUSkillStartEffect2.Init(characterStats);
        }
    }
}

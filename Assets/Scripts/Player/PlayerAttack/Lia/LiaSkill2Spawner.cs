using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Lia�ޯ�2����ͦ��� �t�d�ݩʤ��� Ĳ�oCD�p�� �ͦ�������޲z
/// </summary>
public class LiaSkill2Spawner : MonoBehaviour
{
    private PlayerController controller;
    private PlayerInput playerInput;
    private LiaElementSwitch elementSwitch;
    private PlayerSkillManager skillManager;

    [HideInInspector] public PlayerCharacterStats characterStats;
    [HideInInspector] public SkillDataSO skillData;

    public Transform poolParent;
    public GameObject elementPrefab;
    private ObjectPool<LiaSkill2Effect> elementEffectPool;
    private void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        controller = GetComponentInParent<PlayerController>();
        characterStats = GetComponentInParent<PlayerCharacterStats>();
        skillManager = GetComponentInParent<PlayerSkillManager>();
    }
    private void Start()
    {
        if (skillManager != null)
        {
            skillData = skillManager.skills[1];
        }
        SetProjectilePool();
    }
    public void SetProjectilePool()
    {
        elementEffectPool = ObjectPool<LiaSkill2Effect>.Instance; //�S�Ī����l��
        elementEffectPool.InitPool(elementPrefab, 5, poolParent);
    }
    public void SpawnEffect()
    {
        LiaSkill2Effect elementObject = elementEffectPool.Spawn(controller.transform.position + new Vector3(0, 0.28f), poolParent);
        elementObject.GetCharacterStats(characterStats);
    }
    public void StartSkillCoolDown()
    {
        StartCoroutine(SkillCoolDown());
    }
    private IEnumerator SkillCoolDown()
    {
        playerInput.canSkill2[characterStats.currentCharacterID] = false;
        yield return Yielders.GetWaitForSeconds(skillData.skillCoolDown);
        playerInput.canSkill2[characterStats.currentCharacterID] = true;
    }
}

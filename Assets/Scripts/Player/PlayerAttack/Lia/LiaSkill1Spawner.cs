using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Lia�ޯ�1����ͦ��� �t�d�ݩʤ��� Ĳ�oCD�p�� �ͦ�������޲z
/// </summary>
public class LiaSkill1Spawner : MonoBehaviour
{
    private PlayerController controller;
    private PlayerInput playerInput;
    private LiaElementSwitch elementSwitch;
    private PlayerSkillManager skillManager;

    [HideInInspector] public PlayerCharacterStats characterStats;
    [HideInInspector] public SkillDataSO skillData;

    //public List<GameObject> SkillTypeObjects;

    public Transform poolParent;
    public GameObject elementPrefab;
    private ObjectPool<LiaSkill1Effect> elementEffectPool;

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
            skillData = skillManager.skills[0];
        }
        SetProjectilePool();
    }
    public void SetProjectilePool()
    {
        elementEffectPool = ObjectPool<LiaSkill1Effect>.Instance; //�S�Ī����l��
        elementEffectPool.InitPool(elementPrefab, 5, poolParent);
    }
    public void SpawnEffect(Transform skillCursorPosition)
    {
        LiaSkill1Effect elementObject = elementEffectPool.Spawn(skillCursorPosition.position+new Vector3(0,0.28f), poolParent);
        elementObject.GetCharacterStats(characterStats);
    }

    public void StartSkillCoolDown()
    {
        StartCoroutine(SkillCoolDown());
    }
    private IEnumerator SkillCoolDown()
    {
        playerInput.canSkill1[characterStats.currentCharacterID] = false;
        yield return Yielders.GetWaitForSeconds(skillData.skillCoolDown);
        playerInput.canSkill1[characterStats.currentCharacterID] = true;
    }
}

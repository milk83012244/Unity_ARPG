using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Lia�ޯ�2����������ĪG 
/// �S��ĪG:�ᤩ��e�����e�ޯ઺�ݩʼW�[�Ƨ����ˮ`
/// </summary>
public class LiaSkill2_iceRotateEffectDamage : MonoBehaviour
{
    public LiaSkill2RotateEffect liaSkill2RotateEffect;

    public float damageInterval = 0.5f;

    private bool isDamaging = false;
    private bool isCritical;

    public GameObject centerObject;
    public float rotationRadius;
    public float rotationSpeed;
    public int numObjects;
    public Transform[] objects;

    //�ޯ��������� �x�s�ƭ�
    [HideInInspector] public ElementType currentElement;
    [HideInInspector] public int currentDamage;
    [HideInInspector] public float currentcriticalChance;

    private Dictionary<IDamageable, Coroutine> damageCoroutines = new Dictionary<IDamageable, Coroutine>();

    private void OnEnable()
    {
        SetEffectData();
        StartDamaging();
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private void Start()
    {

    }

    private void Update()
    {
        //����¶���߱���
        for (int i = 0; i < numObjects; i++)
        {
            float angle = (Time.time * rotationSpeed) + ((360f / numObjects) * i);
            Vector3 position = GetPosition(angle);
            objects[i].position = position;
        }

        this.transform.position = centerObject.transform.position;
    }

    /// <summary>
    /// �x�s�I��̸�ƨѤ����������ϥ�
    /// </summary>
    private void SetEffectData()
    {
        currentElement = liaSkill2RotateEffect.element;
        currentDamage = (int)UnityEngine.Random.Range(liaSkill2RotateEffect.characterStats.attackData[liaSkill2RotateEffect.characterStats.currentCharacterID].minDamage, liaSkill2RotateEffect.characterStats.attackData[liaSkill2RotateEffect.characterStats.currentCharacterID].maxDamage);
        currentcriticalChance = liaSkill2RotateEffect.characterStats.attackData[liaSkill2RotateEffect.characterStats.currentCharacterID].criticalChance;
    }

    //�����m����
    private Vector3 GetPosition(float angle)
    {
        float x = centerObject.transform.position.x + rotationRadius * Mathf.Cos(angle * Mathf.Deg2Rad);
        float y = centerObject.transform.position.y;
        float z = centerObject.transform.position.z + rotationRadius * Mathf.Sin(angle * Mathf.Deg2Rad);

        return new Vector3(x, y, z);
    }

    private void StartDamaging()
    {
        isDamaging = true;
    }

    private IEnumerator DealDamage(IDamageable damageable, ElementType element)
    {
        if (element == ElementType.None || damageable == null)
        {
          yield break;
        }

        while (isDamaging)
        {
            isCritical = Random.value < currentcriticalChance;
            if (damageable != null)
            {
                Enemy enemyUnit = damageable as Enemy;
                OtherCharacterStats defander = enemyUnit.GetComponent<OtherCharacterStats>();

                if (element == ElementType.Ice)
                {
                    liaSkill2RotateEffect.characterStats.TakeDamage(liaSkill2RotateEffect.characterStats, defander, ElementType.Ice);
                    currentDamage = liaSkill2RotateEffect.characterStats.currentDamage;
                    enemyUnit.SpawnDamageText(currentDamage, ElementType.Ice);

                    //�B�ݩʼh�ŮĪG

                    if (enemyUnit.isMarked)
                    {
                        liaSkill2RotateEffect.characterStats.TakeMarkDamage(liaSkill2RotateEffect.characterStats, defander, isCritical);
                        enemyUnit.SpawnMarkDamageText(currentDamage, isCritical);
                        liaSkill2RotateEffect.characterStats.TakeDamage(liaSkill2RotateEffect.characterStats, defander, isCritical);
                        enemyUnit.SpawnDamageText(currentDamage, isCritical);

                        enemyUnit.ClearMark();
                    }
                    //EnemyUnitType1 enemyUnitType1 = enemyUnit as EnemyUnitType1;
                    ////Ĳ�o�ĤH�������A
                    //enemyUnitType1.isAttackState = true;
                    ////�ĤH�{�{�ĪG
                    //enemyUnitType1.StartFlash();
                }
            }
            yield return new WaitForSeconds(damageInterval);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            IDamageable damageable = collision.GetComponent<IDamageable>();
            //StartCoroutine(DealDamage(damageable, liaSkill1Effect.element));
            if (damageable != null && !damageCoroutines.ContainsKey(damageable))
            {
                Coroutine coroutine = StartCoroutine(DealDamage(damageable, currentElement));
                damageCoroutines.Add(damageable, coroutine);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            IDamageable damageable = collision.GetComponent<IDamageable>();
            if (damageable != null && damageCoroutines.ContainsKey(damageable))
            {
                Coroutine coroutine = damageCoroutines[damageable];
                StopCoroutine(coroutine);
                damageCoroutines.Remove(damageable);
            }
        }
    }

}

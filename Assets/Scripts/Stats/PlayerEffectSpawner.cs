using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerEffectSpawner : MonoBehaviour
{
    private PlayerInput playerInput;

    public Transform effectParent;

    //�{�׷����S��
    [HideInInspector] public Action DodgeSmokeTrigger;
    private Coroutine dodgeSmokeEffectCor;
    [SerializeField] private GameObject smokePrafab;
    //�����S��
    //public UnityEvent SlashHitEffectTrigger;
    public GameObject slashHitEffectPrefab; //�����S��
    public ObjectPool<SlashHitEffect> SlashHitEffectPool;
    public GameObject ballHitEffectPrefab; //�y�ίS��
    public ObjectPool<BallHitEffect> ballHitEffectPool;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }
    private void Start()
    {
        //���������S�Ī�l��
        SlashHitEffectPool = ObjectPool<SlashHitEffect>.Instance; 
        SlashHitEffectPool.InitPool(slashHitEffectPrefab, 10, effectParent);

        ballHitEffectPool = ObjectPool<BallHitEffect>.Instance;
        ballHitEffectPool.InitPool(ballHitEffectPrefab, 10, effectParent);
    }
    private void OnEnable()
    {
        DodgeSmokeTrigger += DodgeSmokeStart;
    }
    private void OnDisable()
    {
        DodgeSmokeTrigger -= DodgeSmokeStart;
    }
    private void DodgeSmokeStart()
    {
        dodgeSmokeEffectCor = StartCoroutine(DodgeSmokeSpawn());
    }
    private IEnumerator DodgeSmokeSpawn()
    {
        GameObject smokeEffectPrafab = Instantiate(smokePrafab, effectParent);
        smokeEffectPrafab.transform.localPosition = this.transform.localPosition;

        if (playerInput.AxisX<0)
        {
            smokeEffectPrafab.transform.localScale = new Vector2(-1, 1);
        }

        yield return Yielders.GetWaitForSeconds(0.5f);
        Destroy(smokeEffectPrafab);
    }
}

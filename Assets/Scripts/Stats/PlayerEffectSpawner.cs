using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerEffectSpawner : MonoBehaviour
{
    private PlayerInput playerInput;

    public Transform effectParent;

    //閃避煙霧特效
    [HideInInspector] public Action DodgeSmokeTrigger;
    private Coroutine dodgeSmokeEffectCor;
    [SerializeField] private GameObject smokePrafab;
    //擊中特效
    //public UnityEvent SlashHitEffectTrigger;
    public GameObject slashHitEffectPrefab; //斬擊特效
    public ObjectPool<SlashHitEffect> SlashHitEffectPool;
    public GameObject ballHitEffectPrefab; //球形特效
    public ObjectPool<BallHitEffect> ballHitEffectPool;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }
    private void Start()
    {
        //斬擊擊中特效初始化
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

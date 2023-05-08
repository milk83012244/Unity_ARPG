using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectSpawner : MonoBehaviour
{
    private PlayerInput playerInput;

    [SerializeField] Transform effectParent;

    [HideInInspector] public Action DodgeSmokeTrigger;
    private Coroutine dodgeSmokeEffectCor;
    [SerializeField] private GameObject smokePrafab;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
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

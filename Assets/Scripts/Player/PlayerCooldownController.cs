using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCooldownController : MonoBehaviour
{
    [HideInInspector]public Action DodgeCooldownTrigger;
    public float dodgeCooldownTime;

    private PlayerInput playerInput;

    private Coroutine dodgeCor;

    private void OnEnable()
    {
        DodgeCooldownTrigger += DodgeCooldownStart;
    }
    private void OnDisable()
    {
        DodgeCooldownTrigger -= DodgeCooldownStart;
        StopAllCoroutines();
    }
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }
    private void DodgeCooldownStart()
    {
        dodgeCor = StartCoroutine(DodgeCooldown());
    }
    private IEnumerator DodgeCooldown()
    {
        playerInput.canDodge = false;
        yield return new WaitForSeconds(dodgeCooldownTime);
        playerInput.canDodge = true;
    }
}

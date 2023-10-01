using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 近戰角色攻擊方向標示
/// </summary>
public class MeleeAttackAim : MonoBehaviour
{
    private PlayerInput playerInput;
    private bool isEnable = true;

    private void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();
    }
    private void Start()
    {
        GameManager.Instance.onNormalGameStateChanged += OnGameStateChanged;
        GameManager.Instance.onBattleGameStateChanged += OnGameStateChanged;
        GameManager.Instance.onPasueGameStateChanged += OnGameStateChanged;
        GameManager.Instance.onGameOverGameStateChanged += OnGameStateChanged;
    }
    private void OnDestroy()
    {
        GameManager.Instance.onNormalGameStateChanged -= OnGameStateChanged;
        GameManager.Instance.onBattleGameStateChanged -= OnGameStateChanged;
        GameManager.Instance.onPasueGameStateChanged -= OnGameStateChanged;
        GameManager.Instance.onGameOverGameStateChanged -= OnGameStateChanged;
    }
    private void Update()
    {
        if (!isEnable)
        {
            return;
        }

        switch (playerInput.currentDirection)
        {
            case 1: //左
                this.transform.localEulerAngles = new Vector3(0, 0, 0);
                this.transform.localScale = new Vector3(1, this.transform.localScale.y, this.transform.localScale.z);
                this.transform.localPosition = new Vector3(-0.25f, 0.25f, this.transform.localPosition.z);
                break;
            case 2: //下
                this.transform.localEulerAngles = new Vector3(0, 0, 90f);
                this.transform.localScale = new Vector3(1, this.transform.localScale.y, this.transform.localScale.z);
                this.transform.localPosition = new Vector3(0, -0.15f, this.transform.localPosition.z);
                break;
            case 3: //右
                this.transform.localEulerAngles = new Vector3(0, 0, 0);
                this.transform.localScale = new Vector3(-1, this.transform.localScale.y, this.transform.localScale.z);
                this.transform.localPosition = new Vector3(0.25f, 0.25f, this.transform.localPosition.z);
                break;
            case 4: //上
                this.transform.localEulerAngles = new Vector3(0, 0, -90f);
                this.transform.localScale = new Vector3(1, this.transform.localScale.y, this.transform.localScale.z);
                this.transform.localPosition = new Vector3(0, 0.65f, this.transform.localPosition.z);
                break;
        }
    }
    private void OnGameStateChanged(GameState newGameState)
    {
        isEnable = newGameState == GameState.Normal || newGameState == GameState.Battle;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Mo反擊檢測
/// </summary>
public class MoCounterCheck : MonoBehaviour
{
    public static bool guardCheckActive;
    public static bool isGuardHit;

    public float checkDuration;
    public Transform MoPosition;

    private Collider2D currentCollsionTarget; //接收攻擊的目標
    private Collider2D hitCollider;

    [Header("觸發反擊瞬間的震動效果")]
    public float duration = 1f;      // 震動的持續時間
    public float strength = 1f;      // 震動的強度
    public int vibrato = 10;        // 震動的次數
    public float randomness = 90f;  // 震動的隨機度

    [Header("觸發反擊瞬間的減速效果")]
    public float slowdownFactor = 0.05f;
    public float slowdownDuration = 1f;
    private float originalTimeScale;

    private void Awake()
    {
        hitCollider = GetComponent<BoxCollider2D>();
    }
    public IEnumerator GuardCheck()
    {
        float currentTime = checkDuration;
        guardCheckActive = true;
        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            yield return null;
        }
        guardCheckActive = false;
    }
    public void ResetState()
    {
        isGuardHit = false;
        guardCheckActive = false;
        currentCollsionTarget = null;
    }
    public void StartSlowMotion(float slowdownFactor)
    {
        Debug.Log(string.Format("<color=#D569FF>{0}</color>", "遊戲開始減速"));
        originalTimeScale = Time.timeScale;
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    public void StopSlowMotion()
    {
        Time.timeScale = originalTimeScale;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        Debug.Log(string.Format("<color=#D569FF>{0}</color>", "遊戲結束減速"));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.Instance.CurrentGameState == GameState.Battle)
        {
            currentCollsionTarget = collision;

            if (collision != null && guardCheckActive)
            {
                if (collision.gameObject.CompareTag("EnemyAttack"))
                {
                    isGuardHit = true;
                    StartSlowMotion(slowdownFactor);
                    Invoke("StopSlowMotion", slowdownDuration);
                    StopAllCoroutines();
                    ObjectShaker.Instance.ObjectShake(MoPosition, duration, strength, strength, vibrato, randomness);
                    Debug.Log(string.Format("<color=#ff0000>{0}</color>", collision.name + "的攻擊觸發反擊"));
                }
            }
        }
    }
}

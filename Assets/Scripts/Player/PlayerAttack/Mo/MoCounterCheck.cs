using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Mo�����˴�
/// </summary>
public class MoCounterCheck : MonoBehaviour
{
    public static bool guardCheckActive;
    public static bool isGuardHit;

    public float checkDuration;
    public Transform MoPosition;

    private Collider2D currentCollsionTarget; //�����������ؼ�
    private Collider2D hitCollider;

    [Header("Ĳ�o�����������_�ʮĪG")]
    public float duration = 1f;      // �_�ʪ�����ɶ�
    public float strength = 1f;      // �_�ʪ��j��
    public int vibrato = 10;        // �_�ʪ�����
    public float randomness = 90f;  // �_�ʪ��H����

    [Header("Ĳ�o������������t�ĪG")]
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
        Debug.Log(string.Format("<color=#D569FF>{0}</color>", "�C���}�l��t"));
        originalTimeScale = Time.timeScale;
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    public void StopSlowMotion()
    {
        Time.timeScale = originalTimeScale;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        Debug.Log(string.Format("<color=#D569FF>{0}</color>", "�C��������t"));
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
                    Debug.Log(string.Format("<color=#ff0000>{0}</color>", collision.name + "������Ĳ�o����"));
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// Cinemachine相機震動
/// </summary>
public class CinemachineShake : MonoBehaviour
{
    private static CinemachineShake instance; //單例

    public static CinemachineShake GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("沒有UIManager實例");
            return instance;
        }
        else
        {
            return instance;
        }
    }

    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private float shakeTimer;
    private float shakeTimerTotal;
    private float startingIntensity;

    private void Awake()
    {
        instance = this;
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }
    private void Update()
    {
        if (shakeTimer > 0) //計時並停止震動
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer<=0f)
            {
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
                // cinemachineBasicMultiChannelPerlin.m_AmplitudeGain= Mathf.Lerp(startingIntensity, 0f, 1- (shakeTimer / shakeTimerTotal)); //緩慢停止
            }
        }

    }
    /// <summary>
    /// 震動攝影機 快速停止
    /// </summary>
    public void ShakeCamera(float intensity,float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity; //設定震幅
        shakeTimer = time;
    }

    /// <summary>
    /// 震動攝影機 緩慢停止
    /// </summary>
    public void ShakeCameraSlow(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity; //設定震幅
        startingIntensity = intensity;
        shakeTimerTotal = time;
        shakeTimer = time;
    }
}

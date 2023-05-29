using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// Cinemachine�۾��_��
/// </summary>
public class CinemachineShake : MonoBehaviour
{
    private static CinemachineShake instance; //���

    public static CinemachineShake GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("�S��UIManager���");
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
        if (shakeTimer > 0) //�p�ɨð���_��
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer<=0f)
            {
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
                // cinemachineBasicMultiChannelPerlin.m_AmplitudeGain= Mathf.Lerp(startingIntensity, 0f, 1- (shakeTimer / shakeTimerTotal)); //�w�C����
            }
        }

    }
    /// <summary>
    /// �_����v�� �ֳt����
    /// </summary>
    public void ShakeCamera(float intensity,float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity; //�]�w�_�T
        shakeTimer = time;
    }

    /// <summary>
    /// �_����v�� �w�C����
    /// </summary>
    public void ShakeCameraSlow(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity; //�]�w�_�T
        startingIntensity = intensity;
        shakeTimerTotal = time;
        shakeTimer = time;
    }
}

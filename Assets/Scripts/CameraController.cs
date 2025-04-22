using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    private CinemachineVirtualCamera _vcam;
    private float _shakeTimer;

    private void Awake()
    {
        _vcam = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeCamera(float time, float intensity)
    {
        CinemachineBasicMultiChannelPerlin perlin = _vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_AmplitudeGain = intensity;
        _shakeTimer = time;
    }

    private void Update()
    {
        if (_shakeTimer > 0)
        {
            _shakeTimer -= Time.deltaTime;
            if (_shakeTimer <= 0)
            {
                CinemachineBasicMultiChannelPerlin perlin = _vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                perlin.m_AmplitudeGain = 0f;
            }
        }
    }
}

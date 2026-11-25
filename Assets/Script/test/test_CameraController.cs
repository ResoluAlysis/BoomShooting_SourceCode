using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public struct _CameraShakeValue
{
    [Header("震动曲线")]public test_CameraController_ShakeData shake_Data;
    //[Header("相对概率")] public float shake_Probability;
}
[System.Serializable]
public class CameraShakeData
{
    [Header("震动名称")]public string ShakeName;
    //[Header("震动曲线列表")]public _CameraShakeValue[] shakeData;
    [Header("震动曲线")]public _CameraShakeValue shakeData;
    [Header("震动时间(范围内随机数，)")] public Vector2 shakeTime;
}

public class test_CameraController : MonoBehaviour
{
    public static test_CameraController instance;
    public CameraShakeData[] shakeData;
    public Camera mainCamera;
    public CinemachineVirtualCamera _VirtualCamera_常规;
    public CinemachineVirtualCamera _VirtualCamera_震动;
    public GameObject cameraFollowObj;

    /// <summary>
    /// 内部使用的字典（便于元素查找）
    /// </summary>
    private Dictionary<string, CameraShakeData> _shakeData = new Dictionary<string, CameraShakeData>();
    
    public void Awake()
    {
        if (instance == null) instance = this;

        StartCoroutine(Init());
    }

    private IEnumerator Init()
    {
        mainCamera = Camera.main;
        _Value_CameraFollowObj_Position = cameraFollowObj.transform.position;

        _VirtualCamera_常规.enabled = true;
        _VirtualCamera_震动.enabled = false;
        
        foreach (var cameraShakeData in shakeData)
        {
            yield return null;
            _shakeData.TryAdd(cameraShakeData.ShakeName, cameraShakeData);
        }
        Debug.Log("【系统_相机】：初始化完成...");
    }

    // ===== 当前震动状态（字段，协程实时读取） =====
    private string   _curShakeName;
    private float    _curShakeMaxTime;
    private float    _curShakeTime;
    private Vector2  _curShakeDir;
    private bool     _isShaking;

// ===== 基准位置（独立于输出位置） =====
    private Vector2  _baseFollowPos;

    public void Action_CameraShake_BulletHit(string shakeName, GameObject hitObj, Vector2 hitPos, Vector2 dir)
    {
        if (!_shakeData.ContainsKey(shakeName)) return;

        float newMaxTime = Random.Range(_shakeData[shakeName].shakeTime.x, _shakeData[shakeName].shakeTime.y);

        if (!_isShaking)
        {
            // 首次触发
            _curShakeName    = shakeName;
            _curShakeMaxTime = newMaxTime;
            _curShakeTime    = 0f;
            _curShakeDir     = dir;
            StartCoroutine(_CameraShake_BulletHit());
        }
        else
        {
            // 已在震动中：切换到新曲线，进度重置，让新曲线从头播
            _curShakeName    = shakeName;
            _curShakeMaxTime = newMaxTime;
            _curShakeTime    = 0f;
            _curShakeDir     = Vector2.Lerp(_curShakeDir, dir, 0.2f);
        }
    }

    private IEnumerator _CameraShake_BulletHit()
    {
        _VirtualCamera_常规.enabled = false;
        _VirtualCamera_震动.enabled = true;
        _isShaking = true;

        while (_curShakeTime < _curShakeMaxTime)
        {
            // 每次循环实时读最新曲线，切换物体时立刻生效
            var data = _shakeData[_curShakeName];
            float t = _curShakeTime / _curShakeMaxTime;
            float curveValue = data.shakeData.shake_Data.shakeCurve.Evaluate(t);

            Vector2 pos = _baseFollowPos + _curShakeDir * (0.5f * curveValue);
            Action_UpdateFollowObj_Pos(pos);

            yield return null;
            _curShakeTime += Time.deltaTime;
        }

        _isShaking = false;
        _VirtualCamera_常规.enabled = true;
        _VirtualCamera_震动.enabled = false;
        Action_UpdateFollowObj_Pos(_baseFollowPos);
        Debug.Log("【系统_相机】：震动结束...");
    }

    private void Action_UpdateFollowObj_Pos(Vector2 position) => _Value_CameraFollowObj_Position = position;

    public Vector2 _Value_CameraFollowObj_Position;
    private void FixedUpdate()
    {
        cameraFollowObj.transform.position = _Value_CameraFollowObj_Position;
    }
}

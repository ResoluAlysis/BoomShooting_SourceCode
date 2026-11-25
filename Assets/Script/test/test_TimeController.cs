using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_TimeController : MonoBehaviour
{
    public static test_TimeController instance;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        Time.timeScale = _Value_CurTimeSize = _Value_NextTimeSize = 1f;
    }
    
    public float _Value_CurTimeSize;
    private float _Value_NextTimeSize;
    private float m_时间缩放持续时间;
    private float m_Max时间缩放持续时间;
    public void Action_AddTimeSize(float targetValue, float time)
    {
        _Value_NextTimeSize = Mathf.Min(_Value_CurTimeSize, targetValue);
        m_Max时间缩放持续时间 = time;
        m_时间缩放持续时间 = 0;
    }
    
    private void Update()
    {
        if (m_时间缩放持续时间 < m_Max时间缩放持续时间)
        {
            m_时间缩放持续时间 += Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Lerp(_Value_NextTimeSize, 1, m_时间缩放持续时间  / m_Max时间缩放持续时间);
        }
        if (m_时间缩放持续时间 >= m_Max时间缩放持续时间)
        {
            m_时间缩放持续时间 = 0;
            _Value_NextTimeSize = 0;
            m_Max时间缩放持续时间 = 0;
            Time.timeScale = 1f;
        }
        _Value_CurTimeSize = Time.timeScale;
    }
}

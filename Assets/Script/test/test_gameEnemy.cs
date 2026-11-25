using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class test_gameEnemy : MonoBehaviour
{
    public GameObject Obj_gameEnemy_DirObj;
    public Rigidbody2D rb2D;

    [Header("被命中参数")] //震动名称，自身位置，命中点，命中方向
    [Header("被命中的事件")]public UnityEvent<string, Vector2, Vector2, Vector2> HitEvent;
    [Header("屏幕震动名称")]public string shakeName;
    
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }
    
    public void Hit(Vector2 hitPoint, Vector2 bulletDir, float damage)
    {
        //Debug.Log("敌人：被命中");
        // 冲量大小（可根据子弹速度模长和质量调整）
        Vector3 force = bulletDir.normalized * bulletDir.magnitude;
        // 在命中点施加力（自动产生扭矩）
        if(!test_UIController.instance.test_敌人钉死开关) rb2D.AddForceAtPosition(force, hitPoint, ForceMode2D.Impulse);

        HitEvent.Invoke(shakeName, transform.position, bulletDir, force);
        
        test_TimeController.instance.Action_AddTimeSize(0.6f, 0.2f);
    }

    public float gameData_移速 => _Value_CurSpeed.magnitude;
    public float gameData_转速 => Mathf.Abs(rb2D.angularVelocity);

    private void FixedUpdate()
    {
        Action_Move();

        if (test_UIController.instance.test_敌人钉死开关)
        {
            rb2D.velocity = Vector2.zero;
            rb2D.angularVelocity = 0;
        }
    }

    public Vector2 _Value_CurDir => transform.up;
    public Vector2 _Value_CurSpeed => rb2D.velocity;
    public Vector2 _Value_CurPosition => transform.position;
    public Vector2 _Value_NextSpeed;
    private void Action_Move()
    {
        if(!test_UIController.instance.test_敌人跟踪开关) return;
        
        var nextDir = test_gamePlayerController.instance._Value_CurPosition - _Value_CurPosition;
        rb2D.AddForce(nextDir.normalized * Mathf.Min((_Value_CurSpeed.magnitude * 0.05f), 50f), ForceMode2D.Impulse);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.E))
        {
            if (Input.GetKeyDown(KeyCode.Minus))
                transform.localScale = Vector2.Max(
                    new Vector2(0.5f, 0.5f),
                    new Vector2(transform.localScale.x - 1f, transform.localScale.y - 1f));
            if (Input.GetKeyDown(KeyCode.Equals))
                transform.localScale = Vector2.Min(
                new Vector2(10, 10),
                new Vector2(transform.localScale.x + 1, transform.localScale.y + 1));
            
            if(Input.GetKeyDown(KeyCode.Space))
            {
                if(m_CopyNum >= 10 || !m_IsMian) return;
                m_CopyNum++;
                var e = Instantiate(gameObject, transform.position, transform.rotation).GetComponent<test_gameEnemy>();
                e.m_IsMian = false;
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                if(!m_IsMian) Destroy(gameObject);
                else m_CopyNum = 1;
            }
        }
    }

    public bool m_IsMian = true;
    public int m_CopyNum = 1;
}

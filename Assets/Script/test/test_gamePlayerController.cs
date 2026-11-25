using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class test_gamePlayerController : MonoBehaviour
{
    public static test_gamePlayerController instance;
    public test_gamePlayer_Data data;

    public GameObject Obj_gamePlayer_DirObj;
    public GameObject Obj_gamePlayer_ShootObj;
    public GameObject Obj_gamePlayer_BodyObj;
    
    private Rigidbody2D rb2D;

    private void Awake()
    {
        if (instance == null) instance = this;
    }
    public void Start()
    {
        _camera = Camera.main;
        rb2D = GetComponent<Rigidbody2D>();
    }

    private Vector2 _Input_move;
    private bool _Input_ShootDown;
    private bool _Input_ShootUp;
    private void Update()
    {
        _Input_move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _Input_ShootDown = Input.GetMouseButton(0);
        
        Shoot();
        
        if(射速预设.Count > 0)
        {
            if (Input.mouseScrollDelta.y != 0 && !s)
            {
                s = true;
                Index射速 += 1 * (int)Mathf.Sign(Input.mouseScrollDelta.y);
                if (Index射速 >= 射速预设.Count - 1) Index射速 = 射速预设.Count - 1;
                if (Index射速 < 0) Index射速 = 0;
                Debug.Log("【角色控制器】：已切换射速为：" + 射速预设[Index射速]);
            }
            else
            {
                s = false;
            }
        }
    }
    private bool s = false;
    public List<float> 射速预设 =  new List<float>();
    public int Index射速 = -1;
    private float Shoot_IntervalTime = 0;
    private void Shoot()
    {
        if (_Input_ShootDown && Shoot_IntervalTime <= 0)
        {
            test_Bullet bullet = test_Pool_Bullet.instance.GetBlockObject();
            if (bullet != null) bullet.Init(Obj_gamePlayer_ShootObj.transform.position, _camera.ScreenToWorldPoint(Input.mousePosition) - transform.position, data.shoot_BulletSpeed, data.shoot_ButtleMass);

            if(Index射速 < 0) Shoot_IntervalTime = data.shoot_IntervalTime;
            else
            {
                Shoot_IntervalTime = 射速预设[Index射速];
            }
                
        }
        else if(Shoot_IntervalTime > 0) Shoot_IntervalTime -= Time.deltaTime;
    }

    
    #region 移动相关变量
    public enum gamePlayer_MoveState
    {
        待机,
        移动,
        冲刺,
        滑移
    }
    /// <summary>
    /// 当前移动状态
    /// </summary>
    public gamePlayer_MoveState _Value_CurState;
    /// <summary>
    /// 当前速度（速度总和）
    /// </summary>
    public Vector2 _Value_CurSpeed_All;
    private Vector2 _Value_NextSpeed_All;
    /// <summary>
    /// 当前移动速度
    /// </summary>
    public Vector2 _Value_CurSpeed_Move;
    private Vector2 _Value_NextSpeed_Move;

    public Vector2 _Value_CurSpeed => rb2D.velocity;
    public Vector2 _Value_CurPosition => transform.position;
    
    private Vector2 Idle_stop;
    private Camera _camera;

    #endregion
    private void FixedUpdate()
    {
        //if (_Input_move.magnitude != 0) //如果有移动输入
        //{
        //    Idle_stop = 0;
        //    
        //    _Value_NextSpeed_Move = _Value_CurSpeed_Move + _Input_move * data.move_Accelerate;
//
        //    if (Mathf.Abs(_Value_NextSpeed_Move.x) > data.move_MaxSpeed)
        //    {
        //        _Value_NextSpeed_Move.x = data.move_MaxSpeed;
        //    }
        //    if (Mathf.Abs(_Value_NextSpeed_Move.y) > data.move_MaxSpeed)
        //    {
        //        
        //    }
        //        
        //    _Value_CurSpeed_Move = new Vector2(
        //        Mathf.Min(Mathf.Abs(_Value_NextSpeed_Move.x), data.move_MaxSpeed) * _Input_move.x,
        //        Mathf.Min(Mathf.Abs(_Value_NextSpeed_Move.y), data.move_MaxSpeed) * _Input_move.y);
        //}
        //else
        //{
        //    _Value_CurSpeed_Move = Vector2.Lerp(_Value_CurSpeed_Move, Vector2.zero, Idle_stop);
        //    Idle_stop += Time.fixedDeltaTime * data.idle_Resistance;
        //    Idle_stop = Mathf.Clamp(Idle_stop, 0, 1);
        //}
//
        //_Value_NextSpeed_All = _Value_CurSpeed_Move;
        //rb2D.velocity = _Value_CurSpeed_All = _Value_NextSpeed_All;

        _Value_CurSpeed_All = rb2D.velocity;
        _Value_CurSpeed_Move = _Value_CurSpeed_All;

        _Value_CurState = _Value_CurSpeed_Move.magnitude != 0 ? gamePlayer_MoveState.移动 : gamePlayer_MoveState.待机;
        
        if (_Input_move.x != 0)
        {
            Idle_stop.x = 0;
            if (Mathf.Abs(_Value_CurSpeed_Move.x) <= data.move_MaxSpeed ||
                !Mathf.Approximately(Mathf.Sign(_Value_CurSpeed_Move.x), Mathf.Sign(_Input_move.x))) 
                _Value_NextSpeed_Move.x = _Value_CurSpeed_Move.x + _Input_move.x * data.move_Accelerate;
        }
        else
        {
            _Value_NextSpeed_Move.x = Mathf.Lerp(_Value_CurSpeed_Move.x, 0, Idle_stop.x);
            Idle_stop.x += Time.fixedDeltaTime * data.idle_Resistance;
            Idle_stop.x = Mathf.Clamp(Idle_stop.x, 0, 1);
        }
        if (_Input_move.y != 0)
        {
            Idle_stop.y = 0;
            if (Mathf.Abs(_Value_CurSpeed_Move.y) <= data.move_MaxSpeed ||
                !Mathf.Approximately(Mathf.Sign(_Value_CurSpeed_Move.y), Mathf.Sign(_Input_move.y)))
                _Value_NextSpeed_Move.y = _Value_CurSpeed_Move.y + _Input_move.y * data.move_Accelerate;
        }
        else
        {
            _Value_NextSpeed_Move.y = Mathf.Lerp(_Value_CurSpeed_Move.y, 0, Idle_stop.y);
            Idle_stop.y += Time.fixedDeltaTime * data.idle_Resistance;
            Idle_stop.y = Mathf.Clamp(Idle_stop.y, 0, 1);
        }

        _Value_NextSpeed_All = _Value_NextSpeed_Move;
        rb2D.velocity = _Value_NextSpeed_All;
        
        Update_DirObj(_Value_CurSpeed_Move, _Value_NextSpeed_All.magnitude);
        Update_PlayerDir(_camera.ScreenToWorldPoint(Input.mousePosition) - transform.position);
    }

    private void Update_DirObj(Vector2 dir, float speed)
    {
        if(Mathf.Abs(speed) < 0.1f) speed = 0f;
        Obj_gamePlayer_DirObj.transform.up = dir;
        Obj_gamePlayer_DirObj.transform.localScale = new Vector2(Obj_gamePlayer_DirObj.transform.localScale.x,
            data.obj_DirObj_MaxSize * Mathf.Abs(speed) / data.move_MaxSpeed);
    }

    private void Update_PlayerDir(Vector2 dir)
    {
        Obj_gamePlayer_BodyObj.transform.up = dir;
    }
    
    [Header("被命中的事件")]public UnityEvent<Vector2, Vector2> HitEvent;
    [Header("子弹命中的事件")]public UnityEvent<bool, GameObject, Vector2, Vector2> bulletHitEvent;
    public void Action_BulletHit(bool m命中有效目标, GameObject hitObj, Vector2 hitPos, Vector2 hitDir)
    {
        if (m命中有效目标)
        {
            test_gameEnemy enemy = hitObj.GetComponent<test_gameEnemy>();
            bulletHitEvent.Invoke(true, hitObj, hitPos, hitDir);
        }
        else
        {
            if(data.game_ComboReward_未击中任何有效目标打断连击) bulletHitEvent.Invoke(false, hitObj, hitPos, hitDir);
        }
    }
    
    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Enemy")) HitEvent.Invoke(transform.position, other.transform.position);
    }
    
}

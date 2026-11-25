using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class test_UIController : MonoBehaviour
{
    public static test_UIController instance;
    
    public GameObject 锁定Bar_Obj;

    public Transform 锁定Bar生成锚点;

    public test_锁定Bar m_锁定Bar;

    public Text ui_总分Text;
    public int score;

    public Text ui_倒计时;

    
    public void Awake()
    {
        if (instance == null) instance = this;
    }

    public void Start()
    {
        score = 0;
        game_time = 60f;
        ui_总分Text.text = score.ToString();
    }

    public void Action_锁定Bar_BulletHit(bool m命中有效目标, GameObject hitObj, Vector2 hitPos, Vector2 dir)
    {
        //Debug.Log("【UI系统】：有物体被子弹命中...");

        if(game_time <= 0f) return;
        
        if (m命中有效目标)
        {
            
            score++;
            ui_总分Text.text = score.ToString();
            
            if(m_锁定Bar == null) m_锁定Bar = Instantiate(锁定Bar_Obj, 锁定Bar生成锚点).GetComponent<test_锁定Bar>();
            
            m_锁定Bar.Hit(hitObj);

            if (hitObj.GetComponent<test_gameEnemy>() == m_锁定Bar.m_锁定目标)
            {
                Debug.Log("【UI系统】：命中了锁定目标...");
                test_命中过锁定目标 = true;
            }
            else if(test_gamePlayerController.instance.data.game_ComboReward_优先击中其他目标打断连击 && !test_命中过锁定目标)
            {
                Debug.Log("【UI系统】：未优先命中锁定目标...");
                if(!test_gamePlayerController.instance.data.game_ComboReward_击中任何有效目标不打断连击) m_锁定Bar.Action_OnHide();
            }
            else if(test_gamePlayerController.instance.data.game_ComboReward_击中任何有效目标不打断连击)
            {
                Debug.Log("【UI系统】：命中了有效目标...");
                test_命中过锁定目标 = true;
            }
        }
        else
        {
            if(m_锁定Bar != null && !test_命中过锁定目标 ) m_锁定Bar.Action_OnHide();
            
            Debug.Log("【UI系统】：命中了非有效目标...");
            
            test_命中过锁定目标 = false;
        }

    }
    private bool test_命中过锁定目标 = false;
    
    public void Action_锁定Bar_结算分数(int score)
    {
        if(game_time <= 0f) return;
        this.score += score;
        ui_总分Text.text = this.score.ToString();
    }

    public void Action_GamePlayerHit(Vector2 player, Vector2 enemy)
    {
        if(test_玩家无敌) return;
        Debug.Log("【系统】：玩家GG...");
        game_time = 0;
        Update_Time("游戏结束...请按住‘左CTRL + R’复原...");
    }
    
    public void Update_Time(string s)
    {
        ui_倒计时.text = s;
    }

    public float game_time;
    private void Update()
    {
        if(test_计时开关)
        {
            if (game_time <= 0f)
            {
                game_time = 0f;
                //Time.timeScale = 0.05f;
                Update_Time("游戏结束...请按住‘左CTRL + R’复原...");
                Debug.Log("【系统】：游戏结束...请按住‘左CTRL + R’复原...");
            }
            else
            {
                game_time -= Time.deltaTime;
                Update_Time($"{game_time:.0000}");
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab)) obj_指令提示栏.gameObject.SetActive(!obj_指令提示栏.gameObject.activeInHierarchy);
        
        if (!Input.GetKey(KeyCode.LeftControl)) return;
        
        if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
        if (Input.GetKeyDown(KeyCode.T)) test_计时开关 = !test_计时开关;
        
        if (Input.GetKey(KeyCode.E))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                test_敌人跟踪开关 = !test_敌人跟踪开关;
                if(test_敌人跟踪开关) test_敌人钉死开关 = false;
            }
                
            if(Input.GetKeyDown(KeyCode.S))
            {
                test_敌人钉死开关 = !test_敌人钉死开关;
                if(test_敌人钉死开关) test_敌人跟踪开关 = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            foreach (var obj in obj_玩家位置指示) obj.SetActive(false);

            obj_玩家位置指示_Index++;
            if (obj_玩家位置指示_Index >= obj_玩家位置指示.Length) obj_玩家位置指示_Index = 0;
            
            obj_玩家位置指示[obj_玩家位置指示_Index].gameObject.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            foreach (var obj in obj_准星样式) obj.SetActive(false);

            obj_准星样式_Index++;
            if (obj_准星样式_Index >= obj_准星样式.Length) obj_准星样式_Index = 0;
            
            obj_准星样式[obj_准星样式_Index].gameObject.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            test_玩家无敌 = !test_玩家无敌;
            obj_玩家无敌提示.SetActive(test_玩家无敌);
            Physics2D.IgnoreLayerCollision(
                LayerMask.NameToLayer("Game_Enemy"),
                LayerMask.NameToLayer("Game_Player"),
                test_玩家无敌);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if(!Input.GetKeyDown(KeyCode.Q)) return;
#if UNITY_EDITOR
            EditorApplication.isPlaying = false; // 停止播放
#else
            Application.Quit(0);
#endif
        }
    }
    public GameObject obj_指令提示栏;
    
    public GameObject[] obj_玩家位置指示;
    private int obj_玩家位置指示_Index = 0;
    public GameObject[] obj_准星样式;
    private int obj_准星样式_Index = 0;
    public GameObject obj_玩家无敌提示;
    
    public bool test_计时开关 = false;
    public bool test_敌人跟踪开关 = true;
    public bool test_敌人钉死开关 = false;
    public bool test_玩家无敌 = false;
}

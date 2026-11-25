using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class test_锁定Bar :  test_UIBass
{
        public test_gameEnemy m_锁定目标;
        
        public Text 总分Text;
        public Text 命中次数Text;
        public Text 总倍率Text;
        public Text 移速倍率Text;
        public Text 转速倍率Text;
        public Image 锁定Image;

        public bool m_初始化;
        public bool m_记录中;

        public int gameData_总分;
        public int gameData_命中次数;
    
        public int gameData_倍率;
        public int gameData_移速倍率;
        public int gameData_转速倍率;
        public float gameData_锁定Time;
        
        public void Hit(GameObject hitObj)
        {
                var now目标 = hitObj.GetComponent<test_gameEnemy>();
                
                if (m_初始化)
                {
                        if(m_锁定目标 != now目标) return;
                        Action_OnUpdate();
                }
                else
                {
                        m_锁定目标 = now目标;
                        m_初始化 = true;
                        m_记录中 = true;
                        gameData_锁定Time = test_gamePlayerController.instance.data.game_ComboReward_连击持续时长;
                        UIUpdate_Image(1f);
                        Action_OnShow();
                        StartCoroutine(Update锁定());
                }
                transform.position = m_锁定目标.transform.position;

                m_锁定Time = Mathf.Min(m_锁定Time + test_gamePlayerController.instance.data.game_ComboReward_命中连击目标恢复持续时长, gameData_锁定Time);
                gameData_命中次数++;
                gameData_移速倍率 = Mathf.Max(1, (int)(m_锁定目标.gameData_移速 / 10 - 1));
                gameData_转速倍率 = Mathf.Max(1, (int)(m_锁定目标.gameData_转速 / 500));
                //gameData_倍率 = (int)((float)gameData_移速倍率/2 + (float)gameData_转速倍率/2);
                gameData_倍率 = gameData_移速倍率 + gameData_转速倍率 - 1;
                gameData_总分 = gameData_命中次数 * gameData_倍率;
                UIUpdate_Data();
        }

        private float m_锁定Time = 0;
        private IEnumerator Update锁定()
        {
                yield return null;
                m_锁定Time = gameData_锁定Time;
                while(m_锁定Time >= 0)
                {
                        UIUpdate_Image(m_锁定Time / gameData_锁定Time);
                        m_锁定Time -= Time.deltaTime;
                        yield return null;
                }

                test_UIController.instance.Action_锁定Bar_结算分数(gameData_总分);
                
                UIUpdate_Image(0);
        }

        private void UIUpdate_Data()
        {
                this.总分Text.text = gameData_总分.ToString();
                this.命中次数Text.text = gameData_命中次数.ToString();
                this.总倍率Text.text = gameData_倍率.ToString();
                this.移速倍率Text.text = gameData_移速倍率.ToString();
                this.转速倍率Text.text = gameData_转速倍率.ToString();
        }

        public void UIUpdate_Image(float v)
        {
                if (v <= 0f)
                {
                        v = 0;
                        Action_OnHide();
                }
                锁定Image.fillAmount = v;
        }
        
        public override void Action_OnUpdate()
        {
                gameData_总分++;
                
                base.Action_OnUpdate();
        }

        public override void Action_OnShow()
        {
                base.Action_OnShow();
        }

        public override void Action_OnHide()
        {
                m_记录中 = false;
                m_初始化 = false;
                
                gameData_总分 = 0;
                gameData_命中次数 = 0;
                gameData_倍率 = 0;
                gameData_移速倍率 = 0;
                gameData_转速倍率 = 0;
                StopAllCoroutines();
                base.Action_OnHide();
        }

        private void FixedUpdate()
        {
                if(m_初始化) transform.position = m_锁定目标.transform.position;
        }
        
        public void OnSetHide()
        {
                gameObject.SetActive(false);
        }
}
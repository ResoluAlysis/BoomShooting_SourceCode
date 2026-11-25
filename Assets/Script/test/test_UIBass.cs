using System;
using UnityEngine;
using UnityEngine.Events;

public class test_UIBass : MonoBehaviour
{
        public UnityEvent OnShow;
        public UnityEvent OnHide;

        /// <summary>
        /// 首次显示UI的行为
        /// </summary>
        public virtual void Action_OnShow()
        {
                OnShow.Invoke();
                //gameObject.SetActive(true);
                
                //Debug.Log("Action_OnShow");
        }

        /// <summary>
        /// 首次隐藏UI的行为
        /// </summary>
        public virtual void Action_OnHide()
        {
                OnHide.Invoke();
                //gameObject.SetActive(false);
                
                //Debug.Log("Action_OnHide");
        }
        
        /// <summary>
        /// 刷新UI的事件
        /// </summary>
        public UnityEvent OnUpdate;

        public virtual void Action_OnUpdate()
        {
                OnUpdate.Invoke();
                
                //Debug.Log("Action_OnUpdate");
        }

        
}

// public class test_UIBass<T1> : test_UIBass
// { 
//         public virtual void Action_OnUpdate(T1 t1){OnUpdate.Invoke();}
// }
// public class test_UIBass<T1, T2> : test_UIBass
// {
//         public virtual void Action_OnUpdate(T1 t1){OnUpdate.Invoke();}
//         public virtual void Action_OnUpdate(T1 t1, T2 t2){OnUpdate.Invoke();}
// }
//
// public class test_UIBass<T1, T2, T3> : test_UIBass
// {
//         public virtual void Action_OnUpdate(T1 t1){OnUpdate.Invoke();}
//         public virtual void Action_OnUpdate(T1 t1, T2 t2){OnUpdate.Invoke();}
//         public virtual void Action_OnUpdate(T1 t1, T2 t2, T3 t3){OnUpdate.Invoke();}
// }
//
// public class test_UIBass<T1, T2, T3, T4> : test_UIBass
// {
//         public virtual void Action_OnUpdate(T1 t1){OnUpdate.Invoke();}
//         public virtual void Action_OnUpdate(T1 t1, T2 t2){OnUpdate.Invoke();}
//         public virtual void Action_OnUpdate(T1 t1, T2 t2, T3 t3){OnUpdate.Invoke();}
//         public virtual void Action_OnUpdate(T1 t1, T2 t2, T3 t3, T4 t4){OnUpdate.Invoke();}
// }
//
// public class test_UIBass<T1, T2, T3, T4, T5> : test_UIBass
// {
//         public virtual void Action_OnUpdate(T1 t1){OnUpdate.Invoke();}
//         public virtual void Action_OnUpdate(T1 t1, T2 t2){OnUpdate.Invoke();}
//         public virtual void Action_OnUpdate(T1 t1, T2 t2, T3 t3){OnUpdate.Invoke();}
//         public virtual void Action_OnUpdate(T1 t1, T2 t2, T3 t3, T4 t4){OnUpdate.Invoke();}
//         public virtual void Action_OnUpdate(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5){OnUpdate.Invoke();}
// }
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class effectPrefab
{
    public string Name;
    public GameObject Obj;
    public float time;
}

public struct effectObj
{
    public GameObject Obj;
    public float MaxTime;
    public float CurTime;
}
public class test_Pool_Effect : MonoBehaviour
{
    public static test_Pool_Effect instance;
    public effectPrefab[] effectObj;
    public int effectCountInitial = 5;    //初始数量
    public int effectCountMax = 50;        //上限
    public Dictionary<string,int> effectNum = new Dictionary<string, int>();        //数量记录

    private Dictionary<string, List<effectObj>> effectPool = new Dictionary<string, List<effectObj>>();    //存储砖块的列表
    
    private void Awake()
    {
        instance = this;                  //实例化
    }
 
    private void Start()
    {
        foreach (var Obj in effectObj)
        {
            effectNum.TryAdd(Obj.Name, 0);
            
            if(Obj.Obj == null) return;
            for (var i = 0; i < effectCountInitial; i++) 
            {
                var obj = Instantiate(Obj.Obj, this.transform);
                if(!effectPool.ContainsKey(Obj.Name)) effectPool.Add(Obj.Name, new List<effectObj>(){new effectObj(){Obj = obj,  MaxTime = Obj.time, CurTime = Obj.time}});
                else effectPool[Obj.Name].Add(new effectObj(){Obj = obj,  MaxTime = Obj.time, CurTime = Obj.time});
                obj.SetActive(false);         //设置Active为false
            }
        }
        
        if(effectObj.Length > 0) StartCoroutine(UpdateEffect());
    }
 
    public void GetEffectObject(string name, Vector2 pos, Vector2 dir)
    {
        if(!effectPool.ContainsKey(name)) return;
        
        Debug.Log("【效果池系统】：尝试获取效果：" + name);
        
        var list = effectPool[name];
        for (int i = 0; i < list.Count; i++)
        {
            if (!list[i].Obj.activeInHierarchy)
            {
                list[i] = SetEffect(list[i], pos, dir);  // ← 关键：写回
                effectNum[name]++;
                return;
            }
        }

        if (list.Count >= effectCountMax) return;

        var obj = Instantiate(list[0].Obj, this.transform);
        var newEffect = SetEffect(
            new effectObj() { Obj = obj, MaxTime = list[0].MaxTime, CurTime = 0 },
            pos, dir);
        list.Add(newEffect);
        effectNum[name]++;
    }

    private effectObj SetEffect(effectObj obj, Vector2 pos, Vector2 dir)
    {
        obj = new effectObj()
        {
            Obj = obj.Obj,
            CurTime = 0,
            MaxTime = obj.MaxTime,
        };
        obj.Obj.transform.position = pos;
        obj.Obj.transform.up = dir;
        obj.Obj.SetActive(true);
        return obj;
    }

    public IEnumerator UpdateEffect()
    {
        while (true)
        {
            foreach (var effectObjs in effectPool)
            {
                for (int i = 0; i < effectObjs.Value.Count; i++)
                {
                    var e = effectObjs.Value[i];
                    if (!e.Obj.activeInHierarchy) continue;

                    if (e.CurTime < e.MaxTime)
                    {
                        effectObjs.Value[i] = new effectObj()
                        {
                            Obj = e.Obj,
                            CurTime = e.CurTime + Time.deltaTime,
                            MaxTime = e.MaxTime
                        };
                    }
                    else
                    {
                        effectObjs.Value[i] = new effectObj()
                        {
                            Obj = e.Obj,
                            CurTime = 0f,
                            MaxTime = e.MaxTime
                        };
                        if (effectNum[effectObjs.Key] > 0) effectNum[effectObjs.Key]--;
                        e.Obj.SetActive(false);
                    }
                }
            }

            yield return null; // 整轮只等一帧，所有对象时间步调一致
        }
    }
}

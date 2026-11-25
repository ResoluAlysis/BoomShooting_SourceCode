using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_Pool_Bullet : MonoBehaviour
{
    public static test_Pool_Bullet instance;
    public GameObject bulletObj;
    public int bulletCountInitial = 20;    //初始数量
    public int bulletCountMax = 500;        //上限
    public int bulletNum = 0;        //数量记录
    
    private List<test_Bullet> bulletPool = new List<test_Bullet>();     //存储砖块的列表
 
    private void Awake()
    {
        instance = this;                  //实例化
    }
 
    private void Start()
    {
        for (int i = 0; i < bulletCountInitial; i++) 
        {
            GameObject obj = Instantiate(bulletObj, this.transform);
            obj.SetActive(false);         //设置Active为false
            bulletPool.Add(obj.GetComponent<test_Bullet>());
        }
        Debug.Log(bulletPool.Count);
    }
 
    public test_Bullet GetBlockObject()
    {
        // test_Bullet bb = Instantiate(bulletObj).GetComponent<test_Bullet>();
        // bulletPool.Add(bb);
        // bulletNum++;
        // return bb;
        
        foreach (var b in bulletPool)
        {
            if (!b.gameObject.activeInHierarchy)
            {
                bulletNum++;
                return b;
            }
        }

        if(bulletPool.Count < bulletCountMax)
        {
            test_Bullet bullet = Instantiate(bulletObj).GetComponent<test_Bullet>();
            bulletPool.Add(bullet);
            bulletNum++;
            return bullet;
        }
        else
        {
            return null;
        }
    }
 
    public void DestroyBulletObject(GameObject bullet)
    {
        if(bulletNum > 0) bulletNum--;
        bullet.SetActive(false);
    }
}

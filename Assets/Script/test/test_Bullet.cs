using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_Bullet : MonoBehaviour
{
    
    RaycastHit hitInfo;
    
    Vector2 endPos;

    public LayerMask mask;
    
    public void Init(Vector2 pos, Vector2 dir, float speed, float mass)
    {
        HitList.Clear();
        transform.position = pos;
        transform.up = dir;
        bulletSpeed = speed;
        bulletMass = mass;
        _Hit = false;
        _Init = true;
        gameObject.SetActive(true);
    }

    public float bulletSpeed;
    public float bulletMass;
    public bool _Init = false;
    public bool _Hit = false;
    private List<GameObject> HitList = new List<GameObject>();
    void Update()
    {
        if(!_Init) return;
        if(_Hit) return;
        
        endPos = transform.position + transform.up * (bulletSpeed * Time.deltaTime);
        float distance = Vector3.Distance(transform.position, endPos);
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, transform.up, distance, mask);
        if (hit.Length > 0)
        {
            foreach (var hitObj in hit)
            {
                if (hitObj.collider.gameObject.layer == LayerMask.NameToLayer("Scene_Wall"))
                {
                    transform.position = hitObj.point;
                    _Init = false;
                    _Hit = true;
                    Invoke("DestroyBullet", 1f);
                    test_Pool_Effect.instance.GetEffectObject("Effect_BulletHit_Wall", hitObj.point, transform.up);
                    test_CameraController.instance.Action_CameraShake_BulletHit("test_BulletHit_Wall", this.gameObject, hitObj.point, transform.up);
                    test_gamePlayerController.instance.Action_BulletHit(false, hitObj.collider.gameObject, hitObj.point, transform.up * (bulletSpeed * bulletMass));
                    return;
                }
                else
                {
                    if(HitList.Contains(hitObj.collider.gameObject)) continue;
                    HitList.Add(hitObj.collider.gameObject);
                    
                    // Debug.Log("碰撞到了物体--------------------" +
                    //           "\n物体名字:" + hitObj.collider.gameObject.name +
                    //           "\n碰撞到的点: " + hitObj.point +
                    //           "\n法线信息: " + hitObj.normal +
                    //           "\n碰撞对象的位置: " + hitObj.transform.position +
                    //           "\n碰撞对象的距离: " + hitObj.distance);
                    
                    if (hitObj.collider.gameObject.layer == LayerMask.NameToLayer("Game_Enemy"))
                    {
                        test_gameEnemy enemy = hitObj.collider.GetComponent<test_gameEnemy>();
                        if (enemy != null)
                        {
                            Debug.Log("命中敌人");
                            enemy.Hit(hitObj.point, transform.up * (bulletSpeed * bulletMass), 1);
                            
                            test_Pool_Effect.instance.GetEffectObject("Effect_BulletHit_0", hitObj.point, transform.up);
                            test_CameraController.instance.Action_CameraShake_BulletHit("test_BulletHit_000", this.gameObject, hitObj.point, transform.up);
                            test_gamePlayerController.instance.Action_BulletHit( true, hitObj.collider.gameObject, hitObj.point, transform.up * (bulletSpeed * bulletMass));
                            
                            var insideOrigin = hitObj.point + (Vector2)transform.up.normalized * 10f;
                            var hitExit = Physics2D.RaycastAll(insideOrigin, -transform.up, 100f, 1 << LayerMask.NameToLayer("Game_Enemy"));
                            foreach (var h in hitExit)
                            {
                                if(h.collider != hitObj.collider) return;
                                Debug.Log("/////进入点：" + hitObj.point + " /////射出点：" + h.point);
                                Debug.DrawRay(insideOrigin, (Vector2)transform.up * 100f, Color.red, 2f);
                                test_Pool_Effect.instance.GetEffectObject("Effect_BulletHit_1", h.point, transform.up);
                            }
                        }
                    }
                }
            }
        }
        
        transform.position = endPos;
    }

    void DestroyBullet()
    {
        test_Pool_Bullet.instance.DestroyBulletObject(gameObject);
    }
}

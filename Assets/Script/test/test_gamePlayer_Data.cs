using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "gamePlayer_Data", menuName = "test/gameData", order = 1)]
public class test_gamePlayer_Data : ScriptableObject
{
    [Header("玩家数据")]
    [Header("移动数据  //////////////////////////////")]
    
    [Header("平移最高速度")] public float move_MaxSpeed = 12f;
    [Header("平移加速度")] public float move_Accelerate = 4f;

    [Header("待机空气阻力")] public float idle_Resistance = 1f;

    [Header("指向道具大小")] public float obj_DirObj_MaxSize = 5f;

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    [Header("射击数据  //////////////////////////////")]
    
    [Header("子弹速度")] public float shoot_BulletSpeed = 100f;

    [Header("子弹质量")] public float shoot_ButtleMass = 0.01f;

    [Header("射击间隔")] public float shoot_IntervalTime = 1.4f;
    
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    [Header("游戏数据  //////////////////////////////")]
    
    [Header("连击奖励？")] public bool game_ComboReward_Switch = true;
    [Header("连击-连击持续时长（单位秒，小于0为永久持有直到被打断）"), Range(-1, 999)] public float game_ComboReward_连击持续时长 = 5f;
    [Header("连击-命中连击目标恢复持续时长（单位秒, 无法超过连击持续时长）"), Range(0, 999)] public float game_ComboReward_命中连击目标恢复持续时长 = 0.2f;
    
    [Header("连击-未击中任何有效目标打断连击？")] public bool game_ComboReward_未击中任何有效目标打断连击 = true;
    [Header("连击-‘优先’击中其他目标打断连击？")] public bool game_ComboReward_优先击中其他目标打断连击 = false;
    [Header("连击-击中任何有效目标不打断连击？"), Header("（注意激活该选项会禁用上一条）")] public bool game_ComboReward_击中任何有效目标不打断连击 = false;
}

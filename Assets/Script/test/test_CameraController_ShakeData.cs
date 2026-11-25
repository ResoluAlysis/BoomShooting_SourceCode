using UnityEngine;

[CreateAssetMenu(fileName = "CameraController_ShakeData", menuName = "test/CameraData", order = 1)]
public class test_CameraController_ShakeData : ScriptableObject
{ 
    [Header("震动曲线（纵轴为偏移程度，横轴为震动时间）")] public AnimationCurve shakeCurve =  new AnimationCurve(new Keyframe(0, -1), new Keyframe(1, 1));
}
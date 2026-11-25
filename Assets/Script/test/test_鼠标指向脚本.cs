using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_鼠标指向脚本 : MonoBehaviour
{
    public GameObject _线段;
    public Transform _起点;
    public RectTransform _UI;
    private Camera _camera;
    private RectTransform _rect;
    private void Start()
    {
        _camera = Camera.main;
        _rect = GetComponent<RectTransform>();
    }

    public void FixedUpdate()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_UI, Input.mousePosition, _camera, out Vector2 position);
        transform.localPosition = position;
        
        Vector2 pos = _camera.WorldToScreenPoint(_起点.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_UI, pos, _camera, out Vector2 localPosition);
        Vector2 dir = _rect.anchoredPosition - localPosition;
        _线段.transform.up = dir.normalized;
        _线段.transform.localScale = new Vector2(1f, Mathf.Abs(dir.magnitude)/100);
        
        //Debug.Log($"准星位置：{_rect.anchoredPosition}, 玩家位置：{localPosition}");
    }
}

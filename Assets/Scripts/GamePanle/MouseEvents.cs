using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseEvents : MonoBehaviour
{
    private RaycastHit hitInfo;
    [SerializeField] private Texture2D 箭头鼠标;
    [SerializeField] private Texture2D 准星鼠标;

    private void Update()
    {
    }

    // public void 鼠标控制()
    // {
    //     if (EventSystem.current.IsPointerOverGameObject())
    //     {
    //         Cursor.SetCursor(箭头鼠标, new Vector2(0, 0), CursorMode.Auto);
    //     }
    //     else
    //     {
    //         Cursor.SetCursor(准星鼠标, new Vector2(32, 32), CursorMode.Auto);
    //     }
    // }
}
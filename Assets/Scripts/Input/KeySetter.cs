using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeySetter : MonoBehaviour
{
    public static InputControler input;

    public static event Action 绑定完成;
    public static event Action 绑定取消;
    public static event Action<InputAction, int> 绑定开始;
    private void Awake()
    {
        if (input == null)
            input = new InputControler();
    }

    public static void 开启重绑(string action名字,int 绑定下标, TMP_Text 按钮文字)
    {
        InputAction action = input.asset.FindAction(action名字);
        if (action == null || action.bindings.Count <= 绑定下标)
        {
            Debug.Log("找不到action或绑定按键捏");
            return;
        }

        执行重绑(action, 绑定下标, 按钮文字);
    }

    private static void 执行重绑(InputAction 重绑action, int 绑定下标, TMP_Text 按钮文字)
    {
        if (重绑action == null) return;

        按钮文字.text = $"请按键";

        重绑action.Disable();

        var 重绑 = 重绑action.PerformInteractiveRebinding(绑定下标);
        重绑.OnComplete(operation =>
        {
            重绑action.Enable();
            operation.Dispose();

            保存绑定覆盖(重绑action);
            绑定完成?.Invoke();
        });

        重绑.OnCancel(operation =>
        {
            重绑action.Enable();
            operation.Dispose();
            绑定取消?.Invoke();
        });

        重绑.WithCancelingThrough("<Keyboard>/Backspace");

        绑定开始?.Invoke(重绑action, 绑定下标);
        重绑.Start(); //真正启动重绑进程
    }

    public static string 获取绑定名称(string action名字, int 绑定下标)
    {
        if(input == null)
            input = new InputControler();

        InputAction action = input.asset.FindAction(action名字);
        return action.GetBindingDisplayString(绑定下标);
    }

    private static void 保存绑定覆盖(InputAction action)
    {
        for (int i = 0; i < action.bindings.Count; i++)
        {
            PlayerPrefs.SetString(action.actionMap + action.name + i, action.bindings[i].overridePath);
        }
    }

    public static void 加载绑定覆盖(string actionName)
    {
        if(input == null)
            input=new InputControler();

        InputAction action = input.asset.FindAction(actionName);

        for (int i = 0; i < action.bindings.Count; i++)
        {
            if (!string.IsNullOrEmpty(PlayerPrefs.GetString(action.actionMap + action.name + i)))
                action.ApplyBindingOverride(i, PlayerPrefs.GetString(action.actionMap + action.name + i));
        }
    }

    public static void 重置绑定(string actionName)
    {
        InputAction action = input.asset.FindAction(actionName);

        if (action == null)
        {
            Debug.Log("妹有捏");
            return;
        }

        for (int i = 0; i < action.bindings.Count; i++)
        {
            if (!string.IsNullOrEmpty(PlayerPrefs.GetString(action.actionMap + action.name + i)))
                PlayerPrefs.DeleteKey(action.actionMap + action.name + i);
        }

        action.RemoveAllBindingOverrides();
    }
}

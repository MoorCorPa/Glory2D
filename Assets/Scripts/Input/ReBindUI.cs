using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Windows;

public class ReBindUI : MonoBehaviour
{
    [SerializeField]
    private InputActionReference reference;

    private int 下标;

    [SerializeField]
    private Button resetButton;

    private void OnEnable()
    {
        resetButton.onClick.AddListener(() => 重置绑定());

        if (reference != null)
        {
            KeySetter.加载绑定覆盖("Movement");
            更新UI();
        }

        KeySetter.绑定完成 += 更新UI;
        KeySetter.绑定取消 += 更新UI;
    }

    private void OnDisable()
    {
        KeySetter.绑定完成 += 更新UI;
        KeySetter.绑定取消 += 更新UI;
    }

    private void OnValidate()
    {
        if (reference != null)
        {
            更新UI();
        }
    }

    public void 重绑(Button 按钮)
    {
        var 按钮文字 = 按钮.GetComponentInChildren<TMP_Text>();
        switch (按钮.name)
        {
            case "跳跃设置":
                下标 = 1;
                break;
            case "左设置":
                下标 = 2;
                break;
            case "右设置":
                下标 = 3;
                break;
            default:
                break;
        }
        KeySetter.开启重绑("Movement", 下标, 按钮文字);
    }

    private void 重置绑定()
    {
        KeySetter.重置绑定("Movement");
        更新UI();
    }


    private void 更新UI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetChild(1).GetComponentInChildren<TMP_Text>().text = Application.isPlaying? KeySetter.获取绑定名称("Movement", i + 1):reference.action.GetBindingDisplayString(i + 1);
        }
    }
}

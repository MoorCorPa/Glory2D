using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Windows;

public class ReBindUI : MonoBehaviour
{
    [SerializeField]
    private InputActionReference moveReference;
    [SerializeField]
    private InputActionReference fireReference;
    [SerializeField]
    private InputActionReference reloadReference;
    [SerializeField]
    private InputActionReference chmodReference;
    [SerializeField]
    private InputActionReference jumpReference;
    [SerializeField]
    private InputActionReference downReference;

    [SerializeField]
    private Button resetButton;

    private void OnEnable()
    {
        加载绑定();
    }

    void Update(){
       if(KeySetter.可以更新){
            更新UI();
            KeySetter.可以更新 = false;
       } 
    }

    public void 加载绑定()
    {

        resetButton.onClick.AddListener(() => 重置绑定());

        if (moveReference != null)
        {
            KeySetter.加载绑定覆盖("Movement");
            KeySetter.加载绑定覆盖("Fire");
            KeySetter.加载绑定覆盖("Reload");
            KeySetter.加载绑定覆盖("Chmod");
            KeySetter.加载绑定覆盖("Jump");
            KeySetter.加载绑定覆盖("Down");
            更新UI();
        }

        //KeySetter.绑定完成 += 更新UI;
        //KeySetter.绑定取消 += 更新UI;
    }

    private void OnDisable()
    {
        //KeySetter.绑定完成 -= 更新UI;
        //KeySetter.绑定取消 -= 更新UI;
    }

    private void OnValidate()
    {
        if (moveReference != null)
        {
            更新UI();
        }
    }

    public void 重绑(Button 按钮)
    {
        var 按钮文字 = 按钮.GetComponentInChildren<TMP_Text>();
        string actionName = "Movement";
        int 下标 = 0;

        switch (按钮.name)
        {
            case "左设置":
                下标 = 1;
                break;
            case "右设置":
                下标 = 2;
                break;
            case "跳跃设置":
                actionName = "Jump";
                break;
            case "射击设置":
                actionName = "Fire";
                break;
            case "换弹设置":
                actionName = "Reload";
                break;
            case "模式设置":
                actionName = "Chmod";
                break;
            case "下设置":
                actionName = "Down";
                break;
            default:
                break;
        }
        KeySetter.开启重绑(actionName, 下标, 按钮文字);
    }

    public void 重置绑定()
    {
        KeySetter.重置绑定("Movement");
        KeySetter.重置绑定("Fire");
        KeySetter.重置绑定("Reload");
        KeySetter.重置绑定("Jump");
        KeySetter.重置绑定("Chmod");
        KeySetter.重置绑定("Down");
        更新UI();
    }


    private void 更新UI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            string text = "";

            switch (i)
            {
                case 2:
                    text = Application.isPlaying ? KeySetter.获取绑定名称("Jump", 0) : jumpReference.action.GetBindingDisplayString(0);
                    break;
                case 3:
                    text = Application.isPlaying ? KeySetter.获取绑定名称("Fire", 0) : fireReference.action.GetBindingDisplayString(0);
                    break;
                case 4:
                    text = Application.isPlaying ? KeySetter.获取绑定名称("Reload", 0) : reloadReference.action.GetBindingDisplayString(0);
                    break;
                case 5:
                    text = Application.isPlaying ? KeySetter.获取绑定名称("Chmod", 0) : chmodReference.action.GetBindingDisplayString(0);
                    break;
                case 6:
                    text = Application.isPlaying ? KeySetter.获取绑定名称("Down", 0) : downReference.action.GetBindingDisplayString(0);
                    break;
                default:
                    text = Application.isPlaying ? KeySetter.获取绑定名称("Movement", i+1) : moveReference.action.GetBindingDisplayString(i+1);
                    break;
            }
            transform.GetChild(i).GetChild(1).GetComponentInChildren<TMP_Text>().text = text;
        }
    }
}

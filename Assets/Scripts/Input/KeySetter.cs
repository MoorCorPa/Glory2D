using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class KeySetter : MonoBehaviour
{
    public static InputControler input;

    public static event Action �����;
    public static event Action ��ȡ��;
    public static event Action<InputAction, int> �󶨿�ʼ;
    private void Awake()
    {
        if (input == null)
            input = new InputControler();
    }

    public static void �����ذ�(string action����,int ���±�, TMP_Text ��ť����)
    {
        InputAction action = input.asset.FindAction(action����);
        if (action == null || action.bindings.Count <= ���±�)
        {
            Debug.Log("�Ҳ���action��󶨰�����");
            return;
        }

        ִ���ذ�(action, ���±�, ��ť����);
    }

    private static void ִ���ذ�(InputAction �ذ�action, int ���±�, TMP_Text ��ť����)
    {
        if (�ذ�action == null) return;

        ��ť����.text = $"�밴��";

        �ذ�action.Disable();

        var �ذ� = �ذ�action.PerformInteractiveRebinding(���±�);
        �ذ�.OnComplete(operation =>
        {
            �ذ�action.Enable();
            operation.Dispose();

            ����󶨸���(�ذ�action);
            �����?.Invoke();
        });

        �ذ�.OnCancel(operation =>
        {
            �ذ�action.Enable();
            operation.Dispose();
            ��ȡ��?.Invoke();
        });

        �ذ�.WithCancelingThrough("<Keyboard>/Backspace");

        �󶨿�ʼ?.Invoke(�ذ�action, ���±�);
        �ذ�.Start(); //���������ذ����
    }

    public static string ��ȡ������(string action����, int ���±�)
    {
        if(input == null)
            input = new InputControler();

        InputAction action = input.asset.FindAction(action����);
        return action.GetBindingDisplayString(���±�);
    }

    private static void ����󶨸���(InputAction action)
    {
        for (int i = 0; i < action.bindings.Count; i++)
        {
            PlayerPrefs.SetString(action.actionMap + action.name + i, action.bindings[i].overridePath);
        }
    }

    public static void ���ذ󶨸���(string actionName)
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

    public static void ���ð�(string actionName)
    {
        InputAction action = input.asset.FindAction(actionName);

        if (action == null)
        {
            Debug.Log("������");
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

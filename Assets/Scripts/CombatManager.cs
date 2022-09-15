using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class CombatManager : MonoBehaviour
{

    public static CombatManager instance;

    public bool canReceiveInput;
    public bool inputReceived;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Attack();
    }

    //public void Attack(InputAction.CallbackContext context)
    public void Attack()
    {
        if (Keyboard.current.jKey.wasPressedThisFrame)
        {
            Debug.Log(canReceiveInput);
            if (canReceiveInput)
            {
                inputReceived = true;
                canReceiveInput = false;
            }
            else
            {
                return;
            }
        }
    }

    public void InputManager()
    {
        canReceiveInput = !canReceiveInput ? true:false;
    }
}

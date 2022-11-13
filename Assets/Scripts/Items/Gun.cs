using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    public static Gun instance;
    public Bullet bullet;
    public Transform muzzle;

    public bool isColldown = true;
    public float timeToColldown = 0.15f;
    private float startTime;
    private float timeCount = 0;

    public int 最大子弹数量 = 12;
    public int 当前子弹数量;
    public int 散射数量 = 1;
    public float 换弹时间 = 1f;
    public float 换弹计时 = 0;
    public bool 是否正在换弹 = false;
    public bool 主动换弹 = false;
    public float 射击切回瞄准时间;
    public float 换弹动画切换时间;
    public float 最大散射偏移;
    public float 最大速度偏移;
    private bool 开火;

    public GameObject 换弹进度条;

    public Vector3 换弹进度条缩放;

    public AudioSource 开枪音效;
    public AudioSource 换弹音效;

    [SerializeField] private Animator 换弹动画;
    [SerializeField] private Texture2D 默认指针;
    [SerializeField] private Texture2D 瞄准指针;
    [SerializeField] private Texture2D 射击指针;
    [SerializeField] private Texture2D[] 换弹指针;

    private bool 正在射击;
    private float 射击切回瞄准时间计时;
    private float 换弹动画切换计时;
    private int 换弹指针序号;
    
    private InputControler 行为控制;

    [SerializeField] private GameObject 激光;
    [SerializeField] private bool 激光模式 = false;

    private void OnEnable()
    {
        行为控制 = KeySetter.input;
        行为控制.Player.Fire.performed += ctx => 开火 = true;
        行为控制.Player.Fire.canceled += ctx => 
        {
            开火 = false;
            激光.SetActive(false);
        };
        行为控制.Player.Reload.started += 触发换弹;
        行为控制.Player.Chmod.started += ctx => 激光模式 = !激光模式;

        行为控制.Player.Fire.Enable();
        行为控制.Player.Reload.Enable();
        行为控制.Player.Chmod.Enable();
    }
    private void Awake()
    {
        instance = this;
        //startTime = Time.time;
    }

    private void Start()
    {
        Cursor.SetCursor(默认指针, new Vector2(0, 0), CursorMode.Auto);
        Physics2D.queriesStartInColliders = false;
        当前子弹数量 = 最大子弹数量;
        换弹进度条缩放 = 换弹进度条.transform.localScale;
        正在射击 = false;
        换弹指针序号= 0;
    }

    private void Update()
    {
        if (Time.timeScale > 0)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Cursor.SetCursor(默认指针, new Vector2(0, 0), CursorMode.Auto);
            }
            else if (开火 && isColldown && !是否正在换弹)
            {
                正在射击 = true;
                Fire();
                Cursor.SetCursor(射击指针, new Vector2(32, 32), CursorMode.Auto);
            }
            else if (是否正在换弹)
            {
                换弹动画切换计时+= Time.deltaTime;
                if (换弹动画切换计时>换弹动画切换时间)
                {
                    Cursor.SetCursor(换弹指针[换弹指针序号], new Vector2(32, 32), CursorMode.Auto);
                    换弹指针序号++;
                    if (换弹指针序号 >= 换弹指针.Length)
                    {
                        换弹指针序号 = 0;
                    }
                    换弹动画切换计时= 0;
                }

            }
            else if (!正在射击)
            {
                Cursor.SetCursor(瞄准指针, new Vector2(32, 32), CursorMode.Auto);
            }

            if (正在射击)
            {
                射击切回瞄准时间计时 += Time.deltaTime;
                if (射击切回瞄准时间计时 > 射击切回瞄准时间)
                {
                    射击切回瞄准时间计时 = 0;
                    正在射击 = false;
                }
            }
            
            检测子弹();
        }
    }

    public void Fire()
    {
        开枪音效.PlayOneShot(开枪音效.clip);
        当前子弹数量--;
        isColldown = false;
        //startTime = Time.time;
        //Instantiate(bullet, muzzle.position, muzzle.rotation);
        if (激光模式)
            激光.SetActive(true);
        else
            散射();
    }

    public void 散射()
    {
        int 计数 = 0;

        for (int i = 0; i < 散射数量; i++)
        {
            if (i % 2 == 1) 计数++;
            var 偏移量 = muzzle.rotation.z + 计数 * (i % 2 == 0 ? Random.Range(0,最大散射偏移) : Random.Range(-最大散射偏移, 0));
            var 子弹 = Instantiate(bullet, muzzle.position, new Quaternion(muzzle.rotation.x, muzzle.rotation.y, 偏移量, muzzle.rotation.w));
            子弹.GetComponent<Rigidbody2D>().velocity *= Random.Range(1+ 最大速度偏移, 1- 最大速度偏移);
        }
    }

    public void handleCooldown()
    {
        if (!isColldown)
        {
            /*if (Time.time - startTime > timeToColldown)
            {
                isColldown = true;
            }*/
            timeCount += Time.deltaTime;
            if (timeCount > timeToColldown)
            {
                timeCount = 0;
                isColldown = true;
                // Cursor.SetCursor(瞄准指针, new Vector2(32, 32), CursorMode.Auto);
            }
        }
    }

    public void 检测子弹()
    {
        if (当前子弹数量 > 0)
        {
            if (主动换弹)
            {
                换弹();
            }
            else
            {
                handleCooldown();
            }
        }
        else
        {
            换弹();
        }
    }
    
    public void 触发换弹(InputAction.CallbackContext context)
    {
        if (!是否正在换弹 && 最大子弹数量 > 当前子弹数量)
        {
            主动换弹 = true;
        }
    }
    
    public void 换弹()
    {
        if (是否正在换弹 == false)
        {
            换弹动画.SetLayerWeight(1, 1);
            换弹音效.Play();
            换弹进度条.SetActive(true);
        }

        是否正在换弹 = true;
        换弹计时 += Time.deltaTime;
        换弹进度条.GetComponent<Slider>().value = 换弹计时 / 换弹时间;
        激光.SetActive(false);

        if (换弹计时 > 换弹时间)
        {
            换弹计时 = 0;
            当前子弹数量 = 最大子弹数量;
            是否正在换弹 = false;
            主动换弹 = false;
            换弹动画.SetLayerWeight(1, 0);
            换弹进度条.SetActive(false);
        }
    }


}
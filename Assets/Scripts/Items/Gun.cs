using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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
    public float 换弹时间 = 1f;
    public float 换弹计时 = 0 ;
    public bool 是否正在换弹 = false ;
    public bool 主动换弹 = false ;
    
    public GameObject 换弹进度条;
    public Animator animator;

    public Vector3 换弹进度条缩放;
    
    public AudioSource 开枪音效;
    public AudioSource 换弹音效;
    
    private Ray2D ray;
    private void Awake()
    {
        instance = this;
        //startTime = Time.time;
    }

    void Start()
    {
        当前子弹数量 = 最大子弹数量;
        换弹进度条缩放 = 换弹进度条.transform.localScale;
    }

    void Update()
    {
        Fire();
        检测子弹();
      
        if (Input.GetKeyDown(KeyCode.R)&& !是否正在换弹  && 最大子弹数量 > 当前子弹数量)
        {
            主动换弹 = true;
        }
    }

    public void Fire()
    {
        if (Input.GetMouseButton(0) && isColldown && !是否正在换弹)
        {
            开枪音效.PlayOneShot(开枪音效.clip);
            当前子弹数量--;
            isColldown = false;
            //startTime = Time.time;
            Instantiate(bullet, muzzle.position, muzzle.rotation);
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

    public void 换弹()
    {
        
        if (是否正在换弹 == false)
        {
            animator.SetLayerWeight(1,1);
            换弹音效.Play();
            换弹进度条.SetActive(true);
        }
        是否正在换弹 = true;
        换弹计时 += Time.deltaTime;
        换弹进度条.GetComponent<Slider>().value = 换弹计时 / 换弹时间;
        if (换弹计时 > 换弹时间)
        {
            换弹计时 = 0;
            当前子弹数量 = 最大子弹数量;
            是否正在换弹 = false;
            主动换弹 = false;
            animator.SetLayerWeight(1,0); 
            换弹进度条.SetActive(false);
        }
    }
    
}

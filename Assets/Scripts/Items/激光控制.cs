using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class 激光控制 : MonoBehaviour
{
    public static 激光控制 instance;
    public int 伤害 = 3;   
    public float 攻击冷却 = 0.2f;

    private float 攻击冷却计时 = 0;

    [SerializeField] private Color 颜色 = new Color(191/255,36/255,0);
    [SerializeField] private float 颜色强度 = 4.3f; //HDR >1光晕效果

    [SerializeField] private float 最大长度 = 100;
    [SerializeField] private float 强度 = 9;//材质强度
    [SerializeField] private float 噪音缩放 = 3.14f;
    [SerializeField] private GameObject 起点VFX;
    [SerializeField] private GameObject 终点VFX;
    [SerializeField] private GameObject 枪口;

    private LineRenderer 线渲染器;

    private void Awake()
    {
        instance = this; 
        线渲染器 = GetComponentInChildren<LineRenderer>();

        线渲染器.material.color = 颜色 * 颜色强度;
        线渲染器.material.SetFloat("_LaserThickness", 强度);
        线渲染器.material.SetFloat("_LaserScale", 噪音缩放);

        ParticleSystem[] 粒子s = transform.GetComponentsInChildren<ParticleSystem>();
        foreach (var p in 粒子s)
        {
            Renderer r = p.GetComponent<Renderer>();
            r.material.SetColor("_EmissionColor", 颜色*颜色强度);
        }
    }

    void Start()
    {
        
    }
      
    // Update is called once per frame
    void Update()
    {
        更新末端位置();
    }

    private void 更新末端位置()
    {
        var flag = PlayerController.instance.flag;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, flag* transform.right);

        float 长度 = 最大长度;
        float 激光末端旋转向量 = 180;
        
        
        if (hit)
        {
            长度 = (hit.point - new Vector2(transform.position.x, transform.position.y)).magnitude;
            激光末端旋转向量 = Vector2.Angle(flag*transform.right, hit.normal);
        }

        线渲染器.SetPosition(1, new Vector2(长度, 0));
        Debug.DrawLine(transform.position, hit.point,Color.red);
        Vector2 终点 = 枪口.transform.position + 长度 * flag * transform.right;
        起点VFX.transform.position = 枪口.transform.position;
        if (hit)
        {
            终点VFX.SetActive(true);
            终点VFX.transform.position = hit.point;
        }
        else
        {
            终点VFX.SetActive(false);
        }
        
        终点VFX.transform.rotation = Quaternion.Euler(0, 0, 激光末端旋转向量);

        if (攻击冷却计时 > 攻击冷却)
        {
            攻击(hit);
            攻击冷却计时 = 0;
        }
        else
            攻击冷却计时 += Time.deltaTime;
        
            
    }

    private void 攻击(RaycastHit2D hit)
    {
        var e = hit.collider.GetComponent<Enemy>();
        if (e!=null)
        {
            e.掉血(伤害);
        };
    }
}

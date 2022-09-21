using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    public Bullet bullet;
    public Transform muzzle;

    public bool isColldown = true;
    public float timeToColldown = 21f;
    private float startTime;
    private float timeCount = 0;

    private void Awake()
    {
        //startTime = Time.time;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    void Update()
    {
        Fire();
        handleCooldown();
    }

    public void Fire()
    {
        if (Input.GetMouseButton(0) && isColldown) 
        {
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
}

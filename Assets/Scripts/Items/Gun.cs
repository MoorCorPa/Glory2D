using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    public Bullet bullet;
    public Transform muzzle;
    // Start is called before the first frame update
    void Start()
    {
    }

    void Update()
    {
        Fire();
        
    }

    public void Fire()
    {
        if(Input.GetMouseButton(0)) Instantiate(bullet, muzzle.position, muzzle.rotation);
    }

    
}

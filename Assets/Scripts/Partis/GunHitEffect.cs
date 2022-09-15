using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunHitEffect : MonoBehaviour
{
    private float timeToDestory = 1f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timeToDestory);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

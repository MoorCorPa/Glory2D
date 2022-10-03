using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using Random = UnityEngine.Random;

public abstract class Enemy : MonoBehaviour
{
    public Animator animator;

    // Ѫ��
    public int health;
    // ������
    public int damage;

    // �ƶ��ٶ�
    public float moveSpeed;
    // �ƶ���ʱ
    public float moveTimeCount = 0;
    // �ƶ����
    public float moveColldownTime;

    // ����ʱ��
    public float attackTime;
    // �������
    public float attackColldown;

    //���а뾶
    public float perceptionRadius;

    public Collider2D attackTriggerColl;
    // ����ƶ�����
    private float moveX;

    public bool isAttacked = true;

    // Start is called before the first frame update
    public void Start()
    {
        //transform.position = Vector2.MoveTowards(transform.position, -transform.position, moveSpeed * Time.deltaTime);
    }

    // Update is called once per frame
    public void Update()
    {
        // Ѫ��Ϊ0��ʧ
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        moveColldown();
    }

    // ����ƶ�
    public void moveColldown()
    {
        
        if (perceptionRadius > (transform.position - PlayerController.instance.transform.position).sqrMagnitude)
        {

            //Debug.Log("�ѽ��빥����Χ");
            transform.localScale = new Vector3(getFaceAt()? 1 : -1, 1, 1);
            transform.position = Vector2.MoveTowards(transform.position, new Vector3(PlayerController.instance.transform.position.x, transform.position.y, transform.position.z), moveSpeed* Time.deltaTime);
        }
        else
        {
            moveTimeCount += Time.deltaTime;
            if (moveTimeCount > moveColldownTime)
            {
                moveX = Random.Range(-3, 3f) + transform.position.x;
                moveTimeCount = 0;
            }

            transform.localScale = new Vector3(moveX < transform.position.x ? 1 : -1, 1, 1);
            transform.position = Vector2.MoveTowards(transform.position, new Vector3(moveX, transform.position.y, transform.position.z), moveSpeed * Time.deltaTime);
        }
    }

    public void takeDamage(int damage)
    {
        health -= damage;
    }

    // ��ȡ���
    public bool getFaceAt()
    {
        return transform.position.x>PlayerController.instance.transform.position.x ? true:false;
    }
}

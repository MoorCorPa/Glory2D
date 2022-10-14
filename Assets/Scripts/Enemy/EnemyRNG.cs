using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyRNG : Enemy
{
    [Header("ɨ����������")] 
    [Tooltip("ɨ�蹥������")] 
    [Range(0.0f, 360.0f)]
    public float viewDirection = 0.0f;

    [Range(0.0f, 360.0f)] 
    public float viewFov;
    [Min(0f)]
    public float viewDistance;
    
    //Ray2D ray;

    void Start()
    {
        /*Physics2D.queriesStartInColliders = false; //��֤Raycast�ڿ�ʼ���ʱ�ܺ����Լ������Collider���
        ray = new Ray2D(transform.position, Vector2.left);
        Debug.DrawRay(ray.origin, ray.direction, Color.red); //��㣬������ɫ����ѡ��
        RaycastHit2D info = Physics2D.Raycast(ray.origin, ray.direction);
        if (info.collider != null)
        {
            if (info.transform.gameObject.CompareTag("Player"))
            {
                Debug.Log("��⵽����");
            }
            else
            {
                //Debug.Log(info.transform.gameObject.name);
                //Debug.Log("��⵽��������");
            }
        }
        else
        {
            Debug.Log("û����ײ�κζ���");
        }*/

    }

    void Update()
    {
        base.Update();
        
        if (!isAttacked)
        {
            if (attackTime < attackColldown)
            {
                attackTime += Time.deltaTime;
            }
            else
            {
                attackTime = 0;
                isAttacked = true;
            }
        }

        // ����ҵľ���
        float distance = Vector2.Distance(transform.position, PlayerController.instance.transform.position);
        // ��ǰ��������
        Vector2 norVec = transform.rotation * (transform.localScale.x > 0 ? Vector2.left : Vector2.right);
        Vector3 v = Quaternion.Euler(0, 0, transform.localScale.x > 0 ? viewDirection : -viewDirection) * norVec;
        // ���������ҷ������� 
        Vector2 temVec = PlayerController.instance.transform.position - transform.position;
        // ���������ļн�
        float jiajiao = Mathf.Acos(Vector2.Dot(v.normalized, temVec.normalized)) * Mathf.Rad2Deg;

        if (distance < viewDistance)
        {
            if (jiajiao <= viewFov * 0.5f)
            {
                Debug.Log("��ҳ��������η�Χ�ڣ�");
                
                if (isAttacked)
                {
                    Debug.Log("��ȴ��Ͽ�ʼ��������");
                    AttackAnim();
                }
            }
        }
    }

    public void FixedUpdate()
    {
        base.FixedUpdate();
    }

    // ���������
    public void AttackAnim()
    {
        ActiveAttack();
        transform.localScale = new Vector3(getFaceAt() ? 1 : -1, 1, 1);
        animator.SetTrigger("Attack");
    }

    // �����ɹ�,�жϵ�Ѫ
    public void Cause()
    {
        if (isAttackHitRange)
        {
            PlayerController.instance.TakeDamage(damage);
        }
        else
        {
            Debug.Log("����δ����!!!");
        }
    }

    public void ActiveAttack()
    {
        isAttacked = false;
    }

    public void resetAnim()
    {
        animator.ResetTrigger("Attack");
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) isAttackHitRange = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) isAttackHitRange = false;
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isNearPlayer = true;
             // Debug.Log(other.rigidbody.velocity * other.rigidbody.mass);
             // GetComponent<Rigidbody2D>().AddForce(other.rigidbody.velocity * other.rigidbody.mass ,ForceMode2D.Force);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player")) isNearPlayer = false;
    }

    //����
    private void OnDrawGizmosSelected()
    {
        // ������Χ
        Vector3 forward = transform.localScale.x > 0 ? Vector2.left : Vector2.right;
        forward = Quaternion.Euler(0, 0, transform.localScale.x > 0 ? viewDirection : -viewDirection) * forward;
        if (GetComponent<SpriteRenderer>().flipX) forward.x = -forward.x;

        Vector3 endpoint = transform.position + (Quaternion.Euler(0, 0, viewFov * 0.5f) * forward);

        Handles.color = new Color(0, 1.0f, 0, 0.2f);
        Handles.DrawSolidArc(transform.position, -Vector3.forward, (endpoint - transform.position).normalized, viewFov,
            viewDistance);

        // ���з�Χ
        Handles.color = new Color(1.0f, 0, 0, 0.1f);
        Handles.DrawSolidDisc(transform.position, Vector3.back,  Mathf.Sqrt(perceptionRadius));
    }
}
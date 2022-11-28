using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class Enemy������ : Enemy
{
    [Min(0f)] public float ���˱�ɫʱ��;

    [Header("ɨ����������")]
    [Tooltip("ɨ�蹥������")]
    [Range(0.0f, 360.0f)]
    public float �ӽǷ���;

    [Range(0.0f, 360.0f)] public float �ӽ�FOV;

    [Min(0f)] public float ��Ұ����;

    public int ��ǰ�׶�;

    [Header("���׶�״̬������")]
    public AnimatorController[] ����������;

    private Rigidbody2D ����;
    private Animator ����;
    private SpriteRenderer ����;
    private Color32 ��ʼ��ɫ;

    private Vector3 ���λ��;
    private Vector3 ��ʼ����;

    private Vector3 ��ǰλ��
    {
        get => transform.position;
        set => transform.position = value;
    }

    private Vector3 ���λ�� => PlayerController.instance.transform.position;

    private float ����Ҿ��� => Vector2.Distance(��ǰλ��, ���λ��);

    void Start()
    {
        ��ʼ���� = transform.localScale;
        ���� = GetComponent<Rigidbody2D>();
        ���� = GetComponent<Animator>();
        ���� = GetComponent<SpriteRenderer>();
        ��ʼ��ɫ = ����.color;
        ��ǰѪ�� = ���Ѫ��;
        ��ǰ�׶� = 1;
    }

    void Update()
    {
        if (��ǰ�׶� < 3 && ��ǰѪ�� <= 10)
        {
            //�����л�״̬����
            ����.SetTrigger("Ability");
            return;
        }

        if (��ǰ�׶� is 3 && ��ǰѪ�� is 0)
        {
            ����.SetTrigger("Death");
            ִ������();
        }
    }

    void �л�״̬()
    {
        ����.runtimeAnimatorController = ����������[��ǰ�׶�++];
        ��ǰѪ�� = ���Ѫ��;
    }

    void ִ������()
    {
        ����.runtimeAnimatorController = ����������[0];
        ����.SetTrigger("Death");
    }

    void ����()
    {
        Destroy(gameObject);
    }

    public override void ��Ѫ(int �˺�)
    {
        if (��ǰ�׶�<3 && ��ǰѪ�� <= 10) return;

        ��ǰѪ�� -= �˺�;
        if (��ǰѪ�� > 0)
        {
            if (��ǰѪ�� % 5 == 0)
            {
                Instantiate(������, ��ǰλ��, transform.rotation);
            }

            ����.color = new Color(0.99f, 0.3f, 0.3f, 1f);
            ����.SetTrigger("Hit");
            Invoke("�ָ���ɫ", ���˱�ɫʱ��);
        }
        else
        {
            ����.gravityScale = 1.5f;
            ����.SetTrigger("Death");
            ִ������();
        }
    }

    private void OnDrawGizmosSelected()
    {
        var ��ɫ = new Color(1.0f, 0, 0, 0.1f);
        var ��ɫ = new Color(0.1f, 0.2f, 0.9f, 0.1f);

        // ������Χ
        Vector3 forward = transform.localScale.x > 0 ? Vector2.left : Vector2.right;
        forward = Quaternion.Euler(0, 0, transform.localScale.x > 0 ? �ӽǷ��� : -�ӽǷ���) * forward;
        if (GetComponent<SpriteRenderer>().flipX) forward.x = -forward.x;

        Vector3 endpoint = transform.position + (Quaternion.Euler(0, 0, �ӽ�FOV * 0.5f) * forward);

        Handles.color = ��ɫ;
        Handles.DrawSolidArc(transform.position, -Vector3.forward, (endpoint - transform.position).normalized, �ӽ�FOV,
            ��Ұ����);
    }
}

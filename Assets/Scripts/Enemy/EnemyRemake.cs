using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyRemake : MonoBehaviour
{
    public int ����ID;
    [Min(0f)] public int ���Ѫ��;
    [Min(0f)] public int ��ǰѪ��;
    [Min(0f)] public int ������;

    [Header("ɨ����������")]
    [Tooltip("ɨ�蹥������")]
    [Range(0.0f, 360.0f)]
    public float �ӽǷ��� = 0.0f;

    [Range(0.0f, 360.0f)] public float �ӽ�FOV;
    [Min(0f)] public float ��Ұ����;

    [Min(0f)] public float �ƶ��ٶ�;
    [Min(0f)] public float �����뾶;
    [Min(0f)] public float ���а뾶;
    [Min(0f)] public float �������;
    [Min(0f)] public float ���˱�ɫʱ��;

    public float ��·���߳���;

    public bool �Ƿ�Զ��;

    public GameObject �ӵ�;

    private float ���������ʱ;
    private float ��ɫ͸����;

    private Rigidbody2D ����;
    private Animator ����;
    private SpriteRenderer ����;
    private Color32 ��ʼ��ɫ;

    private Vector3 ���λ��;
    private Vector3 ��ʼλ��;
    private Vector3 ����λ��;
   
    private Vector3 ��ǰλ��
    {
        get => transform.position;
        set => transform.position = value;
    }

    private Vector3 ���λ�� => PlayerController.instance.transform.position;

    private float ����Ҿ���;

    // Start is called before the first frame update
    void Start()
    {
        ���� = GetComponent<Rigidbody2D>();
        ���� = GetComponent<Animator>();
        ���� = GetComponent<SpriteRenderer>();
        ��ʼ��ɫ = ����.color;
        ��ɫ͸���� = ��ʼ��ɫ.r;
        ����.SetInteger("����ID", ����ID);
        ��ʼλ�� = transform.position;
        ��ǰѪ�� = ���Ѫ��;
        ���������ʱ = 0;
        ����λ�� = ��ǰλ��;
    }

    // Update is called once per frame
    void Update()
    {
        if (��ǰѪ�� > 0)
        {
            ����Ҿ��� = Vector2.Distance(��ǰλ��, ���λ��);
            var ���� = Physics2D.Raycast(��ǰλ��, ���λ�� - ��ǰλ��, Vector2.Distance(��ǰλ��, ���λ��), ~LayerMask.GetMask("Enemy"));
            Debug.DrawLine(��ǰλ��, ����.point, Color.red);
            ���������ʱ += Time.deltaTime;

            if (������� < ���������ʱ)
            {
                if (�Ƿ�Զ��)
                {
                    if (����Ҿ��� < �����뾶 && !����.collider.CompareTag("��ͼ��ײ����"))
                    {
                        Instantiate(�ӵ�, ��ǰλ��, transform.rotation);
                    }
                    else
                    {
                        ����ƶ�();
                    }
                }
                else
                {
                    if (����Ҿ��� < ���а뾶 && !ǰ��·��Ϊ��())
                    {
                        if (��������η�Χ())
                        {
                            ����.SetTrigger("Attack");
                        }
                        else
                        {
                            ��ǰλ�� = Vector2.MoveTowards(��ǰλ��, new Vector3(���λ��.x, ��ǰλ��.y, ��ǰλ��.z), �ƶ��ٶ� * Time.deltaTime);
                            Debug.Log("����������");
                        }
                    }
                    else
                    {
                        ����ƶ�();
                    }
                }
            }
            else
            {
                ����ƶ�();
            }
            ת��();
        }
        else
        {
            ��ɫ͸���� -= Time.deltaTime * 100;
            ����.color = new Color32(��ʼ��ɫ.a, ��ʼ��ɫ.b, ��ʼ��ɫ.g, (byte)��ɫ͸����);
            if (��ɫ͸���� < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public virtual void ����ƶ�()
    {
        if ((Vector2.Distance(��ǰλ��, ���λ��) < 0.3f) || ǰ��·��Ϊ��())
        {
            ��ȡ��λ��();
        }
        ��ǰλ�� = Vector2.MoveTowards(��ǰλ��, ���λ��, �ƶ��ٶ� * Time.deltaTime);
    }

    public void ��ȡ��λ��()
    {
        ���λ�� = new Vector3(��ǰλ��.x+Random.Range(-3, 3f), ��ǰλ��.y, ��ǰλ��.z);
    }

    public bool ��������η�Χ()
    {
        Vector2 ��ǰ������ = transform.rotation * (transform.localScale.x > 0 ? Vector2.left : Vector2.right);
        Vector3 v = Quaternion.Euler(0, 0, transform.localScale.x > 0 ? �ӽǷ��� : -�ӽǷ���) * ��ǰ������;
        Vector2 ����ҵ����� = ���λ�� - ��ǰλ��;
        // ���������ļн�
        float �н� = Mathf.Acos(Vector2.Dot(v.normalized, ����ҵ�����.normalized)) * Mathf.Rad2Deg;

        if (����Ҿ��� < ��Ұ����)
        {
            if (�н� <= �ӽ�FOV * 0.5f)
            {
                Debug.Log("��ҳ��������η�Χ�ڣ�");
                return true;
            }
        }
        Debug.Log("������������");
        return false;
    }

    public bool ǰ��·��Ϊ��()
    {
        Vector3 ���� = new Vector3(-transform.localScale.x, -1, 0);
        Debug.DrawLine(transform.position, transform.position + ����.normalized * ��·���߳���, Color.yellow);

        if (Physics2D.Raycast(transform.position, ����, ��·���߳���, LayerMask.GetMask("Ground")))
        {
            Debug.Log("��·�����߲���");
            return false;
        }
        Debug.Log("������������");
        return true;
    }

    public void ת��()
    {
        if (����λ��.x - ��ǰλ��.x != 0)
        {
            transform.localScale = new Vector3(����λ��.x > ��ǰλ��.x ? 1 : -1, 1, 1);
        }
        ����λ�� = ��ǰλ��;
    }

    public void ��Ѫ(int �˺�)
    {
        ��ǰѪ�� -= �˺�;
        if (��ǰѪ�� > 0)
        {
            ����.color = new Color(0.99f, 0.3f, 0.3f, 1f);
            ����.SetTrigger("��Ѫ");
            Invoke("�ָ���ɫ", ���˱�ɫʱ��);
        }
        else
        {
            ����.gravityScale = 1;
            ����.SetTrigger("����");
        }
    }

    public void �ָ���ɫ()
    {
        ����.color = ��ʼ��ɫ;
    }

    //����
    private void OnDrawGizmosSelected()
    {
        // ������Χ
        Vector3 forward = transform.localScale.x > 0 ? Vector2.left : Vector2.right;
        forward = Quaternion.Euler(0, 0, transform.localScale.x > 0 ? �ӽǷ��� : -�ӽǷ���) * forward;
        if (GetComponent<SpriteRenderer>().flipX) forward.x = -forward.x;

        Vector3 endpoint = transform.position + (Quaternion.Euler(0, 0, �ӽ�FOV * 0.5f) * forward);

        Handles.color = new Color(0, 1.0f, 0, 0.2f);
        Handles.DrawSolidArc(transform.position, -Vector3.forward, (endpoint - transform.position).normalized, �ӽ�FOV,
            ��Ұ����);

        // ���з�Χ
        Handles.color = new Color(1.0f, 0, 0, 0.1f);
        Handles.DrawSolidDisc(transform.position, Vector3.back, Mathf.Sqrt(���а뾶));
    }
}

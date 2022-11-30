public class EnemyRNG : EnemyGround
{
    //Ray2D ray;

    new void Start()
    {
        /*Physics2D.queriesStartInColliders = false; //保证Raycast在开始检测时能忽略自己本身的Collider组件
        ray = new Ray2D(transform.position, Vector2.left);
        Debug.DrawRay(ray.origin, ray.direction, Color.red); //起点，方向，颜色（可选）
        RaycastHit2D info = Physics2D.Raycast(ray.origin, ray.direction);
        if (info.collider != null)
        {
            if (info.transform.gameObject.CompareTag("Player"))
            {
                Debug.Log("检测到敌人");
            }
            else
            {
                //Debug.Log(info.transform.gameObject.name);
                //Debug.Log("检测到其他对象");
            }
        }
        else
        {
            Debug.Log("没有碰撞任何对象");
        }*/
        base.Start();
    }

    new void Update()
    {
        base.Update();
    }

}
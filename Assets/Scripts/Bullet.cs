using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 50;
    public float speed = 30;
    private Transform target;//要攻击的目标

    public GameObject explosionEffectPrefab;//销毁敌人
    private float distanceArriveTarget=0.2f;//用于判断炮弹与目标的距离

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    void Update()
    {
        if (target==null)
        {
            Destroy(gameObject);
            return;
        }
        transform.LookAt(target.position);//炮弹指向目标
        transform.Translate(Vector3.forward*speed*Time.deltaTime);//移动炮弹
        
        Vector3 dir = target.position - transform.position;
        if (dir.magnitude<distanceArriveTarget)//判断距离
        {
            target.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(gameObject);//Bullet自我销毁
            GameObject effect = GameObject.Instantiate(explosionEffectPrefab, transform.position, transform.rotation);
            Destroy(effect, 1);//销毁特效
        }
    }
}

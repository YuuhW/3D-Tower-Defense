using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float speed=10;
    private Transform[] positions;
    public GameObject explosionEffect;//被销毁时的特效
    public Slider hpSlider;//血条

    public float hp = 200;
    private float totalHp;
    private int index = 0;
    void Start()
    {
        positions = WayPoints.positions;
        totalHp = hp;
        //hpSlider = GetComponentInChildren<Slider>();  //直接获取子类中的slider(当敌人很多时，会耗费性能)
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        if (index > positions.Length - 1) return;//判断索引是否超出界限
        transform.Translate((positions[index].position-transform.position).normalized*Time.deltaTime*speed);//先找方向(向量)，再归一化(长度为1)，最后乘距离
        if (Vector3.Distance(positions[index].position,transform.position)<0.2f)//Distance用于判断两个坐标间的距离
        {
            index++;
        }
        if (index>positions.Length-1)
        {
            ReachDestration();
        }
    }

    //到达终点
    void ReachDestration()
    {
        GameManage.Instance.Failed();//调用ENDUI
        Destroy(this.gameObject);
    }

    void OnDestroy()
    {
        EnemySpawner.AliveEnemyCount--;
    }

    public void TakeDamage(float damage)
    {
        if (hp <= 0) return;
        hp -= damage;
        hpSlider.value = (float)hp/totalHp;
        if (hp<=0)
        {
            GameObject effect = GameObject.Instantiate(explosionEffect, transform.position, transform.rotation);//实例化特效
            Destroy(effect,1.5f);//销毁特效
            Destroy(this.gameObject);//销毁目标
        }
    }
}

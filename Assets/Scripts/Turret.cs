using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private List<GameObject> enemys=new List<GameObject>(); 
    void OnTriggerEnter(Collider col)
    {
        if (col.tag=="Enemy")
        {
            enemys.Add(col.gameObject);
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.tag=="Enemy")
        {
            enemys.Remove(col.gameObject);
        }
    }


    //攻击
    public float attackRateTime = 1f;//多长时间攻击一次
    private float timer = 0;

    public GameObject bulletPrefab;
    public Transform head;
    public Transform firePosition;
    public bool useLaser=false;
    public LineRenderer laserRenderer;
    public float damageRate = 70;
    public GameObject laserEffect;

    void Start()
    {
        timer = attackRateTime;
    }

    void Update()
    {
        if (enemys.Count>0&&enemys[0]!=null)
        {
            Vector3 targetPosition = enemys[0].transform.position;
            targetPosition.y = head.position.y;
            head.LookAt(targetPosition);
        }
        if (useLaser==false)
        {
            timer += Time.deltaTime;
            if (enemys.Count > 0 && timer >= attackRateTime)
            {
                if (enemys[0]==null)
                {
                    UpdateEnemys();
                }
                timer = 0;
                Attack();
            }
        }
        else if(enemys.Count>0)
        {
            if (laserRenderer.enabled == false) laserRenderer.enabled = true;
            laserEffect.SetActive(true);//启用激光命中特效
            if (enemys[0] == null)
            {
                UpdateEnemys();
            }
            if (enemys.Count>0)
            {
                laserRenderer.SetPositions(new Vector3[]{firePosition.position,enemys[0].transform.position});//实例化激光
                enemys[0].GetComponent<Enemy>().TakeDamage(damageRate*Time.deltaTime);
                laserEffect.transform.position = enemys[0].transform.position;//命中特效
                Vector3 pos = transform.position;
                pos.y = enemys[0].transform.position.y;
                laserEffect.transform.LookAt(pos);//特效指向炮台
            }
        }
        else
        {
            laserEffect.SetActive(false);//关闭激光命中特效
            laserRenderer.enabled = false;//禁用激光
        }
    }

    void Attack()
    {
        if (enemys.Count>0)
        {
            GameObject bullet = GameObject.Instantiate(bulletPrefab, firePosition.position, firePosition.rotation);
            bullet.GetComponent<Bullet>().SetTarget(enemys[0].transform);   
        }
        else
        {
            timer = attackRateTime;
        }
    }

    void UpdateEnemys()
    {
        List<int>emptyIndex=new List<int>();
        for (int i = 0; i < enemys.Count; i++)
        {
            if (enemys[i]==null)
            {
                emptyIndex.Add(i);
            }
        }
        for (int i = 0; i < emptyIndex.Count; i++)
        {
            enemys.RemoveAt(emptyIndex[i]-i);
        }
    }
}

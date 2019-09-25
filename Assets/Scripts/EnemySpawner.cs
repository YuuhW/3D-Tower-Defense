using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static int AliveEnemyCount;//计算当前存活的敌人数量
    public Wave[] waves;
    public Transform start;
    public float WaveRate = 3;
    private Coroutine coroutine;

    void Start()
    {
        coroutine = StartCoroutine(SpawnEnemy());//启动协程
    }

    public void Stop()
    {
        StopCoroutine("coroutine");//结束协程
    }

    IEnumerator SpawnEnemy()//协程
    {
        foreach (Wave wave in waves)//先以波为单位进行遍历
        {
            for (int i = 0; i < wave.count; i++)//遍历一个波中的元素
            {
                GameObject.Instantiate(wave.enemyPrefab,start.position, Quaternion.identity);//生成对象，坐标，旋转
                if (i!=wave.count)
                {
                    yield return new WaitForSeconds(wave.rate);
                    AliveEnemyCount++;
                }
            }
            while (AliveEnemyCount > 0)//保证在当前敌人未全部消失前不会出现下一波
            {
                yield return 0;
            }
            yield return new WaitForSeconds(WaveRate);//波与波之间的间隔
        }
        while (AliveEnemyCount>0)
        {
            yield return 0;
        }
        GameManage.Instance.Win();
    }
}

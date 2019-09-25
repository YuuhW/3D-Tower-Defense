using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManage : MonoBehaviour
{
    public GameObject endUI;
    public Text endMessage;
    private EnemySpawner enemySpawner;//用于暂停敌人生成

    public static GameManage Instance;//在外界调用Win和Failed方法
    void Awake()
    {
        Instance = this;
        enemySpawner=GetComponent<EnemySpawner>();
    }

    void Start()
    {
        endUI.SetActive(false);//可以在这里把UI设置为不激活状态，或者直接将此场景禁用掉，满足条件是会自动调用
    }
    
    public void Win()
    {
        endUI.SetActive(true);
        endMessage.text = "胜 利";
    }

    public void Failed()
    {
        enemySpawner.Stop();
        endUI.SetActive(true);
        endMessage.text = "失 败";
    }

    public void OnButtonRetry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);//加载当前场景(建立当前场景的索引)
    }

    public void OnButtonMenu()
    {
        SceneManager.LoadScene(0);
    }
}

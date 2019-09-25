using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildManage : MonoBehaviour
{
    public TurretData laserTurretData;
    public TurretData MissileTurretData;
    public TurretData StandardTurretData;
    public Animator moneyAnimator;
    public GameObject UpgradeCanvas;
    public Button ButtonUpgrade;
    private MapCube selectedMapcube;//表示当前选择的炮塔
    private Animator upgradeCanvasAnimator;

    private int money=1000;
    public Text moneyText;

    void Start()
    {
        upgradeCanvasAnimator = UpgradeCanvas.GetComponent<Animator>();
    }

    void Update()
    {
        //对鼠标按下的cube进行检测，是否能进行创建
        if (Input.GetMouseButtonDown(0))
        {
            //检测指针是否在UI上
            if (EventSystem.current.IsPointerOverGameObject()==false)
            {
                //满足以上两个条件才能创建塔
                //先进行mapCube检测
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//将鼠标点作为射线
                RaycastHit hit;//检测结果
                bool isCollider = Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("MapCube"));
                if (isCollider)
                {
                    MapCube mapCube = hit.collider.GetComponent<MapCube>();//获取mapCube GameObject
                    if (selectedTurretData!=null && mapCube.turretGo==null)
                    {
                        //可以创建
                        if (money>=selectedTurretData.cost)
                        {
                            changeMoney(-selectedTurretData.cost);
                            mapCube.BuildTurret(selectedTurretData);//在MapCube中创建
                        }
                        else
                        {
                            //No Enough Money
                            moneyAnimator.SetTrigger("Flicker");
                        }
                    }
                    else if (mapCube.turretGo!=null)
                    {
                        if (mapCube==selectedMapcube&&UpgradeCanvas.activeInHierarchy)
                        {
                            StartCoroutine(HideUpgradeUI());//启动协程
                        }
                        else
                        {
                            ShowUpgradeUI(mapCube.transform.position, mapCube.isUpgrade);//升级处理
                        }
                        selectedMapcube = mapCube;//更新当前选择的炮台
                    }
                }
            }
        }
    }

    //存储选择的炮台(即将要创建的)
    private TurretData selectedTurretData;
    //监听炮台的选择
    public void OnLaserSelected(bool isOn)
    {
        if (isOn)
        {
            selectedTurretData = laserTurretData;
        }
    }
    public void OnMissileSelected(bool isOn)
    {
        if (isOn)
        {
            selectedTurretData = MissileTurretData;
        }
    }
    public void OnStandardSelected(bool isOn)
    {
        if (isOn)
        {
            selectedTurretData = StandardTurretData;
        }
    }

    //改变余额,并显示在UI中Canvas MoneyText中
    public void changeMoney(int change = 0)
    {
        money += change;
        moneyText.text = "￥" + money;
    }

    void ShowUpgradeUI(Vector3 pos, bool isDisableUpgrafe=false)//显示升级UI
    {
        StopCoroutine("HideUpgradeUI");//先检测HideUpgradeUI是否在执行，若执行，则暂停
        UpgradeCanvas.SetActive(false);//关闭画布
        UpgradeCanvas.SetActive(true);//设置为显示(这两步相当于初始化画布)
        // bug：当正在关闭上一个画布，并且单击显示新画布时，会出现新画布也关闭的情况
        pos.y += 2.55f;
        UpgradeCanvas.transform.position = pos;//设置显示位置
        ButtonUpgrade.interactable=!isDisableUpgrafe;//设置是否可交互
    }

    //使用协程
    IEnumerator HideUpgradeUI()//隐藏升级UI
    {
        upgradeCanvasAnimator.SetTrigger("Hide");//先播放动画
        yield return new WaitForSeconds(0.6f);//等待一段时间
        UpgradeCanvas.SetActive(false);//禁用
    }

    public void OnUpgradeButtonDown()
    {
        if (money >= selectedMapcube.turretData.costUpgraded)//判断剩余的钱是否足够
        {
            changeMoney(-selectedMapcube.turretData.costUpgraded);
            selectedMapcube.UpgradeTurret();
        }
        else
        {
            moneyAnimator.SetTrigger("Flicker");
        }
        StartCoroutine(HideUpgradeUI());
    }

    public void OnDestroyButtonDown()
    {
        selectedMapcube.DestroyTurret();
        StartCoroutine(HideUpgradeUI());
    }
}

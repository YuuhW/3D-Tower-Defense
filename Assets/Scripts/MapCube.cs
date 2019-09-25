using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapCube : MonoBehaviour
{
    [HideInInspector]//不在Inspector中显示
    public GameObject turretGo;//保存当前cube身上的炮台
    [HideInInspector]
    public bool isUpgrade=false;
    [HideInInspector]
    public TurretData turretData;//用于保存当前要升级的Turret

    //添加创建特效
    public GameObject buildEffect;

    private Renderer renderer;

    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    public void BuildTurret(TurretData turretData)
    {
        //实例化炮塔，指定位置与旋转
        this.turretData = turretData;
        isUpgrade = false;
        turretGo = GameObject.Instantiate(turretData.turretPrefab, transform.position, Quaternion.identity);
        GameObject effect = GameObject.Instantiate(buildEffect, transform.position, Quaternion.identity);
        Destroy(effect,1);//创建后销毁
    }

    public void UpgradeTurret()
    {
        if (isUpgrade == true) return;
        Destroy(turretGo);
        isUpgrade = true;
        turretGo = GameObject.Instantiate(turretData.turretUpgradedPrefab, transform.position, Quaternion.identity);
        GameObject effect = GameObject.Instantiate(buildEffect, transform.position, Quaternion.identity);
        Destroy(effect, 1);//创建后销毁
    }

    public void DestroyTurret()
    {
        Destroy(turretGo);
        isUpgrade = false;
        turretGo = null;
        turretData = null;
        GameObject effect = GameObject.Instantiate(buildEffect, transform.position, Quaternion.identity);
        Destroy(effect, 1);//创建后销毁
    }

    void OnMouseEnter()
    {
        if (turretGo==null&&EventSystem.current.IsPointerOverGameObject()==false)
        {
            renderer.material.color=Color.red;
        }
    }

    void OnMouseExit()
    {
        renderer.material.color=Color.white;
    }
}

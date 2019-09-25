using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewController : MonoBehaviour
{
    public float speed = 25;//移动速度
    public float mouseSpeed = 100;
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float mouse = -Input.GetAxis("Mouse ScrollWheel")*mouseSpeed;
        transform.Translate(new Vector3(h,mouse,v)*Time.deltaTime*speed,Space.World);
    }
}

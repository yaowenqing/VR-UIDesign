using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Rolling : MonoBehaviour
{

    public GameObject tar;
    public float k;

    private float speed1;//y方向旋转的速度
    private float speed2;//x方向旋转的速度
    private float num1;//y方向的位置
    private float num2;//x方向的位置

    void Start()
    {
        num1 = 0;
        num2 = 0;
    }

    void Update()
    {
        //i.x是y方向旋转速度，i.y是x方向旋转速度
        Vector2 i = SteamVR_Input._default.inActions.TouchPad.GetAxis(SteamVR_Input_Sources.Any);
        speed1 = k * i.x;
        speed2 = k * i.y;

        num1 += speed1;
        num2 += speed2;

        tar.transform.rotation = Quaternion.Euler(num2%360,num1%360, 0);

        Debug.Log(tar.transform.localRotation);

       
    }
}

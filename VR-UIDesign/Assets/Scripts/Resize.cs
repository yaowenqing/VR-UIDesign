using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Resize : MonoBehaviour
{
    public GameObject target;
    public float BiggestScale;//允许放大的最大倍数
    public float startScale;//原始倍数
    public float speed;//每一帧放大的倍数

    private float currScale;//当前放大倍数
    private bool bigger = false;
    private bool smaller = false;
    
    void Start()
    {
        currScale = startScale;
    }

    void Update()
    {
        if (SteamVR_Input._default.inActions.InteractUI.GetStateDown(SteamVR_Input_Sources.Any))
        {
            Big();
            //Debug.Log("按下去");
       }
        if (SteamVR_Input._default.inActions.InteractUI.GetStateUp(SteamVR_Input_Sources.Any) )
        {
            Small();
            //Debug.Log("松开");
        }

        if (bigger)
        {
            //Debug.Log("大大大");
            currScale += speed;
            target.transform.localScale = new Vector3(currScale, currScale, currScale);
        }

        if (smaller)
        {
            //Debug.Log("小小小");
            currScale -= speed;
            target.transform.localScale = new Vector3(currScale, currScale, currScale);
        }

        if (currScale > BiggestScale)
        {
            //Debug.Log("不能再大了");
            bigger = false;
        }

        if (currScale < startScale) {
            //Debug.Log("不能再小了");
            smaller = false;
            target.transform.localScale = new Vector3(startScale, startScale, startScale);
        }
    }

    public void Big()
    {   
        smaller = false;
        bigger = true;
    }

    public void Small()
    {
        bigger = false;
        smaller = true;
    }

    

}



    



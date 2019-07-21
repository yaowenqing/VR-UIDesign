using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.UI;

public class btnResize : MonoBehaviour
{
    public GameObject target;
    private Button btn;

    public float startScale;//原始倍数
    public float speed;//每一帧放大的倍数

    private float currScale;//当前放大倍数
    private bool bigger = false;
    private bool smaller = false;
    private bool duang=false;
    private bool up = true;
    private bool down = false;

    ColorBlock origin;
    Image im;
    Color or;

    void Start()
    {
        currScale = startScale;

        btn = this.GetComponent<Button>();
        origin = btn.colors;
        im = GetComponent<Image>();
        or = im.color;
    }

    void OnTriggerEnter(Collider other)
    {
        duang = true;
        //Debug.Log(this.gameObject + "撞上了");
    }

    void OnTriggerExit(Collider other)
    {
        duang = false;
        //Debug.Log(this.gameObject + "离开了");
    }

    void Update()
    {
        if (SteamVR_Input._default.inActions.InteractUI.GetStateDown(SteamVR_Input_Sources.Any)) {
            down = true;
            up = false;
            bigger = true;
            smaller = false;
            //Debug.Log("按下去111");
        }
        if (SteamVR_Input._default.inActions.InteractUI.GetStateUp(SteamVR_Input_Sources.Any)) {
            down = false;
            up = true;
            bigger = false;
            smaller = true;
            //Debug.Log("松开");
        }

        if (duang)
        {
            ColorBlock cb = new ColorBlock();
            cb.normalColor = Color.blue;
            cb.highlightedColor = Color.blue;
            cb.pressedColor = Color.blue;
            cb.disabledColor = Color.blue;

            //Debug.Log(this.gameObject + "撞上了");
            im.color = Color.blue;

            if (down && bigger) {
                currScale += speed;
                target.transform.localScale = new Vector3(currScale, currScale, currScale);
                //Debug.Log("按下去" + currScale);
            }
        }


        if ((up || duang == false)&&smaller)
        {
            currScale -= speed;
            target.transform.localScale = new Vector3(currScale, currScale, currScale);
            //Debug.Log("松开");
        }

        if (duang == false) {
            //Debug.Log("离开了");
            im.color = or;
        }
  
        if (currScale < startScale)
        {
            //Debug.Log("不能再小了");
            smaller = false;
            target.transform.localScale = new Vector3(startScale, startScale, startScale);
        }
    }
}


















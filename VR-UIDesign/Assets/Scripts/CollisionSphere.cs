using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollisionSphere : MonoBehaviour
{
    private Button btn;

    ColorBlock origin;
    Image im;
    Color or;
    void Start()
    {
        btn = this.GetComponent<Button>();
        origin = btn.colors;
        im = GetComponent<Image>();
        or = im.color;
    }

    void OnTriggerEnter(Collider other)
    {

        ColorBlock cb = new ColorBlock();
        cb.normalColor = Color.blue;
        cb.highlightedColor = Color.blue;
        cb.pressedColor = Color.blue;
        cb.disabledColor = Color.blue;

        //Debug.Log(this.gameObject + "撞上了");
        im.color = Color.red;
    }



    void OnTriggerExit(Collider other)
    {

        //Debug.Log("离开了");
        im.color = or;
    }

}

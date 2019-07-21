using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class menu : MonoBehaviour
{
    public InputField txt_username;

    // Start is called before the first frame update
    void Start()
    {
        txt_username.text = Tool.username;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickV()
    {
        Tool.exptype = moveball.ExpType.V;
        Tool.username = txt_username.text;
        SceneManager.LoadScene("EXPT");
    }

    public void ClickA()
    {
        Tool.exptype = moveball.ExpType.A;
        Tool.username = txt_username.text;
        SceneManager.LoadScene("EXPT");
    }

    public void ClickH()
    {
        Tool.exptype = moveball.ExpType.H;
        Tool.username = txt_username.text;
        SceneManager.LoadScene("EXPT");
    }

    public void ClickVA()
    {
        Tool.exptype = moveball.ExpType.VA;
        Tool.username = txt_username.text;
        SceneManager.LoadScene("EXPT");
    }

    public void ClickVH()
    {
        Tool.exptype = moveball.ExpType.VH;
        Tool.username = txt_username.text;
        SceneManager.LoadScene("EXPT");
    }

    public void ClickAH()
    {
        Tool.exptype = moveball.ExpType.AH;
        Tool.username = txt_username.text;
        SceneManager.LoadScene("EXPT");
    }

    public void ClickVAH()
    {
        Tool.exptype = moveball.ExpType.VAH;
        Tool.username = txt_username.text;
        SceneManager.LoadScene("EXPT");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    void Start()
    {
        //GameObject btnObj = GameObject.Find("Button (1)");
        //Button btn = btnObj.GetComponent<Button>();
        //btn.onClick.AddListener(delegate ()
        //{
        //    this.GoNextScene(btnObj);
        //});
       // UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }

    //public void GoNextScene(GameObject NScene) {
    //    UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    //}

    public void OnStartScene(int SceneNumber)
    {
        //Application.LoadLevel(SceneNumber);
        SceneManager.LoadScene(SceneNumber);
        Debug.Log(SceneNumber);
    }

    public void testBtn() {
        Debug.Log("hello");
    }
}



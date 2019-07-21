using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.SceneManagement;

public class sceneRoll : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 i = SteamVR_Input._default.inActions.TouchPad.GetAxis(SteamVR_Input_Sources.Any);
        Debug.Log(i.x+"+"+i.y);
        if (i.x > -0.1 && i.x < 0.1 && i.y > 0.9)
            SceneManager.LoadScene(0);
        if (i.x > -0.1 && i.x < 0.1 && i.y < -0.9)
            SceneManager.LoadScene(3);
        if (i.y > -0.1 && i.y < 0.1 && i.x > 0.9)
            SceneManager.LoadScene(2);
        if (i.y > -0.1 && i.y < 0.1 && i.x < -0.9)
            SceneManager.LoadScene(1);
    }
}

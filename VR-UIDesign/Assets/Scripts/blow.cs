using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blow : MonoBehaviour
{
    public bool blowing = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void blowBtn()
    {
        blowing = true;
    }
    public void releaseBtn()
    {
        blowing = false;
    }
}

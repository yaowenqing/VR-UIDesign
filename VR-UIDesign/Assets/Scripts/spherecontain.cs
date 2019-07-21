using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Pointer3D;
using Valve.VR;

public class spherecontain : MonoBehaviour
{
    public Transform tar;
    public Pointer3DRaycaster raycaster;
    // Start is called before the first frame update
    void Start()
    {
        tar = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        var result = raycaster.FirstRaycastResult();
        //Debug.Log(result.worldPosition);
        tar.position = result.worldPosition;
    }
}

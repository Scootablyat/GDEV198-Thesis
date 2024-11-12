using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CamVisibility : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera RTSCam;
    void Start()
    {
        if(!RTSCam) RTSCam = Camera.main;
    }

    void GetCorners(){
        Ray ray = RTSCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Vector3 worldBottomLeft = RTSCam.ScreenToWorldPoint(new Vector3(0, 0, RTSCam.nearClipPlane));
        Vector3 worldBottomRight = RTSCam.ScreenToWorldPoint(new Vector3(Screen.width, 0, RTSCam.nearClipPlane));
        Vector3 worldTopLeft = RTSCam.ScreenToWorldPoint(new Vector3(0, Screen.height, RTSCam.nearClipPlane));
        Vector3 worldTopRight = RTSCam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, RTSCam.nearClipPlane));
        
        /*
        Debug.Log("worldTopRight: " + worldTopRight);
        Debug.Log("worldTopLeft: " + worldTopLeft);
        Debug.Log("worldBottomRight: " + worldBottomRight);
        Debug.Log("worldBottomLeft: " + worldBottomLeft);
        */

    }

    // Update is called once per frame
    void Update()
    {
        GetCorners();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapManager : MonoBehaviour
{
    public RectTransform minimapRectTransform;
    public LayerMask uiLayer;
    // Start is called before the first frame update
    void Start()
    {
        if(!minimapRectTransform) minimapRectTransform = transform.GetChild(0).gameObject.GetComponent<RectTransform>();
    }

/*
    void getScreenPointToLocalPointInRectangle(){
        Vector2 screenPoint = Input.mousePosition;
        Vector2 localPoint;
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(minimapRectTransform, screenPoint, Camera.main, out localPoint)){
            Debug.Log("Screen Point: " + screenPoint + "/ Local Point: " + localPoint);
        }
    }
*/
    

    private bool getScreenPointToLocalPointInRectangle(){
        Vector2 screenPoint = Input.mousePosition;
        Vector2 localPoint;
        if(!RectTransformUtility.ScreenPointToLocalPointInRectangle(minimapRectTransform, screenPoint, null, out localPoint)){
            return false;
        }
        Debug.Log("Screen Point: " + screenPoint + "/ Local Point: " + localPoint);
        return true;
    }

    // Update is called once per frame
    void Update()
    {
        bool test = getScreenPointToLocalPointInRectangle();
    }
}

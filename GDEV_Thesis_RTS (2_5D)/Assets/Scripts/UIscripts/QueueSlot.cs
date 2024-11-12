using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QueueSlot : MonoBehaviour
{
    public GameObject queueSlotObj;
    public GameObject currentUnitInQueue;
    public GameObject Resources;
    
    void Start()
    {
        queueSlotObj = this.gameObject;
    }

    public void SetQueueSlotToDefault(){
        queueSlotObj.GetComponent<Image>().sprite = null;
        //queueSlotObj.GetComponent<Image>().color = new Color(107,149,173,255);
        queueSlotObj.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = null;
        queueSlotObj.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(255,255,255,0);
        if(currentUnitInQueue != null){
            currentUnitInQueue = null;
        }
    }

    void cancelUnitInQueue(){
        SetQueueSlotToDefault();
        
    }

    
    void Update()
    {
        
    }
}

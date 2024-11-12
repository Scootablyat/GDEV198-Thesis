using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QueueManager : MonoBehaviour
{
    // Start is called before the first frame update    
    private GameObject queueManagerObj;
    private GameObject queueSlotsObj;
    [SerializeField]
    private List<GameObject> queueSlotsList;

    [SerializeField]
    List<GameObject> trainingQueueList;
    public GameObject player;
    public GameObject barracks;
    public GameObject currentTimeUiObj;
    public bool isActive;
    float trainingTime;

    void Start()
    {
        queueManagerObj = this.gameObject;
        queueSlotsObj = this.gameObject.transform.GetChild(0).gameObject;
        trainingTime = 0;
        GetAllQueueSlots();
    }

    public void GetAllQueueSlots(){
        int queueSlotsCount = queueSlotsObj.transform.childCount;
        for (int i = 1; i < queueSlotsCount; i++){
            queueSlotsList.Add(queueSlotsObj.transform.GetChild(i).gameObject);
        }
    }

    public bool activateQueueSlotsUI(){
        if(player.GetComponent<UnitController>().selectedStructure == barracks){
            queueSlotsObj.SetActive(true);
            currentTimeUiObj.SetActive(true);
            return true;
        }
        queueSlotsObj.SetActive(false);
        currentTimeUiObj.SetActive(false);
        return false;
    }

    void updateCurrentTimeText(){
        if(trainingQueueList.Count > 0){
            if(trainingTime <= 0){ // fix the bug: timer is counting down even when unit in queue is bufferred due to manpower cap!
                trainingTime = barracks.GetComponent<Barracks>().trainingTime;
            }
            trainingTime-=Time.deltaTime;
            currentTimeUiObj.GetComponent<TextMeshProUGUI>().text = Math.Round(trainingTime).ToString();
        }
        else{
            currentTimeUiObj.SetActive(false);
        }
    }

    void SetAllQueueSlotsSprite(){
        if(isActive){
            for(int i = 0; i < queueSlotsList.Count; i++){
                if(i >= 0 && i < trainingQueueList.Count){
                    queueSlotsList[i].transform.GetChild(0).gameObject.GetComponent<Image>().sprite = trainingQueueList[i].GetComponent<SpriteRenderer>().sprite;
                    queueSlotsList[i].transform.GetChild(0).gameObject.GetComponent<Image>().color  = new Color(255,255,255,255);
                    queueSlotsList[i].GetComponent<QueueSlot>().currentUnitInQueue = trainingQueueList[i];
                }
                else{
                    queueSlotsList[i].GetComponent<QueueSlot>().SetQueueSlotToDefault();
                }
                //Debug.Log("Set Sprite to Queue Sprite");
            }
        }
    }

    void Update()
    {
        isActive = activateQueueSlotsUI();
        trainingQueueList = barracks.GetComponent<Barracks>().trainingQueueList;
        SetAllQueueSlotsSprite();
        updateCurrentTimeText();
    }
}
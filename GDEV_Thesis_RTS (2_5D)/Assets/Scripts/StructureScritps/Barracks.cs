using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Barracks : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public GameObject barracksObj;
    public GameObject commandBarObj;
    [SerializeField]
    private List<GameObject> trainableUnitsList;
    [SerializeField]
    private List<GameObject> commandBarButtons;
    [SerializeField]
    public Queue<GameObject> trainingQueue;

    public List<GameObject> trainingQueueList; // THIS LIST EXISTS FOR THE SOLE PURPOSE OF EXTRACTING SPRITES FROM THE QUEUE
    Vector3 spawnPoint;
    Vector3 rallyPoint;
    [SerializeField]
    public float trainingTime;
    [SerializeField]
    float currentTime;

    public bool isSettingRallyPoint;
    void Start()
    {
        spawnPoint = this.gameObject.transform.position + new Vector3(5,0,5);
        rallyPoint = spawnPoint + new Vector3(10,0,0);
        barracksObj = this.gameObject;
        commandBarButtons = commandBarObj.GetComponent<CommandBar>().getAllCommandButtons();
        trainingQueue = new Queue<GameObject>();
        trainingQueueList = new List<GameObject>();
    }
    
    void Update()
    {
        queueManager();
    }

    void setDefaultRallyPoint(){
        rallyPoint = spawnPoint;
    }

    public float getCurrentTime(){
        return currentTime;
    }

    public void LoadUI(){
        for(int i = 0; i < commandBarButtons.Count; i++){
            commandBarButtons[i].transform.GetChild(0).GetComponent<Image>().sprite = trainableUnitsList[i].GetComponent<SpriteRenderer>().sprite;
            commandBarButtons[i].transform.GetChild(0).GetComponent<Image>().color = new Color(255, 255, 255, 255);
            commandBarButtons[i].GetComponent<ButtonCommandBar>().attachCommandToButton(delegate { trainUnit(trainableUnitsList[i].gameObject); });
            if(i+1 >= trainableUnitsList.Count){
                break;
            }
        }
    }

    void queueManager(){
        if(trainingQueue.Count != 0){
            trainingTime = trainingQueue.Peek().GetComponent<UnitStats>().trainingTime;
            currentTime += Time.deltaTime;
            if(currentTime >= trainingTime 
            && player.GetComponent<ResourceCollection>().manpower + trainingQueue.Peek().GetComponent<UnitStats>().unitManpowerCost <= player.GetComponent<ResourceCollection>().currentMaxManpower){
                spawnUnit(trainingQueue.Peek(), spawnPoint);
                currentTime = 0;
                trainingQueueList.Remove(trainingQueue.Peek());
                trainingQueue.Dequeue();

            }
        }
    }

    void spawnUnit(GameObject prefabUnit, Vector3 spawnLocation){
        GameObject newUnit = Instantiate(prefabUnit, spawnLocation, Quaternion.identity);
        newUnit.GetComponent<NavMeshAgent>().SetDestination(rallyPoint);
        player.GetComponent<UnitController>().setUnitProperties(newUnit);

    }

    bool isUnitAffordable(GameObject unit){
        if(player.GetComponent<ResourceCollection>().food >= unit.GetComponent<UnitStats>().unitFoodCost
        && player.GetComponent<ResourceCollection>().ammo >= unit.GetComponent<UnitStats>().unitAmmoCost
        && player.GetComponent<ResourceCollection>().manpowerCap >= unit.GetComponent<UnitStats>().unitManpowerCost + player.GetComponent<ResourceCollection>().manpower){
            return true;
        }
        /*
        if(!(player.GetComponent<ResourceCollection>().food > unit.GetComponent<UnitStats>().unitFoodCost)){

        }
        */
        
        return false;
    }

    void trainUnit(GameObject unit){
        if(trainingQueue.Count < 7 && isUnitAffordable(unit)){
            trainingQueue.Enqueue(unit);
            trainingQueueList.Add(unit);
            player.GetComponent<ResourceCollection>().food -= unit.GetComponent<UnitStats>().unitFoodCost;
            player.GetComponent<ResourceCollection>().ammo -= unit.GetComponent<UnitStats>().unitAmmoCost;
        }
    }

    public void setRallyPoint(){
        Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, Mathf.Infinity, player.GetComponent<UnitController>().includedLayers )){
            rallyPoint = hit.point;
        }
    }

    // Update is called once per frame
    
}

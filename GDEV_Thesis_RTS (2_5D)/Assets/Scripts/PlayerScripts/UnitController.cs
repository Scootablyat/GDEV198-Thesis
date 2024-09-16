using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class UnitController : MonoBehaviour
{
    public RectTransform boxSelector;
    public LayerMask PlayerUnit;
    public LayerMask PlayerStructure;
    public LayerMask terrainMask;
    public LayerMask sectorMask;
    public Camera mainCamera;

    private Vector2 mouseStartPos;
    private Vector2 mouseEndPos;

    public enum mouseState { attackMoveCommand, defaultState };
    public mouseState currentMouseState;

    [SerializeField]
    public List<GameObject> playerOwnedUnits;
    [SerializeField]
    public List<GameObject> selectedUnits;
    // Start is called before the first frame update
    void Start()
    {
        setMouseStateDefault();
        boxSelector.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(currentMouseState == mouseState.defaultState){
            //Debug.Log("unit selection");
            unitSelection();
        }
        attackMove();
        toggleHighlight();
        moveSelectedUnits();
        stopSelectedUnits();
        getAllSelectedUnitsVelocity();
        
    }

    public void addUnitToPlayerUnits(GameObject unit){
        playerOwnedUnits.Add(unit);
    }

    public int getPlayerOwnedUnitsCount(){
        return playerOwnedUnits.Count;
    }

    void setMouseStateAttackMove(){
        currentMouseState = mouseState.attackMoveCommand;
    }
    void setMouseStateDefault(){
        currentMouseState = mouseState.defaultState;
    }
    public void stopSelectedUnits(){
        if(Input.GetKey(KeyCode.S)){
            foreach(GameObject unit in selectedUnits){
                //unit.GetComponent<NavMeshAgent>().isStopped = true;
                unit.GetComponent<NavMeshAgent>().SetDestination(unit.transform.position);
            }
        }
    }

    public void getAllSelectedUnitsVelocity(){
        if(Input.GetKey(KeyCode.Q)){
            foreach(GameObject unit in selectedUnits){
                //unit.GetComponent<NavMeshAgent>().isStopped = true;
                Debug.Log("Velocity: " + unit.GetComponent<NavMeshAgent>().velocity);
            }
        }
    }
    
    public void moveSelectedUnits(){
        if(Input.GetMouseButtonDown(1)){
            foreach(GameObject unit in selectedUnits){ // NOTE TO FUTURE JT: this seems extremely inefficient, FIX THIS LATER!!!! DONT FORGET THIS TIME!!!
                unit.GetComponent<UnitBehavior>().setStateMoving();
                unit.GetComponent<UnitBehavior>().setAttackMoveDestinationToDefault();
                Ray ray = mainCamera.ScreenPointToRay (Input.mousePosition);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit, 10000, terrainMask)){
                    // Debug.Log("hit point: " + hit.point);
                    unit.GetComponent<NavMeshAgent>().SetDestination(hit.point);
                }
            }
        }
    }

    public void unitSelection(){ //eventually also add a check for a box selector similar to how classic RTS works.
        if(Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftShift)){ // press once
            mouseStartPos = Input.mousePosition;
            clearSelectedUnits();
            individualSelect();
        }
        else if(Input.GetKey(KeyCode.LeftShift)){
            //Debug.Log("Shift Key Select Mode");
            shiftSelect();
        }
        else if(Input.GetMouseButtonUp(0) && boxSelector.gameObject.activeSelf == true){ // on release
            Debug.Log("unit selection");
            releaseBoxSelect();
        }
        else if(Input.GetMouseButton(0)){ // Hold Down Mouse
            //Debug.Log("Box Select Mode");
            boxSelect(Input.mousePosition);
        }
    }

    void structureSelect(){
        if(Input.GetMouseButtonDown(0)){
            
        }
    }

    void selectStructure(){
        
    }
    public void individualSelect(){
        Ray ray = mainCamera.ScreenPointToRay (Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, Mathf.Infinity) && !selectedUnits.Contains(hit.collider.gameObject)){
            if(hit.collider.gameObject.layer == LayerMask.NameToLayer("PlayerUnit")){
                //Debug.Log("HIT");
                //clearSelectedUnits();
                selectedUnits.Add(hit.collider.gameObject);
                //hit.collider.gameObject.GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineAll;
            }
        }
    }
    
    public void shiftSelect(){
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, Mathf.Infinity) && !selectedUnits.Contains(hit.collider.gameObject) && Input.GetKeyUp(KeyCode.Mouse0)){
            if(hit.collider.gameObject.layer == LayerMask.NameToLayer("PlayerUnit")){
                Debug.Log("SHIFT SELECT HIT");
                selectedUnits.Add(hit.collider.gameObject);
                //hit.collider.gameObject.GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineAll;                
            }
        }
        else if(Physics.Raycast(ray, out hit, Mathf.Infinity) && selectedUnits.Contains(hit.collider.gameObject) && Input.GetKeyUp(KeyCode.Mouse0)){
            if(hit.collider.gameObject.layer == LayerMask.NameToLayer("PlayerUnit")){
                selectedUnits.Remove(hit.collider.gameObject);
                hit.collider.gameObject.GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineHidden;                
            }
        }
    }

    public void clearSelectedUnits(){
        // clear and unhighlight units
        foreach(GameObject unit in selectedUnits){
            unit.GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineHidden;
        }
        selectedUnits.Clear();
    }

    public void boxSelect(Vector2 currentMousePos){
        if(!boxSelector.gameObject.activeInHierarchy){
            boxSelector.gameObject.SetActive(true);
        }

        float width = currentMousePos.x - mouseStartPos.x;
        float height = currentMousePos.y - mouseStartPos.y;

        boxSelector.sizeDelta = new Vector2(Mathf.Abs(width),Mathf.Abs(height));
        boxSelector.anchoredPosition = mouseStartPos + new Vector2(width/2,height/2);

    }

    public void releaseBoxSelect(){
        boxSelector.gameObject.SetActive(false);

        Vector2 minPos = boxSelector.anchoredPosition - (boxSelector.sizeDelta/2);
        Vector2 maxPos = boxSelector.anchoredPosition + (boxSelector.sizeDelta/2);

        foreach(GameObject unit in playerOwnedUnits){
            Vector3 sPos = mainCamera.WorldToScreenPoint(unit.transform.position);
            if(sPos.x > minPos.x && sPos.y > minPos.y && sPos.x < maxPos.x && sPos.y < maxPos.y && !selectedUnits.Contains(unit)){
                //unit.GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineAll;
                selectedUnits.Add(unit);
            }
        }
    }

    public void toggleBox(){
        if(!boxSelector.gameObject.activeInHierarchy){
            boxSelector.gameObject.SetActive(true);
        }
        else{
            boxSelector.gameObject.SetActive(false);
        }
    }

    public void toggleHighlight(){
        foreach(GameObject unit in selectedUnits){
            unit.GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineAll;
        }
    }

    public void attackCommand(){

    }

    public void attackMove(){
        if(Input.GetKey(KeyCode.A) && selectedUnits.Count != 0){
            setMouseStateAttackMove();
        }
        if(selectedUnits.Count != 0 && currentMouseState == mouseState.attackMoveCommand){
            if(Input.GetMouseButtonUp(0)){
                Ray ray = mainCamera.ScreenPointToRay (Input.mousePosition);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit, 10000, terrainMask)){
                    foreach(GameObject unit in selectedUnits){
                        unit.GetComponent<UnitBehavior>().setStateAttackMove(hit.point);
                    }
                }
                setMouseStateDefault();
            }
        }
    }
    
    public void createOutline(GameObject selectedUnit){
        if(selectedUnit.GetComponent<Outline>() == null){
            var outline = selectedUnit.AddComponent<Outline>();
            outline.OutlineMode = Outline.Mode.OutlineHidden;
            outline.OutlineColor = Color.green;
            outline.OutlineWidth = 5f;
        }
    }
}

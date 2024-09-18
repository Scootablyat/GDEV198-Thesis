using System.Collections;
using System.Collections.Generic;
using FischlWorks_FogWar;
using Unity.VisualScripting;
using UnityEngine;

public class UnitVisibility : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject thisUnit;
    public GameObject fogOfWar;
    public SpriteRenderer minimapIcon;

    public bool unitVisible;
    void Start()
    {
        
    }

    bool isUnitVisible(GameObject unit){
        if(fogOfWar.GetComponent<csFogWar>().CheckVisibility(unit.transform.position, 0)){
            return true;
        }
        return false;
    }

    void drawUnit(){
        if(!isUnitVisible(thisUnit)){
            thisUnit.GetComponent<SpriteRenderer>().enabled = false;
            minimapIcon.enabled = false;
        }
        else{
            thisUnit.GetComponent<SpriteRenderer>().enabled = true;
            minimapIcon.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        unitVisible = isUnitVisible(thisUnit);
        drawUnit();
    }
}

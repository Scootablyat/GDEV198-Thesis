using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{
    public GameObject unit;
    public Image HealthBar;
    public float HealthBarFillValue;
    // Start is called before the first frame update
    void updateHealthBarPosition(){
        this.gameObject.transform.GetChild(0).gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 25, 0);
    }
    void Start()
    {
        if(!unit) unit = this.gameObject.transform.parent.gameObject;
        if(!HealthBar) HealthBar = this.gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).GetComponent<Image>();
        updateHealthBarPosition();
    }

    

    void updateHealth(){
        double unitMaxHealth = unit.GetComponent<UnitStats>().unitMaxHP;
        double unitCurrentHealth = unit.GetComponent<UnitStats>().unitCurrentHP;
        double unitPercentageHP = unitCurrentHealth/(unitMaxHealth/100);
        HealthBarFillValue = (float)unitPercentageHP/100;
        HealthBar.fillAmount = HealthBarFillValue;
    }

    // Update is called once per frame
    void Update()
    {
        updateHealth();
    }
}

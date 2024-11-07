using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ResourceCollection : MonoBehaviour
{
    public GameObject PlayerManager;
    public float food;
    public float foodRate;
    public float foodPerSector; //foodPerSector
    [SerializeField]
    int foodCap;

    public float ammo;
    public float ammoRate;
    public float ammoPerSector; //
    [SerializeField]
    int ammoCap;
    public int manpower;
    public int currentMaxManpower;
    public int manpowerCap;
    public int manpowerPerSector;

    public GameObject sectors;
    public LayerMask sectorLayer;
    public List<GameObject> allSectors;
    public List<GameObject> allOwnedSectors;
    public List<GameObject> ownedFoodSectors;
    public List<GameObject> ownedAmmoSectors;
    public GameObject barracks;
    // Start is called before the first frame update

    public GameObject foodText;
    public GameObject ammoText;
    public GameObject manpowerText;
    public GameObject maxManpowerText;
    void Start()
    {
        manpowerCap = 100;
        foodCap = 5000;
        ammoCap = 1000;
        getConsumedManpower();
    }
    void getAllSectors(){
        for (int i = 0; i < 1000; i++){
            if(sectors.transform.GetChild(i).gameObject.layer == sectorLayer){
                allSectors.Add(sectors.transform.GetChild(i).gameObject);
            }
        }
    }

    bool isInList(GameObject x, List<GameObject> y){
        for(int i = 0; i < y.Count; i++){
            if(x == y[i]){
                return true;
            }
        }
        return false;
    }
    
    void getAllOwnedSectors(){
        foreach(GameObject sector in allSectors){
            if(sector.GetComponent<sectorManager>().sectOwner == sectorManager.sectorOwner.player && isInList(sector, allOwnedSectors) == false){
                allOwnedSectors.Add(sector);
            }
        }

        foreach(GameObject sector in allOwnedSectors){
            if(sector.GetComponent<sectorManager>().sectOwner == sectorManager.sectorOwner.neutral || sector.GetComponent<sectorManager>().sectOwner == sectorManager.sectorOwner.enemy){
                allOwnedSectors.Remove(sector);
                ownedFoodSectors.Remove(sector);
                ownedAmmoSectors.Remove(sector);
            }
        }
    }

    void getAllOwnedFoodSectors(){
        foreach(GameObject sector in allOwnedSectors){
            if(sector.tag == "FoodSector" && isInList(sector, ownedFoodSectors) == false){
                ownedFoodSectors.Add(sector);
            }
        }
    }

    void getAllOwnedAmmoSectors(){
        foreach(GameObject sector in allOwnedSectors){
            if(sector.tag == "AmmoSector" && isInList(sector, ownedAmmoSectors) == false){
                ownedAmmoSectors.Add(sector);
            }
        }
    }

    float getFoodRate(float foodPerSector, List<GameObject> foodSectors){
        return foodPerSector * foodSectors.Count;
    }
    float getAmmoRate(float ammoPerSector, List<GameObject> ammoSectors){
        return ammoPerSector * ammoSectors.Count;
    }

    void getCurrentMaxManpower(int manpowerPerSector, List<GameObject> allOwnedSectors){
        currentMaxManpower = manpowerPerSector * allOwnedSectors.Count;
        manpowerText.GetComponent<TextMeshProUGUI>().text = manpower + "/" + currentMaxManpower;
    }

    void getConsumedManpower(){
        int manpowerTemp = 0;
        foreach(GameObject unit in PlayerManager.GetComponent<UnitController>().playerOwnedUnits){
            manpowerTemp += unit.GetComponent<UnitStats>().unitManpowerCost;
        }
        manpower = manpowerTemp;
    }

    void increaseFood(){
        food += Time.deltaTime * foodRate;
        foodText.GetComponent<TextMeshProUGUI>().text = Math.Round(food).ToString() + "(+" + Math.Round(foodRate) + ")";
    }

    void increaseAmmo(){
        ammo += Time.deltaTime * ammoRate;
        ammoText.GetComponent<TextMeshProUGUI>().text = Math.Round(ammo).ToString() + "(+" + Math.Round(ammoRate) + ")";
    }

    // Update is called once per frame
    void Update()
    {

        getAllOwnedSectors();
        getAllOwnedAmmoSectors();
        getAllOwnedFoodSectors();
        foodRate = getFoodRate(foodPerSector, ownedFoodSectors);
        ammoRate = getAmmoRate(ammoPerSector, ownedAmmoSectors);
        getConsumedManpower();
        getCurrentMaxManpower(manpowerPerSector, allOwnedSectors);
        increaseFood();
        increaseAmmo();
    }
}

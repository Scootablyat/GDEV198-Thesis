using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitStats : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    public int unitManpowerCost;
    public int unitFoodCost;
    public int unitAmmoCost;
    public float unitMaxHP;
    public double unitCurrentHP;
    public float unitSpeed;
    public int sightRange;
    public int unitArmor;
    public int damage;
    public int attackSpeed;
    public int attackRange;
    public int aggroRange;
    public float areaOfEffect;
    public float trainingTime;

    public int fogRevealerIndex;

    public GameObject player;

    public int getArmor(){
        return unitArmor;
    }

    public double getArmorMultiplier(){
        return unitArmor * 0.05;
    }


    public int getAggroRange(){
        return aggroRange;
    }

    public int getAttackRange(){
        return attackRange;
    }

    public int getDamage(){
        return damage;
    }
    public void checkIfDead(){
        if(unitCurrentHP <= 0){
            // insert death animation stuff here
            player.GetComponent<UnitController>().onUnitDestroy(this.gameObject);
            Destroy(this.gameObject);
        }
        // add on unit destroy function
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        checkIfDead();
    }
}

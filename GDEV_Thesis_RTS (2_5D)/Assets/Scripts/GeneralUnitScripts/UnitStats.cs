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
    public float unitCurrentHP;
    public float unitSpeed;
    public int unitArmor;
    public int damage;
    public int attackSpeed;
    public int attackRange;
    public int aggroRange;
    public float areaOfEffect;

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
            Destroy(this.gameObject);
        }
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

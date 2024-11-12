using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class UnitBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject unit;
    public int aggroRange;
    public int attackRange;
    public LayerMask enemyUnits;
    public GameObject TargetOpponent;
    public enum unitState {idle, moving, aggro, attacking, attackMove};
    public unitState currentState;
    public Vector3 attackMoveDestination;


    [SerializeField]
    Collider[] AggroRangeColliders;
    [SerializeField]
    Collider[] AttackRangeColliders; // problems with this implementation: multiple enemies can be attacked at once. FIX THIS.

    void Start()
    {
        setAttackMoveDestinationToDefault();
        aggroRange = unit.GetComponent<UnitStats>().getAggroRange();
        attackRange = unit.GetComponent<UnitStats>().getAttackRange();
    }

    public void setAttackMoveDestinationToDefault(){
        attackMoveDestination = new Vector3(0, 0, 0);
    }

    float getDistance(GameObject s1 /*s1 is the primary subject/center point*/, GameObject s2){
        return Vector3.Distance(s1.transform.position, s2.transform.position);
    }

    // these are kinda pointless but my brain needs it for code readability lol
    public void setStateIdle(){
        currentState = unitState.idle;
    }
    public void setStateAggro(){
        currentState = unitState.aggro;
    }
    public void setStateAttacking(){
        currentState = unitState.attacking;
    }
    public void setStateAttackMove(Vector3 attackMoveDestination){
        this.attackMoveDestination = attackMoveDestination;
        currentState = unitState.attackMove;
    }
    public void setStateMoving(){
        currentState = unitState.moving;
    }

    GameObject getNearestEnemy(Collider[] hitColliders){
        GameObject nearestEnemy = null;
        float shortestDistance = 0;
        //Debug.Log("All Enemies in Collider[]: " + hitColliders);
        foreach (var enemy in hitColliders){
            if(shortestDistance == 0){
                shortestDistance = getDistance(unit, enemy.gameObject);
                nearestEnemy = enemy.gameObject;
            }
            if(getDistance(unit, enemy.gameObject) < shortestDistance){
                shortestDistance = getDistance(unit, enemy.gameObject);
                nearestEnemy = enemy.gameObject;
            }
        }
        return nearestEnemy;
    }

    void pickTarget(Collider[] AggroRangeColliders){
        if(AggroRangeColliders.Length != 0){
            setStateAggro();
            TargetOpponent = getNearestEnemy(AggroRangeColliders);
        }
    }

    void pickTargetWhileIdle(Collider[] AggroRangeColliders){
        if(currentState == unitState.idle){
            pickTarget(AggroRangeColliders);
        }
    }

    void dealDamage(GameObject enemyUnit, float damage){
        // add armor modifier
        double finalDamageValue = damage * (enemyUnit.GetComponent<UnitStats>().getArmorMultiplier());
        enemyUnit.GetComponent<UnitStats>().unitCurrentHP -= Time.deltaTime * damage;
    }

    bool isUnitIdle(){
        if(unit.GetComponent<NavMeshAgent>().velocity == new Vector3(0,0,0)){
            //Debug.Log("unit is idle!!");
            setStateIdle();
            return true;
        }
        return false;
    }

    bool isUnitMoving(){
        if(unit.GetComponent<NavMeshAgent>().velocity != new Vector3(0,0,0) && 
        currentState != unitState.aggro && currentState != unitState.attacking){
            //Debug.Log("unit is idle!!");
            setStateMoving();
            return true;
        }
        return false;
    }

    bool isInAttackRange(GameObject unit, GameObject aggroTarget){
        float distance = getDistance(unit, aggroTarget);
        //Debug.Log("distance from unit: " + distance);
        if(distance <= attackRange){
            //Debug.Log(aggroTarget + "in attack range of unit: " + unit);
            return true;
        }
        return false;
    }

    void moveToAttackRange(GameObject aggroTarget){
        if(TargetOpponent != null){
            if(isInAttackRange(unit, aggroTarget) == true){
                unit.GetComponent<NavMeshAgent>().SetDestination(unit.transform.position);
            }
            if(isInAttackRange(unit, aggroTarget) == false){
                unit.GetComponent<NavMeshAgent>().SetDestination(aggroTarget.transform.position);
            }
        }
    }

    void attackTarget(Collider[] AttackRangeColliders){
        if(TargetOpponent != null && currentState != unitState.moving){
            moveToAttackRange(TargetOpponent);
        }
        if(AttackRangeColliders.Length != 0){
            // insert attack animation stuff here
            //Debug.Log("bruh");
            dealDamage(TargetOpponent, unit.GetComponent<UnitStats>().getDamage());
        }
    }

    void attackMoveToDestination(Collider[] AggroCollider, Collider[] attackRangeCollider){
        if(attackMoveDestination != new Vector3(0, 0, 0)){
            if(AggroCollider.Length == 0){
                unit.GetComponent<NavMeshAgent>().SetDestination(attackMoveDestination);
                if(unit.transform.position == attackMoveDestination){
                    setAttackMoveDestinationToDefault();
                }
            }
            if(AggroCollider.Length != 0){
                pickTarget(AggroCollider);
                attackTarget(attackRangeCollider);
            }
        }
    }

    void SetBackToIdle(Collider[] AggroRangeColliders){
        bool isAggroRangeCollidersEmpty = AggroRangeColliders.Length <= 0;
        if(TargetOpponent == null && isAggroRangeCollidersEmpty && currentState != unitState.moving && currentState != unitState.attackMove){
            setStateIdle();
        }
    }

    // Update is called once per frame
    void Update()
    {
        AggroRangeColliders = Physics.OverlapSphere(unit.transform.position, aggroRange, enemyUnits);
        AttackRangeColliders = Physics.OverlapSphere(unit.transform.position, attackRange, enemyUnits);
        isUnitIdle();
        isUnitMoving();
        SetBackToIdle(AggroRangeColliders);
        pickTargetWhileIdle(AggroRangeColliders);
        attackTarget(AttackRangeColliders);
        attackMoveToDestination(AggroRangeColliders, AttackRangeColliders);
    }
}

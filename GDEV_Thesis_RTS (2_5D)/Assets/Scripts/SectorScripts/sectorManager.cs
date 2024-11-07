using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FischlWorks_FogWar;
using Unity.VisualScripting;
using UnityEngine;

public class sectorManager : MonoBehaviour
{
    public float captureMeter;
    public float captureRate;
    
    public enum captureStatus { enemyCapturing, playerCapturing, contested, none };
    public enum sectorType { food, ammo, core }
    public enum sectorOwner { player, enemy, neutral };
    public captureStatus currentCaptureStatus;
    public sectorOwner sectOwner;
    public SphereCollider sectorCollider;
    public LayerMask playerUnitMask;
    public LayerMask enemyUnitMask;
    public List<GameObject> playerUnits;
    public List<GameObject> enemyUnits;
    public GameObject FogOfWar; 

    private csFogWar.FogRevealer thisFogRevealer;
    

    // only either enemy or player can be capturing at any given time

    void addUnit(GameObject unit){
        if(unit.tag == "Unit"){
            if(unit.layer == LayerMask.NameToLayer("PlayerUnit")){
                playerUnits.Add(unit);
                Debug.Log("Player Unit added");
            }
            if(unit.layer == LayerMask.NameToLayer("EnemyUnit")){
                enemyUnits.Add(unit);
                Debug.Log("Enemy Unit added");
            }
        }
    }

    void removeUnit(GameObject unit){
        if(unit.tag == "Unit"){
            if(unit.layer == LayerMask.NameToLayer("PlayerUnit")){
                playerUnits.Remove(unit);
                Debug.Log("Player Unit added");
            }
            if(unit.layer == LayerMask.NameToLayer("EnemyUnit")){
                enemyUnits.Remove(unit);
                Debug.Log("Enemy Unit added");
            }
        }
    }

    captureStatus getCaptureStatus(){
        if(playerUnits.Count > 0 && enemyUnits.Count <= 0){
            return captureStatus.playerCapturing;
        }
        if(enemyUnits.Count > 0 && playerUnits.Count <= 0){
            return captureStatus.enemyCapturing;
        }
        else if(playerUnits.Count > 0 && enemyUnits.Count > 0){
            return captureStatus.contested;
        }
        return captureStatus.none;
    }

    float getCaptureRate(List<GameObject> playerUnits, List<GameObject> enemyUnits){ 
        float playerUnitCount = playerUnits.Count;
        float enemyUnitCount = enemyUnits.Count;
        float maxCaptureRate = 10; // 10 per second
        float minCaptureRate = 1; // 1 per second
        float totalUnits = playerUnitCount + enemyUnitCount;
        captureStatus currentCaptureStatus = getCaptureStatus();

        if(currentCaptureStatus == captureStatus.playerCapturing){
            float currentCaptureRate = playerUnitCount - enemyUnitCount;
            if(currentCaptureRate >= maxCaptureRate){
                return maxCaptureRate;
            }
            if(currentCaptureRate >= minCaptureRate && currentCaptureRate <= maxCaptureRate){
                return currentCaptureRate;
            }
            else{
                return minCaptureRate;
            }
        }
        if(currentCaptureStatus == captureStatus.enemyCapturing){
            float currentCaptureRate = enemyUnitCount - playerUnitCount;
            if(currentCaptureRate >= maxCaptureRate){
                return maxCaptureRate;
            }
            if(currentCaptureRate >= minCaptureRate && currentCaptureRate <= maxCaptureRate){
                return currentCaptureRate;
            }
            else{
                return minCaptureRate;
            }
        }
        if(currentCaptureStatus == captureStatus.contested){
            float currentCaptureRate = 0;
            return currentCaptureRate;
        }
        return 0;
    }

    void capturing(captureStatus capStatus, float currentCapRate){
        if(capStatus == captureStatus.playerCapturing && sectOwner == sectorOwner.neutral){
            captureRate = currentCapRate;
            captureMeter += currentCapRate * Time.deltaTime;
            setOwnerToPlayer();
        }
        if(capStatus == captureStatus.enemyCapturing && sectOwner == sectorOwner.neutral){
            captureRate = currentCapRate;
            captureMeter += currentCapRate * Time.deltaTime;
            setOwnerToEnemy();
        }
        if(capStatus == captureStatus.contested){
            captureRate = 0;
        }
    }

    int findFogRevealerIndex(){ //deprecated
        List<csFogWar.FogRevealer> allFogRevealers = FogOfWar.GetComponent<csFogWar>()._FogRevealers;
        for(int i = 0; i < FogOfWar.GetComponent<csFogWar>()._FogRevealers.Count; i++){
            if(allFogRevealers[i] == thisFogRevealer){
                Debug.Log("Index of Fog Revealer " + this.gameObject.name + ": " + i);
                return i;
            }
        }
        return 0;
    }

    void setOwnerToPlayer(){
        if(captureMeter >= 100){
            captureMeter = 100;
            FogOfWar.GetComponent<csFogWar>().AddFogRevealer(new csFogWar.FogRevealer(this.gameObject.transform, 25, false));
            thisFogRevealer = new csFogWar.FogRevealer(this.gameObject.transform, 25, false);
            sectOwner = setSectorOwner(sectorOwner.player);
        }
    }

    void setOwnerToEnemy(){
        if(captureMeter >= 100){
            captureMeter = 100;
            FogOfWar.GetComponent<csFogWar>().RemoveFogRevealerByTransform(this.gameObject.transform);
            sectOwner = setSectorOwner(sectorOwner.enemy);
        }
    }

    void setOwnerToNeutral(){
        if(captureMeter <= 0){
            captureMeter = 0;
            FogOfWar.GetComponent<csFogWar>().RemoveFogRevealerByTransform(this.gameObject.transform);
            sectOwner = setSectorOwner(sectorOwner.neutral);
        }
    }

    void unCapturing(captureStatus capStatus, float currentCapRate){
        if(capStatus == captureStatus.playerCapturing && sectOwner == sectorOwner.enemy){
            captureRate = currentCapRate;
            captureMeter -= currentCapRate * Time.deltaTime;
            setOwnerToNeutral();
        }
        if(capStatus == captureStatus.enemyCapturing  && sectOwner == sectorOwner.player){
            captureRate = currentCapRate;
            captureMeter -= currentCapRate * Time.deltaTime;
            setOwnerToNeutral();
        }
        if(capStatus == captureStatus.contested){
            captureRate = 0;
        }
    }

    sectorOwner setSectorOwner(sectorOwner owner){
        /* 
        Yes. You are correct. This is indeed unnecessary. 
        I'm just doing this to help with code readability since I'm actually quite stupid.
        */
        return owner;
    }

    private void OnTriggerEnter(Collider col){
        if(col.gameObject.tag == "Unit"){
            Debug.Log("Collision Triggered with: " + col.gameObject.tag);
            addUnit(col.gameObject);
        }
    }

    private void OnTriggerExit(Collider col){
        if(col.gameObject.tag == "Unit"){
            Debug.Log("Unit Exiting: " + col.gameObject.tag);
            removeUnit(col.gameObject);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        capturing(getCaptureStatus(), getCaptureRate(playerUnits, enemyUnits));
        unCapturing(getCaptureStatus(), getCaptureRate(playerUnits, enemyUnits));
    }
}

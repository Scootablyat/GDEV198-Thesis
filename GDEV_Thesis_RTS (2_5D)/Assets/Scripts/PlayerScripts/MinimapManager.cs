using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class MinimapManager : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public RectTransform minimapRectTransform;
    public Camera RTSCam;
    public LayerMask uiLayer;
    public Terrain terrain;
    [SerializeField]
    public bool isMovingCameraOnMinimap;
    bool isHeldDown;

    void Start()
    {
        if(!RTSCam) RTSCam = Camera.main;
        if(!minimapRectTransform) minimapRectTransform = transform.GetChild(0).gameObject.GetComponent<RectTransform>();
        isMovingCameraOnMinimap = false;
        isHeldDown = false;
    }

    Vector3 localPointToTerrainPoint(Vector2 normalizedLocalPoint){
        Bounds terrainBounds = terrain.terrainData.bounds;
        Vector3 upperRight = terrainBounds.max;
        Vector3 upperLeft = new Vector3(terrainBounds.max.x - (terrainBounds.extents.x * 2), terrainBounds.max.y , terrainBounds.max.z );
        Vector3 lowerRight = new Vector3(terrainBounds.min.x + (terrainBounds.extents.x * 2), terrainBounds.min.y , terrainBounds.min.z );
        Vector3 lowerLeft = terrainBounds.min;

        //Debug.Log("Point on Terrain: " + new Vector3(normalizedLocalPoint.x * terrainBounds.max.x, 0 , normalizedLocalPoint.y * terrainBounds.max.z));
        return new Vector3(normalizedLocalPoint.x * terrainBounds.max.x, 0 , normalizedLocalPoint.y * terrainBounds.max.z);
    }

    void setCamPos(){
        Vector2 screenPoint = Input.mousePosition;
        Vector2 localPoint;
        Vector2 normalizedPos;
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(minimapRectTransform, screenPoint, null, out localPoint)){
            normalizedPos = Rect.PointToNormalized(minimapRectTransform.rect, localPoint);
            Debug.Log("Normalized Position on Minimap: " + normalizedPos);
            Vector3 terrainPoint = localPointToTerrainPoint(normalizedPos);
            RTSCam.transform.position = new Vector3(terrainPoint.x, RTSCam.transform.position.y, terrainPoint.z - 20);
        }
    }

    public void OnPointerDown(PointerEventData eventData){
        isHeldDown = true;
    }

    public void OnPointerUp(PointerEventData eventData){
        isHeldDown = false;
    }

    void Update()
    {
        if(isHeldDown == true){
            isMovingCameraOnMinimap = true;
            setCamPos();
        }
        else{
            isMovingCameraOnMinimap = false;
        }
        
    }
}

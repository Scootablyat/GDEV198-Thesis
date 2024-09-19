using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barracks : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject barracksObj;
    public GameObject commandBarObj;
    [SerializeField]
    private List<GameObject> trainableUnitsList;
    [SerializeField]
    private List<GameObject> commandBarButtons;
    void Start()
    {
        barracksObj = this.gameObject;
        commandBarButtons = commandBarObj.GetComponent<CommandBar>().getAllCommandButtons();
    }

    void LoadUI(){
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

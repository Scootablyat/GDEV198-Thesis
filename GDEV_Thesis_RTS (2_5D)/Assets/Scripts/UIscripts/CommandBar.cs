using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.UI;

public class CommandBar : MonoBehaviour
{

    public GameObject commandBar;
    [SerializeField]
    private List<GameObject> testList;
    void Start()
    {
        commandBar = this.gameObject;
        testList = getAllCommandButtons();
    }

    public List<GameObject> getAllCommandButtons(){
        List<GameObject> AllCommandButtons = new List<GameObject>();
        int numberOfChildren = commandBar.transform.childCount;
        for(int i = 0; i < numberOfChildren; i++){
            AllCommandButtons.Add(commandBar.transform.GetChild(i).gameObject);
        }
        return AllCommandButtons;
    }

    public void clearAllCommandButtons(){
        List<GameObject> AllCommandButtons = getAllCommandButtons();
        int numberOfChildren = commandBar.transform.childCount;
        for(int i = 0; i < numberOfChildren; i++){
            AllCommandButtons[i].GetComponent<ButtonCommandBar>().setDefaultImageState();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

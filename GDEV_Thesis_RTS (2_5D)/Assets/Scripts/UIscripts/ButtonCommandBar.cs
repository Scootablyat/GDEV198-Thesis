using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCommandBar : MonoBehaviour
{

    public GameObject buttonObj;
    public Button button;
    // Start is called before the first frame update
    void Start()
    {
        buttonObj = this.gameObject;
        button = this.gameObject.GetComponent<Button>();
    }

    public void attachCommandToButton(){

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

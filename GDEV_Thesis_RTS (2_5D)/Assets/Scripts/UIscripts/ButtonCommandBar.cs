using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonCommandBar : MonoBehaviour
{

    public GameObject buttonObj;
    public Button button;

    [SerializeField]
    public UnityAction AttachedCommand;
    // Start is called before the first frame update
    void Start()
    {
        buttonObj = this.gameObject;
        button = this.gameObject.GetComponent<Button>();
    }

    public void setDefaultImageState(){
        this.gameObject.GetComponent<Image>().sprite = null;
        this.gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 190);
        this.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = null;
        this.gameObject.transform.GetChild(0).GetComponent<Image>().color = new Color(0, 0, 0, 0);
        button.onClick.RemoveAllListeners();
    }

    public void attachCommandToButton(UnityAction unityAction){
        button.onClick.RemoveAllListeners();
        AttachedCommand = unityAction;
        button.onClick.AddListener(unityAction);
    }

    // Update is called once per frame
    void Update()
    {

    }
}

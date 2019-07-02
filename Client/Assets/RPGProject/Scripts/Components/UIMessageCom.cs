using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMessageCom : MonoBehaviour {

    public string Id;
    Text myText;
    private void Awake()
    {
        myText = GetComponent<Text>();
    }

    private void Start()
    {
    
        myText.text = ConfigInfo.Instance.MessageDir[Id].Text;
    }
}

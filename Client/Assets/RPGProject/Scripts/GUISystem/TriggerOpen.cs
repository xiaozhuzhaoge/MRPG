using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerOpen : MonoBehaviour {

    public string uiName;

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == PlayerData.myRole.gameObject)
        {
            MUIMgr.Instance.OpenUI(uiName);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject == PlayerData.myRole.gameObject)
        {
            MUIMgr.Instance.CloseUI(uiName);
        }
    }
}

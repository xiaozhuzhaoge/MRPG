using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenSystem : MonoBehaviour {

    public Button btn;
    public string UIname;
    private void Awake()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(Click);
    }

    private void Click()
    {
        MUIMgr.Instance.OpenUI(UIname);
    }
 
    [ContextMenu("Loadin")]
    public void MoveSence()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Demo");
    }
}

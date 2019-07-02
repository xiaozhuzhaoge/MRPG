using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MUISceneLoader : MonoBehaviour {

    public string loadLevelName;
    public Button load;
	// Use this for initialization
	void Awake () {
        load = GetComponent<Button>();
        load.onClick.AddListener(() => {
            MUIMgr.Instance.ShowAlert(OpenType.TwoButton, () => { SceneMgr.Instance.LoadSceneAsync(loadLevelName); }, null, "返回", "是否返回", "确定", "取消");
        });
	}
 
}

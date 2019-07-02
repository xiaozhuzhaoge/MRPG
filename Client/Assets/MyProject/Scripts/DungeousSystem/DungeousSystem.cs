using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeousSystem : MUIBase {

    Button close;
    Transform content;
    GameObject currentSelect;
    List<GameObject> items = new List<GameObject>();

    public override void OnAwake()
    {
        base.OnAwake();
        close = Utility.FindChild<Button>(transform, "Close");
        content = Utility.FindChild<Transform>(transform, "Content");
        close.onClick.AddListener(Back);
        CreateDungeousItems();
    }
     
    public void CreateDungeousItems()
    {
        foreach(var data in ConfigInfo.Instance.Dungeous.Values)
        {
           var item = ResourceMgr.CreateUIPrefab("GUIs/Dungeous/DungeousItem",content);
            Button btn = item.GetComponent<Button>();
            Text btnName = Utility.FindChild<Text>(item.transform, "name");
            items.Add(item);
            Image btnIcon = item.GetComponent<Image>();
            btnIcon.sprite = ResourceMgr.LoadSpriteFromAtals(Utility.CTInt(data.Index), "Sprites/Dungeous");
            btnName.text = data.Name;
            btn.name = data.Id;
            btn.onClick.AddListener(() => {
                currentSelect.transform.SetParent(btn.transform,false);
                MUIMgr.Instance.ShowAlert(OpenType.TwoButton, () => {
                    SceneMgr.Instance.LoadSceneAsync(btn.name);
                }, null, "进入地下城", "是否进入" + btn.name, "确定", "取消");
            });
        }
        currentSelect = ResourceMgr.CreateUIPrefab("GUIs/Dungeous/DungeousSelect", items[0].transform);
    }
 
}

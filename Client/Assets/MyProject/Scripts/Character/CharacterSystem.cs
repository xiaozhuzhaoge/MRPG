using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSystem : MUIBase {

    public Dictionary<int, GameObject> slots = new Dictionary<int, GameObject>();
    public Text content;
    public List<GameObject> props = new List<GameObject>();

    string text = "<color=#12fffff><b>Name</b></Color>\n" +
        "{0}\n" +
        "<color=#12fffff><b>ATK</b></Color>\n" +
        "{1}\n" +
        "<color=#12fffff><b>DEF</b></Color>\n" +
        "{2}\n" +
        "<color=#12fffff><b>ADV</b></Color>\n" +
        "{3}\n" +
        "<color=#12fffff><b>CTL</b></Color>\n" +
        "{4}\n" +
        "<color=#12fffff><b>MAXHP</b></Color>\n" +
        "{5}\n" +
        "<color=#12fffff><b>MAXMP</b></Color>\n" +
        "{6}\n" +
        "<color=#12fffff><b>MoveSpeed</b></Color>\n" +
        "{7}\n" +
        "<color=#12fffff><b>AtkSpeed</b></Color>\n" +
        "{8}\n";
    public override void OnAwake()
    {
        base.OnAwake();
        for(int i = 1201; i< 1208; i++)
        {
            GameObject go = Utility.FindChild<BagBlock>(transform, i.ToString()).gameObject;
            Debug.Log(go);
            slots.Add(i, go);
        }
        content = Utility.FindChild<Text>(transform, "detail");
        Utility.FindChild<Button>(transform, "Close").onClick.AddListener(Back);
        MessageCenter.Instance.RegiseterMessage("ItemExChange",gameObject,Refresh);
    }

    public override void Open(params object[] parms)
    {
        base.Open(parms);
        Refresh(parms);
    }

    public override void Refresh(params object[] parms)
    {
        base.Refresh(parms);
        content.text = string.Format(text, PlayerData.Me.info.name, PlayerData.myRole.Atk, PlayerData.myRole.Def, PlayerData.myRole.Adv, PlayerData.myRole.Cri, PlayerData.myRole.MaxHp, PlayerData.myRole.MaxMp, PlayerData.myRole.MoveSpeed,PlayerData.myRole.AtkSpeed);
        for(int i = 0; i < props.Count; i++)
        {
            Destroy(props[i]);
        }
        props.Clear();

        foreach (var data in PlayerData.BagInfos)
        {
            if (slots.ContainsKey(data.Value.slot))
            {
                Transform grid = slots[data.Value.slot].transform;
                var item = ResourceMgr.CreateUIPrefab("GUIs/Bag/Prop", grid);
                item.GetComponent<Image>().sprite = ResourceMgr.Load<Sprite>("Sprites/Props/" + ConfigInfo.GetProp(data.Value.propId.ToString()).Icon);
                Utility.FindChild<Text>(item.transform, "num").gameObject.SetActive(false);
                item.name = data.Key.ToString();
                props.Add(item);
            }
        }
    }
}

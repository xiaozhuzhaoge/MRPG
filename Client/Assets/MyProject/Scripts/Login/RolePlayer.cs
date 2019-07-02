using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using proto.MyProto;
using UnityEngine.UI;
public class RolePlayer : MonoBehaviour {

    public PlayerInfo info;
    Image job;
    Text nametext;
    Button btn;
    public void Awake()
    {
        job = transform.Find("job").GetComponent<Image>();
        nametext = transform.Find("name").GetComponent<Text>();
        btn = transform.GetComponent<Button>();
        btn.onClick.AddListener(() => {
            MessageCenter.Instance.BoradCastMessage("RoleSelect", info);
        });
    }
 
    public void SetUI(PlayerInfo info)
    {
        this.info = info;
        nametext.text = info.name;
        job.sprite = ResourceMgr.Load<Sprite>("Sprites/Icon/" + ConfigInfo.Instance.GetJobIcon(info.type.ToString()));
    }
}

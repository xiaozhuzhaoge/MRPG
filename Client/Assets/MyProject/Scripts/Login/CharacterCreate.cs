using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using proto.MyProto;
using System;
public class CharacterCreate : MUIBase {

    Transform layout;
    Button close;
    Button create;
    InputField inputName;

    /// <summary>
    /// 当前选择的职业ID
    /// </summary>
    public string currentTypeId;

    public override void OnAwake()
    {
        base.OnAwake();
        layout = transform.Find("Layout");
        close = transform.Find("Close").GetComponent<Button>();
        create = transform.Find("Create").GetComponent<Button>();
        inputName = transform.Find("InputName").GetComponent<InputField>();

        close.onClick.AddListener(Back);
        create.onClick.AddListener(CreatePlayer);
        CreateJobItems();
    }

    /// <summary>
    /// 生成职业图标按键
    /// </summary>
    public void CreateJobItems()
    {
        foreach(var job in ConfigInfo.Instance.Jobs)
        {
            var jobGO =  GameObject.Instantiate<GameObject>(ResourceMgr.Load<GameObject>("GUIs/JobItem"));
            jobGO.GetComponent<Image>().sprite = ResourceMgr.Load<Sprite>("Sprites/Icon/" + ConfigInfo.Instance.GetJobIcon(job.Key));
            Button btn = jobGO.AddComponent<Button>();
            jobGO.name = job.Key;
            btn.onClick.AddListener(() => {
                currentTypeId = btn.name;
            });
            jobGO.transform.SetParent(layout, false);
        }
    }

    /// <summary>
    /// 创建角色
    /// </summary>
    private void CreatePlayer()
    {
        if (currentTypeId == "" || currentTypeId == null)
        {
            MUIMgr.Instance.ShowAlert(OpenType.Long, null, null, "错误", "请选择一个职业", "确定");
            return;
        }
        if (inputName.text == null || inputName.text == "")
        {
            MUIMgr.Instance.ShowAlert(OpenType.Long, null, null, "错误", "请输入角色名", "确定");
            return;
        }

        CreatePlayerInfoReq req = new CreatePlayerInfoReq();
        req.accountId = PlayerData.AccountId.ToString();
        req.name = inputName.text;
        req.type = Convert.ToInt32(currentTypeId);
        

        MobaNetwork.Send((int)MessageId.ECreatePlayerInfoReq, req);
    }

    private void Start()
    {
        LunaMessage.AddMsgHandler((int)MessageId.ECreatePlayerInfoRes, OnCreatePlayer);
       
    }

    private void OnCreatePlayer(byte[] res)
    {
        CreatePlayerInfoRes response = ProtoBufUtils.Deserialize<CreatePlayerInfoRes>(res);
        int status = response.code.status;
        if(status == 0)
        {
            Back();
            MessageCenter.Instance.BoradCastMessage("OnCharacterCreate");
        }
        else if(status == 1)
        {
            Debug.LogError("没有生成了呢....");
        }
    }
}

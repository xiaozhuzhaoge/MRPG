using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using proto.MyProto;
using System;
using DG.Tweening;

public class SignInSystem : MUIBase {

    public Transform layout;
    public Button btn;
    public List<GameObject> signs = new List<GameObject>();
    public Dictionary<string, GameObject> singIned = new Dictionary<string, GameObject>();
    public int days;

    public override void OnAwake()
    {
        base.OnAwake();
        layout = Utility.FindChild<Transform>(transform, "Content");
        btn = Utility.FindChild<Button>(transform, "Close");
        btn.onClick.AddListener(Back);
         
    }
    public void Start()
    {
        days = TimeSystem.DaysInMonth();
        CreateSignItems();
        Refresh();
        LunaMessage.AddMsgHandler((int)MessageId.ESignInRes, OnSignRes);
    }

    /// <summary>
    /// 发送签到数据
    /// </summary>
    /// <param name="id"></param>
    public void SendSignedRequest(int id)
    {
        SignInReq req = new SignInReq();
        req.signId = id;
        req.SignInTime = new TimeInfo();
        req.SignInTime.time = TimeSystem.ServerTime;
        MobaNetwork.Send((ushort)MessageId.ESignInReq, req);
    }

    /// <summary>
    /// 接收签到数据
    /// </summary>
    /// <param name="res"></param>
    private void OnSignRes(byte[] res)
    {
        SignInRes response = ProtoBufUtils.Deserialize<SignInRes>(res);
        GameObject today = signs[TimeSystem.GetCurrentDay - 1];
        GameObject signed = ResourceMgr.CreateUIPrefab("GUIs/SignIn/siged", today.transform);
        signed.transform.localScale = Vector3.one * 2;
        Tweener tween = signed.transform.DOScale(Vector3.one, 1);
      
    }

    public void CreateSignItems()
    {
        int count = 1;
       
        foreach(var sign in ConfigInfo.Instance.SignIn)
        {
            if (count > days)
                break;
            var sg = ResourceMgr.CreateUIPrefab("GUIs/SignIn/SignIn", layout);
            sg.name = sign.Key.ToString();
            signs.Add(sg);
            singIned.Add(sign.Key, sg);
           
            Button btn = Utility.FindChild<Button>(sg.transform, "icon");
            btn.onClick.AddListener(() => { SendSignedRequest(Convert.ToInt32(sg.name)); });
            string Icon = ConfigInfo.GetProp(ConfigInfo.Instance.GetPropId(sign.Key, days)).Icon;
 
            btn.GetComponent<Image>().sprite = ResourceMgr.Load<Sprite>("Sprites/Props/" + Icon);
            Utility.FindChild<Text>(sg.transform, "num").text = ConfigInfo.Instance.GetNum(sign.Key, days).ToString();
            Utility.FindChild<Text>(sg.transform, "day").text = count.ToString();
            count++;
        }
 
    }

    public override void Refresh(params object[] parms)
    {
        base.Refresh(parms);
        SetInfo();
    }

    public void SetInfo()
    {
        ///设置已签到的对象
        foreach(var data in PlayerData.Signs)
        {
            GameObject signItem = singIned[data.signId.ToString()];
            ResourceMgr.CreateUIPrefab("GUIs/SignIn/siged", signItem.transform);
            Utility.FindChild<Button>(signItem.transform, "icon").interactable = false;
        }

        ///设置过期对象
        for(int i = 0; i < TimeSystem.GetCurrentDay - 1; i++)
        {
            GameObject signItem = signs[i];
            if(signItem.transform.childCount == 3)
            {
                ResourceMgr.CreateUIPrefab("GUIs/SignIn/passed", signs[i].transform);
                Utility.FindChild<Button>(signItem.transform, "icon").interactable = false;
            }
        }

        GameObject today = signs[TimeSystem.GetCurrentDay - 1];
        ResourceMgr.CreateUIPrefab("GUIs/SignIn/SignInSelect", today.transform);

        for (int i = TimeSystem.GetCurrentDay; i < TimeSystem.DaysInMonth(); i++)
        {
            GameObject signItem = signs[i];
            Utility.FindChild<Button>(signItem.transform, "icon").interactable = false;
            
        }
    }
}

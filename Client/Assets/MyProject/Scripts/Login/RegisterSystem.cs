using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using proto.MyProto;
using System;

public class RegisterSystem : MUIBase {

    InputField username;
    InputField password;
    InputField repassword;
    Button registerBtn;
    Button closeBtn;


    public override void OnAwake()
    {
        base.OnAwake();
        username = transform.Find("UserName").GetComponent<InputField>();
        password = transform.Find("Password").GetComponent<InputField>();
        repassword = transform.Find("RePassword").GetComponent<InputField>();

        registerBtn = transform.Find("Register").GetComponent<Button>();
        closeBtn = transform.Find("Close").GetComponent<Button>();

        registerBtn.onClick.AddListener(Register);
        closeBtn.onClick.AddListener(Back);
    }

    public void Start()
    {
        LunaMessage.AddMsgHandler((int)MessageId.ERegisterRes, OnRegister);
    }

    private void OnRegister(byte[] res)
    {
        RegisterRes response = ProtoBufUtils.Deserialize<RegisterRes>(res);
        int stat = response.code.status;
        if(stat == 0)
        {
            Debug.Log("注册成功了呢");
            Back();
        }
        else if(stat == 1)
        {
            Debug.Log("注册失败了呢");
        }
    }

    public void Register()
    {
        //限制逻辑


        RegisterReq req = new RegisterReq();
        req.username = username.text;
        req.password = password.text;
        req.deviceID = SystemInfo.deviceUniqueIdentifier;//设备ID
        req.channel = "UC";
        MobaNetwork.Send((ushort)MessageId.ERegisterReq, req);
    }
}

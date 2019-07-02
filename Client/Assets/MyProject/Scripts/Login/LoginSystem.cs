using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using proto.MyProto;
using UnityEngine.UI;
using System;

public class LoginSystem : MUIBase {

    InputField username;
    InputField password;
    Button login;
    Button register;
    Toggle remember;
    bool rem;

    public override void OnAwake()
    {
        base.OnAwake();
        username = transform.Find("UserName").GetComponent<InputField>();
        password = transform.Find("Password").GetComponent<InputField>();
        login = transform.Find("Login").GetComponent<Button>();
        register = transform.Find("Register").GetComponent<Button>();
        remember = transform.Find("Remember").GetComponent<Toggle>();

        remember.onValueChanged.AddListener((sw) => { rem = sw; });

        login.onClick.AddListener(Login);

        register.onClick.AddListener(() => {
            MUIMgr.Instance.OpenUI("Systems/RegisterSystem");
        });
    }

    private void Start()
    {
        LunaMessage.AddMsgHandler((int)MessageId.ELoginRes, OnLoginResponse);
        
        username.text = PlayerPrefs.GetString("username");
        password.text = PlayerPrefs.GetString("password");
        
    }

    private void OnLoginResponse(byte[] res)
    {
        LoginRes response = ProtoBufUtils.Deserialize<LoginRes>(res);
        PlayerData.AccountId = response.accountId;
        int stat = response.code.status;
        if(stat == 0)
        {
            MUIMgr.Instance.OpenUI("Systems/CharacterSelectSystem");
            if (rem)
            {
                PlayerPrefs.SetString("username", username.text);//本地持久化储存 基于string类型 储存到硬盘 键值对
                PlayerPrefs.SetString("password", password.text);//本地持久化储存 基于string类型 储存到硬盘 键值对
            }
              
        }
        else if(stat == 1)
        {
            Debug.LogError("登录失败了呢");
        }
    }

    public void Login() {

        LoginReq req = new LoginReq();
        req.username = username.text;
        req.password = password.text;
        req.serverIp = "172.16.12.1";

        MobaNetwork.Send((ushort)MessageId.ELoginReq, req);
    }

}


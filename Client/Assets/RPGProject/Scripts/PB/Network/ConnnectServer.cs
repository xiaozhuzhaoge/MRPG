using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ConnnectServer : MonoSingleton<ConnnectServer> {

    public string ip;
    public int port;
	// Use this for initialization
	void Start () {
        MobaNetwork.ConnectServer(ip, port);
	}

    public void Update()
    {
        MobaNetwork.Update();
    }
}

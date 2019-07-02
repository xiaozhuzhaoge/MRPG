using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILoader : MonoBehaviour {

	// Use this for initialization
	void Start () {
         
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnGUI()
    {
        if (GUILayout.Button("OpenB"))
        {
            MUIMgr.Instance.OpenUI("B");
        }
        if (GUILayout.Button("OpenA"))
        {
            MUIMgr.Instance.OpenUI("A");
        }
        if (GUILayout.Button("OpenC"))
        {
            MUIMgr.Instance.OpenUI("C");
        }
        if (GUILayout.Button("OpenD"))
        {
            MUIMgr.Instance.OpenUI("D");
        }

        if (GUILayout.Button("CloseB"))
        {
            MUIMgr.Instance.CloseUI("B");
        }
        if (GUILayout.Button("CloseA"))
        {
            MUIMgr.Instance.CloseUI("A");
        }
        if (GUILayout.Button("CloseC"))
        {
            MUIMgr.Instance.CloseUI("C");
        }
        if (GUILayout.Button("CloseD"))
        {
            MUIMgr.Instance.CloseUI("D");
        }
        if (GUILayout.Button("Pop"))
        {
            MUIMgr.Instance.PopUI();
        }
    }
}

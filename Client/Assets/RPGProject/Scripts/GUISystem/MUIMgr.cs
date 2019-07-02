using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 管理整个UI的对象的
/// </summary>
public class MUIMgr : Singleton<MUIMgr>{

    Dictionary<string, MUIBase> Uis = new Dictionary<string, MUIBase>();
    Stack<MUIBase> stack = new Stack<MUIBase>();

    public void PushUI(MUIBase ba)
    {
        stack.Push(ba);
         
    }
 
    public void PopUI()
    {
        if(stack.Count > 0)
        {
            while (stack.Count != 0)
            {
                MUIBase go = stack.Peek();
                if (go == null)
                    stack.Pop(); //如果为空删除
                else
                {
                    if(go.gameObject.activeSelf == false) //删除关闭的
                    {
                        stack.Pop();
                    }
                    if (go.gameObject.activeSelf == true) //发现有开启的
                        break;
                }
            }

            if (stack.Count == 0)
                return;

            MUIBase ui = stack.Pop();
            ui.Close();
        }
      
    }

    Transform canvas;
    public Transform Canvas
    {
        get { if(canvas == null)
            {
                canvas = GameObject.FindGameObjectWithTag("Canvas").transform;
                if(canvas == null)
                   canvas = GameObject.Find("Canvas").transform;
                if (canvas == null)
                    canvas = ResourceMgr.CreateUIPrefab("GUIs/Canvas", null).transform;
            }
            return canvas;
        }
    }
 
    public void AddUI(MUIBase ba)
    {
        if(!Uis.ContainsKey(ba.UiName))
            Uis.Add(ba.UiName, ba);
    }

    public void OpenUI(string UiName)
    {
        if (Uis.ContainsKey(UiName))
        {
            Debug.Log(Uis[UiName].gameObject);
            Uis[UiName].Open();
        }
        else
        {
            Debug.Log(UiName);
            var ui = GameObject.Instantiate<GameObject>(ResourceMgr.Load<GameObject>("GUIs/" + UiName));
            MUIBase mb = ui.GetComponent<MUIBase>();
            mb.Open();
        }
    }

    public void CloseUI(string UiName)
    {
        if (Uis.ContainsKey(UiName))
        {
            Uis[UiName].Close();
        }
    }

    public void RefreshUI(string UiName)
    {
        if (Uis.ContainsKey(UiName))
        {
            Uis[UiName].Refresh();
        }
    }

    public void RemoveUI(string UiName)
    {
        if (Uis.ContainsKey(UiName))
        {
            Uis.Remove(UiName);
        }
    }

     
    /// <summary>
    /// 获取UI对象
    /// </summary>
    /// <param name="uiName"></param>
    /// <returns></returns>
    public T GetUI<T>(string uiName) where T:MUIBase
    {
        return Uis[uiName] as T;
    }

    /// <summary>
    /// 显示提示框
    /// </summary>
    /// <param name="type"></param>
    /// <param name="one"></param>
    /// <param name="two"></param>
    /// <param name="title"></param>
    /// <param name="content"></param>
    /// <param name="oneText"></param>
    /// <param name="twoText"></param>
    public void ShowAlert(OpenType type,Action one,Action two = null,string title = "",string content = "", string oneText = "",string twoText = "")
    {
        MUIAlert alert = ResourceMgr.CreateUIPrefab("GUIs/Alert/Alert", Canvas).GetComponent<MUIAlert>();
        switch (type)
        {
            case OpenType.Long:
                alert.OpenLong(one, content, title, oneText);
                break;
            case OpenType.TwoButton:
                alert.OpenTwoButton(one, two, content, title, oneText, twoText);
                break;
        }
    }

    public override void Dispose()
    {
        base.Dispose();
        stack.Clear();
        Uis.Clear();
    }
}

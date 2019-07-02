using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 进度条显示组件
/// </summary>
public class MUILoading : MUIBase {

    public Image loadImage;
    public Text messageT;

    public override void OnAwake()
    {
        base.OnAwake();
        loadImage = Utility.FindChild<Image>(transform, "Loading");
        messageT = Utility.FindChild<Text>(transform, "message");
        ResourceMgr.Instance.OnPercent += OnPercentView;//资源加载需要用到进度条
        SceneMgr.Instance.OnPercent += OnPercentView;
    }
     

    /// <summary>
    /// 显示进度
    /// </summary>
    /// <param name="percent"></param>
    /// <param name="message"></param>
    private void OnPercentView(float percent, string message)
    {
         
        loadImage.fillAmount = percent;
        messageT.text = message;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        ResourceMgr.Instance.OnPercent -= OnPercentView;
        SceneMgr.Instance.OnPercent -= OnPercentView;
    }
}

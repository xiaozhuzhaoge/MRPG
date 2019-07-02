using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum OpenType
{
    Long,
    TwoButton
}

public class MUIAlert : MonoBehaviour
{

    public Button[] btns;
    public Text[] texts;
    public Text title;
    public Text content;
    public Button close;

    public void Awake()
    {
        close = Utility.FindChild<Button>(transform, "Close");
        close.onClick.AddListener(() => { Destroy(gameObject); });
        transform.SetAsLastSibling();
    }

    /// <summary>
    /// 打开长键提示框
    /// </summary>
    /// <param name="LongAction"></param>
    /// <param name="content"></param>
    /// <param name="title"></param>
    /// <param name="longText"></param>
    public void OpenLong(Action LongAction, string content = "", string title = "", string longText = "OK")
    {

        btns[0].gameObject.SetActive(true);
        btns[1].gameObject.SetActive(false);
        btns[2].gameObject.SetActive(false);
        btns[0].onClick.AddListener(() =>
        {
            if (LongAction != null)
                LongAction();
            Destroy(gameObject);
        });

        this.content.text = content;
        this.title.text = title;
        texts[0].text = longText;
    
    }
    /// <summary>
    /// 打开双键提示框
    /// </summary>
    /// <param name="LongAction"></param>
    /// <param name="content"></param>
    /// <param name="title"></param>
    /// <param name="longText"></param>
    public void OpenTwoButton(Action leftAction, Action rightAction, string content = "", string title = "", string Left = "Comfirm", string right = "Cancel")
    {
        btns[0].gameObject.SetActive(false);
        btns[1].gameObject.SetActive(true);
        btns[2].gameObject.SetActive(true);
        btns[1].onClick.AddListener(() =>
        {
            if (leftAction != null)
                leftAction();
            Destroy(gameObject);
        });

        btns[2].onClick.AddListener(() =>
        {
            if (rightAction != null)
                rightAction();
            Destroy(gameObject);
        });

        this.content.text = content;
        this.title.text = title;
        texts[1].text = Left;
        texts[2].text = right;
    }
}

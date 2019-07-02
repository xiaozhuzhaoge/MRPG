using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr : MonoSingleton<SceneMgr> {

    public event OnPercentEventHandler OnPercent;
    public event Action OnLoadLevel;
    public event Action OnDone;

    public string SceneName;
    AsyncOperation ao;
    float percent;

    protected override void Initialize()
    {
        base.Initialize();
        ResourceMgr.Instance.OnDone += () => { LoadSceneAsync("Login"); };
    }

    public void LoadSceneAsync(string sceneName)
    {
        MUIMgr.Instance.Dispose();
        MUIMgr.Instance.OpenUI("MUILoading");
        if (OnLoadLevel != null)
            OnLoadLevel();
        StartCoroutine(LoadSceneAsy(sceneName));
       
    }

    IEnumerator LoadSceneAsy(string sceneName)
    {
        percent = 0;
        ao = SceneManager.LoadSceneAsync(sceneName);

        SceneName = sceneName;
        ao.allowSceneActivation = false;
        if(OnPercent != null)
        {
            OnPercent(0f, "开始加载场景" + sceneName);
        }
        while (ao.progress < 0.9f || percent < 1)
        {
            percent += 0.01f;
            if (OnPercent != null)
            {
                OnPercent(percent, String.Format("开始加载场景{0}%" , (int)(percent * 100)));
            }
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(2);
        OnPercent(percent, String.Format("加载完毕", (int)(percent * 100)));
        ao.allowSceneActivation = true;
        if (OnDone != null)
            OnDone();
    }
}

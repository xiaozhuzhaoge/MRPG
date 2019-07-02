using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinView : MonoBehaviour {

    Text goldText;
    Text bloodText;
    Text GemText;
    Button back;

    private void Awake()
    {
        goldText = Utility.FindChild<Text>(transform,"gold");
        bloodText = Utility.FindChild<Text>(transform, "bloodstone");
        GemText = Utility.FindChild<Text>(transform, "gem");
        back = Utility.FindChild<Button>(transform, "Back");
        PlayerData.OnBloodStoreChanged += SetBooldStore;
        PlayerData.OnGemChanged += SetGem;
        PlayerData.OnGoldChanged += SetGold;

        bloodText.text = PlayerData.BloodStore.ToString();
        GemText.text = PlayerData.Gem.ToString();
        goldText.text = PlayerData.Gold.ToString();
        back.onClick.AddListener(() => {
            MUIMgr.Instance.ShowAlert(OpenType.TwoButton, () => {
                SceneMgr.Instance.LoadSceneAsync("Login");
                LunaMessage.ClearAllHandlers();
            }, null, "退出", "是否回到登陆界面", "确定", "取消");
        });
    }

    public void SetGold(int last,int current)
    {
        goldText.text = current.ToString();
    }

    public void SetBooldStore(int last, int current)
    {
        bloodText.text = current.ToString();
    }

    public void SetGem(int last, int current)
    {
        GemText.text = current.ToString();
    }

    private void OnDestroy()
    {
        PlayerData.OnBloodStoreChanged -= SetBooldStore;
        PlayerData.OnGoldChanged -= SetGold;
        PlayerData.OnGemChanged -= SetGem;
    }

}

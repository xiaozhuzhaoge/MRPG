using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

    public Image hp;
    public Image mp;
    public Text hpLabel;
    public Text mpLabel;
    public Text nameLabel;

    private void Awake()
    {
        hp = Utility.FindChild<Image>(transform, "HP");
        mp = Utility.FindChild<Image>(transform, "MP");
        hpLabel = Utility.FindChild<Text>(transform, "hpValue");
        mpLabel = Utility.FindChild<Text>(transform, "mpValue");
        nameLabel = Utility.FindChild<Text>(transform, "name");
    }

    // Use this for initialization
    void Start () {
        nameLabel.text = "";
        
        PlayerData.myRole.OnHpChange += OnHpChange;
        PlayerData.myRole.OnMpChange += OnMpChange;

        hp.fillAmount = 1;
        mp.fillAmount = 1;
        hpLabel.text = String.Format("{0} / {1}", (int)(PlayerData.myRole.CurrentHp), (int)(PlayerData.myRole.MaxHp));
        mpLabel.text = String.Format("{0} / {1}", (int)(PlayerData.myRole.CurrentMp), (int)(PlayerData.myRole.MaxMp));
    }

    private void OnMpChange(float lastMp, float currentMp, float maxMp)
    {
        mp.fillAmount = currentMp / maxMp;
        mpLabel.text = String.Format("{0} / {1}", (int)(currentMp), (int)(maxMp));
    }

    private void OnHpChange(float lastHp, float currentHp, float maxHp)
    {
        hp.fillAmount = currentHp / maxHp;
        hpLabel.text = String.Format("{0} / {1}", (int)(currentHp), (int)(maxHp));
    }


}

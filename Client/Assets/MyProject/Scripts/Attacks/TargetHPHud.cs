using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetHPHud : MonoBehaviour {

    public Image levelIcon;
    public Text targetName;
    public Image hp;
    public CharacterInfo target;
    public RectTransform rect;
    public Vector2 offset;
    public Color Health;
    public Color Bad;

    private void Awake()
    {
        targetName = Utility.FindChild<Text>(transform, "name");
        hp = Utility.FindChild<Image>(transform, "bg");
        levelIcon = Utility.FindChild<Image>(transform, "level");
        rect = transform as RectTransform;
        rect.SetAsFirstSibling();
    }

    private void Start()
    {
        targetName.text = target.Config.Name;
        target.OnDead += (p1,p2,p3)=> { Destroy(gameObject); };
        target.OnHpChange += HpChange;
        hp.color = Health;
    }

    private void HpChange(float lastHp, float currentHp, float maxHp)
    {
        if (lastHp < currentHp)
            return;
        float amount = currentHp / maxHp;
        hp.fillAmount = amount;
        hp.color = Color.Lerp(Bad, Health, amount);
    }


    // Update is called once per frame
    void Update () {

        if (target == null)
        {
            Destroy(gameObject);
        }


        Vector2 uguiPos = RectTransformUtility.WorldToScreenPoint(Camera.main, target.transform.position);
        rect.position = uguiPos + offset;

    }
}

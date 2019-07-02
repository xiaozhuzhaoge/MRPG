using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PopHpCost : MonoBehaviour {

    public Text text;
    RectTransform rect;
    private void Awake()
    {
        text = GetComponent<Text>();
        rect = transform as RectTransform;
        rect.SetAsFirstSibling();
    }
 

    public void SetValue(float value,CharacterInfo target,bool isCritical)
    {
        text.text = value.ToString();
        Vector2 uguiPos = RectTransformUtility.WorldToScreenPoint(Camera.main, target.transform.position);
        rect.position = uguiPos + new Vector2(Random.Range(-50f,50f), Random.Range(-50f, 50f));

        if (isCritical)
        {
            Tweener tween = transform.DOScale(2f, 1);
            text.DOColor(Color.red, 0.2f);
            tween.SetEase(Ease.InOutQuad);
            transform.DOMoveY(transform.position.y + 120, 1).onComplete += () =>
            {
                text.DOFade(0, 1).OnComplete<Tween>(() => { Destroy(gameObject); });
            };
        }
        else
        {
            Tweener tween = transform.DOScale(1.3f, 1);
            tween.SetEase(Ease.InOutQuad);
            transform.DOMoveY(transform.position.y + 120, 1).onComplete += () =>
            {
                text.DOFade(0, 1).OnComplete<Tween>(() => { Destroy(gameObject); });
            };
        }

    }
	 
}

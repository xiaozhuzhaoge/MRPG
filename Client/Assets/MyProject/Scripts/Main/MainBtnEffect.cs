using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class MainBtnEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
 

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOLocalMoveX(-80, 0.5f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOLocalMoveX(-52f, 0.5f);
    }
}

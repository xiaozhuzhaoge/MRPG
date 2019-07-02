using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PropItem : MonoBehaviour, IPropInfoWindow, IPointerEnterHandler, IPointerExitHandler
{
    protected PropInfoConfig propInfo;
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = Vector3.one * 1.1f;
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = Vector3.one;
    }

    public virtual void ShowPropInfo(PropInfoConfig config)
    {
        propInfo = config;
    }

}

public interface IPropInfoWindow {

    void ShowPropInfo(PropInfoConfig config);
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillItemBtn : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    public int Index;
    public bool isPress;


    public KeyCode code;
    Button btn;
    private void Start() {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(()=>{
            AttackMenu.Instance.SetFlag(Index);

        });
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPress = true;
        AttackMenu.Instance.OnButtonClick(Index, isPress);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPress = false;
        AttackMenu.Instance.OnButtonClick(Index, isPress);
    }

    public void Update()
    {
        if (Input.GetKeyDown(code))
        {
            isPress = true;
            AttackMenu.Instance.OnButtonClick(Index, isPress);
        }
        else if(Input.GetKeyUp(code))
        {
            isPress = false;
            AttackMenu.Instance.OnButtonClick(Index, isPress);
        }
    }

}

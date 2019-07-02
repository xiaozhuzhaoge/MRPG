using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public delegate void OnJoyStickerMove(Vector3 dir);

public class JoySticker : MonoSingleton<JoySticker>, IPointerDownHandler, IPointerUpHandler
{

    [SerializeField]
    private RectTransform bg;
    [SerializeField]
    private RectTransform role;
    [SerializeField]
    private Vector2 startPos;
    public bool isHover;
    public float limit;
    private CanvasGroup joySticker;

    public Vector3 dir;
    public OnJoyStickerMove OnJoyMove;
    public bool Move;

    protected override void Initialize()
    {
        bg = transform.Find("JoyStickerBG") as RectTransform;
        role = transform.Find("Role") as RectTransform;
        joySticker = GetComponent<CanvasGroup>();
    }

    // Use this for initialization
    void Start()
    {
        startPos = role.position;
    }

    // Update is called once per frame
    void Update()
    {


        if (Move)
        {
            Vector2 v = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if(v.x != 0 || v.y != 0)
            {
                isHover = true;
            }
            else
            {
                isHover = false;
            }
            dir = v;
        }
        else
        {
            if (isHover)
            {
                OnDraging();
            }
            else if (dir != Vector3.zero && isHover == false)
            {
                RecoverToZero();
            }
        }



    }

    float timeAcc;
    void RecoverToZero()
    {
        timeAcc += Time.fixedDeltaTime;
        dir = Vector3.Lerp(dir, Vector3.zero, timeAcc / 1f);
        if (OnJoyMove != null)
            OnJoyMove(dir);
    }

    void OnDraging()
    {
        joySticker.alpha = 0.6f;
#if UNITY_EDITOR
        role.position = Input.mousePosition;
#elif UNITY_ANDROID
       //Touch[] touches = Input.touches;//手指数组
       //foreach (var mf in touches)
       //{//循环 手指是否含有 -1这个值的
       //    if(mf.id == -1){
       //        continue;
       //    }

       //    if(mf.position.x < Screen.width / 2){
       //        role.pos = mf.position;
       //        break;
       //    }
       //}
#endif
        bg.position = (Vector2)role.position + Vector2.ClampMagnitude(bg.position - role.position, limit);
        dir = bg.InverseTransformPoint(role.position).normalized;
        if (OnJoyMove != null)
            OnJoyMove(dir);
    }

    void ResetRole()
    {
        role.position = bg.position = startPos;
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        isHover = false;
        joySticker.alpha = 0;
        if (OnJoyMove != null)
            OnJoyMove(dir);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ResetRole();
        bg.position = role.transform.position = eventData.position;
        joySticker.alpha = 0.6f;
        isHover = true;
        timeAcc = 0;
        if (OnJoyMove != null)
            OnJoyMove(dir);
    }

}

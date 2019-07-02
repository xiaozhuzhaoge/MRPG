using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.StateMachine)]
    public class JoyStickerRotation : FsmStateAction
    {
        [RequiredField]
        [CheckForComponent(typeof(Animator))]
        [Tooltip("The target.")]
        public FsmOwnerDefault gameObject;

        GameObject go;
        Character role;
        public override void Reset()
        {
            gameObject = null;

        }

        public override void OnEnter()
        {
            base.OnEnter();
            go = Fsm.GetOwnerDefaultTarget(gameObject);
            role = go.GetComponent<Character>();
            if (go == null)
            {
                Finish();
                return;
            }
        }

        Vector3 dir;
        public override void OnUpdate()
        {
            base.OnUpdate();
            if(JoySticker.Instance != null)
            {
                if (JoySticker.Instance.isHover)
                {
                    dir = JoySticker.Instance.dir;
                    Vector3 moveDir = Camera.main.transform.TransformDirection(new Vector3(dir.x, 0, dir.y));
                    moveDir.y = 0;
                    if (moveDir != Vector3.zero)
                    {
                        go.transform.rotation = Quaternion.Slerp(role.transform.rotation, Quaternion.LookRotation(moveDir), Time.deltaTime * role.rotationSpeed);
                    }

                }
           
            }
        }
         
    }

}
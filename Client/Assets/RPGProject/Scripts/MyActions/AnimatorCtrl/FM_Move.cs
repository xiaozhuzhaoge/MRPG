using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.StateMachine)]
    public class FM_Move : FsmStateAction
    {
        [RequiredField]
        [CheckForComponent(typeof(Animator))]
        public FsmOwnerDefault gameObject;
        public CharacterCtrlBase role;
        Animator _animator;
        Vector3 dir;
        GameObject go;
        [Tooltip("变量字段名")]
        public FsmString fieldName;
        public FsmBool CanMove;
        public FsmBool CanRotation;
        public FsmBool CanChangeAnimator;

        public override void Reset()
        {
            gameObject = null; 
            _animator = null;
        }

        public override void OnEnter()
        {
            go = Fsm.GetOwnerDefaultTarget(gameObject);
            _animator = go.GetComponent<Animator>();
            role = go.GetComponent<CharacterCtrlBase>();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            dir = JoySticker.Instance.dir;
            Vector3 moveDir = Camera.main.transform.TransformDirection(new Vector3(dir.x, 0, dir.y));
            moveDir.y = 0;
            if (CanMove.Value)
            {
                role.cc.SimpleMove(moveDir * role.speed);
            }
               
            if (CanRotation.Value)
                if (moveDir != Vector3.zero)
                {
                    go.transform.rotation = Quaternion.Slerp(go.transform.rotation, Quaternion.LookRotation(moveDir), role.rotationSpeed * Time.deltaTime);
                }
            if (CanChangeAnimator.Value)
                _animator.SetFloat(fieldName.Value, dir.magnitude * role.speed);
        }

        
    }
}

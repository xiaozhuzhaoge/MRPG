using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HutongGames.PlayMaker.Actions
{
    public class RootMotionFlag : FsmStateAction
    {
        public FsmOwnerDefault gameObject;
        public Animator ani;
        public FsmBool flag;


        public override void OnEnter()
        {
            //gameobject 当前挂载脚本的游戏物体
            base.OnEnter();

            GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
            ani = go.GetComponent<Animator>();
            ani.applyRootMotion = flag.Value;
        }
      
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HutongGames.PlayMaker.Actions
{
    public class KeyCodeListener : FsmStateAction
    {
        public KeyCode[] codes;
        public FsmEvent[] events;
        public FsmFloat[] triggerTime;//触发时间
        PlayMakerFSM fsm;
        public FsmOwnerDefault gameObject;
        public Animator ani;

        public override void OnEnter()
        {
            //gameobject 当前挂载脚本的游戏物体
            base.OnEnter();

            GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
            fsm = go.GetComponent<PlayMakerFSM>();
            ani = go.GetComponent<Animator>();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            AnimatorStateInfo info = ani.GetCurrentAnimatorStateInfo(0);
            for (int i = 0; i < codes.Length; i++)
            {
                if (Input.GetKeyDown(codes[i]) && info.normalizedTime > triggerTime[i].Value)
                {
                    fsm.SendEvent(events[i].Name);
                }
            }
        }
    }
}

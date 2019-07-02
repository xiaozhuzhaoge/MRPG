using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    public class AnimationPlayReturnState : FsmStateAction
    {
        public FsmString ReturnStateName;
        public FsmFloat NormlizedTime;

        public FsmOwnerDefault owner;
        GameObject go;
        Animator ani;
        PlayMakerFSM fsm;
        AnimatorCrossFade play;
        public override void OnEnter()
        {
            base.OnEnter();
            go = Fsm.GetOwnerDefaultTarget(owner);
            ani = go.GetComponent<Animator>();
            fsm = go.GetComponent<PlayMakerFSM>();

            if (play == null)
                for (int i = 0; i < State.Actions.Length; i++)
                {
                    if (State.Actions[i] is AnimatorCrossFade)
                    {
                        play = (State.Actions[i] as AnimatorCrossFade);
                        break;
                    }
                }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            AnimatorStateInfo state = ani.GetCurrentAnimatorStateInfo(0);
            if (state.normalizedTime > NormlizedTime.Value && state.IsName(play.stateName.Value))
            {
                fsm.SetState(ReturnStateName.Value);
            }
        }

    }
}


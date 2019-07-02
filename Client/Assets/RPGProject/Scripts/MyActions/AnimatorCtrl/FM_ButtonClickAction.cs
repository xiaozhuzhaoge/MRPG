using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.StateMachine)]
    public class ButtonClickAction : FsmStateAction
    {
        [RequiredField]
        public FsmEvent[] events;

        [RequiredField]
        [UIHint(UIHint.FsmArray)]
        public FsmFloat[] NormalizedTime;

        [RequiredField]
        public FsmOwnerDefault gameObject;
        
        float startTime;
        public FsmInt eventIndex;


        UnityEngine.Animator _animator;

        bool StartUpdate;

        public override void Reset()
        {
            gameObject = null;
            events = new FsmEvent[1];
            _animator = null;
        }

        public override void OnEnter()
        {
             
            var go = Fsm.GetOwnerDefaultTarget(gameObject);
            _animator = go.GetComponent<Animator>();
            eventIndex.Value = -1;
            if (AttackMenu.Instance.flagCallback.Length < events.Length)
                return;

                for (int i = 0; i < events.Length; i++)
            {
                
                AttackMenu.Instance.flagCallback[Convert.ToInt32(events[i].Name)] -= OnButtonClick;
                AttackMenu.Instance.flagCallback[Convert.ToInt32(events[i].Name)] += OnButtonClick;
            }
        }

        public void OnButtonClick(int index,bool flag) {

            int realIndex = GetEventIndex(index);
            if(realIndex != -1)
            {
                if (NormalizedTime == null || NormalizedTime.Length != events.Length)
                {
                    Fsm.Event(events[realIndex]);
                    Finish();
                    RemoveAllClickEvents();
                    return;
                }

                if (NormalizedTime[realIndex].Value < 0.001f)
                {
                    Fsm.Event(events[realIndex]);
                    RemoveAllClickEvents();
                    Finish();
                }
                else
                {
                    eventIndex = realIndex;
                    StartUpdate = true;
                    RemoveAllClickEvents();
                }

            }

            return;
        }

        public void RemoveAllClickEvents()
        {
            for (int i = 0; i < events.Length; i++)
            {
                AttackMenu.Instance.flagCallback[Convert.ToInt32(events[i].Name)] -= OnButtonClick;
            }
        }

        public override void OnUpdate()
        {
            //FixChangeAnimationBug();
            //Debug.Log(NormalizedTime.Length + "  " + eventIndex.Value);
            //Debug.Log(AttackMenu.Instance.flags[2] + "  " + AttackMenu.Instance.flagCallback[2]);
            if (StartUpdate == false || eventIndex.Value == -1)
                return;
            if (NormalizedTime != null && NormalizedTime.Length == events.Length)
            {
                if (NormalizedTime[eventIndex.Value].Value != 0)
                {
                    for(int i = 0; i < Fsm.ActiveState.Actions.Length; i++)
                    {
                        if(Fsm.ActiveState.Actions[i] as AnimatorCrossFade != null)
                        {
                            AnimatorCrossFade data = Fsm.ActiveState.Actions[i] as AnimatorCrossFade;
                            if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= NormalizedTime[eventIndex.Value].Value && 
                                _animator.GetCurrentAnimatorStateInfo(0).IsName(data.stateName.Value))
                            {
                                Fsm.Event(events[eventIndex.Value]);
                                Finish();
                            }
                            return;
                        }
                    }
                    
                }
            }
        }

        /// <summary>
        /// 由于状态跳转动画播放和状态机跳转之间会产生异步导致动画播放卡主，所以进行动画状态判断，如果当前状态动画名和播放不一样，重新播放一边
        /// </summary>
        public void FixChangeAnimationBug() {
            for (int i = 0; i < Fsm.ActiveState.Actions.Length; i++)
            {
                if (Fsm.ActiveState.Actions[i] as AnimatorCrossFade != null)
                {
                    AnimatorCrossFade data = Fsm.ActiveState.Actions[i] as AnimatorCrossFade;
                    ///如果当前播放的动画状态与状态内设置不一 强制跳转动画
                    if (!_animator.GetCurrentAnimatorStateInfo(0).IsName(data.stateName.Value))
                    {
                        _animator.CrossFade(data.stateName.Value,0);
                    }
                }

            }
        }

        public FsmEvent GetEventFromBtnIndex(int index)
        {
            for(int i = 0; i < events.Length; i++)
            {
                if(Convert.ToInt32(events[i].Name) == index)
                {
                    return events[i];
                }
            }

            return null;
        }

        public int GetEventIndex(int index)
        {
            for (int i = 0; i < events.Length; i++)
            {
                if (Convert.ToInt32(events[i].Name) == index)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}

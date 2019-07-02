using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HutongGames.PlayMaker.Actions
{
    public class PlayMakerFSMB : FsmStateAction
    {
        BirdgeCaches caches;
        public FsmOwnerDefault gameObject;
        public bool EnableLateUpdate;
        public bool EnableFixedUpdate;

        public override void Awake()
        {
            base.Awake();
            GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
            caches = go.GetComponent<BirdgeCaches>();

            C_Lua_Bridge birdge = new C_Lua_Bridge(State.Name,go, caches.prefix_脚本前缀);
            caches.AddBirdage(birdge.BirdageId, birdge);
            Fsm.HandleLateUpdate = EnableLateUpdate;
            Fsm.HandleFixedUpdate = EnableFixedUpdate;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            caches.currentBirdge = caches.GetBirdage(State.Name);
            caches.currentBirdge.OnEnter();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            caches.currentBirdge.OnUpdate();
 
        }

        public override void OnExit()
        {
            base.OnExit();
            caches.currentBirdge.OnExit();
        }
         

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
 
        }

        public override void OnLateUpdate()
        {
            base.OnLateUpdate();
 
            caches.currentBirdge.OnLateUpdate();
        }

    }

}
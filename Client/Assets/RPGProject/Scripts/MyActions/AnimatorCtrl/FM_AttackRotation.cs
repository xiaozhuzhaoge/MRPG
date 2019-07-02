using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    

    public class FM_AttackRotation : FsmStateAction
    {
        public FsmOwnerDefault gameobject;
        public CharacterCtrlBase anieve;
        GameObject go;

        public override void Awake()
        {
            base.Awake();
            go = Fsm.GetOwnerDefaultTarget(gameobject);
            anieve = go.GetComponent<CharacterCtrlBase>();
        }


        public override void OnUpdate()
        {
            base.OnUpdate();
            if(anieve.target != null)
            {
                go.transform.rotation = Quaternion.LookRotation(anieve.target.transform.position - go.transform.position);
            }
        }
    }

}

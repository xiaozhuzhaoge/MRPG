using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HutongGames.PlayMaker.Actions
{
   
    public class FM_AttackFollow : FsmStateAction
    {
        public FsmOwnerDefault gameobject;
        public CharacterCtrlBase anieve;
        GameObject go;
        
        /// <summary>
        /// 检测半径
        /// </summary>
        public FsmFloat checkRadius = 5;

        public override void Awake()
        {
            base.Awake();
            go = Fsm.GetOwnerDefaultTarget(gameobject);
            anieve = go.GetComponent<CharacterCtrlBase>();
            
        }

        public override void OnEnter()
        {
            base.OnEnter();
            
        }

        RaycastHit hitInfo;
        public Transform hitTarget;
        public override void OnUpdate()
        {
            base.OnUpdate();
            if (anieve.target != null)
            {
                go.transform.rotation = Quaternion.LookRotation(anieve.target.transform.position - go.transform.position);
            }
            if (Physics.SphereCast(go.transform.position, checkRadius.Value, go.transform.forward, out hitInfo, 0.1f, 1 << anieve.GetAttackLayer())) {
                Debug.Log("球形射线碰撞" + hitInfo.transform);
                hitTarget = hitInfo.transform;
            }
           
        }
    }

}
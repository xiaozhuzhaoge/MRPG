using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace HutongGames.PlayMaker.Actions
{
    public class FM_AI_Follow : FsmStateAction
    {
        public FsmOwnerDefault gameObject;
        public Transform 玩家;
        public NavMeshAgent 寻路组件;
        public float 警戒范围半径;
        public float 攻击范围半径;
        public GameObject self;
        public PlayMakerFSM fsm;
        public Animator ani;
        public override void Awake()
        {
            self = Fsm.GetOwnerDefaultTarget(gameObject);
            base.Awake();
            
            寻路组件 = self.GetComponent<NavMeshAgent>();
            玩家 = GameObject.FindGameObjectWithTag("Player").transform;
            fsm = self.GetComponent<PlayMakerFSM>();
            ani = self.GetComponent<Animator>();

        }


        public override void OnUpdate()
        {
            base.OnUpdate();

            float disntace_与玩家距离 = Vector3.Distance(self.transform.position, 玩家.position);

            if (disntace_与玩家距离 > 警戒范围半径)
            {
                fsm.SetState("巡逻");
           
            }

            if (disntace_与玩家距离 <= 攻击范围半径)
            {
                fsm.SendEvent("进入红圈");
            }

            寻路组件.SetDestination(玩家.position);
            ani.SetFloat("Speed", 寻路组件.velocity.magnitude);

        }
    }

}

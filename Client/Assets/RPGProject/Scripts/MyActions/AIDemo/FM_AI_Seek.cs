using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace HutongGames.PlayMaker.Actions
{
    public class FM_AI_Seek : FsmStateAction
    {
        public FsmOwnerDefault gameObject;
        public float 巡逻半径;
        public Vector3 出生点;
        public Transform 玩家;
        public NavMeshAgent 寻路组件;
        public float 警戒范围半径;
        public GameObject self;
        public Vector3 当前巡逻点;
        public PlayMakerFSM fsm;
        public Animator ani;
        public override void Awake()
        {
            self = Fsm.GetOwnerDefaultTarget(gameObject);
            base.Awake();
            出生点 = self.transform.position;
            寻路组件 = self.GetComponent<NavMeshAgent>();
            玩家 = GameObject.FindGameObjectWithTag("Player").transform;
            fsm = self.GetComponent<PlayMakerFSM>();
            ani = self.GetComponent<Animator>();

        }

        public override void OnEnter()
        {
            base.OnEnter();
            当前巡逻点 = 出生点 + new Vector3(Random.Range(-巡逻半径, 巡逻半径), 0, Random.Range(-巡逻半径, 巡逻半径));
            寻路组件.enabled = true;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            float disntace_与玩家距离 = Vector3.Distance(self.transform.position, 玩家.position);
            if(disntace_与玩家距离 <= 警戒范围半径)
            {
                fsm.SendEvent("进入绿圈");
            }


            float distance_到达巡逻点的距离 = Vector3.Distance(self.transform.position, 当前巡逻点);
            if (distance_到达巡逻点的距离 < 1)
            {
                当前巡逻点 = 出生点 + new Vector3(Random.Range(-巡逻半径, 巡逻半径), 0, Random.Range(-巡逻半径, 巡逻半径));
            }

            寻路组件.SetDestination(当前巡逻点);//执行巡逻
            ani.SetFloat("Speed", 寻路组件.velocity.magnitude);



        }
    }

}

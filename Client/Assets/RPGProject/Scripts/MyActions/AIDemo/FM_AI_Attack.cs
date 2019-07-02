using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace HutongGames.PlayMaker.Actions
{
    public class FM_AI_Attack : FsmStateAction
    {
        public FsmOwnerDefault gameObject;
        public Transform 玩家;
        public float 攻击范围半径;
        public GameObject self;
        public PlayMakerFSM fsm;
        public Animator ani;
        public NavMeshAgent agent;

        public override void Awake()
        {
            self = Fsm.GetOwnerDefaultTarget(gameObject);
            base.Awake();
            玩家 = GameObject.FindGameObjectWithTag("Player").transform;
            fsm = self.GetComponent<PlayMakerFSM>();
            ani = self.GetComponent<Animator>();
            agent = self.GetComponent<NavMeshAgent>();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            agent.enabled = false;
            Vector3 dir = (玩家.transform.position - self.transform.position).normalized;
            dir.y = 0;
            self.transform.rotation = Quaternion.LookRotation(dir);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            int ran = Random.Range(0, 2);
            if(ran == 0)
            {
                Fsm.SetState("攻击1");
            }
            else
            {
                Fsm.SetState("攻击2");
            }
        }
    }

}

 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using XLua;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayMakerFSM))]
[RequireComponent(typeof(CharacterInfo))]
[RequireComponent(typeof(HitEffect))]
public class Character : CharacterCtrlBase, IHitAnalysis
{
    [HideInInspector]
    public NavMeshObstacle nmo;
    [HideInInspector]
    public NavMeshAgent nma;
 
    [Tooltip("角色背面受击状态名")]
    public string[] groundHitBackState;
    [Tooltip("角色正面受击状态名")]
    public string[] groundHitForwardState;

    /// <summary>
    /// 受击高亮效果组件
    /// </summary>
    [HideInInspector]
    public HitEffect hiteffect;
    /// <summary>
    /// 角色基本数据
    /// </summary>
    [HideInInspector]
    public CharacterInfo characterInfo;

    /// <summary>
    /// 状态机模板名字
    /// </summary>
    public string fsmTemplateName;

    public override void OnAwake()
    {
        base.OnAwake();
        gameObject.layer = LayerMask.NameToLayer(SelfLayer);
        nmo = GetComponent<NavMeshObstacle>();
        nma = GetComponent<NavMeshAgent>();
        characterInfo = GetComponent<CharacterInfo>();
        fsm.SetFsmTemplate(ResourceMgr.Load<FsmTemplate>("FSMTemplate/" + fsmTemplateName));
        hiteffect = GetComponent<HitEffect>();
        characterInfo.OnDead += Dead;
        
    }

    private void Dead(float lastHp, float currentHp, float maxHp)
    {
        if (isDead == true)
            return;
        isDead = true;
        if (nma != null)
            nma.enabled = false;
        cc.enabled = false;
        fsm.SetState("Dead");
    }

    /// <summary>
    /// 受击做不同的状态逻辑
    /// </summary>
    /// <param name="dot">点积</param>
    /// <param name="from">来源对象</param>
    public void DoHitState(float dot, Transform from)
    {
        float isFoward = Vector3.Dot(transform.forward, (from.position - transform.position).normalized);
        if (isFoward >= 0)
        {
            if (groundHitForwardState.Length > 0)
                fsm.SetState(groundHitForwardState[Random.Range(0, groundHitForwardState.Length)]);
        }
        else
        {
            if (groundHitBackState.Length > 0)
                fsm.SetState(groundHitBackState[Random.Range(0, groundHitBackState.Length)]);
        }

    }

    /// <summary>
    /// 受击接口
    /// </summary>
    /// <param name="args"></param>
    public void BeHit(params object[] args)
    {
        ///如果不为空 则高亮显示受击效果
        if(hiteffect != null)
            hiteffect.BeHit(args);
        SkillData skillData = args[0] as SkillData;
        switch(skillData.atkType)
        {
            case SkillData.AttackType.Near:
                Character nfrom = args[1] as Character;

                RaycastHit hit = (RaycastHit)args[2];
                NearHit(skillData.nearSkillData, nfrom, hit, skillData);
                break;
            case SkillData.AttackType.Throw:
                Character tfrom = args[1] as Character;
                Transform box = args[2] as Transform;
                ThrowHit(skillData.throwSkillData, tfrom, box, skillData);
                break;
        }
    }

    /// <summary>
    /// 是否暴击
    /// </summary>
    [HideInInspector]
    public bool IsCritical;

    public void NearHit(NearSkillData nearSkillData, Character from,RaycastHit hit,SkillData skill)
    {
        IsCritical = false;
        var effect= ResourceMgr.CreateEffect(nearSkillData.HitEffect, hit.point);
        effect.transform.LookAt(hit.normal);
        if (nearSkillData.ReParent)
            effect.transform.SetParent(transform);
        ///暴击逻辑
        if (Utility.Roll(from.characterInfo.Cri))
        {
            IsCritical = true;
            characterInfo.CurrentHp -= ((skill.SkillAttack * from.characterInfo.Atk) - characterInfo.Def * 0.3f) * 1.5f;
        }
        else
        {
            characterInfo.CurrentHp -= ((skill.SkillAttack * from.characterInfo.Atk) - characterInfo.Def * 0.3f);
        }
    }

    public void ThrowHit(ThrowSkillData throwSkillData, Character from, Transform box, SkillData skill) {

        ResourceMgr.CreateEffect(throwSkillData.HitEffect, transform.position);

        ///暴击逻辑
        if (Utility.Roll(from.characterInfo.Cri))
        {
            characterInfo.CurrentHp -= ((skill.SkillAttack * from.characterInfo.Atk) - characterInfo.Def * 0.3f) * 1.5f;
        }
        else
        {
            characterInfo.CurrentHp -= ((skill.SkillAttack * from.characterInfo.Atk) - characterInfo.Def * 0.3f);
        }
    }
 
    public void SetMoveSpeedValue(float moveSpeed)
    {
        speed = moveSpeed;
        if (nma != null)
            nma.speed = moveSpeed;
    }

    public void SetAtkSpeedValue(float atkSpeed)
    {
        ani.speed = atkSpeed;
    }

}

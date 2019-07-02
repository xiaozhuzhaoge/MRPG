using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void DoEvetHandler(string stateName);

/// <summary>
/// 角色类战斗框架基类
/// </summary>
public class CharacterCtrlBase : MonoBehaviour
{
    ///是否使用帧冻结
    public bool useFrezzen;
    ///是否死亡
    [HideInInspector]
    public bool isDead;
    [HideInInspector]
    public CharacterController cc;
    ///基本移动速度
    public float speed = 3.5f;
    [HideInInspector]
    public Animator ani;
    ///基本旋转速度
    public float rotationSpeed = 10;
    ///自身攻击层级
    public string SelfLayer;
    ///技能组ID
    public int GroupIdFromConfig;
    public int FreezeFrameTime;

    ///当前锁定目标
    public Transform target;
    [HideInInspector]
    public PlayMakerFSM fsm;

    #region Life_Recycle
    private void Awake() {
       OnAwake();
    }

    private void Start() {
        OnStart();
    }


    public virtual void OnAwake(){
        fsm = GetComponent<PlayMakerFSM>();
        cc = GetComponent<CharacterController>();
        ani = GetComponent<Animator>();
        Utility.SetLayerWithChildren(gameObject, LayerMask.NameToLayer(SelfLayer));
    }

    public virtual void OnStart()
    {
        
        AnimationEvents();
    }
 

    /// <summary>
    /// 动画事件注册并且根据数据表动态匹配伤害逻辑
    /// </summary>
    public virtual void AnimationEvents()
    {
        if(GroupIdFromConfig == 0)
            return;
        Debug.Log(ani + " " + GroupIdFromConfig);
        AnimeMgr.RegisterAniEvent(ani, GroupIdFromConfig);
    }

    #endregion

    #region 技能脚本

    public virtual void CastEffect(string value)
    {
        SkillData data = new SkillData();
        ///获取技能指令
        data.SetJsonData(value);
        switch (data.Type)
        {
            case "Radius"://产生扇形检测区域 第一个碰到停止
                RadiusCastHit(data);
                break;
            case "RadiusPenetrated"://产生扇形检测穿透区域 带穿透
                RadiusCastHitAll(data);
              
                break;
            case "SpherePenetrated"://产生球形穿透区域 拉克丝大招
                CastSphereRayDirectionAll(data);
               
                break;
            case "Sphere"://产生球形区域 拉克丝大招 第一个碰到停止
                CastSphereRayDirection(data);
              
                break;
            case "ThrowBox"://投掷物 第一个碰到停止
                ThrowBox(data);
                break;
            case "ThrowBoxPenetrated"://投掷物穿透
                ThrowBoxAll(data);
                break;

        }

        ///创建额外攻击特效
        if(data.nearSkillData != null)
           CreateEffect(data.nearSkillData.EffectData);

    }

    /// <summary>
    /// 本次受击对象
    /// </summary>
    public Queue<IHitAnalysis> BeHits = new Queue<IHitAnalysis>();
	public RaycastHit lastHitInfo;

    /// <summary>
    /// 射线推送碰撞信息
    /// </summary>
    /// <param name="from"></param>
    /// <param name="hit"></param>
    /// <param name="data"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public bool HitEnemey(Transform from, RaycastHit hit, SkillData data, Action callback = null)
    {
        IHitAnalysis beHit = hit.collider.GetComponent<IHitAnalysis>();
		lastHitInfo = hit;
        if(beHit != null)
        if (!BeHits.Contains(beHit))
        {
            BeHits.Enqueue(beHit);
            beHit.BeHit(data, this, hit);
            if(useFrezzen)
                FreezeFrame(FreezeFrameTime);//帧冻结 可以变成变量

            if (callback != null)
            {
                callback();
            }
            return true;
        }
        return false;
    }
    #endregion

    #region UseRay
    
    /// <summary>
    /// 扇形检测范围
    /// </summary>
    /// <param name="angle"></param>
    /// <param name="lookAccurate"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    public bool RadiusCastHit(SkillData data)
    {
        ///清除缓存
        BeHits.Clear();
        RaycastHit hit;
        float subAngle = (data.nearSkillData.Angle / 2) / data.nearSkillData.RayNum;
        bool isHit = false;

        for (int i = 0; i < data.nearSkillData.RayNum; i++)
        {
            if (CastRadiusRay(Quaternion.Euler(0, -1 * subAngle * (i + 1), 0),data.nearSkillData.Radius, out hit, Color.red, GetAttackLayer()))
            {
                HitEnemey(transform, hit ,data, () => { isHit = true; });
            }
            if (CastRadiusRay(Quaternion.Euler(0, 1 * subAngle * (i + 1), 0), data.nearSkillData.Radius, out hit, Color.red, GetAttackLayer()))
            {
                HitEnemey(transform, hit, data,  () => { isHit = true; });
            }
        }
        return isHit;
    }
 
    
    /// <summary>
    /// 扇形检测范围穿透
    /// </summary>
    /// <param name="angle"></param>
    /// <param name="lookAccurate"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    public bool RadiusCastHitAll(SkillData data)
    {
        ///清除缓存
        BeHits.Clear();

        float subAngle = (data.nearSkillData.Angle / 2) / data.nearSkillData.RayNum;
        bool isHit = false;
        List<RaycastHit> hits = new List<RaycastHit>();
        for (int i = 0; i < data.nearSkillData.RayNum; i++)
        {
            if (CastRadiusRayAll(Quaternion.Euler(0, -1 * subAngle * (i + 1), 0), data.nearSkillData.Radius, out hits, Color.red, GetAttackLayer()))
            {
                for (int j = 0; j < hits.Count; j++)
                {
                    HitEnemey(transform, hits[j], data,  () => { isHit = true; });
                }

            }
            if (CastRadiusRayAll(Quaternion.Euler(0, 1 * subAngle * (i + 1), 0), data.nearSkillData.Radius, out hits, Color.red, GetAttackLayer()))
            {
                for (int j = 0; j < hits.Count; j++)
                {
                    HitEnemey(transform, hits[j], data,  () => { isHit = true; });
                }

            }
        }
        return isHit;
    }
    /// <summary>
    /// 发送扇形体射线检测 
    /// </summary>
    /// <param name="eulerAngler">偏移角</param>
    /// <param name="range">发射距离</param>
    /// <param name="hit">碰撞信息</param>
    /// <param name="debugColor">debug颜色</param>
    /// <param name="yoffset">厚度</param>
    /// <param name="duration">debug持续时间</param>
    /// <returns></returns>
    public bool CastRadiusRay(Quaternion eulerAngler, float range, out RaycastHit hit, Color debugColor, int layMask, float yoffset = 1, float duration = 2)
    {
        Debug.DrawRay(transform.position + new Vector3(0, yoffset, 0), eulerAngler * transform.forward * range, debugColor);
        return  Physics.Raycast(transform.position + new Vector3(0, yoffset, 0), eulerAngler * transform.forward, out hit, range, 1 << layMask);
    }

    /// <summary>
    /// 发送扇形体射线检测穿透 
    /// </summary>
    /// <param name="eulerAngler">偏移角</param>
    /// <param name="range">发射距离</param>
    /// <param name="hit">碰撞信息</param>
    /// <param name="debugColor">debug颜色</param>
    /// <param name="yoffset">厚度</param>
    /// <param name="duration">debug持续时间</param>
    /// <returns></returns>
    public bool CastRadiusRayAll(Quaternion eulerAngler, float range, out List<RaycastHit> hits, Color debugColor, int layMask, float yoffset = 1, float duration = 2)
    {
        hits = new List<RaycastHit>();
        //Debug.DrawRay(transform.position, eulerAngler * transform.forward * range, debugColor,5f);
        Debug.DrawRay(transform.position + new Vector3(0, yoffset, 0), eulerAngler * transform.forward * range, debugColor, 5f);
   
        //RaycastHit[] hitdown = Physics.RaycastAll(transform.position, eulerAngler * transform.forward, range, 1 << layMask);
        RaycastHit[] hitup = Physics.RaycastAll(transform.position + new Vector3(0, yoffset, 0), eulerAngler * transform.forward, range, 1 << layMask);

        //for (int i = 0; i < hitdown.Length; i++)
            //hits.Add(hitdown[i]);
        for (int i = 0; i < hitup.Length; i++)
            hits.Add(hitup[i]);
        
        return hits.Count != 0;
    }
 
    /// <summary>
    /// 发送球形射线指定方向 穿透所有
    /// </summary>
    /// <param name="localPos">相对坐标</param>
    /// <param name="range">范围</param>
    /// <param name="localDir">相对方向</param>
    /// <param name="Distance">距离</param>
    /// <param name="layMask">层级</param>
    /// <returns></returns>
    public RaycastHit[] CastSphereRayDirectionAll(SkillData data)
    {
        Debug.Log("球形射线");
        Debug.DrawRay(transform.TransformPoint(data.nearSkillData.LocalPosition), transform.TransformDirection(data.nearSkillData.LocalDir) * (data.nearSkillData.Distance + data.nearSkillData.Radius), Color.blue, 1);
        BeHits.Clear();
        RaycastHit[] hits = Physics.SphereCastAll(transform.TransformPoint(data.nearSkillData.LocalPosition), 
            data.nearSkillData.Radius, transform.TransformDirection(data.nearSkillData.LocalDir), data.nearSkillData.Distance, 1 << GetAttackLayer());
        for (int i = 0; i < hits.Length; i++)
        {
            HitEnemey(transform, hits[i],data);
        }
        return hits;
    }

 
    /// <summary>
    /// 发送球形射线指定方向 碰撞第一个消失
    /// </summary>
    /// <param name="localPos">相对坐标</param>
    /// <param name="range">范围</param>
    /// <param name="localDir">相对方向</param>
    /// <param name="Distance">距离</param>
    /// <param name="layMask">层级</param>
    /// <returns></returns>
    public bool CastSphereRayDirection(SkillData data)
    {
        Debug.Log("球形射线");
        Debug.DrawRay(transform.TransformPoint(data.nearSkillData.LocalPosition), transform.TransformDirection(data.nearSkillData.LocalDir) * (data.nearSkillData.Distance + data.nearSkillData.Radius), Color.blue, 1);
        BeHits.Clear();
        RaycastHit hit;
        if (Physics.SphereCast(transform.TransformPoint(data.nearSkillData.LocalPosition), data.nearSkillData.Radius, transform.TransformDirection(data.nearSkillData.LocalDir), out hit, data.nearSkillData.Distance, 1 << GetAttackLayer()))
        {
            if (HitEnemey(transform, hit, data))
                return true;
        }

        return false;
    }

    #endregion

    #region UseCoilider

    /// <summary>
    /// 碰撞第一个
    /// </summary>
    /// <param name="data"></param>
    public void ThrowBox(SkillData data)
    {
        ThrowHit hit= CreateThrowEffect(data);
        hit.OneHit = true;
    }

 
    /// <summary>
    /// 穿透碰撞
    /// </summary>
    /// <param name="data"></param>
    public void ThrowBoxAll(SkillData data)
    {
        ThrowHit hit = CreateThrowEffect(data);
        hit.OneHit = false;
    }

    public ThrowHit CreateThrowEffect(SkillData data)
    {
        var go = ResourceMgr.CreateEffect(data.throwSkillData.Appearance);
        if (go == null)
            return null;
        go.transform.position = transform.TransformPoint(data.throwSkillData.LocalPosition);
        Rigidbody rig = go.AddComponent<Rigidbody>();
        rig.useGravity = false;
        rig.velocity = transform.TransformDirection(data.throwSkillData.LocalDir) * data.throwSkillData.Speed;
        go.transform.rotation = Quaternion.LookRotation(transform.TransformDirection(data.throwSkillData.LocalDir));
        go.AddComponent<DestorySelf>().delay = data.throwSkillData.Duration;
        ThrowHit hit = go.AddComponent<ThrowHit>();
        hit.from = this;
        hit.skillData = data;
        
        Utility.SetLayerWithChildren(go, LayerMask.NameToLayer(SelfLayer));
        if (data.throwSkillData.ReParent)
        {
            go.transform.SetParent(transform);
        }
        return hit;
    }

    /// <summary>
    /// 获取当前攻击层级
    /// </summary>
    /// <returns></returns>
    public int GetAttackLayer()
    {
        if(LayerMask.NameToLayer(SelfLayer) == LayerMask.NameToLayer("Player"))
        {
            return LayerMask.NameToLayer("Enemy");
        }
        if (LayerMask.NameToLayer(SelfLayer) == LayerMask.NameToLayer("Enemy"))
        {
            return LayerMask.NameToLayer("Player");
        }
        return 0;
    }

    #endregion

    #region Effect
 
    /// <summary>
    /// 生成特效
    /// </summary>
    /// <param name="effect"></param>
    /// <param name="pos"></param>
    /// <param name="dir"></param>
    /// <param name="time"></param>
    /// <param name="angle"></param>
    public void CreateEffect(AppearEffectData data)
    {
        
        if(data != null)
        {
            var go = ResourceMgr.CreateObj("Effects/"+ data.EffectName);
            Debug.Log(data.EffectName);
            if (go == null) return;
            go.transform.position = transform.TransformPoint(data.LocalPosition);
            go.transform.rotation = Quaternion.LookRotation(transform.TransformDirection(data.LocalDir));
            go.AddComponent<DestorySelf>().delay = data.Duration;
            if (data.Reparent)
                go.transform.SetParent(transform);
        }
       
    }


    #endregion


    /// <summary>
    /// 检测是否落地
    /// </summary>
    public virtual void CheckOnGround()
    {
    }

    /// <summary>
    /// 帧冻结 
    /// </summary>
    public void FreezeFrame(int frame)
    {
        ani.speed = 0.2f;
        Utility.Instance.WaitForFrame(frame, () => { ani.speed = 1; });
    }

}

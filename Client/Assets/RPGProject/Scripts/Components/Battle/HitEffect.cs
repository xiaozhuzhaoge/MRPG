using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HitEffect : MonoBehaviour
{
    [Tooltip("单位的渲染器 要高亮显示受击一定要加上")]
    public List<Renderer> renders = new List<Renderer>();
    public List<Material> mats = new List<Material>();
    public List<Shader> shader = new List<Shader>();
 
    public void Awake()
    {
        FindRenders();
        renders.ForEach(render => { shader.Add(render.material.shader); mats.Add(render.material); });
        
    }

    public void BeHit(params object[] args)
    {
        Utility.BeHitEffect(mats, shader);
    }

    public void FindRenders()
    {
        renders = transform.GetComponentsInChildren<Renderer>().ToList<Renderer>();
    }
}

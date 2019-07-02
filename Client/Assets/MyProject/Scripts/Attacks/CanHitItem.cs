using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CanHitItem : MonoBehaviour, IHitAnalysis
{
    public List<Renderer> renders = new List<Renderer>();
    public List<Material> mats = new List<Material>();
    public List<Shader> shader = new List<Shader>();

    public void Awake()
    {
        FindRenders();
        renders.ForEach(render => { shader.Add(render.material.shader); mats.Add(render.material);  });
        Utility.SetLayerWithChildren(gameObject, LayerMask.NameToLayer("Enemy"));
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

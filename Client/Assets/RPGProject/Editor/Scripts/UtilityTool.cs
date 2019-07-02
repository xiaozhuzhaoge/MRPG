using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;



public class UtiltyTool : EditorWindow {

    [MenuItem("Plugins/小工具")]
	static void Init()
    {
        UtiltyTool abt = EditorWindow.GetWindow<UtiltyTool>();
        abt.Show();
    }
 

    private void OnGUI()
    {
        //UGUI 横向布局 纵向布局

        GUILayout.BeginVertical();//开始纵向布局


        if (GUILayout.Button("快速添加MeshColider"))//按钮
        {
            AddBoxColider();
        }
        if (GUILayout.Button("快速添加MeshColider OpenConvex"))//按钮
        {
            AddMeshColiderWithConvex();
        }
        if (GUILayout.Button("快速添加MeshColider OpenConvex OpenTrigger"))//按钮
        {
            AddMeshColiderWithTrigger();
        }
        if (GUILayout.Button("快速查找指定对象删除BoxColider组件"))//按钮
        {
            RemoveBoxColider();
        }
        if (GUILayout.Button("修改lua文件后缀为lua.txt"))
        {
            ReNameText();
        }


        GUILayout.EndVertical();//结束纵向布局
    }

    static void AddBoxColider()
    {
        GameObject[] gos =  Selection.GetFiltered<GameObject>(SelectionMode.Deep);
        MeshCollider mc;
        foreach (var data in gos)
        {
            mc = data.GetComponent<MeshCollider>();
            if (mc)
            {
                mc.convex = false;
                mc.isTrigger = false;
            }
            else
            {
                mc = data.AddComponent<MeshCollider>();
                mc.convex = false;
                mc.isTrigger = false;
            }
        }
    }

    static void AddMeshColiderWithConvex()
    {
        GameObject[] gos = Selection.GetFiltered<GameObject>(SelectionMode.Deep);
        MeshCollider mc;
        foreach (var data in gos)
        {
            mc = data.GetComponent<MeshCollider>();
            if (mc)
                mc.convex = true;
            else
            {   mc = data.AddComponent<MeshCollider>();
                mc.convex = true;
            }
        }
    }

    static void AddMeshColiderWithTrigger()
    {
        GameObject[] gos = Selection.GetFiltered<GameObject>(SelectionMode.Deep);
        MeshCollider mc;
        foreach (var data in gos)
        {
            mc = data.GetComponent<MeshCollider>();
            if (mc)
            {
                mc.convex = true;
                mc.isTrigger = true;
            }
            else
            {
                mc = data.AddComponent<MeshCollider>();
                mc.convex = true;
                mc.isTrigger = true;
            }
        }
    }

    static void RemoveBoxColider()
    {
        GameObject[] gos = Selection.GetFiltered<GameObject>(SelectionMode.Deep);
        foreach (var data in gos)
        {
            if(data.GetComponent<MeshCollider>())
                DestroyImmediate(data.GetComponent<MeshCollider>());
        }
    }

    static void ReNameText()
    {
        Object[] gos = Selection.GetFiltered<Object>(SelectionMode.Deep);
        foreach (var data in gos)
        {
            string newName = data.name.Replace(".lua","txt");
            string assetpath = AssetDatabase.GetAssetPath(data);
            AssetDatabase.RenameAsset(assetpath, newName);
           
        }
        AssetDatabase.Refresh();
    }
}

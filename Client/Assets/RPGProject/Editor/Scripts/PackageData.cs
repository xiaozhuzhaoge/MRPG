using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// 可编程对象
/// </summary>
public class PackageData : ScriptableObject
{
    [SerializeField]
    public string Abpath;
    [SerializeField]
    public string ext;
    [SerializeField]
    public string outPath;
    [SerializeField]
    public BuildTarget platform;
}

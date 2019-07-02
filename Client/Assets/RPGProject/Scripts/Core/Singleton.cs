using UnityEngine;
using System.Collections;

public abstract class Singleton<T> : System.IDisposable where T : new()
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new T();
                Init();
            }
            
            return instance;

        }
    }

    public static void Init() { }

    public virtual void Dispose()
    {

    }
}

/// <summary>
/// Persistent manager - a singleton component, that will not
/// be destroyed between levels.
/// </summary>
public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    protected static T _instance;

    protected virtual void Awake()
    {
        // First we check if there are any other instances conflicting
        if (Instance != null && Instance != this)
        {
            // If that is the case, we destroy other instances
            Destroy(gameObject);
            return;
        }

        // Here we save our singleton instance
        Instance = this as T;

        Initialize();
    }

    /// <summary>
    /// Use this for one time initialization
    /// </summary>
    protected virtual void Initialize()
    {
        // Furthermore we make sure that we don't destroy between scenes (this is optional)
        DontDestroyOnLoad(gameObject);
    }

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (T)Object.FindObjectOfType(typeof(T));
                if(_instance == null)
                {
                    _instance = new GameObject().AddComponent<T>();
                }
            }
            return _instance;
        }
        protected set
        {
            _instance = value;
        }
    }

    public void DestroySelf()
    {
        Dispose();
        MonoSingleton<T>.Instance = null;
        UnityEngine.Object.Destroy(gameObject);
    }

    public static bool HasInstance
    {
        get { return _instance != null; }
    }

    public virtual void Dispose()
    {

    }
}
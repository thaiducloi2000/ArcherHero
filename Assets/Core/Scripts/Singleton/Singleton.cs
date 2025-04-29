using UnityEngine;
/// <summary>
/// Singleton in single Scene
/// </summary>
/// <typeparam name="T"></typeparam>
public class Singleton<T> : MonoBehaviour where T : Component
{
    protected static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindFirstObjectByType(typeof(T));
                if (instance == null)
                {
                    SetupInstance();
                }
            }

            return instance;
        }
    }

    public virtual void Awake()
    {
        RemoveDuplicates();
    }

    private static void SetupInstance()
    {
        instance = (T)FindFirstObjectByType(typeof(T));
        if (instance == null)
        {
            GameObject gameObj = new GameObject();
            gameObj.name = typeof(T).Name;
            instance = gameObj.AddComponent<T>();
        }
    }


    private void RemoveDuplicates()
    {
        if (instance == null)
        {
            instance = this as T;
        }
        else
        {
            if (instance == this)
            {
                return;
            }
            Destroy(gameObject);
        }
    }
}

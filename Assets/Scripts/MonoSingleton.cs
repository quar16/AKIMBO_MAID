using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance { get { return _instance; } }

    // Optional: Awake 메서드를 사용하여 인스턴스를 생성합니다.
    protected virtual void Awake()
    {
        _instance = this as T;
    }
}

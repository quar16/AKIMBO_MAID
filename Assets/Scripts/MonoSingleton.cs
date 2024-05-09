using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            // 인스턴스가 없는 경우 새로 생성합니다.
            if (_instance == null)
            {
                // 새 GameObject를 생성하고 컴포넌트를 추가합니다.
                GameObject singletonObject = new GameObject(typeof(T).Name);
                _instance = singletonObject.AddComponent<T>();
                // 씬이 바뀌어도 파괴되지 않도록 설정합니다.
                DontDestroyOnLoad(singletonObject);

                // 상정하지 않은 경우이기 때문에 에러 코드를 띄운다.
                Debug.LogWarning(typeof(T).Name + " has been created. Please check.");
            }
            return _instance;
        }
    }

    // Optional: Awake 메서드를 사용하여 인스턴스를 생성합니다.
    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

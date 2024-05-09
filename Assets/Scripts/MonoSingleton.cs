using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            // �ν��Ͻ��� ���� ��� ���� �����մϴ�.
            if (_instance == null)
            {
                // �� GameObject�� �����ϰ� ������Ʈ�� �߰��մϴ�.
                GameObject singletonObject = new GameObject(typeof(T).Name);
                _instance = singletonObject.AddComponent<T>();
                // ���� �ٲ� �ı����� �ʵ��� �����մϴ�.
                DontDestroyOnLoad(singletonObject);

                // �������� ���� ����̱� ������ ���� �ڵ带 ����.
                Debug.LogWarning(typeof(T).Name + " has been created. Please check.");
            }
            return _instance;
        }
    }

    // Optional: Awake �޼��带 ����Ͽ� �ν��Ͻ��� �����մϴ�.
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

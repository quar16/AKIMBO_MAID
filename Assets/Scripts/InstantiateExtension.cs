using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class InstantiateExtension
{
    public static T Instantiate<T>(this MonoBehaviour caller, T original) where T : Component
    {
        T instance = Object.Instantiate(original);
        SceneManager.MoveGameObjectToScene(instance.gameObject, caller.gameObject.scene);
        return instance;
    }

    public static T Instantiate<T>(this MonoBehaviour caller, T original, Vector3 position, Quaternion rotation) where T : Component
    {
        T instance = Object.Instantiate(original, position, rotation);
        SceneManager.MoveGameObjectToScene(instance.gameObject, caller.gameObject.scene);
        return instance;
    }

    public static T InstantiateEffect<T>(this MonoBehaviour caller, T original) where T : Component
    {
        T instance = Object.Instantiate(original);
        SceneManager.MoveGameObjectToScene(instance.gameObject, caller.gameObject.scene);
        instance.AddComponent<AnimEffectRemover>();
        return instance;
    }

    public static T InstantiateEffect<T>(this MonoBehaviour caller, T original, Vector3 position, Quaternion rotation) where T : Component
    {
        T instance = Object.Instantiate(original, position, rotation);
        SceneManager.MoveGameObjectToScene(instance.gameObject, caller.gameObject.scene);
        instance.AddComponent<AnimEffectRemover>();
        return instance;
    }

    public static T InstantiateEffect<T>(this MonoBehaviour caller, T original, Vector3 position, Quaternion rotation, float scale) where T : Component
    {
        T instance = Object.Instantiate(original, position, rotation);
        instance.transform.localScale *= scale;
        SceneManager.MoveGameObjectToScene(instance.gameObject, caller.gameObject.scene);
        instance.AddComponent<AnimEffectRemover>();
        return instance;
    }

    public static T InstantiateEffect<T>(this MonoBehaviour _, T original, Vector3 position, Quaternion rotation, float scale, Transform parent) where T : Component
    {
        T instance = Object.Instantiate(original, position, rotation, parent);
        instance.transform.localEulerAngles = rotation.eulerAngles;
        instance.transform.localScale *= scale;
        instance.AddComponent<AnimEffectRemover>();
        return instance;
    }

    public static GameObject Instantiate(this MonoBehaviour caller, GameObject original)
    {
        GameObject instance = Object.Instantiate(original);
        SceneManager.MoveGameObjectToScene(instance, caller.gameObject.scene);
        return instance;
    }

    public static GameObject Instantiate(this MonoBehaviour caller, GameObject original, Vector3 position, Quaternion rotation)
    {
        GameObject instance = Object.Instantiate(original, position, rotation);
        SceneManager.MoveGameObjectToScene(instance, caller.gameObject.scene);
        return instance;
    }

    public static GameObject InstantiateChild(this MonoBehaviour caller, GameObject original, Vector3 position, Quaternion rotation)
    {
        GameObject instance = Object.Instantiate(original, caller.transform);
        instance.transform.localPosition = position;
        instance.transform.localRotation = rotation;

        return instance;
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public static class InstantiateExtension
{
    public static T Instantiate<T>(this MonoBehaviour caller, T original) where T : MonoBehaviour
    {
        T instance = Object.Instantiate(original);
        SceneManager.MoveGameObjectToScene(instance.gameObject, caller.gameObject.scene);
        return instance;
    }

    public static T Instantiate<T>(this MonoBehaviour caller, T original, Vector3 position, Quaternion rotation) where T : MonoBehaviour
    {
        T instance = Object.Instantiate(original, position, rotation);
        SceneManager.MoveGameObjectToScene(instance.gameObject, caller.gameObject.scene);
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
}

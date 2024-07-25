using System.Diagnostics;
using UnityEngine;

public class TempScript : MonoBehaviour
{
    private void Start()
    {
        this.LogCallerObjectName(gameObject);
    }
}


public static class MonoBehaviourUtility
{
    public static void LogCallerObjectName(this MonoBehaviour monoBehaviour, GameObject target)
    {
        if (monoBehaviour != null)
        {
            UnityEngine.Debug.Log($"Called by object: {monoBehaviour.gameObject.name}");
        }
    }
}

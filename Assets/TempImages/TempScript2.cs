using System.Collections;
using UnityEngine;

public class TempScript2 : MonoBehaviour
{
    private void OnEnable()
    {
        Debug.Log("asdsad");
    }

    public virtual void Activate()
    {
        gameObject.SetActive(true);
    }

    public virtual void Deactivate()
    {
        gameObject.SetActive(false);
    }
}

using UnityEngine;
using System.Diagnostics;

public class TempScript : MonoBehaviour
{
    public float power;
    public float duration;
    public int gap;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            CameraController.Instance.CameraShake(power, duration, gap);
        }
    }

}

using UnityEngine;
using System.Diagnostics;

public class TempScript : MonoBehaviour
{
    public float powerX;
    public float powerY;
    public float duration;
    public int gap;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            CameraController.Instance.CameraShake(powerX, powerY, duration, gap);
        }
    }

}

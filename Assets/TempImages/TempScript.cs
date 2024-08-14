using System.Collections;
using UnityEngine;

public class TempScript : MonoBehaviour
{
    public TempScript2 ts2;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            ts2.Activate();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            ts2.Deactivate();
        }
    }
}

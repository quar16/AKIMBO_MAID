using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform playerT;


    float camTrackingPower = 0.1f;

    void Update()
    {
        if (GameManager.Instance.gameMode == GameMode.RUN)
        {
            Vector3 playerPos = playerT.position;
            Vector3 camPos = transform.position;
            Vector3 targetPos = Vector3.Lerp(camPos, playerPos, camTrackingPower);
            targetPos.z = camPos.z;

            transform.position = targetPos;
        }
    }
}

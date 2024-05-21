using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //플레이어 캐릭터의 트랜스폼, 카메라로 추적할 좌표를 찾는데 사용한다
    public Transform playerT;

    //카메라의 트랜스폼, 오프셋을 적용하는데 사용한다
    public Transform cameraT;

    //플레이어 캐릭터의 좌표를 기준으로 카메라가 어디에 위치할지에 대한 오프셋 값
    public float offsetX = 0;
    public float offsetY = 0;

    //카메라의 추적을 얼마나 빠르게 할지 정하는 변수, lerp에 사용하므로 0~1사이
    public float camTrackingPower = 0.1f;

    void Update()
    {
        cameraT.localPosition = new Vector3(offsetX, offsetY, 0);

        if (GameManager.Instance.gameMode == GameMode.RUN)
        {
            float playerPosX = playerT.position.x;
            float camPosX = transform.position.x;
            Vector3 targetPos = transform.position;
            targetPos.x = Mathf.Lerp(camPosX, playerPosX, camTrackingPower);

            transform.position = targetPos;
        }
    }
}

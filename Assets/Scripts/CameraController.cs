using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //�÷��̾� ĳ������ Ʈ������, ī�޶�� ������ ��ǥ�� ã�µ� ����Ѵ�
    public Transform playerT;

    //ī�޶��� Ʈ������, �������� �����ϴµ� ����Ѵ�
    public Transform cameraT;

    //�÷��̾� ĳ������ ��ǥ�� �������� ī�޶� ��� ��ġ������ ���� ������ ��
    public float offsetX = 0;
    public float offsetY = 0;

    //ī�޶��� ������ �󸶳� ������ ���� ���ϴ� ����, lerp�� ����ϹǷ� 0~1����
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

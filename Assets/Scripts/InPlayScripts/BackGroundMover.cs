using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMover : MonoBehaviour
{
    public float bgLength;

    public float lastBackGroundPosX;
    public float ratio = 0.7f;

    int bgCount = 5;//3

    public int targetBackGroundIndex;

    public List<GameObject> backGroundList;

    public GameObject backgroundPrefab;

    IEnumerator moveBackGround;

    public void Initiate()
    {
        InitBackGround();

        moveBackGround = MoveBackGround();
        StartCoroutine(moveBackGround);
        StartCoroutine(MoveBackGroundByPlayer());
    }

    public void InitBackGround()
    {
        for (int i = 0; i < bgCount; i++)
        {
            GameObject bg = Instantiate(backgroundPrefab, bgLength * i * Vector3.right, Quaternion.identity, transform);

            backGroundList.Add(bg);
        }
    }

    public IEnumerator MoveBackGround()
    {
        while (true)
        {
            // 오른쪽으로 이동할 때
            if (CameraController.Instance.cameraT.position.x - lastBackGroundPosX >= bgLength)
            {
                backGroundList[targetBackGroundIndex].transform.position += Vector3.right * bgLength * bgCount;
                lastBackGroundPosX += bgLength;
                targetBackGroundIndex = (targetBackGroundIndex + 1) % bgCount;
            }
            // 왼쪽으로 이동할 때
            else if (CameraController.Instance.cameraT.position.x - lastBackGroundPosX <= -bgLength)
            {
                backGroundList[(targetBackGroundIndex - 1 + bgCount) % bgCount].transform.position -= Vector3.right * bgLength * bgCount;
                lastBackGroundPosX -= bgLength;
                targetBackGroundIndex = (targetBackGroundIndex - 1 + bgCount) % bgCount;
            }

            yield return null;
        }
    }


    public IEnumerator MoveBackGroundByPlayer()
    {
        float cameraLastPosX, cameraNowPosX;
        while (true)
        {
            cameraLastPosX = CameraController.Instance.cameraT.position.x;
            yield return null;
            cameraNowPosX = CameraController.Instance.cameraT.position.x;

            float deltaX = cameraNowPosX - cameraLastPosX;

            lastBackGroundPosX += deltaX * ratio;

            transform.position += deltaX * ratio * Vector3.right;
        }
    }
}
